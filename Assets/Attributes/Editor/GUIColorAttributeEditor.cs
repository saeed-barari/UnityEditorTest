using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(GUIColorAttribute))]
    public class GUIColorAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            var oldGUIColor = GUI.color;
            GUI.color = ((GUIColorAttribute) attribute).color;
            EditorGUI.PropertyField(position, property, label);
            GUI.color = oldGUIColor;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}