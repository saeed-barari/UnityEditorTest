using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Reflection;
using DefaultNamespace.GridSystem;
using UnityEditor;
using UnityEngine;

public class GridEditorWindow : EditorWindow
{
    [UnityEditor.MenuItem("Tools/Grid Editor")]
    private static void ShowWindow()
    {
        var window = GetWindow<GridEditorWindow>();
        window.titleContent = new UnityEngine.GUIContent("TITLE");
        window.Show();
    }

#region configs
    public static float MaxGridSelectionWidth = 100;
    public static float CellsLabelWidth = 40;
    public static float CellsSize = 140;
    public static float buttonThickness = 20;
#endregion
    
    CellGrid[] grids;
    int selectedGridIndex;
    CellGrid selectedGrid => grids[selectedGridIndex];
    
    private Vector2 gridCellsScrollPos;
    private Vector2Int selectedCellIndex;
    private Cell selectedCell => selectedGrid.cellRows[selectedCellIndex.x].cells[selectedCellIndex.y];

    private void LoadGrids()
    {
        var paths = AssetDatabase.FindAssets($"t:{nameof(CellGrid)}").Select(AssetDatabase.GUIDToAssetPath).ToArray();
        grids = new CellGrid[paths.Length];
        for (int i = 0; i < paths.Length; i++) grids[i] = AssetDatabase.LoadAssetAtPath<CellGrid>(paths[i]);
        selectedGridIndex = Mathf.Clamp(selectedGridIndex, 0, grids.Length - 1);
    }

    private void OnEnable()
    {
        LoadGrids();
    }


    private void OnGUI()
    {
        DrawGridSelection();
        GUILayout.BeginHorizontal();
        DrawGrid();
        DrawCellDetail();
        GUILayout.EndHorizontal();
    }

