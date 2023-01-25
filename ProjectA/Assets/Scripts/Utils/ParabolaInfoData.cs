using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectA.Parabola {
    
    [CreateAssetMenu(menuName = "HaremCity/ParabolaData")]
    public class ParabolaInfoData : ScriptableObject {
        
        public AnimationCurve AnimationCurve;
        public float DurationToTarget;
        public float HeightApex;
    }
}