using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SCOM.CoreSystem;
public class EnemyCore : Core
{
    public Enemy Enemy { get; private set; }
    public Movement enemyMovement { get; private set; }
    public CollisionSenses enemyCollisionSenses { get; private set; }
    /*public override void Awake()
    {
        base.Awake();
        *//*enemyMovement = Movement as EnemyMovement;
        enemyCollisionSenses = CollisionSenses as EnemyCollisionSenses;
        Enemy = Unit as Enemy;

        if (!Movement)
        {
            Debug.LogError("Missing EnemyMovement Core Componenet");
        }

        if (!CollisionSenses)
        {
            Debug.LogError("Missing EnemyCollisionSenses Core Componenet");
        }*//*
    }*/
}
