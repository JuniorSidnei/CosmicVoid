using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectA.Scriptables {

    [CreateAssetMenu(menuName = "ProjectA/ShakeForce", fileName = "ShaeForce_")]
    public class ShakeForceInfo : ScriptableObject {

        public float ShakeTime;
        public float ShakeForce;
    }
}