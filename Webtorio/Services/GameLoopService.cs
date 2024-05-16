namespace Webtorio.Services;

public class GameLoopService : BackgroundService
{
    private readonly IHostApplicationLifetime _lifetime;
    private readonly GameTickHandler _gameTickHandler;

    public GameLoopService(IHostApplicationLifetime lifetime, GameTickHandler gameTickHandler)
    {
        _lifetime = lifetime;
        _gameTickHandler = gameTickHandler;
    }
 
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
       if (!await WaitForAppStartup(_lifetime, cancellationToken))
            return;
 
        // Приложение запущено и готово к обработке запросов
        
        // Выполняем задачу пока не будет запрошена остановка приложения
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                await _gameTickHandler.OnGameTickAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
 
            await Task.Delay(1000, cancellationToken);
        }
 
        // Если нужно дождаться завершения очистки, но контролировать время,
        // то стоит предусмотреть в контракте использование CancellationToken.
        // await _someService.DoSomeCleanupAsync(stoppingToken);
    }
 
    static async Task<bool> WaitForAppStartup(IHostApplicationLifetime lifetime, CancellationToken stoppingToken)
    {
        // Создаём TaskCompletionSource для ApplicationStarted
        var startedSource = new TaskCompletionSource();
        using var reg1 = lifetime.ApplicationStarted.Register(() => startedSource.SetResult());
 
        // Создаём TaskCompletionSource для stoppingToken
        var cancelledSource = new TaskCompletionSource();
        using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());
 
        // Ожидаем любое из событий запуска или запроса на остановку
        Task completedTask = await Task.WhenAny(startedSource.Task, cancelledSource.Task).ConfigureAwait(false);
 
        // Если завершилась задача ApplicationStarted, возвращаем true, иначе false
        return completedTask == startedSource.Task;
    }
}