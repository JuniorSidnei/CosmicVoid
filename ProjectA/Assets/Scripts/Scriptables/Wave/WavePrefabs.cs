using System;
using ProjectA.Data.Wave;
using UnityEngine;

namespace ProjectA.Scriptables {
    
    [CreateAssetMenu(menuName = "ProjectA/Data/WavePrefab", fileName = "WavePrefabs")]
    public class WavePrefabs : ScriptableObject {
        
        public GameObject DestructibleProp;
        public GameObject HardProp;
        public GameObject Enemy;
        public GameObject Shooter;
        public GameObject Reflective;
        public GameObject HardProjectile;
        public GameObject Boss;
        
        public GameObject GetEntity(WaveData.EntityType type) {
            return type switch {
                WaveData.EntityType.DestructibleProp => DestructibleProp,
                WaveData.EntityType.HardProp => HardProp,
                WaveData.EntityType.Enemy => Enemy,
                WaveData.EntityType.Shooter => Shooter,
                WaveData.EntityType.Reflective => Reflective,
                WaveData.EntityType.HardProjectile => HardProjectile,
                WaveData.EntityType.Boss => Boss,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
    }
}