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

            GameManager.Instance.Dispatcher.Emit(new OnCameraScreenShake(.8f, .2f));
            
            GameManager.Instance.Dispatcher.Emit(new OnPlayerLifeUpdate(m_currentHealth));
            if (m_currentHealth <= 0) {
                Debug.Log("player morto");
            }
            
        }
    }
}