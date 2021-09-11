// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using FluentAssertions;
using Xunit;
using static ReactiveMarbles.Locator.Tests.FunctionExecution;

namespace ReactiveMarbles.Locator.Tests
{
    /// <summary>
    /// Tests the Service Locator Mixins.
    /// </summary>
    public class ServiceLocatorMixinTests
    {
        /// <summary>
        /// Tests the GetService method returns a concretion for the interface.
        /// </summary>
        [Fact]
        public void ServiceLocatorGetServiceGenericReturnsFromInterface()
        {
            // Given
            var fixture = ServiceLocator.Current();
            fixture.AddService<ITestService>(() => new TestService());

            // When
            var result = fixture.GetService<ITestService>();

            // Then
            result.Should().NotBeNull();
        }

        /// <summary>
        /// Tests the GetServices method returns a concretion for the interface.
        /// </summary>
        [Fact]
        public void ServiceLocatorGetServicesGenericReturnsFromInterface() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given
                var fixture = ServiceLocator.Current();
                fixture.AddService<ITestService>(() => new TestService());
                fixture.AddService<ITestService>(() => new TestService());

                // When
                var result = fixture.GetServices<ITestService>();

                // Then
                result.Should().NotBeNull().And.HaveCount(2);
            });
    }
}
