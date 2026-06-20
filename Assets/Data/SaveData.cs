using System;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int world;
    public float x, y, z;
    public string spawnPointIndicator;
    public float normalDamage;
    public float heavyDamage;
    public float spellDamage;
    public float spellSpeed;
    public float spellLifetime;
}
