using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Data.Status;
using UnityEngine;

namespace ProjectA.Interface {
    
    public class PlayerHealth : MonoBehaviour, IHealthable {
        
        public EntityStatus EntityStatus;

        private float m_currentHealth;
        private float m_currentArmor;
        private float m_currentPower;

        public void TakeDamage(float damage) {
            var totalDamage = damage - m_currentArmor;

            totalDamage = Mathf.Clamp(totalDamage, 1, Mathf.Infinity);
            Debug.Log("tomei: " + totalDamage);
            
            m_currentHealth -= totalDamage;

            if (m_currentHealth <= 0) {
                Debug.Log("player morto");
            }
        }

        public void ReceiveLife(float lifeAmount) {
            m_currentHealth += lifeAmount;
        }
        
        
        private void Awake() {
            m_currentHealth = EntityStatus.MaxHealth;
            m_currentPower = EntityStatus.DamagePower;
            m_currentArmor = EntityStatus.ArmorPower;
        }
    }
}