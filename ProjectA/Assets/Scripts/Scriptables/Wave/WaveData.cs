using System;
using System.Collections.Generic;
using ProjectA.Scriptables;
using UnityEngine;

namespace ProjectA.Data.Wave {
    
    [CreateAssetMenu(menuName = "ProjectA/Data/WaveData", fileName = "WaveData")]
    public class WaveData : ScriptableObject {

        public enum EntityType {
            DestructibleProp, HardProp, Enemy, Shooter, Boss,
            Reflective, HardProjectile, Linker, Linked, Explosive,
            Cloaking, FakeCloaking, ShieldBreaker,
            LaserUp, LaserMid, LaserDown 
        }

        public enum EntityPosition {
            Up, Middle, Down
        }

        [Serializable]
        public struct EntityInfo {
            public EntityType Type;
            public EntityPosition Position;
            public float TimeToNextEntity;
        }

        public bool IsTutorialWave;
        public float InitialTimeSpawn;
        public WavePrefabs WavePrefabs;
        
        public List<EntityInfo> EntityInfos = new List<EntityInfo>();
    }
}