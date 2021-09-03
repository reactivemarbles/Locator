// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using FluentAssertions;
using Xunit;

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
        /// Tests the GetService method returns a contretion for the interface.
        /// </summary>
        [Fact]
        public void ServiceLocatorReturnsServiceFromInterface()
        {
            // Given
            var fixture = ServiceLocator.Current();
            fixture.AddService<ITestService>(() => new TestService());

            // When
            var result = fixture.GetService<ITestService>();

            // Then
            result.Should().NotBeNull();
        }
    }
}
