using ProjectA.Entity;
using ProjectA.Entity.Position;
using ProjectA.Entity.ProcessDamage;
using ProjectA.Movement;

public enum ShakeForce {
    BASIC, MEDIUM, STRONG
}

public enum LaserPosition {
    UP, MID, BOTTOM
}

public class OnPlayerLifeUpdate {
        
    public OnPlayerLifeUpdate(int currentLife) {
        CurrentLife = currentLife;
    }

    public int CurrentLife;
}

public class OnUpdateChargeFill {
    public OnUpdateChargeFill(float currentFill) {
        CurrentFill = currentFill;
    }

    public float CurrentFill;
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
    public OnHitBoss(EntityProcessDamage entity, bool isReflected, int damagePower, bool willOverride = false) {
        Entity = entity;
        IsReflected = isReflected;
        Damage = damagePower;
        WillOverride = willOverride;
    }

    public EntityProcessDamage Entity;
    public bool IsReflected;
    public int Damage;
    public bool WillOverride;
}

public class OnSpawnBoss { }
public class OnBossDeath { }

public class OnWeakSpotDeath {
    public OnWeakSpotDeath(BossWeakSpot weakSpot) {
        WeakSpot = weakSpot;
    }

    public BossWeakSpot WeakSpot;
}

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
public class OnBossStopAttack { }
public class OnBossRageMode { }

public class OnShootLaser {
    public OnShootLaser(LaserPosition type) {
        Type = type;
    }

    public LaserPosition Type;
}

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

public class OnExplosionActivated { }
public class OnCutsceneStarted { }
public class OnCutSceneFinished { }
public class OnReflectFeedback { }