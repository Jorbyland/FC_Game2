using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Game.BuildingFloor))]
public class BuildingFloorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var floor = (Game.BuildingFloor)target;

        GUILayout.Space(10);
        if (GUILayout.Button("Generate triggers"))
        {
            floor.GenerateBoundingTrigger();
        }
        GUILayout.Space(10);
        if (GUILayout.Button("Clear triggers"))
        {
            floor.ClearOldTriggers();
        }
    }
}