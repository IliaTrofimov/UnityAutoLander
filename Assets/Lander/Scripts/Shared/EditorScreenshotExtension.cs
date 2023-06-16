#if UNITY_EDITOR

namespace Lander.Shared
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices.ComTypes;
    using Unity.VisualScripting;
    using UnityEditor;
    using UnityEditorInternal;
    using UnityEngine;

    public static class EditorScreenshotExtension
    {
        [MenuItem("Screenshot/Take Screenshot %#k")]
        private static void Screenshot()
        {
            var activeWindow = EditorWindow.focusedWindow;

            var vec2Position = activeWindow.position.position;
            var sizeX = activeWindow.position.width;
            var sizeY = activeWindow.position.height;


            var colors = InternalEditorUtility.ReadScreenPixel(vec2Position, (int)sizeX, (int)sizeY);

            var result = new Texture2D((int)sizeX, (int)sizeY);
            result.SetPixels(colors);
            var bytes = result.EncodeToPNG();

            UnityEngine.Object.DestroyImmediate(result);

            var timestamp = System.DateTime.Now;
            var stampString = string.Format("_{0}-{1:00}-{2:00}_{3:00}-{4:00}-{5:00}", timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second);
            var file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Screenshot" + stampString + ".png");

            ScreenCapture.CaptureScreenshot(file);
            File.WriteAllBytes(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Screenshot" + stampString + "2.png"), bytes);

            AssetDatabase.Refresh();
           
            Debug.Log($"New Screenshot taken at Screenshot{stampString}.png");
        }
    }
}

#endif