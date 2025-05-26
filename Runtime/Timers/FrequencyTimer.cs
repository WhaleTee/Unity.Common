using System;
using UnityEngine;

namespace WhaleTee.Runtime.Timers {
  /// <summary>
  /// Timer that ticks at a specific frequency. (N times per second)
  /// </summary>
  public class FrequencyTimer : Timer {
    public int ticksPerSecond { get; private set; }

    public Action onTick = delegate { };

    private float timeThreshold;

    public FrequencyTimer(int ticksPerSecond) : base(0) {
      CalculateTimeThreshold(ticksPerSecond);
    }

    public override void Tick() {
      if (isRunning && currentTime >= timeThreshold) {
        currentTime -= timeThreshold;
        onTick.Invoke();
      }

      if (isRunning && currentTime < timeThreshold) {
        currentTime += Time.deltaTime;
      }
    }

    public override bool isFinished => !isRunning;

    public override void Reset() {
      currentTime = 0;
    }

    public void Reset(int newTicksPerSecond) {
      CalculateTimeThreshold(newTicksPerSecond);
      Reset();
    }

    void CalculateTimeThreshold(int ticksPerSecond) {
      this.ticksPerSecond = ticksPerSecond;
      timeThreshold = 1f / this.ticksPerSecond;
    }
  }
}