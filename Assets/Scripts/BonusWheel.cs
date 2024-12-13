using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusWheel : MonoBehaviour
{
    [SerializeField] private PrizeDatabase prizeDatabase;
    [SerializeField] private Transform wheelTransform;
    [SerializeField] private int numberOfSpins = 3;
    [SerializeField] private GameObject sectorPrefab;
    [SerializeField] private float radius = 200f;

    private float[] dropChances;
    private string[] prizeNames;

    private void Start()
    {
        CheckTotalDrop();
        LoadPrizeData();
        PopulateWheelUI();
    }

    void LoadPrizeData()
    {
        dropChances = new float[prizeDatabase.Prizes.Length];
        prizeNames = new string[prizeDatabase.Prizes.Length];

        for (int i = 0; i < prizeDatabase.Prizes.Length; i++)
        {
            dropChances[i] = prizeDatabase.Prizes[i].DropChance;
            prizeNames[i] = prizeDatabase.Prizes[i].PrizeName;
        }
    }
    private void CheckTotalDrop()
    {
        float cumulativeChance = 0;
        foreach (PrizeData p in prizeDatabase.Prizes)
        {
            cumulativeChance += p.DropChance;
        }
        if (cumulativeChance != 1)
        {
            Debug.LogError("Warning: Total of all prize drop chance must add up to 1, cumulativeChance = " + cumulativeChance);
        }
    }

    /// <summary>
    /// Dynamically populates the bonus wheel with sectors. Calculates sector sizes using the amount of total
    /// sectors and incrementing the starting position using the sector size as a multiplier.
    /// Also, dynamically adjusts size and positiion of UI of each sector to scale and position itself in the middle
    /// of the filled area using the midpoint multiplied by the radius we select from the center of the wheel.
    /// </summary>
    void PopulateWheelUI()
    {
        int numberOfSectors = prizeDatabase.Prizes.Length;

        Color color1;
        ColorUtility.TryParseHtmlString("#FED073", out color1);
        Color color2;
        ColorUtility.TryParseHtmlString("#E9DE97", out color2);

        float sectorAngle = 360f / numberOfSectors;

        for (int i = 0; i < numberOfSectors; i++)
        {
            GameObject sectorObj = Instantiate(sectorPrefab, wheelTransform);

            float startAngle = i * sectorAngle; 
            Sector sector = sectorObj.GetComponent<Sector>();

            PrizeData prize = prizeDatabase.Prizes[i];
            sector.SetSector(prize, prize.Icon, prize.Quantity);

            Image sectorImage = sectorObj.GetComponent<Image>();
            sectorImage.color = (i % 2 == 0) ? color2 : color1;
            sectorImage.fillAmount = sectorAngle / 360f;

            sectorObj.transform.localRotation = Quaternion.Euler(0, 0, -startAngle);

            float midpointAngle = (sectorAngle / 2);
            float radians = midpointAngle * Mathf.Deg2Rad;

            float x = radius * Mathf.Sin(radians);
            float y = radius * Mathf.Cos(radians);

            RectTransform iconRectTransform = sector.icon.rectTransform;
            iconRectTransform.localPosition = new Vector3(x, y, 0);

            float scalingFactor = Mathf.Clamp(sectorAngle / 40f, 0.5f, 1.5f);
            iconRectTransform.localScale = Vector3.one * scalingFactor;
        }
    }


    /// <summary>
    /// Uses helper functions to calculate the angle needed to animate the wheel and display the prize.
    /// </summary>
    public void SpinWheel()
    {
        int winningSectorIndex = GetWinningSectorIndex(dropChances);
        float targetAngle = CalculateTargetAngle(winningSectorIndex, prizeDatabase.Prizes.Length, numberOfSpins);
        AnimateWheel(targetAngle);
        Debug.Log("You won: " + prizeDatabase.Prizes[winningSectorIndex].PrizeName);
    }

    public string SpinWheelTester()
    {
        dropChances = new float[prizeDatabase.Prizes.Length];
        prizeNames = new string[prizeDatabase.Prizes.Length];

        for (int i = 0; i < prizeDatabase.Prizes.Length; i++)
        {
            dropChances[i] = prizeDatabase.Prizes[i].DropChance;
            prizeNames[i] = prizeDatabase.Prizes[i].PrizeName;
        }

        int winningSectorIndex = GetWinningSectorIndex(dropChances);
        float targetAngle = CalculateTargetAngle(winningSectorIndex, prizeDatabase.Prizes.Length, numberOfSpins);
        return prizeDatabase.Prizes[winningSectorIndex].PrizeName;
    }

    /// <summary>
    /// This function will select the winning prize based on weighted drop chances of each prize.
    /// Supports total weights over 100% in so program doesn't crash if you have incorrect weights.
    /// </summary>
    /// <param name="dropChances"></param>
    /// <returns></returns>
    private int GetWinningSectorIndex(float[] dropChances)
    {
        float totalWeight = 0f;
        foreach (float chance in dropChances)
        {
            totalWeight += chance;
        }

        float randomValue = Random.Range(0, totalWeight);
        float cumulative = 0f;

        for (int i = 0; i < dropChances.Length; i++)
        {
            cumulative += dropChances[i];
            if (randomValue <= cumulative)
            {
                return i;
            }
        }
        return dropChances.Length - 1;
    }

    /// <summary>
    /// Uses number of sectors, spins, and the target sector index we want to land on to calculate the angle
    /// we want to spin at to reach the final location.
    /// </summary>
    /// <param name="winningSectorIndex"></param>
    /// <param name="numberOfSectors"></param>
    /// <param name="numberOfSpins"></param>
    /// <returns></returns>
    private float CalculateTargetAngle(int winningSectorIndex, int numberOfSectors, int numberOfSpins)
    {
        float anglePerSector = 360f / numberOfSectors;
        float randomOffset = Random.Range(0, anglePerSector);

        return (numberOfSpins * 360f) + (winningSectorIndex * anglePerSector) + randomOffset;
    }

    private void AnimateWheel(float targetAngle)
    {
        float duration = 3f; 
        wheelTransform.DORotate(new Vector3(0, 0, targetAngle), duration, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => Debug.Log("Spin Complete!"));
    }





}
