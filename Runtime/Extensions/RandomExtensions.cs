using System;

namespace WhaleTee.Runtime.Extensions {
  public static class RandomExtensions {
    /// <summary>
    /// Returns a random ulong.
    /// </summary>
    /// <param name="random">A random number generator.</param>
    /// <returns>A 64-bit unsigned integer that is greater than or equal to 0 and less than MaxValue.</returns>
    public static ulong NextULong(this Random random) {
      if (random is null)
        throw new ArgumentNullException(nameof(random));

      var buffer = new byte[8];
      random.NextBytes(buffer);
      return BitConverter.ToUInt64(buffer, 0);
    }

    /// <summary>
    /// Returns a random ulong that is less than the specified maximum.
    /// </summary>
    /// <param name="rand">A random number generator.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to 0.</param>
    /// <returns>A 64-bit unsigned integer that is greater than or equal to 0 and less than maxValue; that is, the range of return values ordinarily inclueds 0 but not maxValue. However, if maxValue equals 0, maxValue is return.</returns>
    public static ulong NextULong(this Random rand, ulong maxValue) => rand.NextULong(ulong.MinValue, maxValue);

    /// <summary>
    /// Returns a random ulong that is within a specified range.
    /// </summary>
    /// <param name="rand">A random number generator.</param>
    /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
    /// <returns>A 64-bit unsigned integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
    public static ulong NextULong(this Random rand, ulong minValue, ulong maxValue) {
      if (rand is null)
        throw new ArgumentNullException(nameof(rand));

      if (minValue > maxValue) {
        throw new ArgumentOutOfRangeException(nameof(minValue), minValue, $"'{minValue}' must be smaller than or equal to {maxValue}.");
      }

      if (minValue == maxValue) return minValue;

      var range = maxValue - minValue;
      var bias = ulong.MaxValue - ulong.MaxValue % range;
      ulong result;

      while ((result = rand.NextULong()) >= bias) { }

      return result % range + minValue;
    }

    /// <summary>
    /// Returns a non-negative random long.
    /// </summary>
    /// <param name="rand">A random number generator.</param>
    /// <returns>A 64-bit signed integer that is greater than or equal to 0 and less than MaxValue.</returns>
    public static long NextLong(this Random rand) => (long)rand.NextULong(long.MaxValue);

    /// <summary>
    /// Returns a non-negative random long that is less than the specified maximum.
    /// </summary>
    /// <param name="rand">A random number generator.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to 0.</param>
    /// <returns>A 64-bit signed integer that is greater than or equal to 0 and less than maxValue; that is, the range of return values ordinarily inclueds 0 but not maxValue. However, if maxValue equals 0, maxValue is return.</returns>
    public static long NextLong(this Random rand, long maxValue) {
      if (maxValue < 0)
        throw new ArgumentOutOfRangeException(nameof(maxValue), maxValue, $"'{maxValue}' must be greater than 0.");

      return (long)rand.NextULong((ulong)maxValue);
    }

    /// <summary>
    /// Returns a random long that is within a specified range.
    /// </summary>
    /// <param name="rand">A random number generator.</param>
    /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
    /// <param name="maxValue">The exclusive upper bound of the random number returned. maxValue must be greater than or equal to minValue.</param>
    /// <returns>A 64-bit signed integer greater than or equal to minValue and less than maxValue; that is, the range of return values includes minValue but not maxValue. If minValue equals maxValue, minValue is returned.</returns>
    public static long NextLong(this Random rand, long minValue, long maxValue) {
      if (rand is null)
        throw new ArgumentNullException(nameof(rand));

      if (minValue > maxValue) {
        throw new ArgumentOutOfRangeException(nameof(minValue), minValue, $"'{minValue}' must be smaller than or equal to {maxValue}.");
      }

      if (minValue == maxValue) return minValue;

      var min = minValue < 0 ? (ulong)(minValue - long.MinValue) : (ulong)minValue + long.MaxValue + 1;
      var max = maxValue < 0 ? (ulong)(maxValue - long.MinValue) : (ulong)maxValue + long.MaxValue + 1;
      var result = rand.NextULong(min, max);
      return result >= (ulong)long.MaxValue + 1 ? (long)(result - long.MaxValue) - 1 : long.MaxValue + (long)result;
    }
  }
}