using System;
using System.Collections;
using System.Collections.Generic;
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

        [Header("entity infos")]
        public EntityInfo DestructibleEntity;
        public EntityInfo HardPorpEntity;
        public EntityInfo EnemyEntity;
        public EntityInfo ShooterEntity;

        [Header("general settings")]
        public LayerMask PlayerLayer;
        public LayerMask EntitiesLayer;

        private List<LayerMask> m_layers = new List<LayerMask>();
        public EntityPosition GetFromPool() {
            return Get();
        }

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnEntityRelease>(OnEntityRelease);
            
            m_layers.Add(PlayerLayer);
            m_layers.Add(EntitiesLayer);
        }

        private void OnEntityRelease(OnEntityRelease ev) {
            ev.Entity.GetComponent<Actions.Movement>().ResetVelocity();
            Release(ev.Entity);
            // var coroutine = ReleaseEntity(ev.Entity);
            // StartCoroutine(coroutine);
        }

        private void Start() {
            InitPool(Prefab, 20, 40);
        }

        public List<LayerMask> Layers() {
            return m_layers;
        }

        private IEnumerator ReleaseEntity(EntityPosition entityPosition) {
            yield return new WaitForSeconds(0.5f);
            
        }
    }
}