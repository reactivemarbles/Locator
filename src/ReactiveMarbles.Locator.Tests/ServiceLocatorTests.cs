// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
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
        public void HasInstance()
        {
            var fixture = ServiceLocator.Current();

            fixture.Should().NotBeNull();
        }

        /// <summary>
        /// Tests the RemoveServices method removes the services.
        /// </summary>
        [Fact]
        public void SetReturnsInstance() =>
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
        /// Tests the Set method throws an exception when a null parameter passed.
        /// </summary>
        [Fact]
        public void SetNullThrows() =>
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

        /// <summary>
        /// Tests the HasService method returns a true value for the interface.
        /// </summary>
        [Fact]
        public void HasServiceReturnsFromInterface() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given
                var testService = new TestService();
                var fixture = ServiceLocator.Current();
                fixture.AddService<ITestService>(() => testService);

                // When
                var result = fixture.HasService<ITestService>();

                // Then
                result.Should().Be(true);
            });

        /// <summary>
        /// Tests the HasService method returns a true value for the interface.
        /// </summary>
        /// <param name="contract">The contract.</param>
        [Theory]
        [InlineData("contract")]
        [InlineData("interface")]
        [InlineData("abstract")]
        public void HasServiceReturnsFromInterfaceWithContract(string contract) =>
            XUnitFunctionExecutor.Run(
                args =>
                {
                    // Given
                    var testService = new TestService();
                    var fixture = ServiceLocator.Current();
                    fixture.AddService<ITestService>(() => testService, args[0]);

                    // When
                    var result = fixture.HasService<ITestService>(contract);

                    // Then
                    result.Should().Be(true);
                },
                new[] { contract });

        /// <summary>
        /// Tests the GetService method returns a concretion for the interface.
        /// </summary>
        [Fact]
        public void GetServiceReturnsFromInterface() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given
                var testService = new TestService();
                var fixture = ServiceLocator.Current();
                fixture.AddService<ITestService>(() => testService);

                // When
                var result = fixture.GetService<ITestService>();

                // Then
                result.Should().Be(testService);
            });

        /// <summary>
        /// Tests the GetService method returns a concretion for the interface.
        /// </summary>
        [Fact]
        public void GetServiceThrowsIfNotFound() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given
                var fixture = ServiceLocator.Current();

                // When
                var result = Record.Exception(() => fixture.GetService<ITestService>());

                // Then
                result
                    .Should()
                    .BeOfType<InvalidOperationException>()
                    .Which
                    .Message
                    .Should()
                    .Be("No service for the provided type exists.");
            });

        /// <summary>
        /// Tests the GetService method returns a concretion for the interface.
        /// </summary>
        /// <param name="argument">The test argument.</param>
        [Theory]
        [InlineData("contract")]
        [InlineData("interface")]
        [InlineData("abstract")]
        public void GetServiceReturnsFromInterfaceWithContract(string argument) =>
            XUnitFunctionExecutor.Run(
                args =>
                {
                    // Given
                    var testService = new TestService();
                    var fixture = ServiceLocator.Current();
                    var contract = args[0];
                    fixture.AddService<ITestService>(() => testService, contract);

                    // When
                    var result = fixture.GetService<ITestService>(contract);

                    // Then
                    result.Should().Be(testService);
                },
                new[] { argument });

        /// <summary>
        /// Tests the GetService method returns a concretion for the interface.
        /// </summary>
        /// <param name="contract">The contract.</param>
        [Theory]
        [InlineData("contract")]
        [InlineData("interface")]
        [InlineData("abstract")]
        public void GetServiceWithContractThrowsIfNotFound(string contract) =>
            XUnitFunctionExecutor.Run(
                args =>
                {
                    // Given
                    var fixture = ServiceLocator.Current();

                    // When
                    var result = Record.Exception(() => fixture.GetService<ITestService>(args[0]));

                    // Then
                    result
                        .Should()
                        .BeOfType<InvalidOperationException>()
                        .Which
                        .Message
                        .Should()
                        .Be("No service for the provided type exists.");
                },
                new[] { contract });

        /// <summary>
        /// Tests the GetService method returns a concretion for the interface.
        /// </summary>
        [Fact]
        public void TryGetServiceReturnsReturnsTrue() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given
                var testService = new TestService();
                var fixture = ServiceLocator.Current();
                fixture.AddService<ITestService>(() => testService);

                // When
                var result = fixture.TryGetService<ITestService>(out var service);

                // Then
                result.Should().Be(true);
                service.Should().Be(testService);
            });

        /// <summary>
        /// Tests the GetService method returns a concretion for the interface.
        /// </summary>
        [Fact]
        public void TryGetServiceReturnsFalse() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given
                var fixture = ServiceLocator.Current();

                // When
                var result = fixture.TryGetService<ITestService>(out var service);

                // Then
                result.Should().Be(false);
                service.Should().Be(null);
            });

        /// <summary>
        /// Tests the GetServices method returns a concretion for the interface.
        /// </summary>
        [Fact]
        public void GetServicesReturnsFromInterface() =>
            XUnitFunctionExecutor.Run(() =>
            {
                // Given
                var testServiceOne = new TestService();
                var testServiceTwo = new TestService();
                var fixture = ServiceLocator.Current();
                fixture.AddService<ITestService>(() => testServiceOne);
                fixture.AddService<ITestService>(() => testServiceTwo);

                // When
                var result = fixture.GetServices<ITestService>();

                // Then
                result.Should().Contain(new[] { testServiceOne, testServiceTwo }).And.HaveCount(2);
            });

        /// <summary>
        /// Tests the GetServices method returns a concretion for the interface.
        /// </summary>
        /// <param name="argument">The argument.</param>
        [Theory]
        [InlineData("contract")]
        [InlineData("interface")]
        [InlineData("abstract")]
        public void GetServicesReturnsFromInterfaceWithContract(string argument) =>
            XUnitFunctionExecutor.Run(
                args =>
                {
                    // Given
                    var contract = args[0];
                    var fixture = ServiceLocator.Current();
                    fixture.AddService<ITestService>(() => new TestService(), contract);
                    fixture.AddService<ITestService>(() => new TestService(), contract);

                    // When
                    var result = fixture.GetServices<ITestService>(contract);

                    // Then
                    result.Should().NotBeNull().And.HaveCount(2);
                },
                new[] { argument });
    }
}
