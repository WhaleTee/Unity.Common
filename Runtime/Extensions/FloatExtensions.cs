using UnityEngine;

namespace WhaleTee.Runtime.Extensions {
  public static class FloatExtensions {
    
    /// <summary>
    /// Rounds the given float value to the nearest multiple of the specified value.
    /// </summary>
    /// <param name="value">The float value to be rounded.</param>
    /// <param name="roundTo">The multiple to which the value should be rounded.</param>
    /// <returns>The value rounded to the nearest multiple of the specified value.</returns>
    public static float RoundToNearestMultiple(this float value, float roundTo) => Mathf.Round(value / roundTo) * roundTo;
  }
}