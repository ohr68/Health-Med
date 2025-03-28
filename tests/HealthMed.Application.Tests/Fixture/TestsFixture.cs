using HealthMed.Application.Extensions;
using HealthMed.Application.Interfaces.Service;
using HealthMed.Application.Services;
using HealthMed.Domain.Extensions;
using HealthMed.Messaging.Extensions;
using HealthMed.ORM.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HealthMed.Application.Tests.Fixture;

[CollectionDefinition("Tests collection")]
public class TestsCollection : ICollectionFixture<TestsFixture>
{
}

public class TestsFixture : IDisposable
{
    public IServiceProvider ServiceProvider { get; }
    
    public TestsFixture()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        
        var services = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .Build();

        services
            .AddDomainLayer()    
            .AddApplicationLayer()
            .AddPersistenceLayer(configuration, false)
            .AddMessagingLayer(configuration);
        
        ServiceProvider = services.BuildServiceProvider();
    }

    public void Dispose()
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", null);
    }
}