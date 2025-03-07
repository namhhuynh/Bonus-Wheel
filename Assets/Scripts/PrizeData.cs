using UnityEngine;
using UnityEngine.UI;
using System;


[CreateAssetMenu(fileName = "NewPrize", menuName = "Bonus Wheel/Prize")]
public class PrizeData: ScriptableObject
{
    [SerializeField] private string prizeName;
    [SerializeField, Range(0,1)] private float dropChance;
    [SerializeField] private Sprite icon;
    [SerializeField] private int quantity;
    [SerializeField] private PrizeType type;

    
    public string PrizeName{ get { return prizeName; } }

    public float DropChance { get { return dropChance; } }

    public Sprite Icon { get { return icon; } }

    public int Quantity { get { return quantity; } }

    public PrizeType Type { get {  return type; } }


}

public enum PrizeType
{
    Life,
    Brush,
    Gems,
    Hammer,
    Coins
}
