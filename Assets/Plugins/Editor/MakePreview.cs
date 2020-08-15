using System.IO;
using UnityEngine;
using UnityEditor;


class PreviewUtility {
    private static string lastPath = Application.dataPath;

    [MenuItem("Tools/Create thumbnail(s)")]
    static void CreateThumbnails() {
        if (Selection.gameObjects == null || Selection.gameObjects.Length == 0) return;

        float steps = Selection.gameObjects.Length;
        int step = 0;

        if (Selection.gameObjects.Length == 1) {
            GameObject gameObject = Selection.gameObjects[0];
            string filename = EditorUtility.SaveFilePanel(
                "Generate thumbnail for " + gameObject.name,
                lastPath,
                gameObject.name + ".png",
                "png"
            );
            if (filename.Length == 0) return;
            lastPath = Path.GetDirectoryName(filename);
            EditorUtility.DisplayProgressBar("Generating thumbnails", gameObject.name, step ++ / steps);
            RenderThumbnail(gameObject, filename);
        } else {
            string path = EditorUtility.SaveFolderPanel(
                "Generate thumbnails for " + Selection.gameObjects.Length.ToString() + " game objects",
                lastPath,
                ""
            );
            if (path.Length == 0) return;
            lastPath = path;
            foreach (GameObject gameObject in Selection.gameObjects) {
                string filename = Path.Combine(path, gameObject.name + ".png");
                EditorUtility.DisplayProgressBar("Generating thumbnails", gameObject.name, step ++ / steps);
                RenderThumbnail(gameObject, filename);
            }
        }

        EditorUtility.DisplayProgressBar("Generating thumbnails", "Refreshing asserts", step / steps);
        AssetDatabase.Refresh();

        EditorUtility.ClearProgressBar();
    }

    static PreviewUtility() {
        RuntimePreviewGenerator.BackgroundColor = Color.clear;
        RuntimePreviewGenerator.MarkTextureNonReadable = false;
    }

    static private void RenderThumbnail(GameObject gameObject, string path) {
        Texture2D thumbnail = RuntimePreviewGenerator.GenerateModelPreview(gameObject.transform);
        File.WriteAllBytes(path, thumbnail.EncodeToPNG ());
    }
}