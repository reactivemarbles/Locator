﻿//HintName: Splat.DI.Extensions.Registrations.SourceGenerated.cs

// <auto-generated />
namespace ReactiveMarbles.Locator
{
    internal static partial class LocatorRegistrations
    {
        static partial void SetupIOCInternal(ReactiveMarbles.Locator.IServiceLocator resolver) 
        {
            resolver.AddLazySingleton<global::Test.ITest>(() => new global::Test.TestConcrete1(), "Test1");
            resolver.AddLazySingleton<global::Test.ITest>(() => new global::Test.TestConcrete2(), "Test1");
        }
    }
}