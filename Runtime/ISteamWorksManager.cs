using UnityEngine;

#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX || UNITY_EDITOR)
namespace GameFrameX.SteamWorks.Runtime
{
    /// <summary>
    /// SteamWorks接口
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public partial interface ISteamWorksManager
    {
        /// <summary>
        /// 获取SteamID
        /// </summary>
        /// <returns></returns>
        ulong GetSteamID();

        /// <summary>
        /// 获取AccountID
        /// </summary>
        /// <returns></returns>
        ulong GetAccountID();

        /// <summary>
        /// 获取玩家名称
        /// </summary>
        /// <returns></returns>
        string GetPersonaName();

        /// <summary>
        /// 获取中等头像
        /// </summary>
        /// <returns></returns>
        Texture2D GetMediumFriendAvatar();

        /// <summary>
        /// 获取中等头像
        /// </summary>
        /// <returns></returns>
        Texture2D GetSmallFriendAvatar();

        /// <summary>
        /// 获取大头像
        /// </summary>
        /// <returns></returns>
        Texture2D GetLargeFriendAvatar();

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mAppId">程序ID</param>
        void Init(uint mAppId);

        /// <summary>
        /// 启用
        /// </summary>
        void Enable();
    }
}
#endif