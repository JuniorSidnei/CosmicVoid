using ProjectA.Modals;
using ProjectA.Movement;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Interface {
    
    public class PlayerHealth : MonoBehaviour {

        public int MaxHealth;
        public GameObject OnHitPrefab;
        public GameObject OnDeathPrefab;
        
        private int m_currentHealth;
        
        private void Awake() {
            m_currentHealth = MaxHealth;
            GameManager.Instance.Dispatcher.Subscribe<OnDamagePlayer>(OnDamagePlayer);
        }

        private void OnDamagePlayer(OnDamagePlayer ev) {
            m_currentHealth -= ev.Damage;
            Instantiate(OnHitPrefab, transform.position, Quaternion.identity, transform);
            
            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(ev.ShakeForce));

            if (m_currentHealth <= 0) {
                m_currentHealth = 0;
                Instantiate(OnDeathPrefab, transform.position, Quaternion.identity, transform);
                PauseModal.Instance.PauseGame(true);
            }
            
            GameManager.Instance.Dispatcher.Emit(new OnPlayerStateChange(PlayerMovement.PlayerStates.HIT));
            GameManager.Instance.Dispatcher.Emit(new OnPlayerLifeUpdate(m_currentHealth));
        }
    }
}