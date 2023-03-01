using System;
using ProjectA.Interface;
using ProjectA.Singletons.Managers;
using ProjectA.Utils;
using UnityEngine;

namespace ProjectA.Pools {
    
    public class HardPropPool : PoolBase<HardEntity> {

        public HardEntity Prefab;

        public GameObject GetFromPool() {
            return Get().gameObject;
        }

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnHardPropEntityRelease>(OnHardPropEntityRelease);
        }

        private void OnHardPropEntityRelease(OnHardPropEntityRelease ev) {
            Release(ev.Entity);
        }

        private void Start() {
            InitPool(Prefab, 10, 20);
        }
        
    }
}