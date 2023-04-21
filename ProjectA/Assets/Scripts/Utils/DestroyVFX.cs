using UnityEngine;

namespace ProjectA.Utils {

    public class DestroyVFX : MonoBehaviour {

        private void Start() {
            
            Destroy(gameObject,
                GetComponent<ParticleSystem>().main.duration +
                GetComponent<ParticleSystem>().main.startLifetime.constantMax);
        }
    }
}