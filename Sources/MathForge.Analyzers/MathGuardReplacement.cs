using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;

namespace MathForge.Analyzers;

internal static class MathGuardReplacement
{
    public static ExpressionStatementSyntax? TryCreate(IfStatementSyntax statement, SemanticModel model,
        CancellationToken cancellationToken)
    {
        if (statement.Else is not null || statement.ContainsDirectives ||
            statement.DescendantTrivia().Any(trivia => trivia.IsKind(SyntaxKind.SingleLineCommentTrivia) ||
                                                       trivia.IsKind(SyntaxKind.MultiLineCommentTrivia)))
        {
            return null;
        }

        var helperType = model.Compilation.GetTypeByMetadataName(MathArgumentExceptionAnalyzer.HelperTypeName);
        if (helperType is null ||
            SymbolEqualityComparer.Default.Equals(
                model.GetEnclosingSymbol(statement.SpanStart, cancellationToken)?.ContainingType, helperType))
        {
            return null;
        }

        var body = statement.Statement is BlockSyntax { Statements.Count: 1 } block
            ? block.Statements[0]
            : statement.Statement;
        if (body is not ThrowStatementSyntax { Expression: ObjectCreationExpressionSyntax creation } ||
            model.GetOperation(creation, cancellationToken) is not IObjectCreationOperation operation ||
            operation.Type?.ToDisplayString() != "System.ArgumentOutOfRangeException")
        {
            return null;
        }

        ExpressionSyntax condition = Unwrap(statement.Condition);
        string? methodName = null;
        ExpressionSyntax? value = null;
        ExpressionSyntax? bound = null;
        var negated = condition is PrefixUnaryExpressionSyntax prefix && prefix.IsKind(SyntaxKind.LogicalNotExpression);
        if (negated)
        {
            condition = Unwrap(((PrefixUnaryExpressionSyntax)condition).Operand);
        }

        if (condition is InvocationExpressionSyntax invocation &&
            model.GetOperation(invocation, cancellationToken) is IInvocationOperation predicate &&
            predicate.TargetMethod.IsStatic && predicate.Arguments.Length == 1 &&
            IsNumeric(predicate.TargetMethod.ContainingType))
        {
            methodName = (predicate.TargetMethod.Name, negated) switch
            {
                ("IsNaN", false) => "ThrowIfIsNaN",
                ("IsInfinity", false) => "ThrowIfIsInfinity",
                ("IsFinite", true) => "ThrowIfNotFinite",
                _ => null
            };
            value = invocation.ArgumentList.Arguments[0].Expression;
        }
        else if (!negated && condition is BinaryExpressionSyntax binary &&
                 model.GetOperation(binary, cancellationToken) is IBinaryOperation { OperatorMethod: null })
        {
            value = Unwrap(binary.Left);
            bound = Unwrap(binary.Right);
            var zero = IsZero(bound, model, cancellationToken);
            methodName = binary.Kind() switch
            {
                SyntaxKind.LessThanExpression => zero ? "ThrowIfNegative" : "ThrowIfLessThan",
                SyntaxKind.LessThanOrEqualExpression => zero ? "ThrowIfNegativeOrZero" : "ThrowIfLessThanOrEqual",
                SyntaxKind.GreaterThanExpression => zero ? "ThrowIfPositive" : "ThrowIfGreaterThan",
                SyntaxKind.GreaterThanOrEqualExpression => zero ? "ThrowIfPositiveOrZero" : "ThrowIfGreaterThanOrEqual",
                SyntaxKind.EqualsExpression when zero => "ThrowIfZero",
                _ => null
            };
            if (zero)
            {
                bound = null;
            }
        }

        if (methodName is null || value is null || !IsStableNumber(value, model, cancellationToken) ||
            (bound is not null && (!IsStableNumber(bound, model, cancellationToken) ||
                                   !SymbolEqualityComparer.Default.Equals(
                                       model.GetTypeInfo(value, cancellationToken).Type,
                                       model.GetTypeInfo(bound, cancellationToken).Type))))
        {
            return null;
        }

        var arguments = operation.Arguments;
        var isNaN = methodName == "ThrowIfIsNaN";
        if (arguments.Length != (isNaN ? 2 : 3) ||
            arguments[0].Parameter?.Name != "paramName" ||
            arguments[0].Value.ConstantValue is not { HasValue: true, Value: string } ||
            arguments[arguments.Length - 1].Value.ConstantValue is not { HasValue: true, Value: string })
        {
            return null;
        }

        if (!isNaN && (arguments[1].Parameter?.Name != "actualValue" ||
                       arguments[1].Syntax is not ArgumentSyntax actual ||
                       !SyntaxFactory.AreEquivalent(Unwrap(actual.Expression), Unwrap(value))))
        {
            return null;
        }

        if (creation.ArgumentList is null ||
            creation.ArgumentList.Arguments.Any(argument => argument.NameColon is not null))
        {
            return null;
        }

        var helperArguments = new List<ArgumentSyntax> { SyntaxFactory.Argument(value.WithoutTrivia()) };
        if (bound is not null)
        {
            helperArguments.Add(SyntaxFactory.Argument(bound.WithoutTrivia()));
        }

        helperArguments.Add(SyntaxFactory.Argument(creation.ArgumentList.Arguments[0].Expression.WithoutTrivia()));
        helperArguments.Add(
            SyntaxFactory.Argument(creation.ArgumentList.Arguments[isNaN ? 1 : 2].Expression.WithoutTrivia()));
        var call = SyntaxFactory.InvocationExpression(
            SyntaxFactory.ParseExpression($"global::{MathArgumentExceptionAnalyzer.HelperTypeName}.{methodName}"),
            SyntaxFactory.ArgumentList(SyntaxFactory.SeparatedList(helperArguments)));

        if (model.GetSpeculativeSymbolInfo(statement.SpanStart, call, SpeculativeBindingOption.BindAsExpression).Symbol
                is not IMethodSymbol method ||
            !SymbolEqualityComparer.Default.Equals(method.ContainingType, helperType))
        {
            return null;
        }

        return SyntaxFactory.ExpressionStatement(call).WithTriviaFrom(statement);
    }

