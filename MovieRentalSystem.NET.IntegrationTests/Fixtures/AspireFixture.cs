using Aspire.Hosting;
using Aspire.Hosting.Testing;
using k8s.ClientSets;
using Projects;

namespace MovieRentalSystem.NET.IntegrationTests.Fixtures;

public class AspireFixture : IAsyncLifetime
{
    private static readonly TimeSpan StartupTimeout = TimeSpan.FromSeconds(30);

    private DistributedApplication _app = null!;

    public HttpClient ApiClient { get; private set; } = null!;
    public async Task InitializeAsync()
    {
        var builder = await DistributedApplicationTestingBuilder.CreateAsync<MovieRentalSystem_NET_AppHost>();

        _app = await builder.BuildAsync();

        await _app.StartAsync();

        await _app.ResourceNotifications
            .WaitForResourceHealthyAsync("api")
            .WaitAsync(StartupTimeout);

        ApiClient = _app.CreateHttpClient("api");
    }

    public async Task DisposeAsync()
    {
        ApiClient.Dispose();
        await _app.DisposeAsync();
    }
}
