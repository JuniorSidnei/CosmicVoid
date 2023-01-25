using System.Collections;
using System.Collections.Generic;
using ProjectA.Data.Status;
using UnityEngine;

namespace ProjectA.Interface {

    public interface IHealthable {
        
        public void TakeDamage(float damage);
        public void ReceiveLife(float lifeAmount);
    }
}