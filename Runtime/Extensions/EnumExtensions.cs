using System.Collections;
using System.Linq;

namespace WhaleTee.Runtime.Extensions {
  public static class EnumExtensions {
    public static int CountEnumerable(this IEnumerable enumerable) {
      var count = 0;

      if (enumerable != null) {
        count += enumerable.Cast<object>().Count();
      }

      return count;
    }
  }
}