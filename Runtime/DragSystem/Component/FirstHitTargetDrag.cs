namespace WhaleTee.Runtime.DragSystem.Component {
  /// <summary>
  /// Allow drag only if game object was first hit by raycast.
  /// </summary>
  public class FirstHitTargetDrag : DragComponent {
    private readonly int targetInstanceId;
    private bool dragAllowed;

    public FirstHitTargetDrag(int targetInstanceId) {
      this.targetInstanceId = targetInstanceId;
      // EventBus<RaycastBeforeDragBeginEvent>.Register(new EventBinding<RaycastBeforeDragBeginEvent>(OnRaycastBeforeDrag));
    }

    public int GetTargetInstanceId() => targetInstanceId;

    public bool IsDragAllowed() => dragAllowed;

    // private void OnRaycastBeforeDrag(RaycastBeforeDragBeginEvent ctx) {
    //   dragAllowed = ctx.hitObjects?.Length > 0 && ctx.hitObjects[0] == GetTargetInstanceId();
    // }
  }
}