using System;
using System.Diagnostics.CodeAnalysis;

using Unity.IL2CPP.CompilerServices;

using UnityEngine;

using LogType = IG.HappyCoder.Plugins.Dramework4.Runtime.Constants.LogType;
using Object = UnityEngine.Object;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Tools
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static partial class DW4
    {
        #region ================================ METHODS

        public static void Log(string message)
        {
            if (AppConfig.LogType.HasFlag(LogType.Log) == false) return;
            Debug.Log(message);
        }

        public static void Log(string message, Object context)
        {
            if (AppConfig.LogType.HasFlag(LogType.Log) == false) return;
            Debug.Log(message, context);
        }

        public static void LogAssertion(string message)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogAssertion) == false) return;
            Debug.LogAssertion(message);
        }

        public static void LogAssertion(string message, Object context)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogAssertion) == false) return;
            Debug.LogAssertion(message, context);
        }

        public static void LogAssertionFormat(string message, params object[] args)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogAssertionFormat) == false) return;
            Debug.LogAssertionFormat(message, args);
        }

        public static void LogAssertionFormat(Object context, string message, params object[] args)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogAssertionFormat) == false) return;
            Debug.LogAssertionFormat(context, message, args);
        }

        public static void LogError(string message)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogError) == false) return;
            Debug.LogError(message);
        }

        public static void LogError(string message, Object context)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogError) == false) return;
            Debug.LogError(message, context);
        }

        public static void LogErrorFormat(string message, params object[] args)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogErrorFormat) == false) return;
            Debug.LogErrorFormat(message, args);
        }

        public static void LogErrorFormat(Object context, string message, params object[] args)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogErrorFormat) == false) return;
            Debug.LogErrorFormat(context, message, args);
        }

        public static void LogException(Exception exception)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogException) == false) return;
            Debug.LogException(exception);
        }

        public static void LogException(Exception exception, Object context)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogException) == false) return;
            Debug.LogException(exception, context);
        }

        public static void LogFormat(string message, params object[] args)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogFormat) == false) return;
            Debug.LogFormat(message, args);
        }

        public static void LogFormat(Object context, string message, params object[] args)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogFormat) == false) return;
            Debug.LogFormat(context, message, args);
        }

        public static void LogFormat(UnityEngine.LogType logType, LogOption logOption, Object context, string message, params object[] args)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogFormat) == false) return;
            Debug.LogFormat(logType, logOption, context, message, args);
        }

        public static void LogWarning(string message)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogWarning) == false) return;
            Debug.LogWarning(message);
        }

        public static void LogWarning(string message, Object context)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogWarning) == false) return;
            Debug.LogWarning(message, context);
        }

        public static void LogWarningFormat(string message, params object[] args)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogWarningFormat) == false) return;
            Debug.LogWarningFormat(message, args);
        }

        public static void LogWarningFormat(Object context, string message, params object[] args)
        {
            if (AppConfig.LogType.HasFlag(LogType.LogWarningFormat) == false) return;
            Debug.LogWarningFormat(context, message, args);
        }

        #endregion
    }
}