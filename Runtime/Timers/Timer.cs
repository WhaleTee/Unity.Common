using System;
using UnityEngine;

namespace WhaleTee.Runtime.Timers {
  public abstract class Timer : IDisposable {
    public float currentTime { get; protected set; }
    public bool isRunning { get; private set; }

    protected float initialTime;

    public float progress => Mathf.Clamp(currentTime / initialTime, 0, 1);

    public Action onTimerStart = delegate { };
    public Action onTimerStop = delegate { };

    private bool disposed;

    protected Timer(float value) {
      initialTime = value;
    }

    public void Start() {
      currentTime = initialTime;

      if (!isRunning) {
        isRunning = true;
        TimerManager.RegisterTimer(this);
        onTimerStart.Invoke();
      }
    }

    public void Stop() {
      if (isRunning) {
        isRunning = false;
        TimerManager.DeregisterTimer(this);
        onTimerStop.Invoke();
      }
    }

    public abstract void Tick();
    public abstract bool isFinished { get; }

    public void Resume() => isRunning = true;
    public void Pause() => isRunning = false;

    public virtual void Reset() => currentTime = initialTime;

    public virtual void Reset(float newTime) {
      initialTime = newTime;
      Reset();
    }

    /// <summary>
    /// Define destructor
    /// </summary>
    ~Timer() {
      Dispose(false);
    }

    // Call Dispose to ensure deregistration of the timer from the TimerManager
    // when the consumer is done with the timer or being destroyed
    public void Dispose() {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing) {
      if (disposed) return;

      if (disposing) {
        TimerManager.DeregisterTimer(this);
      }

      disposed = true;
    }
  }
}