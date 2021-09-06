// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using FluentAssertions;
using Tmds.Utils;
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
            GetFunctionExecutor().Run(() =>
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

        /// <summary>
        /// Tests the RemoveService method removes the service.
        /// </summary>
        [Fact]
        public void ServiceLocatorRemoveServiceGenericReturnsFromInterface() =>
            GetFunctionExecutor().Run(() =>
            {
                // Given
                var fixture = ServiceLocator.Current();
                fixture.AddService<ITestService>(() => new TestService());

                // When
                fixture.RemoveService<ITestService>();

                // Then
                fixture.GetService<ITestService>().Should().BeNull();
            });

        /// <summary>
        /// Tests the RemoveService method removes the service.
        /// </summary>
        /// <param name="contract">the contract.</param>
        [Theory]
        [InlineData("")]
        [InlineData("contract")]
        public void ServiceLocatorRemoveServiceGenericWithContractReturnsFromInterface(string contract) =>
            GetFunctionExecutor().Run(
                state =>
                {
                    // Given
                    var fixture = ServiceLocator.Current();
                    fixture.AddService<ITestService>(() => new TestService(), state[0]);

                    // When
                    fixture.RemoveService<ITestService>(state[0]);

                    // Then
                    fixture.GetService<ITestService>().Should().BeNull();
                },
                new[] { contract });

        /// <summary>
        /// Tests the RemoveServices method removes the service.
        /// </summary>
        [Fact]
        public void ServiceLocatorRemoveServicesGenericReturnsFromInterface() =>
            GetFunctionExecutor().Run(() =>
            {
                // Given
                var fixture = ServiceLocator.Current();
                fixture.AddService<ITestService>(() => new TestService());
                fixture.AddService<ITestService>(() => new TestService());

                // When
                fixture.RemoveServices<ITestService>();

                // Then
                fixture.GetService<ITestService>().Should().BeNull();
            });

        /// <summary>
        /// Tests the RemoveServices method removes the service.
        /// </summary>
        /// <param name="contract">the contract.</param>
        [Theory]
        [InlineData("")]
        [InlineData("contract")]
        public void ServiceLocatorRemoveServicesGenericWithContractReturnsFromInterface(string contract) =>
            GetFunctionExecutor().Run(
                state =>
                {
                    // Given
                    var fixture = ServiceLocator.Current();
                    fixture.AddService<ITestService>(() => new TestService(), state[0]);
                    fixture.AddService<ITestService>(() => new TestService(), state[0]);

                    // When
                    fixture.RemoveServices<ITestService>(state[0]);

                    // Then
                    fixture.GetService<ITestService>().Should().BeNull();
                },
                new[] { contract });
    }
}
