// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace ReactiveMarbles.Locator
{
    /// <summary>
    /// I Get Services.
    /// </summary>
    public interface IGetServices
    {
        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <returns>
        /// A service of the provided type.
        /// </returns>
        T GetService<T>();

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <typeparam name="T">The service type.</typeparam>
        /// <returns>
        /// A service of the provided type.
        /// </returns>
        T GetService<T>(string contract);

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <typeparam name="T">The service type.</typeparam>
        /// <returns>
        /// An enumerable of services for the provided type.
        /// </returns>
        IEnumerable<T> GetServices<T>();

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <typeparam name="T">The service type.</typeparam>
        /// <returns>
        /// An enumerable of services for the provided type.
        /// </returns>
        IEnumerable<T> GetServices<T>(string contract);

        /// <summary>
        /// Try and get the service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <typeparam name="T">The service type.</typeparam>
        /// <returns>A value indicating whether the service exists.</returns>
        bool TryGetService<T>(out T service);

        /// <summary>
        /// Try and get the service.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="service">The service.</param>
        /// <typeparam name="T">The service type.</typeparam>
        /// <returns>A value indicating whether the service exists.</returns>
        bool TryGetService<T>(string contract, out T service);

        /// <summary>
        /// Determines whether the specified service type has service.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>
        ///   <c>true</c> if the specified service type has service; otherwise, <c>false</c>.
        /// </returns>
        bool HasService<T>();

        /// <summary>
        /// Determines whether the specified service type has service.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>
        ///   <c>true</c> if the specified service type has service; otherwise, <c>false</c>.
        /// </returns>
        bool HasService<T>(string contract);
    }
}
