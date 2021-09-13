// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ReactiveMarbles.Locator
{
    /// <summary>
    /// Default implementation of <see cref="IServiceLocator"/>.
    /// </summary>
    internal sealed class DefaultServiceLocator : IServiceLocator
    {
        private enum ContainerScope
        {
            Transient,
            Singleton
        }

        /// <inheritdoc />
        public T GetService<T>() => Container<T>.Get();

        /// <inheritdoc />
        public T GetService<T>(string contract) => ContractContainer<T>.Get(contract);

        /// <inheritdoc />
        public IEnumerable<T> GetServices<T>() => Container<T>.GetAll();

        /// <inheritdoc />
        public bool TryGetService<T>(out T service) =>
            Container<T>.Items.TryPeek(out service);

        /// <inheritdoc />
        public bool TryGetService<T>(string contract, out T service) =>
            GetContractContainer<T>(contract).TryPeek(out service);

        /// <inheritdoc />
        public bool HasService<T>() =>
            !Container<T>.Items.IsEmpty ||
            !Container<T>.Factories.IsEmpty ||
            !Container<T>.Lazy.IsEmpty;

        /// <inheritdoc />
        public IEnumerable<T> GetServices<T>(string contract) =>
            ContractContainer<T>.GetAll(contract);

        /// <inheritdoc />
        public bool HasService<T>(string contract) =>
            !GetContractContainer<T>(contract).IsEmpty ||
            !GetFactoryContractContainer<T>(contract).IsEmpty ||
            !GetLazyContractContainer<T>(contract).IsEmpty;

        /// <inheritdoc />
        public void AddService<T>(Func<T> instanceFactory) =>
            Container<T>.Add(instanceFactory, ContainerScope.Transient);

        /// <inheritdoc />
        public void AddService<T>(Func<T> instanceFactory, string contract) =>
            ContractContainer<T>.Add(instanceFactory, contract, ContainerScope.Transient);

        /// <inheritdoc />
        public void AddSingleton<TContract>(TContract instance) =>
            Container<TContract>.Add(instance, ContainerScope.Singleton);

        /// <inheritdoc />
        public void AddSingleton<TContract>(TContract instance, string contract) =>
            ContractContainer<TContract>.Add(instance, contract, ContainerScope.Singleton);

        /// <inheritdoc />
        public void AddSingleton<TContract>(Func<TContract> instanceFactory) =>
            Container<TContract>.Add(instanceFactory, ContainerScope.Singleton);

        /// <inheritdoc />
        public void AddSingleton<TContract>(Func<TContract> instanceFactory, string contract) =>
            ContractContainer<TContract>.Add(instanceFactory, contract, ContainerScope.Singleton);

        /// <inheritdoc />
        public void AddLazySingleton<TContract>(Lazy<TContract> lazy) =>
            Container<TContract>.AddLazy(lazy, ContainerScope.Singleton);

        /// <inheritdoc />
        public void AddLazySingleton<TContract>(Func<TContract> instanceFactory, LazyThreadSafetyMode threadSafetyMode) =>
            Container<TContract>.AddLazy(instanceFactory, threadSafetyMode, ContainerScope.Singleton);

        /// <inheritdoc />
        public void AddLazySingleton<TContract>(Func<TContract> instanceFactory, string contract, LazyThreadSafetyMode threadSafetyMode) =>
            ContractContainer<TContract>.AddLazy(instanceFactory, contract, threadSafetyMode, ContainerScope.Singleton);

        /// <inheritdoc />
        public void AddLazySingleton<TContract>(Lazy<TContract> lazy, string contract) =>
            ContractContainer<TContract>.AddLazy(lazy, contract, ContainerScope.Singleton);

        private static ConcurrentStack<T> GetContractContainer<T>(string contract) =>
            ContractContainer<T>.Instances.GetOrAdd(contract, _ => new ConcurrentStack<T>());

        private static ConcurrentStack<Lazy<T>> GetLazyContractContainer<T>(string contract) =>
            ContractContainer<T>.Lazies.GetOrAdd(contract, _ => new ConcurrentStack<Lazy<T>>());

        private static ConcurrentStack<Func<T>> GetFactoryContractContainer<T>(string contract) =>
            ContractContainer<T>.Factories.GetOrAdd(contract, _ => new ConcurrentStack<Func<T>>());

        private static ConcurrentStack<T> ApplyScope<T>(ConcurrentStack<T> stack, ContainerScope scope)
        {
            if (scope == ContainerScope.Singleton)
            {
                stack.Clear();
            }

            return stack;
        }

        private static class Container<T>
        {
            public static ConcurrentStack<T> Items { get; } = new();

            public static ConcurrentStack<Lazy<T>> Lazy { get; } = new();

            public static ConcurrentStack<Func<T>> Factories { get; } = new();

            public static void Add(T service, ContainerScope scope) => ApplyScope(Items, scope).Push(service);

            public static void Add(Func<T> service, ContainerScope scope) => ApplyScope(Factories, scope).Push(service);

            public static void AddLazy(Func<T> factory, LazyThreadSafetyMode threadSafetyMode, ContainerScope scope) =>
                ApplyScope(Lazy, scope).Push(new Lazy<T>(factory, threadSafetyMode));

            public static void AddLazy(Lazy<T> lazy, ContainerScope scope) => ApplyScope(Lazy, scope).Push(lazy);

            public static T Get()
            {
                if (Lazy.TryPop(out var lazy))
                {
                    Items.Push(lazy.Value);
                    return lazy.Value;
                }

                if (Factories.TryPeek(out var factory))
                {
                    return factory();
                }

                if (Items.TryPeek(out var service))
                {
                    return service;
                }

                throw new InvalidOperationException("No service for the provided type exists.");
            }

            public static IEnumerable<T> GetAll()
            {
                if (!Lazy.IsEmpty)
                {
                    Items.PushRange(Lazy.Select(x => x.Value).ToArray());
                    Lazy.Clear();
                }

                return Items.Concat(Factories.Select(x => x.Invoke()));
            }
        }

        private static class ContractContainer<T>
        {
            public static ConcurrentDictionary<string, ConcurrentStack<T>> Instances { get; } = new();

            public static ConcurrentDictionary<string, ConcurrentStack<Lazy<T>>> Lazies { get; } = new();

            public static ConcurrentDictionary<string, ConcurrentStack<Func<T>>> Factories { get; } = new();

            public static void Add(T service, string contract, ContainerScope scope) =>
                Instances.AddOrUpdate(
                    contract,
                    _ => InitializeConcurrentStack(service, new ConcurrentStack<T>()),
                    (_, stack) => UpdateConcurrentStack(service, scope, stack));

            public static void Add(Func<T> service, string contract, ContainerScope scope) =>
                Factories.AddOrUpdate(
                    contract,
                    _ => InitializeConcurrentStack(service, new ConcurrentStack<Func<T>>()),
                    (_, stack) => UpdateConcurrentStack(service, scope, stack));

            public static void AddLazy(Func<T> factory, string contract, LazyThreadSafetyMode threadSafetyMode, ContainerScope scope) =>
                Lazies.AddOrUpdate(
                    contract,
                    _ => InitializeConcurrentStack(new Lazy<T>(factory, threadSafetyMode), new ConcurrentStack<Lazy<T>>()),
                    (_, stack) => UpdateConcurrentStack(new Lazy<T>(factory, threadSafetyMode), scope, stack));

            public static void AddLazy(Lazy<T> lazy, string contract, ContainerScope scope) =>
                Lazies.AddOrUpdate(
                    contract,
                    _ => InitializeConcurrentStack(lazy, new ConcurrentStack<Lazy<T>>()),
                    (_, stack) => UpdateConcurrentStack(lazy, scope, stack));

            public static T Get(string contract)
            {
                if (GetLazyContractContainer<T>(contract).TryPop(out var lazy))
                {
                    GetContractContainer<T>(contract).Push(lazy.Value);
                    return lazy.Value;
                }

                if (GetFactoryContractContainer<T>(contract).TryPeek(out var factory))
                {
                    return factory();
                }

                if (GetContractContainer<T>(contract).TryPeek(out var service))
                {
                    return service;
                }

                throw new InvalidOperationException("No service for the provided type exists.");
            }

            public static IEnumerable<T> GetAll(string contract)
            {
                if (!GetLazyContractContainer<T>(contract).IsEmpty)
                {
                    GetContractContainer<T>(contract).PushRange(Lazies[contract].Select(x => x.Value).ToArray());
                    Lazies[contract].Clear();
                }

                return GetContractContainer<T>(contract)
                    .Concat(GetFactoryContractContainer<T>(contract).Select(x => x.Invoke()));
            }

            private static ConcurrentStack<TType> InitializeConcurrentStack<TType>(TType service, ConcurrentStack<TType> concurrentStack)
            {
                concurrentStack.Push(service);
                return concurrentStack;
            }

            private static ConcurrentStack<TType> UpdateConcurrentStack<TType>(TType lazy, ContainerScope scope, ConcurrentStack<TType> stack)
            {
                ApplyScope(stack, scope).Push(lazy);
                return stack;
            }
        }
    }
}
