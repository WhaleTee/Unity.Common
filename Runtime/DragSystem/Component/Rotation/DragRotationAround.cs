using UnityEngine;

namespace WhaleTee.Runtime.DragSystem.Component.Rotation {
  public class DragRotationAround : DragRotation {
    private readonly Transform target;
    private readonly Transform relative;
    private readonly float speed;

    public DragRotationAround(
      Transform target, Transform relative,
      float speed, int targetInstanceId
    ) : base(targetInstanceId) {
      this.target = target;
      this.relative = relative;
      this.speed = speed;
    }

    protected override void Rotate() {
      var relativePosition = relative.position;
      target.RotateAround(relativePosition, relative.up, pointerDelta.x * speed * Time.deltaTime);
      target.RotateAround(relativePosition, relative.right, -pointerDelta.y * speed * Time.deltaTime);
      target.LookAt(relative, Vector3.up);
    }
  }
}