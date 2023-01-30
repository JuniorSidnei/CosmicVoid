using System.Collections;
using System.Collections.Generic;
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

