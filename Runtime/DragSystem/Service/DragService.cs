using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WhaleTee.Runtime.DragSystem.Component;

namespace WhaleTee.Runtime.DragSystem.Service {
  public sealed class DragService {
    private const int MAX_HIT_BUFFER = 8;

    private readonly LayerMask draggableLayer;
    private readonly Camera raycastCamera;
    private readonly int maxHits;
    private readonly Dictionary<DragComponent, RaycastHit> rayHits = new Dictionary<DragComponent, RaycastHit>();

    private DragComponent currentDrag;
    private Vector3 pointerScreenPosition;
    private Vector3 pointerHitPositionOnScreen;

    public DragService(LayerMask draggableLayer, int maxHits, Camera raycastCamera) {
      this.draggableLayer = draggableLayer;
      this.maxHits = maxHits;
      this.raycastCamera = raycastCamera;

      // EventBus<PointerDownEvent>.Register(new EventBinding<PointerDownEvent>(RaycastDraggables));
      // EventBus<PointerUpEvent>.Register(new EventBinding<PointerUpEvent>(InvokeDragEnd));
      // EventBus<PointerPositionEvent>.Register(new EventBinding<PointerPositionEvent>(OnPointerPosition));
    }

    /// <summary>
    /// Performs a raycast to detect draggable objects and raises an event with the detected objects.
    /// </summary>
    private void RaycastDraggables() {
      // Buffer to store raycast hits
      var hits = new RaycastHit[MAX_HIT_BUFFER];

      // Perform the raycast from the screen position
      UnityEngine.Physics.RaycastNonAlloc(raycastCamera.ScreenPointToRay(pointerScreenPosition), hits, float.MaxValue, draggableLayer);

      // Iterate through the hits, filter and order them by distance, and take up to maxHits
      foreach (var hit in hits.Where(hit => hit.collider).OrderBy(hit => hit.distance).Take(maxHits)) {
        // Try to get the DragComponent from the hit collider
        // if (ServiceLocator.ServiceLocator.For(hit.collider).TryGet(out DragComponent component)) {
        //   // Add the component and hit to the rayHits dictionary
        //   rayHits.TryAdd(component, hit);
        // }
      }

      // If any hits were detected, update the pointer hit position on screen
      if (rayHits.Count > 0) pointerHitPositionOnScreen = pointerScreenPosition;

      // Raise an event with the detected hit objects and pointer position
      // EventBus<RaycastBeforeDragBeginEvent>.Raise(
      //   new RaycastBeforeDragBeginEvent {
      //     hitObjects = rayHits.Keys.Select(component => component.GetTargetInstanceId()).ToArray(), pointerPosition = pointerHitPositionOnScreen
      //   }
      // );
    }

    /// <summary>
    /// Iterates through the ray hits and invokes the drag start event if a draggable component allows drag.
    /// </summary>
    private void InvokeDragStart() {
      // Find the first draggable component that allows dragging
      var (component, hit) = rayHits.FirstOrDefault(pair => pair.Key.IsDragAllowed());

      if (component != null) {
        currentDrag = component;

        // Raise the DragBeginEvent with the component's instance ID and hit details
        // EventBus<DragBeginEvent>.Raise(
        //   new DragBeginEvent {
        //     instanceId = component.GetTargetInstanceId(), pointerPosition = pointerHitPositionOnScreen, hitPoint = hit.point, hitNormal = hit.normal
        //   }
        // );

        // Clear the ray hits
        FlushRayHits();
      }
    }

    /// <summary>
    /// Invokes the drag event if a drag is currently in progress.
    /// </summary>
    private void InvokeDrag() {
      if (currentDrag != null) {
        // Raise the DragEvent with the current drag component's instance ID
        // EventBus<DragEvent>.Raise(new DragEvent { instanceId = currentDrag.GetTargetInstanceId() });
      }
    }

    /// <summary>
    /// Invokes the drag end event if a drag is currently in progress and clears the current drag.
    /// </summary>
    private void InvokeDragEnd() {
      if (currentDrag != null) {
        // Raise the DragEndEvent with the current drag component's instance ID
        // EventBus<DragEndEvent>.Raise(new DragEndEvent { instanceId = currentDrag.GetTargetInstanceId() });
      }

      // Clear the current drag
      currentDrag = null;
      FlushRayHits();
    }

    /// <summary>
    /// Clears the dictionary of ray hits.
    /// </summary>
    private void FlushRayHits() => rayHits.Clear();

    /// <summary>
    /// Updates the pointer screen position and invokes drag start and drag events.
    /// </summary>
    // private void OnPointerPosition(PointerPositionEvent ctx) {
    //   pointerScreenPosition = ctx.Value;
    //   InvokeDragStart();
    //   InvokeDrag();
    // }
  }
}