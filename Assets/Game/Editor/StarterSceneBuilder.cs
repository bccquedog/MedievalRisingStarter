using System.IO;
using MedievalRising.Presentation;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MedievalRising.Editor
{
    public static class StarterSceneBuilder
    {
        private const string SceneFolder = "Assets/Game/Scenes";
        private const string ScenePath = SceneFolder + "/StarterVillage.unity";

        [MenuItem("Tools/Medieval Rising/Create Starter Scene")]
        public static void CreateStarterScene()
        {
            if (!Directory.Exists(SceneFolder))
            {
                Directory.CreateDirectory(SceneFolder);
            }

            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var cameraObject = new GameObject("Main Camera");
            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 6f;
            camera.backgroundColor = new Color(0.14f, 0.18f, 0.12f);
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0, 0, -10);

            var systems = new GameObject("Game Systems");
            systems.AddComponent<GameBootstrap>();
            systems.AddComponent<DebugHud>();

            var player = new GameObject("Founder Placeholder");
            player.AddComponent<SpriteRenderer>();
            player.AddComponent<PlayerMover2D>();
            player.transform.localScale = new Vector3(0.8f, 1.2f, 1f);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            Selection.activeGameObject = player;
            Debug.Log($"Created {ScenePath}");
        }
    }
}
