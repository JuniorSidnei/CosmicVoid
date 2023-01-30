using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Movement;
using UnityEngine;

namespace ProjectA.Interface {
    
    public interface IDamageable {
        abstract void ProcessDamage(bool isCharged);
        abstract void ProcessPlayerDamage(bool isCharged);
    }
}