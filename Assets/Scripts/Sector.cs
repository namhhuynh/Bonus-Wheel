using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class Sector
{
    [SerializeField] private string prizeName;
    [SerializeField, Range(0,1)] private float dropChance;
    [SerializeField] private Sprite icon;

    public string GetName(){  return prizeName; }

    public float GetDropChance() {  return dropChance; }

    public Sprite GetIcon() { return icon; }


}
