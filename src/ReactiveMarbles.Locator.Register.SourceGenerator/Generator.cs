﻿// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace ReactiveMarbles.Locator.Register.SourceGenerator;

/// <summary>
/// The main generator instance.
/// </summary>
[Generator]
public class Generator : ISourceGenerator
{
    /// <inheritdoc/>
    public void Execute(GeneratorExecutionContext context)
    {
        // add the attribute text.
        context.AddSource("Splat.DI.Extensions.SourceGenerated.cs", SourceText.From(Constants.ExtensionMethodText, Encoding.UTF8));

        if (context.SyntaxReceiver is not SyntaxReceiver syntaxReceiver)
        {
            return;
        }

        var compilation = context.Compilation;

        var options = (compilation as CSharpCompilation)?.SyntaxTrees.FirstOrDefault()?.Options as CSharpParseOptions;
        compilation = context.Compilation.AddSyntaxTrees(CSharpSyntaxTree.ParseText(SourceText.From(Constants.ExtensionMethodText, Encoding.UTF8), options ?? new CSharpParseOptions()));

        var outputText = SourceGeneratorHelpers.Generate(context, compilation, syntaxReceiver);

        context.AddSource("Splat.DI.Extensions.Registrations.SourceGenerated.cs", SourceText.From(outputText, Encoding.UTF8));
        context.AddSource("Splat.DI.Extensions.PreserveAttribute.SourceGenerated.cs", SourceText.From(Constants.PreserveAttribute, Encoding.UTF8));
    }

    /// <inheritdoc/>
    public void Initialize(GeneratorInitializationContext context) => context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
}
