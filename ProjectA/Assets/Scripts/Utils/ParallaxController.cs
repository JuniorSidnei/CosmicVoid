using UnityEngine;

namespace ProjectA.Controllers {
    
    public class ParallaxController : MonoBehaviour {

        public float MinimunOffset;
        public Transform ResetTransform;
        public float ParallaxEffect;
        
        private void Update() {
            transform.localPosition -= new Vector3(ParallaxEffect * Time.deltaTime, transform.localPosition.y, transform.localPosition.z);

            if (!(transform.localPosition.x <= MinimunOffset)) return;
            transform.position = ResetTransform.position;
        }
    }
}