    private static ExpressionSyntax Unwrap(ExpressionSyntax expression)
    {
        while (expression is ParenthesizedExpressionSyntax parenthesized)
        {
            expression = parenthesized.Expression;
        }

        return expression;
    }

    private static bool IsStableNumber(ExpressionSyntax expression, SemanticModel model,
        CancellationToken cancellationToken)
    {
        var operation = model.GetOperation(expression, cancellationToken);
        return IsNumeric(model.GetTypeInfo(expression, cancellationToken).Type) &&
               (operation is IParameterReferenceOperation or ILocalReferenceOperation ||
                operation?.ConstantValue.HasValue == true);
    }

    private static bool IsZero(ExpressionSyntax expression, SemanticModel model, CancellationToken cancellationToken)
    {
        var constant = model.GetConstantValue(expression, cancellationToken);
        return constant.HasValue && constant.Value is byte or sbyte or short or ushort or int or uint or long or ulong or float or double
                   or decimal &&
               Convert.ToDouble(constant.Value, System.Globalization.CultureInfo.InvariantCulture) == 0;
    }

    private static bool IsNumeric(ITypeSymbol? type)
    {
        return type?.SpecialType is SpecialType.System_Byte or SpecialType.System_SByte or
            SpecialType.System_Int16 or SpecialType.System_UInt16 or SpecialType.System_Int32 or
            SpecialType.System_UInt32 or SpecialType.System_Int64 or SpecialType.System_UInt64 or
            SpecialType.System_Single or SpecialType.System_Double or SpecialType.System_Decimal;
    }
}
