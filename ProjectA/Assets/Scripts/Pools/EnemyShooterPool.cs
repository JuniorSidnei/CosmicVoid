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
            GameManager.Instance.Dispatcher.Subscribe<OnEnemyShooterEntityRelease>(OnEnemyShooterEntityRelease);
        }

        private void OnEnemyShooterEntityRelease(OnEnemyShooterEntityRelease ev) {
            Release(ev.Entity);
        }

        private void Start() {
            InitPool(Prefab, 10, 20);
        }
        
    }
}