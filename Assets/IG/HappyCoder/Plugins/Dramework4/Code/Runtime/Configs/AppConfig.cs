using IG.HappyCoder.Plugins.Dramework4.Runtime.ScriptableObjects;

using Sirenix.OdinInspector;

using Unity.Burst;
using Unity.IL2CPP.CompilerServices;

using UnityEngine;

using LogType = IG.HappyCoder.Plugins.Dramework4.Runtime.Constants.LogType;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Configs
{
    [HideMonoScript]
    [BurstCompile]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    internal partial class AppConfig : DScriptableObject
    {
        #region ================================ FIELDS

        internal const string FILENAME = "App Config";
        private const int LABEL_WIDTH_PROJECT_BASE = 80;
        private const int LABEL_WIDTH_LOG_BASE = 70;
        private const int LABEL_WIDTH_LOG_ENABLE = 100;

        [FoldoutGroup("Project Settings")] [BoxGroup("Project Settings/Root Folder", false)]
        [LabelWidth(LABEL_WIDTH_PROJECT_BASE)] [LabelText("Root Folder:")]
        [SerializeField] [FolderPath]
        private string _projectRootFolder = "Assets";

        [FoldoutGroup("Log Settings", 1)] [BoxGroup("Log Settings/Log Type", false)]
        [LabelWidth(LABEL_WIDTH_LOG_BASE)] [LabelText("Log Type:")]
        [SerializeField]
        private LogType _logType;

        [FoldoutGroup("Log Settings", 1)] [BoxGroup("Log Settings/Log File", false)]
        [LabelWidth(LABEL_WIDTH_LOG_BASE)] [LabelText("Log File:")]
        [SerializeField]
        private string _logFile;

        [FoldoutGroup("Log Settings", 1)] [BoxGroup("Log Settings/Log URL", false)]
        [LabelWidth(LABEL_WIDTH_LOG_BASE)] [LabelText("Log URL:")]
        [SerializeField]
        private string _logUrl;

        [FoldoutGroup("Log Settings", 1)] [BoxGroup("Log Settings/Log Dispatcher", false)]
        [LabelWidth(LABEL_WIDTH_LOG_ENABLE)] [LabelText("Log Dispatcher:")]
        [SerializeField]
        private bool _logDispatcher;

        [FoldoutGroup("Log Settings", 1)] [BoxGroup("Log Settings/Log Storage", false)]
        [LabelWidth(LABEL_WIDTH_LOG_ENABLE)] [LabelText("Log Storage:")]
        [SerializeField]
        private bool _logStorage;

        #endregion

        #region ================================ PROPERTIES AND INDEXERS

        internal bool LogDispatcher => _logDispatcher;
        internal string LogFile => _logFile;
        internal bool LogStorage => _logStorage;
        internal LogType LogType => _logType;
        internal string LogUrl => _logUrl;
        internal string ProjectRootFolder => _projectRootFolder;

        #endregion
    }
}