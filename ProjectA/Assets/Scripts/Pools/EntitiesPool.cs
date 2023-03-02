using System;
using ProjectA.Entity;
using ProjectA.Entity.Position;
using ProjectA.Interface;
using ProjectA.Scriptables;
using ProjectA.Singletons.Managers;
using ProjectA.Utils;
using UnityEngine;

namespace ProjectA.Pools {
    
    public class EntitiesPool : PoolBase<EntityPosition> {

        public EntityPosition Prefab;

        public EntityInfo DestructibleEntity;
        public EntityInfo HardPorpEntity;
        public EntityInfo EnemyEntity;
        public EntityInfo ShooterEntity;
        public LayerMask PlayerLayer;
        
        public EntityPosition GetFromPool() {
            return Get();
        }

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnEntityRelease>(OnEntityRelease);
        }

        private void OnEntityRelease(OnEntityRelease ev) {
            Release(ev.Entity);
        }

        private void Start() {
            InitPool(Prefab, 20, 40);
        }
        
    }
}