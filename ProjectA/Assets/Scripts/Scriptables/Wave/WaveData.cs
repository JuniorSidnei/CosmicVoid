using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectA.Data.Wave {
    
    [CreateAssetMenu(menuName = "ProjectA/Data/WaveData", fileName = "WaveData")]
    public class WaveData : ScriptableObject {

        public enum EntityType {
            DestructibleProp, HardProp, Enemy, Shooter, Boss
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

        public float InitialTimeSpawn;
        public GameObject DestructibleProp;
        public GameObject HardProp;
        public GameObject Enemy;
        public GameObject Shooter;
        public GameObject Boss;
        
        public List<EntityInfo> EntityInfos = new List<EntityInfo>();

        public GameObject GetEntity(EntityType type) {
            return type switch {
                EntityType.DestructibleProp => DestructibleProp,
                EntityType.HardProp => HardProp,
                EntityType.Enemy => Enemy,
                EntityType.Shooter => Shooter,
                EntityType.Boss => Boss,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}