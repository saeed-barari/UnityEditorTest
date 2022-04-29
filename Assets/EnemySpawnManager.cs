using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "Enemy Spawn Manager", menuName = "Spawn/Enemy Spawn Manager")]
    public class EnemySpawnManager : ScriptableObject
    {
        [System.Serializable]
        public class EnemySpawn
        {
            public Enemy enemy;
            public SpawnPoint[] spawnPoints;
            public float chance;
        }

        [System.Serializable]
        public class SpawnPoint
        {
            [GUIColor(0.4f, 0.8f, 1)]
            public Vector3 point;
            public enum PointType
            {
                Ground, Air, Dynamic
            }
            [GUIColor(1, 0.4f, 0.4f)]
            public PointType type;
        }


        public SpawnPoint[] spawnPoints;

        [GUIColor(0, 1, 0)] 
        public string[] stringList;
        private List<string> GetStringList() => stringList.ToList();
        
        [StringOptions(nameof(GetStringList))]
        public string value;

    }
}