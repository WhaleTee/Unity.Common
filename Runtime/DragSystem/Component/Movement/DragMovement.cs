using UnityEngine;

namespace WhaleTee.Runtime.DragSystem.Component.Movement {
  public class DragMovement : DragComponent {
    private readonly Transform target;
    private readonly Camera raycastCamera;
    private Vector2 pointerPosition;
    private Vector2 pointerOffset;

    public bool MoveX { get; set; }
    public bool MoveY { get; set; }
    public float Speed { get; set; }

    private Transform CameraTransform => raycastCamera.transform;
    private Vector2 TransformScreenPosition => raycastCamera.WorldToScreenPoint(target.position);

    public DragMovement(Transform target, Camera raycastCamera, bool moveX, bool moveY) {
      this.target = target;
      this.raycastCamera = raycastCamera;
      this.MoveX = moveX;
      this.MoveY = moveY;

      // EventBus<PointerPositionEvent>.Register(new EventBinding<PointerPositionEvent>(ctx => pointerPosition = ctx.Value));
      //
      // EventBus<DragBeginEvent>.Register(
      //   new EventBinding<DragBeginEvent>(
      //     ctx => {
      //       if (ctx.instanceId == GetTargetInstanceId()) pointerOffset = pointerPosition - (Vector2)raycastCamera.WorldToScreenPoint(target.position);
      //     }
      //   )
      // );
      //
      // EventBus<DragEvent>.Register(
      //   new EventBinding<DragEvent>(
      //     ctx => {
      //       if (ctx.instanceId == GetTargetInstanceId()) Move(pointerPosition - pointerOffset);
      //     }
      //   )
      // );
    }

    protected virtual void Move(Vector2 targetPosition) {
      if (targetPosition == TransformScreenPosition) return;

      var velocity = (Vector2.Lerp(TransformScreenPosition, targetPosition, Speed * Time.deltaTime) - TransformScreenPosition) * Time.deltaTime;
      if (MoveX) target.Translate(velocity.x, 0, 0, CameraTransform);
      if (MoveY) target.Translate(0, velocity.y, 0, CameraTransform);
    }

    public int GetTargetInstanceId() => target.gameObject.GetInstanceID();
    public bool IsDragAllowed() => true;
  }
}