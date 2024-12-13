using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PrizeDatabase", menuName = "Bonus Wheel/Prize Database")]
public class PrizeDatabase : ScriptableObject
{
    [SerializeField] private PrizeData[] prizes;

    public PrizeData[] Prizes { get { return prizes; } }
}
