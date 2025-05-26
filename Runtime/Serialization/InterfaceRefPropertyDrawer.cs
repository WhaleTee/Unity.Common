using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace WhaleTee.Runtime.Serialization {
  [CustomPropertyDrawer(typeof(InterfaceRef<>))]
  public class InterfaceRefPropertyDrawer : PropertyDrawer {
    private const string IMPLEMENTER_PROP = "implementer";

    #if UNITY_2022_2_OR_NEWER
    public override VisualElement CreatePropertyGUI(SerializedProperty property) {
      return new PropertyField(property.FindPropertyRelative(IMPLEMENTER_PROP), property.displayName);
    }
    #endif

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
      EditorGUI.PropertyField(position, property.FindPropertyRelative(IMPLEMENTER_PROP), label, true);
    }
  }
}