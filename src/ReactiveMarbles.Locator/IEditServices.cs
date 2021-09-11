// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace ReactiveMarbles.Locator
{
    /// <summary>
    /// I Edit Services.
    /// </summary>
    public interface IEditServices
    {
        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <typeparam name="T">The service type.</typeparam>
        void AddService<T>(Func<T> instanceFactory);

        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <param name="contract">The contract.</param>
        /// <typeparam name="T">The service type.</typeparam>
        void AddService<T>(Func<T> instanceFactory, string contract);
    }
}
