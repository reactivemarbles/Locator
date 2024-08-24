// Copyright (c) 2019-2024 ReactiveUI Association Incorporated. All rights reserved.
// ReactiveUI Association Incorporated licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace ReactiveMarbles.Locator.Register.SourceGenerator.Tests;

[UsesVerify]
public sealed class RegisterTests : TestBase
{
    public RegisterTests(ITestOutputHelper testOutputHelper)
        : base(testOutputHelper, "Register")
    {
    }

    [Theory]
    [InlineData("")]
    [InlineData("Test1")]
    [InlineData("Test2")]
    public Task LazyParameterConstantNotRegisteredLazyFail(string contract)
    {
        var arguments = string.IsNullOrWhiteSpace(contract) ?
            string.Empty :
            $"\"{contract}\"";

        var constantArguments = string.IsNullOrWhiteSpace(contract) ?
            "new Service1()" :
            $"new Service1(), \"{contract}\"";

        var source = @$"
using System;
using System.Threading;
using ReactiveMarbles.Locator;

namespace Test
{{
    public static class DIRegister
    {{
        static DIRegister()
        {{
            LocatorRegistrations.Register<ITest, TestConcrete>({arguments});
            LocatorRegistrations.RegisterConstant({constantArguments});
        }}
    }}

    public interface ITest {{ }}
    public class TestConcrete : ITest
    {{
        public TestConcrete(Lazy<Service1> service1)
        {{
        }}
    }}

    public class Service1 {{ }}
}}";

        return TestHelper.TestFail(source, contract);
    }

    [Theory]
    [InlineData("")]
    [InlineData("Test1")]
    [InlineData("Test2")]
    public Task LazyParameterNotRegisteredLazyFail(string contract)
    {
        var arguments = string.IsNullOrWhiteSpace(contract) ?
            string.Empty :
            $"\"{contract}\"";

        var source = @$"
using System;
using System.Threading;
using ReactiveMarbles.Locator;

namespace Test
{{
    public static class DIRegister
    {{
        static DIRegister()
        {{
            LocatorRegistrations.Register<ITest, TestConcrete>({arguments});
            LocatorRegistrations.Register<IService1, Service1>({arguments});
            LocatorRegistrations.Register<IService2, Service2>({arguments});
        }}
    }}

    public interface ITest {{ }}
    public class TestConcrete : ITest
    {{
        public TestConcrete(IService1 service1, Lazy<IService2> service)
        {{
        }}

        [DependencyInjectionProperty]
        public IServiceProperty1 ServiceProperty1 {{ get; set; }}

        [DependencyInjectionProperty]
        public IServiceProperty2 ServiceProperty2 {{ get; set; }}

        [DependencyInjectionProperty]
        internal IServiceProperty3 ServiceProperty3 {{ get; set; }}
    }}

    public interface IService1 {{ }}
    public class Service1 : IService1 {{ }}
    public interface IService2 {{ }}
    public class Service2 : IService2 {{ }}
    public interface IServiceProperty1 {{ }}
    public interface IServiceProperty2 {{ }}
    public interface IServiceProperty3 {{ }}
}}";

        return TestHelper.TestFail(source, contract);
    }
}