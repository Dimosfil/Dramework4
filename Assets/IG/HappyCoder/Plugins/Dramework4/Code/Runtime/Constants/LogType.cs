using System;


namespace IG.HappyCoder.Plugins.Dramework4.Runtime.Constants
{
    [Flags]
    internal enum LogType
    {
        Log = 2,
        LogFormat = 4,
        LogWarning = 8,
        LogWarningFormat = 16,
        LogError = 32,
        LogErrorFormat = 64,
        LogAssertion = 128,
        LogAssertionFormat = 256,
        LogException = 512
    }
}