using System.Collections.Generic;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Interface;
using ProjectA.Managers;
using ProjectA.Movement;
using ProjectA.Scriptables;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity {

    public class CloakingEntity : EntityProcessDamage {

        private List<EntityPosition> m_cloakingEntities = new List<EntityPosition>();
        
        public override void ProcessDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount(isCharged ? 2 : 1);
            base.ProcessDamage(isCharged);
        }

        public override void ProcessPlayerDamage(bool isCharged) {
            GameManager.Instance.UpdateHitCount(0, true);
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateSet(PlayerMovement.PlayerStates.STUNNED));
            base.ProcessPlayerDamage(isCharged);
        }
        
        public override void Setup(EntityInfo info, LayerMask playerLayer) {
            base.Setup(info, playerLayer);
            name = WaveData.EntityType.FakeCloaking.ToString();
            
            var cloaking = SpawnManager.Instance.EntitiesPool.GetFromPool();
            var cloakingTwo = SpawnManager.Instance.EntitiesPool.GetFromPool();

            m_cloakingEntities.Add(GetComponent<EntityPosition>());
            m_cloakingEntities.Add(cloaking);
            m_cloakingEntities.Add(cloakingTwo);

            var cloakingInfoUp = new WaveData.EntityInfo {
                Type = WaveData.EntityType.FakeCloaking,
                Position = WaveData.EntityPosition.Up
            };
            
            var cloakingInfoMiddle = new WaveData.EntityInfo {
                Type = WaveData.EntityType.FakeCloaking,
                Position = WaveData.EntityPosition.Middle
            };
            
            var cloakingInfoDown = new WaveData.EntityInfo {
                Type = WaveData.EntityType.FakeCloaking,
                Position = WaveData.EntityPosition.Down
            };

            var cloakingInfo = SpawnManager.Instance.EntitiesPool.FakeCloakingEntity;
            cloaking.gameObject.AddComponent<HardEntity>().Setup(cloakingInfo, playerLayer);
            cloaking.name = WaveData.EntityType.FakeCloaking.ToString();
            cloakingTwo.gameObject.AddComponent<HardEntity>().Setup(cloakingInfo, playerLayer);
            cloakingTwo.name = WaveData.EntityType.FakeCloaking.ToString();
            
            var spawnPosition = SpawnManager.Instance.transform;
            var localX = transform.localPosition.x;
            
            m_cloakingEntities[0].SetPositionAndTypeWithX(cloakingInfoUp, spawnPosition, localX);
            m_cloakingEntities[1].SetPositionAndTypeWithX(cloakingInfoMiddle, spawnPosition, localX);
            m_cloakingEntities[2].SetPositionAndTypeWithX(cloakingInfoDown, spawnPosition, localX);
            
            Invoke(nameof(ActiveCloakingEffect), 3f);
        }

        private void ActiveCloakingEffect() {
            var randomCloaking = Random.Range(0, m_cloakingEntities.Count);
            
            var cloakingEntity = new WaveData.EntityInfo {
                Type = WaveData.EntityType.Cloaking
            };

            var cloakingInfo = SpawnManager.Instance.EntitiesPool.CloakingEntity;
            var cloaking = m_cloakingEntities[randomCloaking];
            cloaking.SetType(cloakingEntity);
            cloaking.name = WaveData.EntityType.Cloaking.ToString();
            cloaking.transform.GetChild(0).GetComponent<UnityEngine.Animator>().runtimeAnimatorController = cloakingInfo.Controller;
            
            var hardEntity = cloaking.GetComponent<HardEntity>();
            if (hardEntity) {
                Destroy(hardEntity);
                cloaking.gameObject.AddComponent<DestructibleEntity>().PlayerLayer = SpawnManager.Instance.EntitiesPool.PlayerLayer;
            }
            
            m_cloakingEntities.RemoveAt(randomCloaking);

            foreach (var entity in m_cloakingEntities) {
                if (entity.Type == WaveData.EntityType.FakeCloaking) {
                    var cloakingEntityComponent = entity.GetComponent<CloakingEntity>();
                    if (cloakingEntityComponent) {
                        Destroy(cloakingEntityComponent);
                        var hardEntityComponent = entity.gameObject.AddComponent<HardEntity>();
                        hardEntityComponent.PlayerLayer = SpawnManager.Instance.EntitiesPool.PlayerLayer;
                        hardEntityComponent.DamagePower = 2;
                    } 
                }
            }
        }
    }
}
