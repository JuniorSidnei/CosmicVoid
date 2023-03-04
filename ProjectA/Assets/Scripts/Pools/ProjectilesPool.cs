using System;
using ProjectA.Entity;
using ProjectA.Entity.Position;
using ProjectA.Interface;
using ProjectA.Singletons.Managers;
using ProjectA.Utils;
using UnityEngine;

namespace ProjectA.Pools {
    
    public class ProjectilesPool : PoolBase<EntityPosition> {

        public EntityPosition Prefab;

        public EntityPosition GetFromPool() {
            return Get();
        }

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnProjectileEntityRelease>(OnReflectiveEntityRelease);
        }

        private void OnReflectiveEntityRelease(OnProjectileEntityRelease ev) {
            Release(ev.Entity);
        }

        private void Start() {
            InitPool(Prefab, 20, 40);
        }
        
    }
}