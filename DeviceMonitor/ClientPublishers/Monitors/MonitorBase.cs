using ClientPublishers.Events;
using ClientPublishers.Interfaces;

namespace ClientPublishers.Monitors;

public abstract class Monitor : IMonitor
{
    protected abstract string Topic { get; }
    public delegate void MeasureUpdateHandler(MeasureEventArgs e);

    public delegate Task AsyncEventHandler<in TMeasureEventArgs>(TMeasureEventArgs e);
    public event AsyncEventHandler<MeasureEventArgs>? OnMeasured;
    

    protected Monitor(Func<string?>? measurementFunction, int measurementInterval)
    {
        if (measurementFunction is null)
            return;
        
        RepeatingAction(measurementFunction.Invoke, measurementInterval, new CancellationTokenSource().Token);
    }
    
    private void RepeatingAction(Func<string?> measurementFunction, int seconds, CancellationToken token) 
    {
        Task.Run(async () => {
            while (!token.IsCancellationRequested)
            {
                var measure = measurementFunction();
                if (measure is not null)
                {
                    if (OnMeasured is not null)
                        await OnMeasured(new MeasureEventArgs(Topic, measure));
                }
                
                await Task.Delay(TimeSpan.FromSeconds(seconds), token);
            }
        }, token);
    }
}