using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using WhaleTee.Runtime.Extensions;

namespace WhaleTee.Reactive.Runtime.MVVM.Binders.MethodBinders {
  [RequireComponent(typeof(Button))]
  public class ButtonMethodBinder : EmptyMethodBinder {
    [SerializeField] private Button button;

    private IViewModel viewModel;
    private MethodInfo cachedMethod;

    private void OnEnable() {
      #if UNITY_EDITOR
      if (!Application.isPlaying) {
        return;
      }
      #endif
      button.onClick.AddListener(OnClick);
    }

    private void OnDisable() {
      #if UNITY_EDITOR
      if (!Application.isPlaying) {
        return;
      }
      #endif

      button.onClick.RemoveListener(OnClick);
    }

    protected override IDisposable BindInternal(IViewModel viewModel) {
      this.viewModel = viewModel;
      cachedMethod = viewModel.GetType().GetMethod(MethodName);

      return base.BindInternal(viewModel);
    }

    private void OnClick() => cachedMethod.Invoke(viewModel, null);

    #if UNITY_EDITOR
    private void Reset() { if (button.OrNull() == null) button = GetComponent<Button>(); }
    #endif
  }
}