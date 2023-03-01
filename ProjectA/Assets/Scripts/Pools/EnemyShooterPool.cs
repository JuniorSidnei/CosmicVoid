using System;
using ProjectA.Interface;
using ProjectA.Singletons.Managers;
using ProjectA.Utils;
using UnityEngine;

namespace ProjectA.Pools {
    
    public class EnemyShooterPool : PoolBase<EnemyEntity> {

        public EnemyEntity Prefab;

        public GameObject GetFromPool() {
            return Get().gameObject;
        }

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnEnemyEntityRelease>(OnEnemyEntityRelease);
        }

        private void OnEnemyEntityRelease(OnEnemyEntityRelease ev) {
            Release(ev.Entity);
        }

        private void Start() {
            InitPool(Prefab, 5, 10);
        }
        
    }
}