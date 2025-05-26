using System;
using R3;
using UnityEngine;
using UnityEngine.InputSystem;
using WhaleTee.Runtime.Extensions;
using WhaleTee.Runtime.ServiceLocator;

namespace WhaleTee.Reactive.Runtime.Input {
  [Service]
  public sealed class UserInput : IDisposable {
    private static Camera MainCamera => Camera.main;
    private readonly UserInputActions inputActions = new UserInputActions();
    private DisposableBag subscriptions;
    public ReactiveProperty<bool> Click { get; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> RightClick { get; } = new ReactiveProperty<bool>();
    public ReactiveProperty<bool> MiddleClick { get; } = new ReactiveProperty<bool>();
    public ReactiveProperty<Vector2> MousePosition { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer());
    public ReactiveProperty<Vector2> MousePositionDeltaUpdate { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer());

    public ReactiveProperty<Vector2> MousePositionDeltaFixedUpdate { get; } =
      new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer());

    public ReactiveProperty<Vector2> ScrollWheel { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer());
    public ReactiveProperty<Vector2> ScrollWheelDeltaUpdate { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer());

    public ReactiveProperty<Vector2> ScrollWheelDeltaFixedUpdate { get; } =
      new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer());

    public ReactiveProperty<Vector2> Move { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer());
    public ReactiveProperty<Vector2> MoveUpdate { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer());
    public ReactiveProperty<Vector2> MoveFixedUpdate { get; } = new ReactiveProperty<Vector2>(Vector2.zero, new Vector2EqualityComparer());

    /// <summary>
    /// returns the mouse position in world space related to the main camera, if the camera is not found returns Vector3.zero
    /// </summary>
    public Vector3 GetMousePositionWorld() =>
    ScreenToWorldPoint(MainCamera, MousePosition.Value.With(z: MainCamera.transform.position.z)) ?? Vector3.zero;

    /// <summary>
    /// returns the mouse position in world space related to the main camera, with saved z position of te point, if the camera is not found returns Vector3.zero
    /// </summary>
    public Vector3 GetMousePositionWorldSaveZ(Vector3 point) {
      var mainTransform = MainCamera.transform;
      var mousePos = MousePosition.Value.With(z: mainTransform?.InverseTransformPoint(point).z);
      return ScreenToWorldPoint(MainCamera, mousePos) ?? Vector3.zero;
    }

    private static Vector3? ScreenToWorldPoint(Camera camera, Vector3 point) => camera.ScreenToWorldPoint(point);

    public static void WrapCursorPosition(Vector2 position) => Mouse.current.WarpCursorPosition(position);

    public void SetCursorVisible(bool visible) {
      // Cursor.lockState = !visible ? CursorLockMode.Locked : CursorLockMode.None;
      Cursor.visible = visible;
    }

    public void SetCursorVisible(bool visible, Vector2 atPoint) {
      Cursor.lockState = !visible ? CursorLockMode.Locked : CursorLockMode.None;
      Cursor.visible = visible;
      WrapCursorPosition(atPoint);
    }

    public UserInput() {
      inputActions.Enable();
      UpdateMouseProperties();
    }

    private void UpdateMouseProperties() {
      Observable.EveryUpdate(UnityFrameProvider.EarlyUpdate)
      .Subscribe(_ => {
                   Click.Value = inputActions.UI.Click.IsPressed();
                   RightClick.Value = inputActions.UI.Click.IsPressed();
                   MiddleClick.Value = inputActions.UI.Click.IsPressed();
                 }
      )
      .AddTo(ref subscriptions);

      Observable.FromEvent<InputAction.CallbackContext>(
        handler => inputActions.UI.ScrollWheel.performed += handler,
        handler => inputActions.UI.ScrollWheel.performed -= handler
      )
      .Subscribe(ctx => {
                   var value = ctx.ReadValue<Vector2>();
                   ScrollWheel.Value = value;
                   ScrollWheelDeltaUpdate.Value += value;
                   ScrollWheelDeltaFixedUpdate.Value += value;
                 }
      )
      .AddTo(ref subscriptions);

      Observable.FromEvent<InputAction.CallbackContext>(
        handler => inputActions.Player.Look.performed += handler,
        handler => inputActions.Player.Look.performed -= handler
      )
      .Subscribe(ctx => {
                   var value = ctx.ReadValue<Vector2>();
                   MousePositionDeltaUpdate.Value += value;
                   MousePositionDeltaFixedUpdate.Value += value;
                 }
      )
      .AddTo(ref subscriptions);

      Observable.EveryUpdate(UnityFrameProvider.PreLateUpdate)
      .Subscribe(_ => {
                   MousePositionDeltaUpdate.Value = Vector2.zero;
                   ScrollWheelDeltaUpdate.Value = Vector2.zero;
                   MoveUpdate.Value = Vector2.zero;
                 }
      )
      .AddTo(ref subscriptions);

      Observable.EveryUpdate(UnityFrameProvider.PostFixedUpdate)
      .Subscribe(_ => {
                   MousePositionDeltaFixedUpdate.Value = Vector2.zero;
                   ScrollWheelDeltaFixedUpdate.Value = Vector2.zero;
                   MoveFixedUpdate.Value = Vector2.zero;
                 }
      )
      .AddTo(ref subscriptions);

      Observable.FromEvent<InputAction.CallbackContext>(
        handler => inputActions.UI.Point.performed += handler,
        handler => inputActions.UI.Point.performed -= handler
      )
      .Subscribe(ctx => MousePosition.Value = ctx.ReadValue<Vector2>())
      .AddTo(ref subscriptions);

      Observable.FromEvent<InputAction.CallbackContext>(
        handler => inputActions.Player.Move.performed += handler,
        handler => inputActions.Player.Move.performed -= handler
      )
      .Subscribe(ctx => {
                   var value = ctx.ReadValue<Vector2>();
                   Move.Value = value;
                   MoveUpdate.Value += value;
                   MoveFixedUpdate.Value += value;
                 }
      )
      .AddTo(ref subscriptions);

      Observable.FromEvent<InputAction.CallbackContext>(
        handler => inputActions.Player.Move.canceled += handler,
        handler => inputActions.Player.Move.canceled -= handler
      )
      .Subscribe(_ => Move.Value = Vector2.zero)
      .AddTo(ref subscriptions);
    }

    public void Dispose() {
      subscriptions.Dispose();
      inputActions?.Disable();
      inputActions?.Dispose();
    }
  }
}