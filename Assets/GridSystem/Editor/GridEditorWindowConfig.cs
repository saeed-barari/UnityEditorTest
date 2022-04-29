using System;
using UnityEditor;
using UnityEngine;

namespace GridSystem.Editor
{
    public class GridEditorWindowConfig : EditorWindow
    {
        [MenuItem("Tools/Grid Editor Config")]
        private static void ShowWindow()
        {
            var window = GetWindow<GridEditorWindowConfig>();
            window.titleContent = new GUIContent("TITLE");
            window.Show();
        }

        private void OnGUI()
        {
            GridEditorWindow.MaxGridSelectionWidth =
                EditorGUILayout.FloatField(nameof(GridEditorWindow.MaxGridSelectionWidth), GridEditorWindow.MaxGridSelectionWidth);
            GridEditorWindow.CellsLabelWidth =
                EditorGUILayout.FloatField(nameof(GridEditorWindow.CellsLabelWidth), GridEditorWindow.CellsLabelWidth);
            GridEditorWindow.CellsSize =
                EditorGUILayout.FloatField(nameof(GridEditorWindow.CellsSize), GridEditorWindow.CellsSize);
            GridEditorWindow.buttonThickness =
                EditorGUILayout.FloatField(nameof(GridEditorWindow.buttonThickness), GridEditorWindow.buttonThickness);
        }
    }
}