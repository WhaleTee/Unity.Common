using UnityEngine.UIElements;

namespace WhaleTee.Runtime.Extensions {
  public static class VisualElementExtensions {
    public static void SetDisplay(this VisualElement element, bool display) {
      element.style.display = display ? DisplayStyle.Flex : DisplayStyle.None;
    }
  }
}