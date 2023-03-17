using ProjectA.Entity.ProcessDamage;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity{

    public class BossWeakSpot : EntityProcessDamage {
        
        public int Health;
        public UnityEngine.Animator WeakSpotAnimator;
        
        private int m_health;
        private BoxCollider2D m_collider;

        public void SetOpen() {
            WeakSpotAnimator.CrossFade("weakspot_openned", 0.1f);  
            m_collider.enabled = true;
        }
        
        public void SetIdle() {
            WeakSpotAnimator.CrossFade("weakspot_idle", 0.1f);  
            m_collider.enabled = false;
        }

        public void EnableCollider(bool enabled) {
            m_collider.enabled = enabled;
        }
        
        public override void ProcessDamage(bool isCharged) {
            m_health -= isCharged ? 2 : 1;
            
            WeakSpotAnimator.CrossFade("weakspot_hit", 0.1f);
            
            if (m_health <= 0) {
                Death();   
            }
        }

        public override void ProcessProjectileDamage(bool isReflected, int damagePower) {
            m_health -= damagePower;

            if (m_health <= 0) {
                Death();   
            }
        }

        private void Awake() {
            m_collider = GetComponent<BoxCollider2D>();
            m_health = Health;
        }
        
        private void Death() {
            WeakSpotAnimator.CrossFade("weakspot_deactivated", 0.1f);
            m_collider.enabled = false;
            GameManager.Instance.Dispatcher.Emit(new OnWeakSpotDeath(this));
        }
    }
}
