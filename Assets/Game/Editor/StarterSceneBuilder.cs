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
        private static readonly Rect WorldBounds = new Rect(-7f, -4f, 14f, 8f);

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
            cameraObject.transform.position = new Vector3(0, 1.25f, -10);

            var systems = new GameObject("Game Systems");
            systems.AddComponent<GameBootstrap>();
            systems.AddComponent<DebugHud>();

            BuildVillageCorner();

            var player = new GameObject("Founder Placeholder");
            player.AddComponent<SpriteRenderer>();
            player.AddComponent<IsometricSpriteSorter>();
            player.AddComponent<PlayerMover2D>().ConfigureBounds(WorldBounds);
            player.transform.localScale = new Vector3(0.8f, 1.2f, 1f);
            player.transform.position = new Vector3(0f, -1.5f, 0f);

            cameraObject.AddComponent<IsometricCameraFollow2D>().Configure(player.transform, WorldBounds);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            Selection.activeGameObject = player;
            Debug.Log($"Created {ScenePath}");
        }

        private static void BuildVillageCorner()
        {
            for (int y = -3; y <= 3; y++)
            {
                for (int x = -4; x <= 4; x++)
                {
                    Vector3 position = new Vector3((x - y) * 0.75f, (x + y) * 0.375f, 0f);
                    CreateVisual(
                        $"Ground Tile {x},{y}",
                        position,
                        new Vector3(1.5f, 1.5f, 1f),
                        new Color(0.28f, 0.42f, 0.22f),
                        true,
                        false,
                        Vector2.zero);
                }
            }

            CreateVisual(
                "Cottage Footprint Blocker",
                new Vector3(-2.25f, 0.75f, 0f),
                new Vector3(2.1f, 1.3f, 1f),
                new Color(0.45f, 0.28f, 0.18f),
                false,
                true,
                new Vector2(2.1f, 1.3f));
            CreateVisual(
                "Cottage Roof Sorting Marker",
                new Vector3(-2.25f, 1.45f, 0f),
                new Vector3(2.5f, 0.8f, 1f),
                new Color(0.35f, 0.12f, 0.09f),
                false,
                false,
                Vector2.zero);
            CreateVisual(
                "Open Door Marker",
                new Vector3(-1.45f, 0.05f, 0f),
                new Vector3(0.35f, 0.6f, 1f),
                new Color(0.75f, 0.58f, 0.32f),
                false,
                false,
                Vector2.zero);
            CreateVisual(
                "Market Stall Blocker",
                new Vector3(2.1f, -0.25f, 0f),
                new Vector3(1.6f, 1f, 1f),
                new Color(0.55f, 0.31f, 0.12f),
                false,
                true,
                new Vector2(1.6f, 1f));
            CreateVisual(
                "Well Blocker",
                new Vector3(0.75f, 1.75f, 0f),
                new Vector3(0.9f, 0.9f, 1f),
                new Color(0.38f, 0.42f, 0.48f),
                false,
                true,
                new Vector2(0.9f, 0.9f));
        }

        private static void CreateVisual(
            string name,
            Vector3 position,
            Vector3 scale,
            Color color,
            bool diamond,
            bool blocksMovement,
            Vector2 blockerSize)
        {
            var visual = new GameObject(name);
            visual.transform.position = position;
            visual.transform.localScale = scale;
            visual.AddComponent<SpriteRenderer>();
            visual.AddComponent<IsometricSpriteSorter>();
            visual.AddComponent<IsometricPlaceholderVisual>().Configure(color, diamond);

            if (blocksMovement)
            {
                visual.AddComponent<IsometricBlocker2D>().Configure(blockerSize);
            }
        }
    }
}
