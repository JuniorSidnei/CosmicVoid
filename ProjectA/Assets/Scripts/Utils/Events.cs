using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using ProjectA.Entity;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Interface;
using ProjectA.Movement;
using UnityEngine;

public enum ShakeForce {
    BASIC, STRONG
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
    public OnCameraScreenShake(float force, float duration) {
        Force = force;
        Duration = duration;
    }

    public float Force { get; set; }
    public float Duration { get; set; }
}

public class OnBossStartAttack { }

public class OnBossRageMode { }

public class OnHitCountUpdate {
    
    public OnHitCountUpdate(int count) {
        Count = count;
    }

    public int Count;
}

public class OnDestructibleEntityRelease {
    public OnDestructibleEntityRelease(DestructibleEntity entity) {
        Entity = entity;
    }

    public DestructibleEntity Entity;
}

public class OnHardPropEntityRelease {
    public OnHardPropEntityRelease(HardEntity entity) {
        Entity = entity;
    }

    public HardEntity Entity;
}

public class OnEnemyEntityRelease {
    public OnEnemyEntityRelease(EnemyEntity entity) {
        Entity = entity;
    }

    public EnemyEntity Entity;
}

public class OnEnemyShooterEntityRelease {
    public OnEnemyShooterEntityRelease(EnemyEntity entity) {
        Entity = entity;
    }

    public EnemyEntity Entity;
}

public class OnReflectiveEntityRelease {
    public OnReflectiveEntityRelease(ReflectiveEntity entity) {
        Entity = entity;
    }

    public ReflectiveEntity Entity;
}