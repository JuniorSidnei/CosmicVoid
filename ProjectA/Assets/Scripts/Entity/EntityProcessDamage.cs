using System.Collections.Generic;
using ProjectA.Actions;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Interface;
using ProjectA.Movement;
using ProjectA.Scriptables;
using ProjectA.Singletons.Managers;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace ProjectA.Entity.ProcessDamage {
    
    public abstract class EntityProcessDamage : MonoBehaviour, IDamageable {

        public int DamagePower;
        public LayerMask PlayerLayer;
        public LayerMask EntityLayer;
        public GameObject OnDieParticlePrefab;
        
        public bool IsReflected { get; set; }

        public virtual void ProcessDamage(bool isCharged) {
            Clean();
        }

        public virtual void ProcessPlayerDamage(bool isCharged) {
           Clean();
        }

        public virtual void ProcessProjectileDamage(bool isReflected, int damagePower) {
           Clean();
        }
        
        public virtual void ProcessProjectileDamage(ReflectiveEntity reflectiveEntity) { }

        public virtual void Setup(EntityInfo info, List<LayerMask> layers) {
            PlayerLayer = layers[0];
            EntityLayer = layers[1];
            DamagePower = info.DamagePower;
            transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = info.Controller;
            GetComponent<Actions.Movement>().ResetVelocity();
            GetComponent<Light2D>().color = info.EntityColor;
            OnDieParticlePrefab = info.EntityExplosionParticle;
        }
        
        public virtual void Setup(EntityInfo info, LayerMask playerLayer) {
            PlayerLayer = playerLayer;
            DamagePower = info.DamagePower;
            transform.GetChild(0).GetComponent<Animator>().runtimeAnimatorController = info.Controller;
            GetComponent<Actions.Movement>().ResetVelocity();
            GetComponent<Light2D>().color = info.EntityColor;
            OnDieParticlePrefab = info.EntityExplosionParticle;
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if(other.gameObject.CompareTag("Wall")) {
                ReleaseEntity();
                return;
            } 
            
            
            if(other.gameObject.CompareTag("BossShield")) {
                other.GetComponent<ShieldEntity>().ProcessProjectileDamage(GetComponent<ReflectiveEntity>());
            }
            
            if (IsReflected) {
                ProcessEntityCollision(other.gameObject);
            }
            else {
                ProcessPlayerCollision(other.gameObject);    
            }
        }

        private void Clean() {
            Instantiate(OnDieParticlePrefab, transform.position, Quaternion.identity);
            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
            Destroy(this);
            gameObject.SetActive(false);
        }
        
        private void ReleaseEntity() {
            
            var type = GetComponent<EntityPosition>().Type;
            switch (type) {
                case WaveData.EntityType.DestructibleProp:
                    Destroy(GetComponent<DestructibleEntity>());
                    break;
                case WaveData.EntityType.HardProp:
                    Destroy(GetComponent<HardEntity>());
                    break;
                case WaveData.EntityType.Enemy:
                    Destroy(GetComponent<EnemyEntity>());
                    break;
                case WaveData.EntityType.Shooter:
                    Destroy(GetComponent<ShooterEntity>());
                    Destroy(GetComponent<Shoot>());
                    Destroy(transform.GetChild(1).gameObject);
                    break;
                case WaveData.EntityType.Linker:
                    GetComponent<LinkerEntity>().Clean();
                    break;
                case WaveData.EntityType.Linked:
                    Destroy(GetComponent<LinkerEntity>());
                    break;
                case WaveData.EntityType.Reflective:
                    Destroy(GetComponent<ReflectiveEntity>());
                    transform.localScale = new Vector3(1, 1, 1);
                    GameManager.Instance.Dispatcher.Emit(new OnProjectileEntityRelease(GetComponent<EntityPosition>()));    
                    return;
                case WaveData.EntityType.HardProjectile:
                    Destroy(GetComponent<HardEntity>());
                    GameManager.Instance.Dispatcher.Emit(new OnProjectileEntityRelease(GetComponent<EntityPosition>()));                    
                    return;
                case WaveData.EntityType.Explosive:
                    Destroy(GetComponent<ExplosiveReflectiveProjectile>());
                    GameManager.Instance.Dispatcher.Emit(new OnProjectileEntityRelease(GetComponent<EntityPosition>()));                    
                    return;
                case WaveData.EntityType.FakeCloaking:
                    Destroy(GetComponent<HardEntity>());
                    return;
                case WaveData.EntityType.Cloaking:
                    Destroy(GetComponent<DestructibleEntity>());
                    return;
            }    
            
            gameObject.SetActive(false);
            GameManager.Instance.Dispatcher.Emit(new OnEntityRelease(GetComponent<EntityPosition>()));
        }
        
        private void ProcessPlayerCollision(GameObject player) {
            if(((1 << player.layer) & PlayerLayer) == 0) {
                return;
            }

            var playerState = player.GetComponent<PlayerMovement>().State;
            
            ProcessPlayerDamage(playerState == PlayerMovement.PlayerStates.ATTACK || playerState == PlayerMovement.PlayerStates.CHARGEDATTACK);
        }

        private void ProcessEntityCollision(GameObject entity) {
            if(((1 << entity.layer) & EntityLayer) == 0) {
                return;
            }

            entity.GetComponent<IDamageable>()?.ProcessProjectileDamage(IsReflected, DamagePower);
            ReleaseEntity();
        }
    }
}