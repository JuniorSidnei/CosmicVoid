using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Data.Wave;
using ProjectA.Entity.Position;
using ProjectA.Interface;
using ProjectA.Movement;
using UnityEngine;

namespace ProjectA.Entity.ProcessDamage {
    
    public abstract class EntityProcessDamage : MonoBehaviour {

        public float DamagePower;
        public LayerMask PlayerLayer;

        private void OnTriggerEnter2D(Collider2D other) {
            if(((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
            var o = other.gameObject;

            var playerState = o.GetComponent<PlayerMovement>().State;

            if (playerState == PlayerMovement.PlayerStates.ATTACK ||
                playerState == PlayerMovement.PlayerStates.CHARGEDATTACK) return;
            
            o.GetComponent<PlayerHealth>().TakeDamage(DamagePower);
            Destroy(gameObject);    
        }
        
    }
}