﻿// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.CodeAnalysis;

namespace ReactiveMarbles.Locator.Register.SourceGenerator.Metadata;

internal record PropertyDependencyMetadata : DependencyMetadata
{
    public PropertyDependencyMetadata(IPropertySymbol property)
        : base(property.Type)
    {
        Property = property;

        Name = Property.Name;
    }

    public IPropertySymbol Property { get; }

    public string Name { get; }
}