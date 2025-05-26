using UnityEngine;

namespace WhaleTee.Runtime.Timers {
  /// <summary>
  /// Timer that counts down from a specific value to zero.
  /// </summary>
  public class CountdownTimer : Timer {
    public CountdownTimer(float value) : base(value) { }

    public override void Tick() {
      if (isRunning && currentTime > 0) {
        currentTime -= Time.deltaTime;
      }

      if (isRunning && currentTime <= 0) {
        Stop();
      }
    }

    public override bool isFinished => currentTime <= 0;
  }
}