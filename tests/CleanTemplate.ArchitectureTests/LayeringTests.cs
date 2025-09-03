using NetArchTest.Rules;
using CleanTemplate.Domain.Entities;
using CleanTemplate.Application.Services;
using Xunit;

namespace CleanTemplate.ArchitectureTests;

public class LayeringTests
{
    [Fact]
    public void Domain_Should_Not_Depend_On_Other_Layers()
    {
        var result = Types.InAssembly(typeof(Order).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny("CleanTemplate.Application", "CleanTemplate.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, "Domain layer has forbidden dependencies.");
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types.InAssembly(typeof(OrderService).Assembly)
            .ShouldNot()
            .HaveDependencyOn("CleanTemplate.Infrastructure")
            .GetResult();

        Assert.True(result.IsSuccessful, "Application layer depends on Infrastructure.");
    }
}
