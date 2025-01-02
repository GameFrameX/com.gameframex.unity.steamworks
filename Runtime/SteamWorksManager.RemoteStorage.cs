#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX || UNITY_EDITOR)&& !UNITY_WEBGL
using GameFrameX.Runtime;
using Steamworks;

namespace GameFrameX.SteamWorks.Runtime
{
    /// <summary>
    /// SteamWorks管理器
    /// </summary>
    public sealed partial class SteamWorksManager
    {
    }
}
#endif