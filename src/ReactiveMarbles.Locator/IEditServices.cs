// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;

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
        /// <typeparam name="TContract">The service type.</typeparam>
        void AddService<TContract>(Func<TContract> instanceFactory);

        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <param name="contract">The contract.</param>
        /// <typeparam name="TContract">The service type.</typeparam>
        void AddService<TContract>(Func<TContract> instanceFactory, string contract);

        /// <summary>
        /// Adds a single instance service.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <typeparam name="TContract">The contract type.</typeparam>
        void AddSingleton<TContract>(TContract instance);

        /// <summary>
        /// Adds a single instance service.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="contract">The contract.</param>
        /// <typeparam name="TContract">The contract type.</typeparam>
        void AddSingleton<TContract>(TContract instance, string contract);

        /// <summary>
        /// Adds a single instance service.
        /// </summary>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <typeparam name="TContract">The contract type.</typeparam>
        void AddSingleton<TContract>(Func<TContract> instanceFactory);

        /// <summary>
        /// Adds a single instance service.
        /// </summary>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <param name="contract">The contract.</param>
        /// <typeparam name="TContract">The contract type.</typeparam>
        void AddSingleton<TContract>(Func<TContract> instanceFactory, string contract);

        /// <summary>
        /// Adds a lazy single instance service.
        /// </summary>
        /// <param name="lazy">The lazy instance.</param>
        /// <typeparam name="TContract">The contract type.</typeparam>
        void AddLazySingleton<TContract>(Lazy<TContract> lazy);

        /// <summary>
        /// Adds a lazy single instance service.
        /// </summary>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <param name="threadSafetyMode">The thread safety mode.</param>
        /// <typeparam name="TContract">The contract type.</typeparam>
        void AddLazySingleton<TContract>(Func<TContract> instanceFactory, LazyThreadSafetyMode threadSafetyMode);

        /// <summary>
        /// Adds a lazy single instance service.
        /// </summary>
        /// <param name="lazy">The lazy instance.</param>
        /// <param name="contract">The contract.</param>
        /// <typeparam name="TContract">The contract type.</typeparam>
        void AddLazySingleton<TContract>(Lazy<TContract> lazy, string contract);

        /// <summary>
        /// Adds a lazy single instance service.
        /// </summary>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="threadSafetyMode">The thread safety mode.</param>
        /// <typeparam name="TContract">The contract type.</typeparam>
        void AddLazySingleton<TContract>(Func<TContract> instanceFactory, string contract, LazyThreadSafetyMode threadSafetyMode);
    }
}
