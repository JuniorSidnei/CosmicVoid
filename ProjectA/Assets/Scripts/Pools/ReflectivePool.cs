using System;
using ProjectA.Entity;
using ProjectA.Interface;
using ProjectA.Singletons.Managers;
using ProjectA.Utils;
using UnityEngine;

namespace ProjectA.Pools {
    
    public class ReflectivePool : PoolBase<ReflectiveEntity> {

        public ReflectiveEntity Prefab;

        public GameObject GetFromPool() {
            return Get().gameObject;
        }

        private void Awake() {
            GameManager.Instance.Dispatcher.Subscribe<OnReflectiveEntityRelease>(OnReflectiveEntityRelease);
        }

        private void OnReflectiveEntityRelease(OnReflectiveEntityRelease ev) {
            Release(ev.Entity);
        }

        private void Start() {
            InitPool(Prefab, 10, 20);
        }
        
    }
}