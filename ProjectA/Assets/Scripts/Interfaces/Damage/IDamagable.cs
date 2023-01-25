using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Movement;
using UnityEngine;

namespace ProjectA.Interface {
    
    public interface IDamageable {
        
        void ProcessDamage(bool isCharged, PlayerHealth playerHealth);
    }
}