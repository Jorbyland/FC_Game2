using UnityEditor;
using UnityEngine;

namespace Game
{
    [CustomEditor(typeof(Building_PartsComponent))]
    public class Building_PartsComponentEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            // Dessine l’inspecteur par défaut
            DrawDefaultInspector();

            // Référence au composant
            Building_PartsComponent partsComponent = (Building_PartsComponent)target;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Tools", EditorStyles.boldLabel);

            if (GUILayout.Button("Get All Parts"))
            {
                // Appliquer la fonction
                partsComponent.GetAllParts();

                // Marquer la scène et l’objet comme modifiés pour que Unity sauvegarde les changements
                EditorUtility.SetDirty(partsComponent);
                if (!Application.isPlaying)
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(partsComponent.gameObject.scene);

                Debug.Log($"[Building_PartsComponentEditor] All parts refreshed for {partsComponent.name}");
            }
        }
    }
}