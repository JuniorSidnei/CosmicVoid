using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using HaremCity.Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace HaremCity.Utils {
    
    public class FloatingText : MonoBehaviour {

        public ParabolaController ParabolaController;

        private Vector3 m_initialPosition;
        private Vector3 m_finalPosition;

        private void Start() {
            GetComponent<TextMeshProUGUI>().DOFade(0f, ParabolaController.ParabolaInfoData.DurationToTarget);
            transform.DOScale(new Vector3(1f, 1f, 1f), .25f);
            ParabolaController.SetTarget(m_initialPosition, m_finalPosition, false,() => {
                Destroy(gameObject);
            });
        }
        
        public void SetPositions(Vector3 initialPosition, Vector3 finalPosition) {
            m_initialPosition = initialPosition;
            m_finalPosition = finalPosition;
        }
    }
}