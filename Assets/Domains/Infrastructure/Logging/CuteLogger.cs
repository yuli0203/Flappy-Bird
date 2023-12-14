
using System;
using System.Runtime.CompilerServices;
using UnityEngine;


namespace Logging
{
    public static class CuteLogger
    {
        public static void LogError(string exception, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null, [CallerFilePath] string filePath = "")
        {
            Debug.LogError(GetMessage(exception, lineNumber, caller, filePath));
        }

        public static void LogMessage(string message, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null, [CallerFilePath] string filePath = "")
        {
            Debug.Log(GetMessage(message, lineNumber, caller, filePath));
        }

        public static void LogWarning(string warning, [CallerLineNumber] int lineNumber = 0, [CallerMemberName] string caller = null, [CallerFilePath] string filePath = "")
        {
            Debug.LogWarning(GetMessage(warning, lineNumber, caller, filePath));
        }

        private static string GetMessage(string exception, int lineNumber, string caller, string filePath)
        {
            var pathParts = filePath.Split('\\');
            return $"{exception} | Line: {lineNumber} | function {caller} | file: {pathParts[pathParts.Length-1]}";
        }
    }
}