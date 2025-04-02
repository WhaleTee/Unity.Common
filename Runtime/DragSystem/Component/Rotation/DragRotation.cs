using System;
using UnityEngine;

namespace WhaleTee.Runtime.DragSystem.Component.Rotation {
  public abstract class DragRotation {
    private readonly int targetInstanceId;
    protected Vector2 pointerDelta { get; private set; }

    protected DragRotation(int targetInstanceId, Func<bool> condition = null) {
      this.targetInstanceId = targetInstanceId;

      // EventBus<PointerPositionDeltaEvent>.Register(new EventBinding<PointerPositionDeltaEvent>(ctx => pointerDelta = ctx.Value));

      // EventBus<DragEvent>.Register(
      //   new EventBinding<DragEvent>(
      //     ctx => {
      //       if (ctx.instanceId == GetTargetInstanceId() && (condition == null || condition.Invoke())) Rotate();
      //     }
      //   )
      // );
    }

    public int GetTargetInstanceId() => targetInstanceId;

    protected abstract void Rotate();
  }
}