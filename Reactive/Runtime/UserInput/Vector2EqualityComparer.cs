using System.Collections.Generic;
using UnityEngine;
using WhaleTee.Runtime.Extensions;

namespace WhaleTee.Reactive.Runtime.UserInput {
  public sealed class Vector2EqualityComparer : IEqualityComparer<Vector2> {
    public bool Equals(Vector2 x, Vector2 y) => x.Approximately(y, .001f);
    public int GetHashCode(Vector2 obj) => obj.GetHashCode();
  }
}