using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwordType
{
    Normal = 0,
    Fire = 1,
    Ice = 2,
    Poison = 3,
}

public enum EffectColor
{
    Black = 0,
    Red = 1,
    Blue = 2,
    Green = 3,
    Yellow = 4,
}

public enum ItemGetType
{
    [Tooltip("충돌(Rigid[dynamic], BoxCollider2D)")]
    Collision = 0,
    [Tooltip("감지(Rigid[static], CircleCollider2D")]
    DetectedSense = 1,
    [Tooltip("덩어리(Rigid[dynamic], CircleCollider2D")]
    Chunk = 2,
}

public class ItemValue : MonoBehaviour
{
    private SwordType   ElementalType;
    private int         Price;
    private int         Damage;
    private float       AttackSpeed;
    private float       Weight;
    private EffectColor Color;    
}
