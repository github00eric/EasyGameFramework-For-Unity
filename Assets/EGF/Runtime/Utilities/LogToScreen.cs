/*
 * 日志工具
 * 文本直接输出到屏幕
 */
using System;
using System.Collections.Generic;
using UnityEngine;

namespace EGF.Runtime
{
    public class LogToScreen : MonoBehaviour
    {
        private const int maxLines = 50;
        private const int maxLineLength = 120;
        private const int defaultFontSize = 15;
        private string logStr = "";

        private readonly List<string> lines = new List<string>();

        public int fontSize = 15;

        void OnEnable() { Application.logMessageReceived += Log; }
        void OnDisable() { Application.logMessageReceived -= Log; }

        private void Log(string logString, string stackTrace, LogType type)
        {
            foreach (var line in logString.Split('\n'))
            {
                if (line.Length <= maxLineLength)
                {
                    lines.Add(line);
                    continue;
                }
                var lineCount = line.Length / maxLineLength + 1;
                for (int i = 0; i < lineCount; i++)
                {
                    if ((i + 1) * maxLineLength <= line.Length)
                    {
                        lines.Add(line.Substring(i * maxLineLength, maxLineLength));
                    }
                    else
                    {
                        lines.Add(line.Substring(i * maxLineLength, line.Length - i * maxLineLength));
                    }
                }
            }
            if (lines.Count > maxLines)
            {
                lines.RemoveRange(0, lines.Count - maxLines);
            }
            logStr = string.Join("\n", lines);
        }

        private void OnGUI()
        {
            GUI.matrix = Matrix4x4.TRS(Vector3.zero,
                Quaternion.identity,
                new Vector3(Screen.width / 1200.0f, Screen.height / 800.0f, 1.0f));
            
            GUI.Label(new Rect(10, 10, 800, 370), 
                logStr, 
                new GUIStyle() { fontSize = Math.Max(defaultFontSize, fontSize) });
        }
    }
}