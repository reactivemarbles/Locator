// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading;

namespace ReactiveMarbles.Locator
{
    /// <summary>
    /// Service Locator.
    /// </summary>
    /// <seealso cref="System.IServiceProvider" />
    public sealed class ServiceLocator : IServiceLocator
    {
        private static IServiceLocator _current = new DefaultServiceLocator();

        /// <summary>
        /// Gets the current instance.
        /// </summary>
        /// <returns>Service Locator instance.</returns>
        public static IServiceLocator Current() => _current;

        /// <summary>
        /// Sets the decorated instance of the <see cref="IServiceLocator"/>.
        /// </summary>
        /// <param name="serviceLocator">The new instance.</param>
        public static void Set(IServiceLocator serviceLocator)
        {
            var locator = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));
            Interlocked.Exchange(ref _current, locator);
        }

        /// <inheritdoc/>
        public T GetService<T>() => _current.GetService<T>();

        /// <inheritdoc/>
        public T GetService<T>(string contract) => _current.GetService<T>(contract);

        /// <inheritdoc/>
        public IEnumerable<T> GetServices<T>() => _current.GetServices<T>();

        /// <inheritdoc/>
        public IEnumerable<T> GetServices<T>(string contract) => _current.GetServices<T>(contract);

        /// <inheritdoc/>
        public bool TryGetService<T>(out T service) => _current.TryGetService(out service);

        /// <inheritdoc/>
        public bool TryGetService<T>(string contract, out T service) => _current.TryGetService(contract, out service);

        /// <inheritdoc/>
        public bool HasService<T>() => _current.HasService<T>();

        /// <inheritdoc/>
        public bool HasService<T>(string contract) => _current.HasService<T>(contract);

        /// <inheritdoc/>
        public void AddService<T>(Func<T> instanceFactory) => _current.AddService(instanceFactory);

        /// <inheritdoc/>
        public void AddService<T>(Func<T> instanceFactory, string contract) =>
            _current.AddService(instanceFactory, contract);

        /// <inheritdoc/>
        public void AddSingleton<TContract>(TContract instance) =>
            _current.AddSingleton(instance);

        /// <inheritdoc/>
        public void AddSingleton<TContract>(TContract instance, string contract) =>
            _current.AddSingleton(instance, contract);

        /// <inheritdoc/>
        public void AddSingleton<TContract>(Func<TContract> instanceFactory) => _current.AddSingleton(instanceFactory);

        /// <inheritdoc/>
        public void AddSingleton<TContract>(Func<TContract> instanceFactory, string contract) =>
            _current.AddSingleton(instanceFactory, contract);

        /// <inheritdoc/>
        public void AddLazySingleton<TContract>(Lazy<TContract> lazy) => _current.AddLazySingleton(lazy);

        /// <inheritdoc/>
        public void AddLazySingleton<TContract>(Func<TContract> instanceFactory, LazyThreadSafetyMode threadSafetyMode) =>
            _current.AddLazySingleton(instanceFactory, threadSafetyMode);

        /// <inheritdoc/>
        public void AddLazySingleton<TContract>(Lazy<TContract> lazy, string contract) => _current.AddLazySingleton(lazy, contract);

        /// <inheritdoc/>
        public void AddLazySingleton<TContract>(Func<TContract> instanceFactory, string contract, LazyThreadSafetyMode threadSafetyMode) =>
            _current.AddLazySingleton(instanceFactory, contract, threadSafetyMode);
    }
}
