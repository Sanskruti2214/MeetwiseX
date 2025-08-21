using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
public class MyBackgroundService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    public MyBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // Create a new scope
        using var scope = _serviceProvider.CreateScope();

        // Resolve your scoped service from the scope
        var bookingRepository = scope.ServiceProvider.GetRequiredService<IBookingRepository>();

        // Use the repository
        await bookingRepository.MarkExpiredBookingAsync();
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    
}