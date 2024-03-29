using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Data.Wave;
using ProjectA.Entity;
using ProjectA.Movement;
using UnityEngine;

namespace ProjectA.Interface {
    
    public interface IDamageable {
        void ProcessDamage(bool isCharged);
        void ProcessPlayerDamage(bool isCharged);
        void ProcessProjectileDamage(bool isReflected, int damagePower);
        void ProcessProjectileDamage(ReflectiveEntity reflectiveEntity);
    }
}