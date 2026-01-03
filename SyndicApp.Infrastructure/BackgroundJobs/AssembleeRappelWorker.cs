using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SyndicApp.Application.Interfaces.Assemblees;

public class AssembleeRappelWorker : BackgroundService
{
    private readonly IServiceProvider _provider;

    public AssembleeRappelWorker(IServiceProvider provider)
    {
        _provider = provider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _provider.CreateScope();
            var service = scope.ServiceProvider
                .GetRequiredService<IRappelAssembleeService>();

            await service.ExecuterAsync();

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}
