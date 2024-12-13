using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class WheelTestTool : EditorWindow
{
    private PrizeDatabase prizeDatabase;
    private BonusWheel bonusWheel;
    private int spinCount = 1000;
    private string outputFileName = "SpinResults.txt";
    private string lastFilePath;

    [MenuItem("Tools/Wheel Test Tool")]
    public static void ShowWindow()
    {
        GetWindow<WheelTestTool>("Wheel Test Tool");
    }

    private void OnGUI()
    {
        GUILayout.Label("Wheel Spin Tester", EditorStyles.boldLabel);

        prizeDatabase = (PrizeDatabase)EditorGUILayout.ObjectField(
            "Prize Database",
            prizeDatabase,
            typeof(PrizeDatabase),
            false
        );

        bonusWheel = (BonusWheel)EditorGUILayout.ObjectField(
            "Wheel Controller",
            bonusWheel,
            typeof(BonusWheel),
            true
        );

        spinCount = EditorGUILayout.IntField("Number of Spins", spinCount);

        outputFileName = EditorGUILayout.TextField("Output File Name", outputFileName);

        if (GUILayout.Button("Run Simulation"))
        {
            if (prizeDatabase == null || bonusWheel == null)
            {
                EditorUtility.DisplayDialog("Error", "Please assign a Prize Database and Wheel Controller.", "OK");
            }
            else
            {
                RunSimulation();
            }
        }

        if (!string.IsNullOrEmpty(lastFilePath))
        {
            GUILayout.Space(10);
            GUILayout.Label($"Results saved to:\n{lastFilePath}", EditorStyles.wordWrappedLabel);
            if (GUILayout.Button("Open File Location"))
            {
                EditorUtility.RevealInFinder(lastFilePath);
            }
        }
    }

    private void RunSimulation()
    {
        Dictionary<string, int> spinResults = new Dictionary<string, int>();
        foreach (var prize in prizeDatabase.Prizes)
        {
            if (!spinResults.ContainsKey(prize.PrizeName))
            {
                spinResults[prize.PrizeName] = 0;
            }
        }

        for (int i = 0; i < spinCount; i++)
        {
            string prizeName = bonusWheel.SpinWheelTester();
            spinResults[prizeName]++;
        }

        WriteResultsToFile(spinResults);
    }

    private void WriteResultsToFile(Dictionary<string, int> spinResults)
    {
        string filePath = Path.Combine(Application.dataPath, outputFileName);
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine($"Wheel Spin Results ({spinCount} Spins):\n");
            foreach (var result in spinResults)
            {
                writer.WriteLine($"{result.Key}: {result.Value} spins");
            }
        }

        lastFilePath = filePath;
        Debug.Log($"Results written to: {filePath}");
    }
}

