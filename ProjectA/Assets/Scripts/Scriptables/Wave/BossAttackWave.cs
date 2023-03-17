using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Data.Wave;
using UnityEngine;

namespace ProjectA.Scriptables.Boss {

    [CreateAssetMenu(menuName = "ProjectA/Data/Boss Attack Data", fileName = "BossAttackData")]
    public class BossAttackWave : ScriptableObject {

        public enum ProjectileType {
            Reflective,
            Hard,
            Explosive
        }
        
        [Serializable]
        public struct EntityInfo {
            public ProjectileType Type;
            public WaveData.EntityPosition Position;
            public float TimeToNextEntity;
        }
        
        public float InitialTimeSpawn;
        public GameObject Reflective;
        public GameObject Hard;
        public List<EntityInfo> EntityInfos = new List<EntityInfo>();
        
        public GameObject GetEntity(ProjectileType type) {
            return type switch {
                ProjectileType.Reflective => Reflective,
                ProjectileType.Hard => Hard,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}