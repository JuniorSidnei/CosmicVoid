using System;
using System.Collections.Generic;
using ProjectA.Entity.ProcessDamage;
using UnityEngine;

namespace ProjectA.Entity {

    public class LinePropEntity : MonoBehaviour {

        public LayerMask PlayerLayer;
        private LineRenderer m_lineRenderer;
        private EdgeCollider2D m_edgeCollider2D;

        private bool m_isPositionSetted;
        
        public void SetPositions(Vector3 parent, Vector3 child) {
            m_lineRenderer.SetPosition(0, parent);    
            m_lineRenderer.SetPosition(1, child);

            if (m_isPositionSetted) {
                return;
            }

            //set index based on that random in linker, to set the offset parameter of the edge collider
            SetEdgePositions();
            m_isPositionSetted = true;
            //atrás = -12
            //igual = -10 
            //frente = -6
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
            
            m_lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            m_lineRenderer.widthMultiplier = 0.2f;
        }
        
        private void OnCollisionEnter2D(Collision2D other) {
            if(((1 << other.gameObject.layer) & PlayerLayer) == 0) {
                return;
            }
            
            Debug.Log("bati no player");
        }
    }
}
