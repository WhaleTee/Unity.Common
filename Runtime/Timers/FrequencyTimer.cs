using System;
using UnityEngine;

namespace WhaleTee.Runtime.Timers {
  /// <summary>
  /// Timer that ticks at a specific frequency. (N times per second)
  /// </summary>
  public class FrequencyTimer : Timer {
    public int TicksPerSecond { get; private set; }

    public readonly Action onTick = delegate { };

    private float timeThreshold;

    public FrequencyTimer(int ticksPerSecond) : base(0) {
      CalculateTimeThreshold(ticksPerSecond);
    }

    public override void Tick() {
      if (IsRunning && CurrentTime >= timeThreshold) {
        CurrentTime -= timeThreshold;
        onTick.Invoke();
      }

      if (IsRunning && CurrentTime < timeThreshold) {
        CurrentTime += Time.deltaTime;
      }
    }

    public override bool IsFinished => !IsRunning;

    public override void Reset() {
      CurrentTime = 0;
    }

    public void Reset(int newTicksPerSecond) {
      CalculateTimeThreshold(newTicksPerSecond);
      Reset();
    }

    void CalculateTimeThreshold(int ticksPerSecond) {
      this.TicksPerSecond = ticksPerSecond;
      timeThreshold = 1f / this.TicksPerSecond;
    }
  }
}