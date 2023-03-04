using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ProjectA.Entity;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Interface;
using ProjectA.Movement;
using UnityEngine;

public enum ShakeForce {
    BASIC, MEDIUM, STRONG
}

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
    public OnDamagePlayer(int damage, ShakeForce shakeForce) {
        Damage = damage;
        ShakeForce = shakeForce;
    }

    public int Damage;
    public ShakeForce ShakeForce;
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
    public OnCameraScreenShake(ShakeForce force) {
        Force = force;
    }

    public ShakeForce Force { get; set; }
}

public class OnCameraScreenShakeWithValues {
    public OnCameraScreenShakeWithValues(float force, float time) {
        Force = force;
        Time = time;
    }

    public float Force { get; set; }
    public float Time { get; set; }
}
    
public class OnBossStartAttack { }

public class OnBossRageMode { }

public class OnHitCountUpdate {
    
    public OnHitCountUpdate(int count) {
        Count = count;
    }

    public int Count;
}

public class OnProjectileEntityRelease {
    public OnProjectileEntityRelease(EntityPosition entity) {
        Entity = entity;
    }

    public EntityPosition Entity;
}

public class OnEntityRelease {
    public OnEntityRelease(EntityPosition entity) {
        Entity = entity;
    }

    public EntityPosition Entity;
}