// Copyright (c) 2019-2021 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace ReactiveMarbles.Locator
{
    /// <summary>
    /// Service Locator Mixins.
    /// </summary>
    public static class ServiceLocatorMixins
    {
        /// <summary>
        /// Adds the service.
        /// </summary>
        /// <typeparam name="TContract">The type of the interface.</typeparam>
        /// <typeparam name="TConcrete">The type of the concrete.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="contract">The contract.</param>
        public static void AddService<TContract, TConcrete>(this IEditServices serviceLocator, string contract)
            where TConcrete : TContract, new() =>
            serviceLocator.AddService<TContract>(() => new TConcrete(), contract);

        /// <summary>
        /// Adds the singleton.
        /// </summary>
        /// <typeparam name="TContract">The type used for registration.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="instance">The instance.</param>
        /// <param name="contract">The contract.</param>
        public static void AddSingleton<TContract>(this IEditServices serviceLocator, object instance, string contract) =>
            serviceLocator.AddService(() => (TContract)instance, contract);

        /// <summary>
        /// Adds the singleton.
        /// </summary>
        /// <typeparam name="TContract">The type of the interface.</typeparam>
        /// <typeparam name="TConcrete">The type of the concrete.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="contract">The contract.</param>
        public static void AddSingleton<TContract, TConcrete>(this IEditServices serviceLocator, string contract)
         where TConcrete : TContract, new()
        {
            var instance = new TConcrete();
            serviceLocator.AddService<TContract>(() => instance, contract);
        }

        /// <summary>
        /// Adds the lazy singleton.
        /// </summary>
        /// <typeparam name="TContract">The type of the interface.</typeparam>
        /// <typeparam name="TConcrete">The type of the concrete.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="contract">The contract.</param>
        public static void AddLazySingleton<TContract, TConcrete>(this IEditServices serviceLocator, string contract)
            where TConcrete : TContract, new()
        {
            TConcrete InstanceFactory() => new();
            var val = new Lazy<TConcrete>(InstanceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            serviceLocator.AddService<TContract>(() => val.Value, contract);
        }

        /// <summary>
        /// Adds the lazy singleton.
        /// </summary>
        /// <typeparam name="TContract">The type of the interface.</typeparam>
        /// <typeparam name="TConcrete">The type of the concrete.</typeparam>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="instanceFactory">The instance factory.</param>
        /// <param name="contract">The contract.</param>
        public static void AddLazySingleton<TContract, TConcrete>(this IEditServices serviceLocator, Func<TConcrete> instanceFactory, string contract)
            where TConcrete : TContract
        {
            var val = new Lazy<TConcrete>(instanceFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            serviceLocator.AddService<TContract>(() => val.Value, contract);
        }
    }
}
