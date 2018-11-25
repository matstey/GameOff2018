using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public int MaxHealth;
    public float MaxSpeed;
    public float RangedDelay;
    public int RangedDamage;
    public float RangedRange;
    public float RangedSpeed;
    public float MeleeRange;
    public int MeleeDamage;
    public float MeleeDelay;
    public bool HasMelee = true;
    public bool HasRanged = true;
}