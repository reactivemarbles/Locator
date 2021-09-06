// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Tmds.Utils;
using Xunit;
using static ReactiveMarbles.Locator.Tests.FunctionExecution;

[assembly: ExcludeFromCodeCoverage]

namespace ReactiveMarbles.Locator.Tests
{
    /// <summary>
    /// Service Locator Tests.
    /// </summary>
    public class ServiceLocatorTests
    {
        /// <summary>
        /// The Service locator has an instance.
        /// </summary>
        [Fact]
        public void ServiceLocatorHasInstance()
        {
            var fixture = ServiceLocator.Current();

            fixture.Should().NotBeNull();
        }

        /// <summary>
        /// Tests the GetService method returns a concretion for the interface.
        /// </summary>
        [Fact]
        public void ServiceLocatorGetServiceReturnsFromInterface()
        {
            // Given
            var fixture = ServiceLocator.Current();
            fixture.AddService<ITestService>(() => new TestService());

            // When
            var result = fixture.GetService(typeof(ITestService));

            // Then
            result.Should().NotBeNull();
        }

        /// <summary>
        /// Tests the RemoveService method removes the service.
        /// </summary>
        [Fact]
        public void ServiceLocatorRemoveServiceReturnsFromInterface() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given
                var fixture = ServiceLocator.Current();
                fixture.AddService<ITestService>(() => new TestService());

                // When
                fixture.RemoveService(typeof(ITestService));

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
        public void ServiceLocatorRemoveServiceWithContractReturnsFromInterface(string contract) =>
            XUnitFunctionExecutor.Run(
                state =>
                {
                    // Given
                    var fixture = ServiceLocator.Current();
                    fixture.AddService<ITestService>(() => new TestService(), state[0]);

                    // When
                    fixture.RemoveService(typeof(ITestService), state[0]);

                    // Then
                    fixture.GetService<ITestService>().Should().BeNull();
                },
                new[] { contract });

        /// <summary>
        /// Tests the RemoveServices method removes the services.
        /// </summary>
        [Fact]
        public void ServiceLocatorRemoveServicesReturnsFromInterface() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given
                var fixture = ServiceLocator.Current();
                fixture.AddService<ITestService>(() => new TestService());

                // When
                fixture.RemoveServices(typeof(ITestService));

                // Then
                fixture.GetService<ITestService>().Should().BeNull();
            });

        /// <summary>
        /// Tests the RemoveServices method removes the services.
        /// </summary>
        /// <param name="contract">the contract.</param>
        [Theory]
        [InlineData("")]
        [InlineData("contract")]
        public void ServiceLocatorRemoveServicesWithContractReturnsFromInterface(string contract) =>
            XUnitFunctionExecutor.Run(
                state =>
                {
                    // Given
                    var fixture = ServiceLocator.Current();
                    fixture.AddService<ITestService>(() => new TestService(), state[0]);
                    fixture.AddService<ITestService>(() => new TestService(), state[0]);

                    // When
                    fixture.RemoveServices(typeof(ITestService), state[0]);

                    // Then
                    fixture.GetService<ITestService>().Should().BeNull();
                },
                new[] { contract });

        /// <summary>
        /// Tests the RemoveServices method removes the services.
        /// </summary>
        [Fact]
        public void ServiceLocatorSetReturnsInstance() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given
                var locator = new ServiceLocator();

                // When
                ServiceLocator.Set(locator);

                // Then
                ServiceLocator.Current().Should().Be(locator);
            });

        /// <summary>
        /// Tests the RemoveServices method removes the services.
        /// </summary>
        [Fact]
        public void ServiceLocatorSetNullThrows() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given, When
                var exception = Record.Exception(() => ServiceLocator.Set(null!));

                // Then
                exception
                    .Should()
                    .BeOfType<ArgumentNullException>()
                    .Which
                    .ParamName
                    .Should()
                    .Be("serviceLocator");
            });
    }
}
