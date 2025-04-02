using System;
using UnityEngine;

namespace WhaleTee.Runtime.DragSystem.Component.Rotation {
  public class XDragRotation : DragRotation {
    private readonly Transform target;
    private readonly Transform relative;
    private readonly float speed;

    public XDragRotation(
      Transform target, Transform relative,
      float speed, int targetInstanceId, Func<bool> condition = null
    ) : base(targetInstanceId, condition) {
      this.target = target;
      this.relative = relative;
      this.speed = speed;
    }

    protected override void Rotate() {
      target.Rotate(relative ? relative.up : Vector3.up, Vector3.Dot(pointerDelta * (speed * Time.deltaTime), Vector3.left), Space.World);
    }
  }
}