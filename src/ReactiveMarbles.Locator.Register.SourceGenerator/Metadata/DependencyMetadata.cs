// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;
using ReactiveMarbles.RoslynHelpers;

namespace ReactiveMarbles.Locator.Register.SourceGenerator.Metadata;

internal abstract record DependencyMetadata
{
    protected DependencyMetadata(ITypeSymbol type)
    {
        Type = type;
        TypeName = type.ToDisplayString(RoslynCommonHelpers.TypeFormat);
    }

    public ITypeSymbol Type { get; }

    public string TypeName { get; }
}