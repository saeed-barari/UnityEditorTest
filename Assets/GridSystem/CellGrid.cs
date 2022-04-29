using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace.GridSystem
{
    [CreateAssetMenu(fileName = "Grid", menuName = "Grid")]
    public class CellGrid : ScriptableObject
    {
        public List<CellRow> cellRows;

        [ContextMenu("Cells Set")]
        public void SetCells()
        {
            cellRows = new List<CellRow>()
            {
                new CellRow() {cells = new List<Cell>() {new Cell(), new Cell(), new Cell()}},
                new CellRow() {cells = new List<Cell>() {new Cell(), new Cell(), new Cell()}},
                new CellRow() {cells = new List<Cell>() {new Cell(), new Cell(), new Cell()}}
            };
        }
    }
}