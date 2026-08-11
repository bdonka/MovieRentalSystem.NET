using Aspire.Hosting;
using Aspire.Hosting.Testing;
using MovieRentalSystem.NET.IntegrationTests.Helpers;
using Projects;

namespace MovieRentalSystem.NET.IntegrationTests.Fixtures;

public class AspireFixture : IAsyncLifetime
{
    private static readonly TimeSpan StartupTimeout =
        TimeSpan.FromSeconds(120);

    private DistributedApplication _app = null!;

    public HttpClient ApiClient { get; private set; } = null!;

    public HttpClient CreateAnonymousClient()
    {
        var apiClient = _app.CreateHttpClient("api");

        var anonymousClient =
            new HttpClient(
                new HttpClientHandler
                {
                    UseCookies = false
                })
            {
                BaseAddress = apiClient.BaseAddress
            };

        apiClient.Dispose();

        return anonymousClient;
    }

    public async Task InitializeAsync()
    {
        var builder =
            await DistributedApplicationTestingBuilder
                .CreateAsync<MovieRentalSystem_NET_AppHost>();

        _app = await builder.BuildAsync();

        await _app.StartAsync();

        await _app.ResourceNotifications
            .WaitForResourceHealthyAsync("api")
            .WaitAsync(StartupTimeout);

        ApiClient = CreateClient();
    }

    public HttpClient CreateClient()
    {
        return _app.CreateHttpClient("api");
    }

    public AuthTestHelper CreateAuthHelper()
    {
        return new AuthTestHelper(ApiClient);
    }

    public AuthTestHelper CreateAuthHelper(HttpClient client)
    {
        return new AuthTestHelper(client);
    }

    public async Task DisposeAsync()
    {
        ApiClient.Dispose();

        await _app.DisposeAsync();
    }
}
