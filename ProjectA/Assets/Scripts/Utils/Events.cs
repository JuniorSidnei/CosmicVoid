using System.Collections;
using System.Collections.Generic;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Movement;
using UnityEngine;

public class OnPlayerLifeUpdate {
        
        public OnPlayerLifeUpdate(int currentLife) {
            CurrentLife = currentLife;
        }

        public int CurrentLife;
    }
    
public class OnPlayerStun { }

public class OnPlayerStateChange {
    public OnPlayerStateChange(PlayerMovement.PlayerStates newState) {
        NewState = newState;
    }

    public PlayerMovement.PlayerStates NewState;
}

public class OnPlayerStateSet {
    public OnPlayerStateSet(PlayerMovement.PlayerStates newState) {
        NewState = newState;
    }

    public PlayerMovement.PlayerStates NewState;
}

public class OnPlayerMoving {
    public OnPlayerMoving(bool isMoving) {
        IsMoving = isMoving;
    }

    public bool IsMoving;
}

public class OnDamagePlayer {
    public OnDamagePlayer(int damage) {
        Damage = damage;
    }

    public int Damage;
}

public class OnReflectEntity {
    public OnReflectEntity(EntityProcessDamage entity, bool isCharged) {
        Entity = entity;
        IsCharged = isCharged;
    }

    public EntityProcessDamage Entity;
    public bool IsCharged;
}

public class OnHitBoss {
    public OnHitBoss(EntityProcessDamage entity, bool isReflected, int damagePower) {
        Entity = entity;
        IsReflected = isReflected;
        Damage = damagePower;
    }

    public EntityProcessDamage Entity;
    public bool IsReflected;
    public int Damage;
}

public class OnSpawnBoss { }

public class OnCameraScreenShake {
    public OnCameraScreenShake(float force, float duration) {
        Force = force;
        Duration = duration;
    }

    public float Force { get; set; }
    public float Duration { get; set; }
}

public class OnBossRageMode { }