    private void DrawCellDetail()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Details", EditorStyles.boldLabel);
        if(GUILayout.Button("Set Int Cell"))
        {
            selectedGrid.cellRows[selectedCellIndex.x].cells[selectedCellIndex.y] = new IntCell();
        }
        if(GUILayout.Button("Set String Cell"))
        {
            selectedGrid.cellRows[selectedCellIndex.x].cells[selectedCellIndex.y] = new StringCell();
        }
        if(GUILayout.Button("Set Boolean Cell"))
        {
            selectedGrid.cellRows[selectedCellIndex.x].cells[selectedCellIndex.y] = new BooleanCell();
        }
        DrawCellContaining(selectedCell, true);
        GUILayout.EndVertical();
    }

    private void DrawGrid()
    {
        var oldWidth = EditorGUIUtility.labelWidth;
        EditorGUIUtility.labelWidth = CellsLabelWidth;
        
        GUILayout.BeginVertical(EditorStyles.helpBox);
        gridCellsScrollPos = GUILayout.BeginScrollView(gridCellsScrollPos, GUILayout.ExpandHeight(false));
        GUILayout.BeginHorizontal(GUILayout.ExpandHeight(false));
       
        GUILayout.BeginVertical();
        GUILayout.Space(buttonThickness+3);
        for (int i = 0; i < selectedGrid.cellRows.Count; i++)
        {
            if(GUILayout.Button("-",  GUILayout.Height(CellsSize), GUILayout.Width(buttonThickness)))
            {
                selectedGrid.cellRows.RemoveAt(i);
                Repaint();
                return;
            }
        }
        GUILayout.Space(buttonThickness);
        GUILayout.EndVertical();
        
        GUILayout.BeginVertical();
        
        GUILayout.BeginHorizontal();
        for (int i = 0; i < selectedGrid.cellRows[0].cells.Count; i++)
        {
            if(GUILayout.Button("-", GUILayout.Width(CellsSize), GUILayout.Height(buttonThickness)))
            {
                selectedGrid.cellRows.ForEach(_ => _.cells.RemoveAt(i));
                Repaint();
                return;
            }
        }
        GUILayout.EndHorizontal();
        
        for (int i = 0; i < selectedGrid.cellRows.Count; i++) 
        {
            GUILayout.BeginHorizontal();

            for (int j = 0; j < selectedGrid.cellRows[i].cells.Count; j++)
            {
                var rect = EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(CellsSize), GUILayout.Height(CellsSize));

                var selected = selectedCellIndex.x == i && selectedCellIndex.y == j;
                EditorGUI.BeginDisabledGroup(selected);
                if (GUI.Button(rect, ""))
                {
                    selectedCellIndex = new Vector2Int(i, j);
                }
                EditorGUI.EndDisabledGroup();
                
                EditorGUI.BeginDisabledGroup(!selected);
                if (selectedGrid.cellRows[i].cells[j] == null)
                {
                    GUILayout.Label("Empty", EditorStyles.centeredGreyMiniLabel);   
                }
                else
                {
                    DrawCellContaining(selectedGrid.cellRows[i].cells[j], false);
                }
                EditorGUI.EndDisabledGroup(); 
                
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }
        if (GUILayout.Button("+", GUILayout.Width(CellsSize * selectedGrid.cellRows[0].cells.Count), GUILayout.Height(buttonThickness)))
        {
            var cellsCount = selectedGrid.cellRows.Last().cells.Count;
            selectedGrid.cellRows.Add(new CellRow() {cells = new List<Cell>()});
            selectedGrid.cellRows[0].cells.ForEach(_=>selectedGrid.cellRows.Last().cells.Add(new Cell()));
        }
        GUILayout.EndVertical();
        
        GUILayout.BeginVertical();
        GUILayout.Space(buttonThickness+3);
        if (GUILayout.Button("+", GUILayout.Height(CellsSize * selectedGrid.cellRows.Count), GUILayout.Width(buttonThickness)))
        {
            selectedGrid.cellRows.ForEach(row=>row.cells.Add(new Cell()));
        }
        GUILayout.Space(buttonThickness);
        GUILayout.EndVertical();
        
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.EndScrollView();
        GUILayout.EndVertical();
        
        
        
        EditorGUIUtility.labelWidth = oldWidth;
    }

    private void DrawCellContaining(Cell cell, bool drawDetail)
    {
        if(cell is null) return;
        
        var fields = cell.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public);
        foreach (var field in fields)
        {
            var attributes = field.GetCustomAttributes(typeof(Cell.ExposedAttribute), true);
            if(attributes.Length > 0)
            {
                var val = field.GetValue(cell);
                var exposedAttribute = ((Cell.ExposedAttribute)attributes[0]);
                if(exposedAttribute.isDetail != drawDetail) continue;
                
                switch (exposedAttribute.valueType)
                {
                    case Cell.ExposedAttribute.ValueType.Float:
                        field.SetValue(cell, EditorGUILayout.FloatField(field.Name, (float)val));
                        break;
                    case Cell.ExposedAttribute.ValueType.String:
                        field.SetValue(cell, EditorGUILayout.TextField(field.Name, (string)val));
                        break;
                    case Cell.ExposedAttribute.ValueType.Int:
                        field.SetValue(cell, EditorGUILayout.IntField(field.Name, (int)val));
                        break;
                    case Cell.ExposedAttribute.ValueType.Boolean:
                        field.SetValue(cell, EditorGUILayout.Toggle(field.Name, (bool)val));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void DrawGridSelection()
    {
        GUILayout.BeginVertical(EditorStyles.helpBox);
        GUILayout.Label("Grid List", EditorStyles.boldLabel);
        selectedGridIndex = GUILayout.SelectionGrid(selectedGridIndex, grids.Select(_ => _.name).ToArray(),
            Mathf.Min(grids.Length, (int) (position.width / MaxGridSelectionWidth)));
        GUILayout.EndVertical();
    }
}