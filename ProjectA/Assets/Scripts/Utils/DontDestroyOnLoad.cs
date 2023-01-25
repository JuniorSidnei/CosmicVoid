using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {
    
    private static DontDestroyOnLoad m_instance;
    
    private void Awake() {
        DontDestroyOnLoad (this);
         
        if (m_instance == null) {
            m_instance = this;
        } else {
            Destroy(gameObject);
        }
    }
}
