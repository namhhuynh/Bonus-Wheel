using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class BonusWheel : MonoBehaviour
{
    [SerializeField] private List<Sector> Sectors;
    [SerializeField] private float cumulativeChance;
    [SerializeField] private int testRuns;

    private void Awake()
    {
        Initialize();
        
    }
    private void Start()
    {
        for (int i = 0; i < testRuns; i++)
        {
            Debug.Log(SpinWheel());
        }
    }
    private void Initialize()
    {
        foreach (Sector s in Sectors)
        {
            cumulativeChance += s.GetDropChance();
        }
        if (cumulativeChance != 1)
        {
            Debug.LogError("cumulative chance must total to 1, cumulativeChance = " + cumulativeChance);
        }
    }

    public string SpinWheel()
    {
        float randomValue = Random.Range(0f, 1f);
        float cumulative = 0f;

        foreach (Sector sector in Sectors)
        {
            cumulative += sector.GetDropChance();
            if (randomValue <= cumulative)
                return sector.GetName();
        }
        return Sectors[Sectors.Count].GetName(); 
    }




}
