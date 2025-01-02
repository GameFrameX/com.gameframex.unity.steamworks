#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX || UNITY_EDITOR)
namespace GameFrameX.SteamWorks.Runtime
{
    /// <summary>
    /// SteamWorks  统计接口
    /// </summary>
    public partial interface ISteamWorksManager
    {
        /// <summary>
        /// 获取指定名称的成就状态。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <param name="achieved">输出参数，存储成就是否已解锁的状态。</param>
        /// <returns>如果成功获取成就状态，则返回 true；否则返回 false。</returns>
        bool GetAchievement(string keyName, out bool achieved);

        /// <summary>
        /// 获取指定名称的成就显示属性。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <param name="key">属性键。</param>
        /// <returns>成就的显示属性值。</returns>
        string GetAchievementDisplayAttribute(string keyName, string key);

        /// <summary>
        /// 获取指定名称的成就图标索引。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <returns>成就的图标索引。</returns>
        int GetAchievementIcon(string keyName);

        /// <summary>
        /// 获取指定名称的成就状态及其解锁时间。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <param name="achieved">输出参数，存储成就是否已解锁的状态。</param>
        /// <param name="unlockTime">输出参数，存储成就的解锁时间。</param>
        /// <returns>如果成功获取成就状态和解锁时间，则返回 true；否则返回 false。</returns>
        bool GetAchievementAndUnlockTime(string keyName, out bool achieved, out uint unlockTime);

        /// <summary>
        /// 更新指定名称的成就进度。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <param name="currentProgress">当前进度。</param>
        /// <param name="maxProgress">最大进度。</param>
        /// <returns>如果成功更新成就进度，则返回 true；否则返回 false。</returns>
        bool IndicateAchievementProgress(string keyName, uint currentProgress, uint maxProgress);

        /// <summary>
        /// 设置指定名称的成就为已解锁状态。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <returns>如果成功设置成就并保存统计数据，则返回 true；否则返回 false。</returns>
        bool SetAchievement(string keyName);

        /// <summary>
        /// 清除指定名称的成就
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <returns></returns>
        bool CleanAchievement(string keyName);

        /// <summary>
        /// 设置成就ID列表
        /// </summary>
        /// <param name="idList">成就ID数组</param>
        void SetAchievementIds(string[] idList);

        /// <summary>
        /// 获取成就列表
        /// </summary>
        /// <returns></returns>
        AchievementData[] GetAchievements();
    }
}
#endif