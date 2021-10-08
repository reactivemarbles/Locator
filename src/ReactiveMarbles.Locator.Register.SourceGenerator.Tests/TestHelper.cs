// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using NuGet.LibraryModel;
using NuGet.Versioning;
using ReactiveMarbles.NuGet.Helpers;
using ReactiveMarbles.SourceGenerator.TestNuGetHelper.Compilation;
using VerifyTests;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace ReactiveMarbles.Locator.Register.SourceGenerator.Tests;

public sealed class TestHelper : IDisposable
{
    public TestHelper(ITestOutputHelper testOutput) => TestOutputHelper = testOutput ?? throw new ArgumentNullException(nameof(testOutput));

    private EventBuilderCompiler? EventCompiler { get; set; }

    private ITestOutputHelper TestOutputHelper { get; }

    public async Task InitializeAsync()
    {
        var targetFrameworks = "netstandard2.0".ToFrameworks();

        var framework = targetFrameworks[0];

        var inputGroup = new InputAssembliesGroup();
        inputGroup.IncludeGroup.AddFiles(AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic).Select(x => x.Location));
        EventCompiler = new(inputGroup, inputGroup, framework);
    }

    public Task TestFail(string source, string contractParameter, [CallerFilePath] string file = "")
    {
        if (EventCompiler is null)
        {
            throw new InvalidOperationException("Must have valid compiler instance.");
        }

        var utility = new SourceGeneratorUtility(x => TestOutputHelper.WriteLine(x));

        GeneratorDriver? driver = null;

        Assert.Throws<InvalidOperationException>(() => utility.RunGenerator<Generator>(EventCompiler, out _, out _, out driver, source));

        var settings = new VerifySettings();
        settings.UseParameters(contractParameter);
        settings.AutoVerify();
        return Verifier.Verify(driver, settings, file);
    }

    public Task TestPass(string source, string contractParameter, [CallerFilePath] string file = "")
    {
        var driver = Generate(source);
        var settings = new VerifySettings();
        settings.UseParameters(contractParameter);
        return Verifier.Verify(driver, settings, file);
    }

    public Task TestPass(string source, string contractParameter, LazyThreadSafetyMode mode, [CallerFilePath] string file = "")
    {
        var driver = Generate(source);

        var settings = new VerifySettings();
        settings.UseParameters(contractParameter, mode);
        return Verifier.Verify(driver, settings, file);
    }

    public void Dispose() => EventCompiler?.Dispose();

    private GeneratorDriver Generate(string source)
    {
        if (EventCompiler is null)
        {
            throw new InvalidOperationException("Must have valid compiler instance.");
        }

        var utility = new SourceGeneratorUtility(x => TestOutputHelper.WriteLine(x));

        utility.RunGenerator<Generator>(EventCompiler, out _, out _, out var driver, source);

        return driver;
    }
}
