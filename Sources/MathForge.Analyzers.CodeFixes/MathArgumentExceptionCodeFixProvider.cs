using System.Collections.Immutable;
using System.Composition;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Formatting;
using Microsoft.CodeAnalysis.Simplification;

namespace MathForge.Analyzers;

[ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MathArgumentExceptionCodeFixProvider)), Shared]
public sealed class MathArgumentExceptionCodeFixProvider : CodeFixProvider
{
    public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(MathArgumentExceptionAnalyzer.DiagnosticId);

    public override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

    public override async Task RegisterCodeFixesAsync(CodeFixContext context)
    {
        var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
        var statement = root?.FindNode(context.Span).FirstAncestorOrSelf<IfStatementSyntax>();
        if (statement is null)
        {
            return;
        }

        context.RegisterCodeFix(
            CodeAction.Create("Use MathArgumentException", token => ReplaceAsync(context.Document, statement, token),
                MathArgumentExceptionAnalyzer.DiagnosticId), context.Diagnostics);
    }

    private static async Task<Document> ReplaceAsync(Document document, IfStatementSyntax statement, CancellationToken cancellationToken)
    {
        var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
        var model = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);
        if (root is null || model is null)
        {
            return document;
        }

        var replacement = MathGuardReplacement.TryCreate(statement, model, cancellationToken);
        return replacement is null ? document : document.WithSyntaxRoot(root.ReplaceNode(statement,
            replacement.WithAdditionalAnnotations(Formatter.Annotation, Simplifier.Annotation)));
    }
}
