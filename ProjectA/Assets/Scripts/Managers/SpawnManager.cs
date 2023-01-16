using System;
using System.Collections;
using System.Collections.Generic;
using ProjectA.Data.Wave;
using UnityEngine;

namespace ProjectA.Managers {


    public class SpawnManager : MonoBehaviour {

        public WaveData WaveData;


        private float m_timeToNextSpawn;
        private int m_currentEntityIndex = 0;
        
        private void Start() {
            m_timeToNextSpawn = WaveData.InitialTimeSpawn;
        }

        private void Update() {

            m_timeToNextSpawn -= Time.deltaTime;

            if (m_timeToNextSpawn > 0) return;

            SpawnEntity();
        }

        private void SpawnEntity() {
            var entity = Instantiate(WaveData.GetEntity(WaveData.EntityInfos[m_currentEntityIndex].Type));
            //TODO set entity position
        }
    }
}