using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace MathForge.Analyzers;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class MathArgumentExceptionAnalyzer : DiagnosticAnalyzer
{
    public const string DiagnosticId = "MFA001";
    internal const string HelperTypeName = "MathForge.Numerics.Exceptions.MathArgumentException";

    private static readonly DiagnosticDescriptor Rule = new(
        DiagnosticId, "Use MathArgumentException",
        "Replace this guard with MathArgumentException.{0}", "Usage",
        DiagnosticSeverity.Info, isEnabledByDefault: true,
        description: "Use shared mathematical guards while preserving exception details.");

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.IfStatement);
    }

    private static void Analyze(SyntaxNodeAnalysisContext context)
    {
        var statement = (IfStatementSyntax)context.Node;
        var replacement = MathGuardReplacement.TryCreate(statement, context.SemanticModel, context.CancellationToken);
        if (replacement is null)
        {
            return;
        }

        var invocation = (InvocationExpressionSyntax)replacement.Expression;
        var member = (MemberAccessExpressionSyntax)invocation.Expression;
        context.ReportDiagnostic(Diagnostic.Create(Rule, statement.IfKeyword.GetLocation(), member.Name.Identifier.ValueText));
    }
}
