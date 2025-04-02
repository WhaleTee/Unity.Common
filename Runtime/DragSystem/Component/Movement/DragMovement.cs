using UnityEngine;

namespace WhaleTee.Runtime.DragSystem.Component.Movement {
  public class DragMovement : DragComponent {
    private readonly Transform target;
    private readonly Camera raycastCamera;
    private Vector2 pointerPosition;
    private Vector2 pointerOffset;

    public bool moveX { get; set; }
    public bool moveY { get; set; }
    public float speed { get; set; }

    private Transform cameraTransform => raycastCamera.transform;
    private Vector2 transformScreenPosition => raycastCamera.WorldToScreenPoint(target.position);

    public DragMovement(Transform target, Camera raycastCamera, bool moveX, bool moveY) {
      this.target = target;
      this.raycastCamera = raycastCamera;
      this.moveX = moveX;
      this.moveY = moveY;

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
      if (targetPosition == transformScreenPosition) return;

      var velocity = (Vector2.Lerp(transformScreenPosition, targetPosition, speed * Time.deltaTime) - transformScreenPosition) * Time.deltaTime;
      if (moveX) target.Translate(velocity.x, 0, 0, cameraTransform);
      if (moveY) target.Translate(0, velocity.y, 0, cameraTransform);
    }

    public int GetTargetInstanceId() => target.gameObject.GetInstanceID();
    public bool IsDragAllowed() => true;
  }
}