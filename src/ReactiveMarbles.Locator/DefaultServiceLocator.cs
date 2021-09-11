// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ReactiveMarbles.Locator
{
    /// <summary>
    /// Default implementation of <see cref="IServiceLocator"/>.
    /// </summary>
    internal sealed class DefaultServiceLocator : IServiceLocator
    {
        /// <inheritdoc />
        public T GetService<T>()
        {
            if (Container<T>.Items.TryPeek(out var service))
            {
                return service;
            }

            throw new InvalidOperationException("No service for the provided type exists.");
        }

        /// <inheritdoc />
        public T GetService<T>(string contract)
        {
            if (GetContractContainer<T>(contract).TryPeek(out var service))
            {
                return service;
            }

            throw new InvalidOperationException("No service for the provided type exists.");
        }

        public IEnumerable<T> GetServices<T>() => Container<T>.Items;

        /// <inheritdoc />
        public bool TryGetService<T>(out T service) =>
            Container<T>.Items.TryPeek(out service);

        /// <inheritdoc />
        public bool TryGetService<T>(string contract, out T service) =>
            GetContractContainer<T>(contract).TryPeek(out service);

        public bool HasService<T>() => Container<T>.Items.TryPeek(out _);

        /// <inheritdoc />
        public IEnumerable<T> GetServices<T>(string contract) =>
            GetContractContainer<T>(contract);

        /// <inheritdoc />
        public bool HasService<T>(string contract) =>
            !GetContractContainer<T>(contract).IsEmpty;

        /// <inheritdoc />
        public void AddService<T>(Func<T> instanceFactory) => Container<T>.Items.Push(instanceFactory());

        /// <inheritdoc />
        public void AddService<T>(Func<T> instanceFactory, string contract) => GetContractContainer<T>(contract).Push(instanceFactory());

        private static ConcurrentStack<T> GetContractContainer<T>(string contract) => ContractContainer<T>.Items.GetOrAdd(contract, _ => new ConcurrentStack<T>());

        private static class Container<T>
        {
            public static ConcurrentStack<T> Items { get; } = new();
        }

        private static class ContractContainer<T>
        {
            public static ConcurrentDictionary<string, ConcurrentStack<T>> Items { get; } = new();
        }
    }
}
