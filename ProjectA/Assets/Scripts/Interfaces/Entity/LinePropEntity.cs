using System.Collections.Generic;
using ProjectA.Data.Wave;
using ProjectA.Interface;
using ProjectA.Singletons.Managers;
using UnityEngine;

namespace ProjectA.Entity {

    public class LinePropEntity : MonoBehaviour, IDamageable {

        public LayerMask PlayerLayer;
        private LineRenderer m_lineRenderer;
        private EdgeCollider2D m_edgeCollider2D;
        private Material m_lineMaterial;
        
        private bool m_isPositionSetted;
        
        public int Damage { get; set; }
        
        public void SetPositions(Vector3 parent, Vector3 child, int randomPositionSetter) {
            m_lineRenderer.SetPosition(0, parent);    
            m_lineRenderer.SetPosition(1, child);

            if (m_isPositionSetted) {
                return;
            }

            //set index based on that random in linker, to set the offset parameter of the edge collider
            SetEdgePositions();
            m_isPositionSetted = true;

            switch (randomPositionSetter) {
                case 0:
                    m_edgeCollider2D.offset = new Vector2(-12f, 0f);
                    break;
                case 1:
                    m_edgeCollider2D.offset = new Vector2(-10f, 0f);
                    break;
                case 2:
                    m_edgeCollider2D.offset = new Vector2(-6f, 0f);
                    break;
            }
        }

        private void SetEdgePositions() {
            List<Vector2> edges = new List<Vector2>();
            
            for (var point = 0; point < m_lineRenderer.positionCount; point++) {
                Vector3 lineRendererPoint = m_lineRenderer.GetPosition(point);
                edges.Add(new Vector2(lineRendererPoint.x, lineRendererPoint.y));
            }
            
            m_edgeCollider2D.SetPoints(edges);
            m_edgeCollider2D.offset = new Vector2();
        }
        
        private void Start() {
            m_lineRenderer = GetComponent<LineRenderer>();
            m_edgeCollider2D = GetComponent<EdgeCollider2D>();

            m_lineRenderer.material = Resources.Load<Material>("Material/LineMaterial");
            m_lineRenderer.widthMultiplier = 0.5f;
        }
        
        private void OnCollisionEnter2D(Collision2D other) {
            if(((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
            Destroy(gameObject);
            GameManager.Instance.Dispatcher.Emit(new OnDamagePlayer(Damage, ShakeForce.MEDIUM));
        }

        public void ProcessDamage(bool isCharged) {
            Destroy(gameObject);
        }
        
        public void ProcessPlayerDamage(bool isCharged) { throw new System.NotImplementedException(); }

        public void ProcessProjectileDamage(bool isReflected, int damagePower) {
            if (isReflected) {
                Destroy(gameObject);
            }
        }

        public void ProcessProjectileDamage(ReflectiveEntity reflectiveEntity) {  }
    }
}
