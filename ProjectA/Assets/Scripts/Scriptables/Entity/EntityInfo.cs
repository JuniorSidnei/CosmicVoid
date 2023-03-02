using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace ProjectA.Scriptables {
    
    [CreateAssetMenu(menuName = "ProjectA/Entity/Info", fileName = "_info")]
    public class EntityInfo : ScriptableObject {
        
        public AnimatorController Controller;
        public int DamagePower;
        public LayerMask PlayerLayer;
    }
}