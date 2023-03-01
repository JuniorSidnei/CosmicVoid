using System;
using ProjectA.Interface;
using ProjectA.Singletons.Managers;
using ProjectA.Utils;
using UnityEngine;

namespace ProjectA.Pools {
    
    public class DestructiblePropPool : PoolBase<DestructibleEntity> {

        public DestructibleEntity DestructibleEntityPrefab;

        public GameObject GetFromPool() {
            return Get().gameObject;
        }

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnDestructibleEntityRelease>(OnDestructibleEntityRelease);
        }

        private void OnDestructibleEntityRelease(OnDestructibleEntityRelease ev) {
            Release(ev.Entity);
        }

        private void Start() {
            InitPool(DestructibleEntityPrefab, 10, 20);
        }
        
    }
}