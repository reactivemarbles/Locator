﻿//HintName: Splat.DI.Extensions.Registrations.SourceGenerated.cs

// <auto-generated />
namespace ReactiveMarbles.Locator
{
    internal static partial class LocatorRegistrations
    {
        static partial void SetupIOCInternal( ReactiveMarbles.Locator.IServiceLocator resolver) 
        {
            resolver.AddService<global::Test.ITest>(() => new global::Test.TestConcrete((global::Test.IService1)resolver.GetService<global::Test.IService1>(), (global::System.Lazy<global::Test.IService2>)resolver.GetService<global::System.Lazy<global::Test.IService2>>()){ ServiceProperty1=(global::Test.IServiceProperty1)resolver.GetService<global::Test.IServiceProperty1>(), ServiceProperty2=(global::Test.IServiceProperty2)resolver.GetService<global::Test.IServiceProperty2>(), ServiceProperty3=(global::Test.IServiceProperty3)resolver.GetService<global::Test.IServiceProperty3>()} , "Test2");
            resolver.AddService<global::Test.IService1>(() => new global::Test.Service1(), "Test2");
            resolver.AddLazySingleton<global::Test.IService2>(() => new global::Test.Service2(), "Test2");
        }
    }
}