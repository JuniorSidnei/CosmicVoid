using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectA.Data.Status {
    
    [CreateAssetMenu(menuName = "ProjectA/Data/Status", fileName = "EntityStatus")]
    public class EntityStatus : ScriptableObject {

        public float MaxHealth;
        public float DamagePower;
        public float ArmorPower;
    }
}