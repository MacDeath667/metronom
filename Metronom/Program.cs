using System;
using System.Threading;

class TimerExample
{
    static void Main()
    {
        var autoEvent = new AutoResetEvent(false);          // Create an AutoResetEvent to signal the timeout threshold in the timer callback has been reached.
        var statusChecker = new StatusChecker(5);
        var stateTimer = new Timer(statusChecker.CheckStatus, autoEvent, 1500, 1000);
        autoEvent.WaitOne();         // When autoEvent signals, change the period to every half second.
        stateTimer.Dispose();
    }
}

class StatusChecker
{
    private int invokeCount;
    private int maxCount;

    public StatusChecker(int count)
    {
        invokeCount = 0;
        maxCount = count;
    }

    public void CheckStatus(Object stateInfo)                       // This method is called by the timer delegate.
    {
        AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;
        Console.WriteLine("{0} Checking status {1,2}.", 
            DateTime.Now.ToString("h:mm:ss.fff"), (++invokeCount).ToString());
        Console.Beep();
        if (invokeCount == maxCount)
        {
            // Reset the counter and signal the waiting thread.
            invokeCount = 0;
            autoEvent.Set();
        }
    }
}