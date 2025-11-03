#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Game; // suppose que tes scripts Building.cs, etc. sont dans ce namespace

public class BuildingEditorToolWindow : EditorWindow
{
    [Header("Data")]
    public BuildingPrefabLibraryScriptable prefabLibrary;

    [Header("Placement Grid")]
    public float floorCellSize = 3.1f;
    public float wallCellSize = 3.1f;
    public Color gridColor = new(0.25f, 0.7f, 1f, 0.15f);

    private GameObject currentBuilding;
    private int floorCount = 1;
    private int currentFloor = 0;
    private float floorHeight = 3f;
    private GameObject currentPrefab;
    private Quaternion currentRot = Quaternion.identity;
    private bool placing;

    private enum PlacementMode { Floor, Wall, Roof }
    private PlacementMode mode;

    private Mesh previewMesh;
    private Vector3 previewPos;

    [MenuItem("Tools/Building Editor %#e")]
    public static void Open() => GetWindow<BuildingEditorToolWindow>("Building Editor");

    void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }
    void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        prefabLibrary = (BuildingPrefabLibraryScriptable)EditorGUILayout.ObjectField("Prefab Library", prefabLibrary, typeof(BuildingPrefabLibraryScriptable), false);

        EditorGUILayout.Space();
        currentBuilding = (GameObject)EditorGUILayout.ObjectField("Current Building", currentBuilding, typeof(GameObject), true);

        EditorGUILayout.Space();
        if (GUILayout.Button("New Building"))
            CreateNewBuilding();

        EditorGUILayout.Space();
        floorCount = EditorGUILayout.IntSlider("Floor Count", floorCount, 1, 10);
        currentFloor = Mathf.Clamp(EditorGUILayout.IntSlider("Current Floor", currentFloor, 0, floorCount - 1), 0, floorCount - 1);
        floorHeight = EditorGUILayout.FloatField("Floor Height", floorHeight);

        EditorGUILayout.Space();
        mode = (PlacementMode)EditorGUILayout.EnumPopup("Mode", mode);

        if (prefabLibrary != null)
        {
            GameObject[] list = mode switch
            {
                PlacementMode.Floor => prefabLibrary.floors,
                PlacementMode.Wall => prefabLibrary.walls,
                PlacementMode.Roof => prefabLibrary.roofs,
                _ => null
            };

            DrawPrefabGrid(list);
        }

        placing = GUILayout.Toggle(placing, "Enable Placement (Scene)");

        EditorGUILayout.Space();
        floorCellSize = EditorGUILayout.FloatField("Floor Cell Size", floorCellSize);
        wallCellSize = EditorGUILayout.FloatField("Wall Cell Size", wallCellSize);
        gridColor = EditorGUILayout.ColorField("Grid Color", gridColor);
    }

    void CreateNewBuilding()
    {
        GameObject root = new GameObject("Building");
        root.AddComponent<Building>();
        Building_PartsComponent building_PartsComponent = root.AddComponent<Building_PartsComponent>();
        root.AddComponent<Building_TriggerManagerComponent>();
        root.AddComponent<Building_Controller>();
        currentBuilding = root;

        for (int i = 0; i < floorCount; i++)
        {
            GameObject floorRoot = new GameObject($"Floor_{i}");
            floorRoot.transform.SetParent(root.transform);
            BuildingFloor buildingFloor = floorRoot.AddComponent<BuildingFloor>();

            GameObject floor = new GameObject("Floors");
            floor.transform.SetParent(floorRoot.transform);
            GameObject outside = new GameObject("Outside");
            outside.transform.SetParent(floorRoot.transform);
            GameObject inside = new GameObject("Inside");
            inside.transform.SetParent(floorRoot.transform);
            GameObject triggers = new GameObject("Triggers");
            triggers.transform.SetParent(floorRoot.transform);
            GameObject other = new GameObject("Other");
            other.transform.SetParent(floorRoot.transform);

            BuildingFloor_TriggerComponent buildingFloor_TriggerComponent = triggers.AddComponent<BuildingFloor_TriggerComponent>();

            buildingFloor.SetFloorContent(floor);
            buildingFloor.SetOutsideContent(outside);
            buildingFloor.SetInsideContent(inside);
            buildingFloor.SetOtherContent(other);
            buildingFloor.SetTriggerComponent(buildingFloor_TriggerComponent);
            buildingFloor.positionY = floorHeight * i;

            building_PartsComponent.SetFloor(buildingFloor);
        }

        Selection.activeGameObject = root;
    }

    void DrawPrefabGrid(GameObject[] list)
    {
        if (list == null || list.Length == 0)
        {
            EditorGUILayout.HelpBox("No prefabs available in library", MessageType.Info);
            currentPrefab = null;
            return;
        }

        int cols = Mathf.Clamp(list.Length, 1, 8);
        GUIContent[] icons = new GUIContent[list.Length];
        for (int i = 0; i < list.Length; i++)
        {
            var tex = AssetPreview.GetAssetPreview(list[i]) ?? AssetPreview.GetMiniThumbnail(list[i]);
            icons[i] = new GUIContent(tex, list[i].name);
        }

        int sel = GUILayout.SelectionGrid(System.Array.IndexOf(list, currentPrefab), icons, cols, GUILayout.Height(64));
        if (sel >= 0 && sel < list.Length)
        {
            currentPrefab = list[sel];
            previewMesh = currentPrefab.GetComponentInChildren<MeshFilter>()?.sharedMesh;
        }
    }

    void OnSceneGUI(SceneView scene)
    {
        if (!placing || currentPrefab == null || currentBuilding == null) return;

        Handles.color = gridColor;
        DrawGridPlane(currentFloor * floorHeight, floorCellSize, 40);

        Event e = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            Vector3 pos = mode switch
            {
                PlacementMode.Floor => SnapFloor(hit.point, floorCellSize, currentFloor * floorHeight),
                PlacementMode.Wall => SnapWall(hit.point, wallCellSize, currentFloor * floorHeight),
                _ => SnapFloor(hit.point, floorCellSize, currentFloor * floorHeight)
            };
            previewPos = pos;

            if (previewMesh != null)
                Graphics.DrawMeshNow(previewMesh, Matrix4x4.TRS(previewPos, currentRot, Vector3.one));
            else
                Handles.DrawWireCube(previewPos, Vector3.one);

            scene.Repaint();

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.R)
            {
                currentRot *= Quaternion.Euler(0, 90, 0);
                e.Use();
            }

            if (e.type == EventType.MouseDown && e.button == 0 && !e.alt)
            {
                PlacePrefab();
                e.Use();
            }
        }
    }

    Vector3 SnapFloor(Vector3 pos, float cell, float y)
    {
        pos.x = Mathf.Round(pos.x / cell) * cell;
        pos.z = Mathf.Round(pos.z / cell) * cell;
        pos.y = y;
        return pos;
    }

    Vector3 SnapWall(Vector3 pos, float cell, float y)
    {
        // align along edges, not centers
        pos.x = Mathf.Round((pos.x - cell / 2f) / cell) * cell + cell;
        pos.z = Mathf.Round((pos.z - cell / 2f) / cell) * cell + cell;
        pos.y = y;
        return pos;
    }

    void DrawGridPlane(float y, float cellSize, int halfExtent)
    {
        for (int x = -halfExtent; x <= halfExtent; x++)
            Handles.DrawLine(new Vector3(x * cellSize, y, -halfExtent * cellSize), new Vector3(x * cellSize, y, halfExtent * cellSize));

        for (int z = -halfExtent; z <= halfExtent; z++)
            Handles.DrawLine(new Vector3(-halfExtent * cellSize, y, z * cellSize), new Vector3(halfExtent * cellSize, y, z * cellSize));
    }

    void PlacePrefab()
    {
        string floorName = $"Floor_{currentFloor}";
        Transform floorT = currentBuilding.transform.Find(floorName);
        if (floorT == null)
        {
            Debug.LogError("Floor not found.");
            return;
        }

        string zone = mode switch
        {
            PlacementMode.Floor => "Floors",
            PlacementMode.Wall => "Outside",
            PlacementMode.Roof => "Roof",
            _ => "Other"
        };

        Transform zoneT = floorT.Find(zone);
        if (zoneT == null)
        {
            if (zone == "Roof")
            {
                zoneT = floorT.Find("Outside");
            }
            else
            {
                var go = new GameObject(zone);
                go.transform.SetParent(floorT);
                zoneT = go.transform;
            }
        }

        GameObject inst = (GameObject)PrefabUtility.InstantiatePrefab(currentPrefab);
        Undo.RegisterCreatedObjectUndo(inst, "Place prefab");
        inst.transform.SetParent(zoneT);
        inst.transform.position = previewPos;
        inst.transform.rotation = currentRot;
        Selection.activeGameObject = inst;
    }
}
#endif