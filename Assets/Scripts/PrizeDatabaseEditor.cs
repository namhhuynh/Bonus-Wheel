
#if UNITY_EDITOR
using UnityEditor;
[CustomEditor(typeof(PrizeDatabase))]
public class PrizeDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        PrizeDatabase database = (PrizeDatabase)target;
        float totalDrop = 0f;
        foreach (PrizeData prize in database.Prizes)
        {
            totalDrop += prize.DropChance;
            EditorGUILayout.LabelField("Prize Name:", prize.PrizeName);
            EditorGUILayout.LabelField("Drop Chance:", prize.DropChance.ToString());
        }

        EditorGUILayout.LabelField("Total Drop Chance:", totalDrop.ToString());

        DrawDefaultInspector();
    }
}
#endif
