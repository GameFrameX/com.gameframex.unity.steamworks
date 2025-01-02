#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX || UNITY_EDITOR)&& !UNITY_WEBGL
using Steamworks;

namespace GameFrameX.SteamWorks.Runtime
{
    /// <summary>
    /// SteamWorks管理器
    /// </summary>
    public sealed partial class SteamWorksManager
    {
        /// <summary>
        /// 获取指定名称的成就状态。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <param name="achieved">输出参数，存储成就是否已解锁的状态。</param>
        /// <returns>如果成功获取成就状态，则返回 true；否则返回 false。</returns>
        public bool GetAchievement(string keyName, out bool achieved)
        {
            return SteamUserStats.GetAchievement(keyName, out achieved);
        }

        /// <summary>
        /// 获取指定名称的成就显示属性。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <param name="key">属性键。</param>
        /// <returns>成就的显示属性值。</returns>
        public string GetAchievementDisplayAttribute(string keyName, string key)
        {
            return SteamUserStats.GetAchievementDisplayAttribute(keyName, key);
        }

        /// <summary>
        /// 获取指定名称的成就图标索引。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <returns>成就的图标索引。</returns>
        public int GetAchievementIcon(string keyName)
        {
            return SteamUserStats.GetAchievementIcon(keyName);
        }

        /// <summary>
        /// 获取指定名称的成就状态及其解锁时间。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <param name="achieved">输出参数，存储成就是否已解锁的状态。</param>
        /// <param name="unlockTime">输出参数，存储成就的解锁时间。</param>
        /// <returns>如果成功获取成就状态和解锁时间，则返回 true；否则返回 false。</returns>
        public bool GetAchievementAndUnlockTime(string keyName, out bool achieved, out uint unlockTime)
        {
            return SteamUserStats.GetAchievementAndUnlockTime(keyName, out achieved, out unlockTime);
        }

        /// <summary>
        /// 更新指定名称的成就进度。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <param name="currentProgress">当前进度。</param>
        /// <param name="maxProgress">最大进度。</param>
        /// <returns>如果成功更新成就进度，则返回 true；否则返回 false。</returns>
        public bool IndicateAchievementProgress(string keyName, uint currentProgress, uint maxProgress)
        {
            return SteamUserStats.IndicateAchievementProgress(keyName, currentProgress, maxProgress);
        }

        /// <summary>
        /// 设置指定名称的成就为已解锁状态。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <returns>如果成功设置成就并保存统计数据，则返回 true；否则返回 false。</returns>
        public bool SetAchievement(string keyName)
        {
            if (SteamUserStats.SetAchievement(keyName))
            {
                foreach (var achievement in _mAchievements)
                {
                    if (achievement.Id == keyName)
                    {
                        achievement.MAchieved = true;
                        break;
                    }
                }

                return true;
            }

            return false;
        }


        /// <summary>
        /// 清除指定名称的成就
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <returns></returns>
        public bool CleanAchievement(string keyName)
        {
            if (SteamUserStats.ClearAchievement(keyName))
            {
                return SteamUserStats.StoreStats();
            }

            return false;
        }
    }
}
#endif