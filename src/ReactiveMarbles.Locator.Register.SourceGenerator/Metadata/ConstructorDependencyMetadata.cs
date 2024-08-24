// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;

namespace ReactiveMarbles.Locator.Register.SourceGenerator.Metadata;

internal record ConstructorDependencyMetadata(IParameterSymbol Parameter, ITypeSymbol Type) : DependencyMetadata(Type);