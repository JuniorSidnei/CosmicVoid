using System;
using UnityEngine;
using UnityEngine.Pool;

namespace ProjectA.Utils {
    
    public abstract class PoolBase<T> : MonoBehaviour where T : MonoBehaviour {

        private T m_prefab;
        private ObjectPool<T> m_pool;

        private ObjectPool<T> Pool {
            
            get {
                if (m_pool == null) throw new InvalidOperationException("Need to initialize pool before using!");
                return m_pool;
            }

            set => m_pool = value;
        }
        

        protected void InitPool(T prefab, int initial, int max, bool collectionChecks = false) {
            m_prefab = prefab;
            Pool = new ObjectPool<T>(
                CreateSetup,
                GetSetup,
                ReleaseSetup,
                DestroySetup,
                collectionChecks,
                initial,
                max
            );
        }

        protected virtual T CreateSetup() => Instantiate(m_prefab);
        protected virtual void GetSetup(T obj) => obj.gameObject.SetActive(true);
        protected virtual void ReleaseSetup(T obj) => obj.gameObject.SetActive(false);
        protected virtual void DestroySetup(T obj) => Destroy(obj);

        public T Get() => Pool.Get();
        public void Release(T obj) => Pool.Release(obj);
    }
}