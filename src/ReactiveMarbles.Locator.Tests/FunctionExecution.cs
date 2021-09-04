// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Tmds.Utils;

namespace ReactiveMarbles.Locator.Tests
{
    /// <summary>
    /// Configured <see cref="FunctionExecutor"/>.
    /// </summary>
    internal static class FunctionExecution
    {
        public static FunctionExecutor GetFunctionExecutor() => new(_ => { });
    }
}
