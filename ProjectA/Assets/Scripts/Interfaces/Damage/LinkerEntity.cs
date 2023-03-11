using System;
using ProjectA.Data.Wave;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Managers;
using ProjectA.Singletons.Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectA.Entity {

    public class LinkerEntity : EntityProcessDamage {

        private float m_positionDistance = 5f;

        private GameObject m_linePropPrefab;
        private GameObject m_lineProp;
        private LinePropEntity m_linePropEntity;
        
        private GameObject m_linkedObject;

        public override void ProcessPlayerDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount(true);
            GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(DamagePower, ShakeForce.STRONG));
        }

        public void SetupLinker() {
            m_linePropPrefab = Resources.Load<GameObject>("Prefabs/LineProp");
            
            var randomPositionSetter = Random.Range(0, 3);
            var spawnPosition = Vector3.zero;
            var localPos = transform.localPosition;
            
            spawnPosition.x = randomPositionSetter switch {
                0 => localPos.x + m_positionDistance,
                1 => localPos.x,
                2 => localPos.x - m_positionDistance,
                _ => spawnPosition.x
            };

            var linkedProp = SpawnManager.Instance.EntitiesPool.GetFromPool();
            m_linkedObject = linkedProp.gameObject;
            
            linkedProp.name = WaveData.EntityType.Linked.ToString();
            linkedProp.LinkedSetup(SpawnManager.Instance.EntitiesPool.LinkerEntity, SpawnManager.Instance.EntitiesPool.PlayerLayer);
            
            var entityInfo = new WaveData.EntityInfo {
                Type = WaveData.EntityType.Linked,
                Position = WaveData.EntityPosition.Down
            };

            linkedProp.SetPosition(entityInfo, SpawnManager.Instance.transform, spawnPosition.x);

            var centerPosition = (transform.localPosition + linkedProp.transform.localPosition) / 2;
            m_lineProp = Instantiate(m_linePropPrefab, centerPosition, Quaternion.identity, transform);
            m_linePropEntity = m_lineProp.GetComponent<LinePropEntity>();
        }

        public void Clean() {
            Destroy(m_lineProp);
            Destroy(this);
        }

        private void Update() {
            if(!m_linePropEntity) return;

            
            m_linePropEntity.SetPositions(transform.position, m_linkedObject.transform.position);
        }
    }
}
