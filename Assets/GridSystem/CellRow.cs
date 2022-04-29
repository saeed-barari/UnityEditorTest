using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace.GridSystem
{
    [System.Serializable]
    public class CellRow
    {
        [SerializeReference]
        public List<Cell> cells;
    }
}