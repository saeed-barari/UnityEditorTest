using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(StringOptionsAttribute))]
    public class StringOptionsAttributeEditor : PropertyDrawer
    {
        public List<string> values;
        private GUIContent[] _displayedOptions;
        private MethodInfo _methodInfo;
        private Type _type;

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            RecreateValueList(property);

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            var selectedIndex = values.IndexOf(property.stringValue);

            int val = EditorGUI.Popup(position, label, selectedIndex, _displayedOptions);
            
            if (EditorGUI.EndChangeCheck())
            {
                property.stringValue = values[val];
                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndProperty();
        }

        private void RecreateValueList(SerializedProperty property)
        {
            var attr = attribute as StringOptionsAttribute;
            var targetObject = property.serializedObject.targetObject;
            if (attr != null)
            {
                _type ??= targetObject.GetType();
                _methodInfo ??= _type.GetMethod(attr.methodName, (BindingFlags) (-1));
                values = (List<string>) _methodInfo?.Invoke(targetObject, null);
                _displayedOptions = values!.ToArray().Select(_=>new GUIContent(_)).ToArray();
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) =>
            EditorGUI.GetPropertyHeight(property, label);
    }
}