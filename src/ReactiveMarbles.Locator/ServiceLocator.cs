// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;

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
        public static void Set(IServiceLocator serviceLocator) => _current = serviceLocator ?? throw new ArgumentNullException(nameof(serviceLocator));

        /// <inheritdoc />
        public object GetService(Type serviceType) =>
            _current.GetService(serviceType);

        /// <inheritdoc />
        public object GetService(Type serviceType, string? contract) =>
            _current.GetService(serviceType, contract);

        /// <inheritdoc />
        public object[] GetServices(Type serviceType, string? contract = null) =>
            _current.GetServices(serviceType, contract);

        /// <inheritdoc />
        public void AddService(Func<object?> instanceFactory, Type serviceType, string? contract = null) =>
            _current.AddService(instanceFactory, serviceType, contract);

        /// <inheritdoc />
        public bool HasService(Type serviceType, string? contract = null) =>
            _current.HasService(serviceType, contract);

        /// <inheritdoc />
        public void RemoveService(Type serviceType, string? contract = null) =>
            _current.RemoveService(serviceType, contract);

        /// <inheritdoc />
        public void RemoveServices(Type serviceType, string? contract = null) =>
            _current.RemoveServices(serviceType, contract);
    }
}
