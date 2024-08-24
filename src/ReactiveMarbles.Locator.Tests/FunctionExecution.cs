// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Tmds.Utils;

namespace ReactiveMarbles.Locator.Tests;

/// <summary>
/// Configured <see cref="Tmds.Utils.FunctionExecutor"/>.
/// </summary>
internal static class FunctionExecution
{
    public static FunctionExecutor XUnitFunctionExecutor { get; } =
        new(o =>
        {
            o.StartInfo.RedirectStandardError = true;
            o.OnExit = p =>
            {
                if (p.ExitCode != 0)
                {
                    var message = $"Function exit code failed with exit code: {p.ExitCode.ToString()}" +
                                  Environment.NewLine +
                                  p.StandardError.ReadToEnd();

                    throw new Xunit.Sdk.XunitException(message);
                }
            };
        });
}
