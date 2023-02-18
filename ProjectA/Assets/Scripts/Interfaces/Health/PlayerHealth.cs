using ProjectA.Modals;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Interface {
    
    public class PlayerHealth : MonoBehaviour {

        public int MaxHealth = 3;

        private int m_currentHealth;
        
        private void Awake() {
            m_currentHealth = MaxHealth;
            GameManager.Instance.Dispatcher.Subscribe<OnDamagePlayer>(OnDamagePlayer);
        }

        private void OnDamagePlayer(OnDamagePlayer ev) {
            m_currentHealth -= ev.Damage;

            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(ev.ShakeForce == ShakeForce.BASIC ? 0.8f : 1.2f, .2f));
            
            if (m_currentHealth <= 0) {
                m_currentHealth = 0;
                PauseModal.Instance.PauseGame(true);
            }
            
            GameManager.Instance.Dispatcher.Emit(new OnPlayerLifeUpdate(m_currentHealth));
        }
    }
}