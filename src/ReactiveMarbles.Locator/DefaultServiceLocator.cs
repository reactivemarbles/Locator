// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;

namespace ReactiveMarbles.Locator;

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
    public bool TryGetService<T>([MaybeNullWhen(false)]out T service) => Container<T>.TryGet(out service);

    /// <inheritdoc />
    public bool TryGetService<T>(string contract, [MaybeNullWhen(false)]out T service) =>
        ContractContainer<T>.TryGet(contract, out service);

    /// <inheritdoc />
    public Lazy<T> GetLazyService<T>() => Container<T>.GetLazy();

    /// <inheritdoc />
    public Lazy<T> GetLazyService<T>(string contract) => ContractContainer<T>.GetLazy(contract);

    /// <inheritdoc />
    public bool TryGetLazyService<T>([MaybeNullWhen(false)]out Lazy<T> service) => Container<T>.TryGetLazy(out service);

    /// <inheritdoc />
    public bool TryGetLazyService<T>(string contract, [MaybeNullWhen(false)] out Lazy<T> service) =>
        ContractContainer<T>.TryGetLazy(contract, out service);

    /// <inheritdoc />
    public bool HasService<T>() =>
        !Container<T>.Instances.IsEmpty ||
        !Container<T>.Factories.IsEmpty ||
        !Container<T>.Lazies.IsEmpty;

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

    /// <inheritdoc />
    public void AddLazySingleton<TContract>(Func<TContract> instanceFactory) =>
        Container<TContract>.AddLazy(instanceFactory, ContainerScope.Singleton);

    /// <inheritdoc />
    public void AddLazySingleton<TContract>(Func<TContract> instanceFactory, string contract) =>
        ContractContainer<TContract>.AddLazy(instanceFactory, contract, ContainerScope.Singleton);

    private static ConcurrentStack<T> GetContractContainer<T>(string contract) =>
        ContractContainer<T>.Instances.GetOrAdd(contract, _ => new());

    private static ConcurrentStack<Lazy<T>> GetLazyContractContainer<T>(string contract) =>
        ContractContainer<T>.Lazies.GetOrAdd(contract, _ => new());

    private static ConcurrentStack<Func<T>> GetFactoryContractContainer<T>(string contract) =>
        ContractContainer<T>.Factories.GetOrAdd(contract, _ => new());

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
        public static ConcurrentStack<T> Instances { get; } = new();

        public static ConcurrentStack<Lazy<T>> Lazies { get; } = new();

        public static ConcurrentStack<Func<T>> Factories { get; } = new();

        public static void Add(T service, ContainerScope scope) => ApplyScope(Instances, scope).Push(service);

        public static void Add(Func<T> service, ContainerScope scope) => ApplyScope(Factories, scope).Push(service);

        public static void AddLazy(Func<T> factory, ContainerScope scope) =>
            ApplyScope(Lazies, scope).Push(new(factory));

        public static void AddLazy(Func<T> factory, LazyThreadSafetyMode threadSafetyMode, ContainerScope scope) =>
            ApplyScope(Lazies, scope).Push(new(factory, threadSafetyMode));

        public static void AddLazy(Lazy<T> lazy, ContainerScope scope) => ApplyScope(Lazies, scope).Push(lazy);

        public static Lazy<T> GetLazy()
        {
            if (Lazies.TryPeek(out var lazy))
            {
                return lazy;
            }

            throw new InvalidOperationException("No service for the provided type exists.");
        }

        public static bool TryGetLazy([MaybeNullWhen(false)] out Lazy<T> instance)
        {
            var result = Lazies.TryPeek(out var lazy);

            instance = result ? lazy : default;

            return result;
        }

        public static T Get()
        {
            if (Lazies.TryPeek(out var lazy))
            {
                return lazy.Value;
            }

            if (Factories.TryPeek(out var factory))
            {
                return factory();
            }

            if (Instances.TryPeek(out var service))
            {
                return service;
            }

            throw new InvalidOperationException("No service for the provided type exists.");
        }

        public static bool TryGet([MaybeNullWhen(false)]out T instance)
        {
            if (Lazies.TryPeek(out var lazy))
            {
                instance = lazy.Value;
                return true;
            }

            if (Factories.TryPeek(out var factory))
            {
                instance = factory();
                return true;
            }

            return Instances.TryPeek(out instance);
        }

        public static IEnumerable<T> GetAll()
        {
            if (!Lazies.IsEmpty)
            {
                Instances.PushRange(Lazies.Select(x => x.Value).ToArray());
                Lazies.Clear();
            }

            return Instances.Concat(Factories.Select(x => x.Invoke()));
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
                _ => InitializeConcurrentStack(service, new()),
                (_, stack) => UpdateConcurrentStack(service, scope, stack));

        public static void Add(Func<T> service, string contract, ContainerScope scope) =>
            Factories.AddOrUpdate(
                contract,
                _ => InitializeConcurrentStack(service, new()),
                (_, stack) => UpdateConcurrentStack(service, scope, stack));

        public static void AddLazy(Func<T> factory, string contract, ContainerScope scope) =>
            Lazies.AddOrUpdate(
                contract,
                _ => InitializeConcurrentStack(new(factory), new ConcurrentStack<Lazy<T>>()),
                (_, stack) => UpdateConcurrentStack(new(factory), scope, stack));

        public static void AddLazy(Func<T> factory, string contract, LazyThreadSafetyMode threadSafetyMode, ContainerScope scope) =>
            Lazies.AddOrUpdate(
                contract,
                _ => InitializeConcurrentStack(new(factory, threadSafetyMode), new ConcurrentStack<Lazy<T>>()),
                (_, stack) => UpdateConcurrentStack(new(factory, threadSafetyMode), scope, stack));

        public static void AddLazy(Lazy<T> lazy, string contract, ContainerScope scope) =>
            Lazies.AddOrUpdate(
                contract,
                _ => InitializeConcurrentStack(lazy, new()),
                (_, stack) => UpdateConcurrentStack(lazy, scope, stack));

        public static T Get(string contract)
        {
            if (GetLazyContractContainer<T>(contract).TryPeek(out var lazy))
            {
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

        public static bool TryGet(string contract, [MaybeNullWhen(false)] out T instance)
        {
            if (GetLazyContractContainer<T>(contract).TryPeek(out var lazy))
            {
                instance = lazy.Value;
                return true;
            }

            if (GetFactoryContractContainer<T>(contract).TryPeek(out var factory))
            {
                instance = factory();
                return true;
            }

            return GetContractContainer<T>(contract).TryPeek(out instance);
        }

        public static Lazy<T> GetLazy(string contract)
        {
            if (GetLazyContractContainer<T>(contract).TryPeek(out var lazy))
            {
                return lazy;
            }

            throw new InvalidOperationException("No service for the provided type exists.");
        }

        public static bool TryGetLazy(string contract, [MaybeNullWhen(false)] out Lazy<T> instance)
        {
            var result = GetLazyContractContainer<T>(contract).TryPeek(out var lazy);

            instance = result ? lazy : default;

            return result;
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
