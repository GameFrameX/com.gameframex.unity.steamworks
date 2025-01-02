using System;
using GameFrameX.Runtime;
#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX || UNITY_EDITOR)&& !UNITY_WEBGL && ENABLE_STEAMWORKS_NET
using Steamworks;
#endif
using UnityEngine;

namespace GameFrameX.SteamWorks.Runtime
{
    /// <summary>
    /// 计时器组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/SteamWorks")]
    [UnityEngine.Scripting.Preserve]
    public sealed class SteamWorksComponent : GameFrameworkComponent
    {
        ISteamWorksManager _steamWorksManager;
        [SerializeField] private uint m_appId;

        protected override void Awake()
        {
            ImplementationComponentType = Utility.Assembly.GetType(componentType);
            InterfaceComponentType = typeof(ISteamWorksManager);
            base.Awake();
            _steamWorksManager = GameFrameworkEntry.GetModule<ISteamWorksManager>();
            if (_steamWorksManager == null)
            {
                Log.Fatal("SteamWorks manager is invalid.");
                return;
            }

            _steamWorksManager.Init(m_appId);
        }

        private void OnEnable()
        {
            _steamWorksManager.Enable();
        }

        /// <summary>
        /// 获取指定名称的整型统计数据。
        /// </summary>
        /// <param name="keyName">统计数据的名称。</param>
        /// <param name="data">输出参数，存储获取到的统计数据。</param>
        /// <returns>如果成功获取统计数据，则返回 true；否则返回 false。</returns>
        public bool GetStat(string keyName, out int data)
        {
            return _steamWorksManager.GetStat(keyName, out data);
        }

        /// <summary>
        /// 获取指定名称的浮点型统计数据。
        /// </summary>
        /// <param name="keyName">统计数据的名称。</param>
        /// <param name="data">输出参数，存储获取到的统计数据。</param>
        /// <returns>如果成功获取统计数据，则返回 true；否则返回 false。</returns>
        public bool GetStat(string keyName, out float data)
        {
            return _steamWorksManager.GetStat(keyName, out data);
        }

        /// <summary>
        /// 获取指定名称的成就状态。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <param name="achieved">输出参数，存储成就是否已解锁的状态。</param>
        /// <returns>如果成功获取成就状态，则返回 true；否则返回 false。</returns>
        public bool GetAchievement(string keyName, out bool achieved)
        {
            return _steamWorksManager.GetAchievement(keyName, out achieved);
        }

        /// <summary>
        /// 获取指定名称的成就显示属性。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <param name="key">属性键。</param>
        /// <returns>成就的显示属性值。</returns>
        public string GetAchievementDisplayAttribute(string keyName, string key)
        {
            return _steamWorksManager.GetAchievementDisplayAttribute(keyName, key);
        }

        /// <summary>
        /// 获取指定名称的成就图标索引。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <returns>成就的图标索引。</returns>
        public int GetAchievementIcon(string keyName)
        {
            return _steamWorksManager.GetAchievementIcon(keyName);
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
            return _steamWorksManager.GetAchievementAndUnlockTime(keyName, out achieved, out unlockTime);
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
            return _steamWorksManager.IndicateAchievementProgress(keyName, currentProgress, maxProgress);
        }

        /// <summary>
        /// 设置指定名称的成就为已解锁状态。
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <returns>如果成功设置成就并保存统计数据，则返回 true；否则返回 false。</returns>
        public bool SetAchievement(string keyName)
        {
            if (_steamWorksManager.SetAchievement(keyName))
            {
                return StoreStats();
            }

            Log.Error("设置成就失败");
            return false;
        }

        /// <summary>
        /// 设置成就ID列表
        /// </summary>
        /// <param name="idList">成就ID数组</param>
        public void SetAchievementIds(string[] idList)
        {
            _steamWorksManager.SetAchievementIds(idList);
        }

        /// <summary>
        /// 获取成就列表
        /// </summary>
        /// <returns></returns>
        public AchievementData[] GetAchievements()
        {
            return _steamWorksManager.GetAchievements();
        }

        /// <summary>
        /// 清除指定名称的成就
        /// </summary>
        /// <param name="keyName">成就的名称。</param>
        /// <returns></returns>
        public bool ClearAchievement(string keyName)
        {
            return _steamWorksManager.CleanAchievement(keyName);
        }

        /// <summary>
        /// 保存所有统计数据。
        /// </summary>
        /// <returns>如果成功保存统计数据，则返回 true；否则返回 false。</returns>
        public bool StoreStats()
        {
            return _steamWorksManager.StoreStats();
        }

        /// <summary>
        /// 设置指定名称的整型统计数据。
        /// </summary>
        /// <param name="keyName">统计数据的名称。</param>
        /// <param name="data">要设置的统计数据值。</param>
        /// <returns>如果成功设置统计数据，则返回 true；否则返回 false。</returns>
        public bool SetStat(string keyName, int data)
        {
            return _steamWorksManager.SetStat(keyName, data);
        }

        /// <summary>
        /// 更新指定名称的平均速率统计数据。
        /// </summary>
        /// <param name="keyName">统计数据的名称。</param>
        /// <param name="count">当前计数。</param>
        /// <param name="sessionLength">会话长度（秒）。</param>
        /// <returns>如果成功更新统计数据，则返回 true；否则返回 false。</returns>
        public bool SetStat(string keyName, float count, double sessionLength)
        {
            return _steamWorksManager.UpdateAvgRateStat(keyName, count, sessionLength);
        }

        /// <summary>
        /// 重置所有统计数据
        /// </summary>
        /// <param name="isResetAchievements">是否同时重置成就，默认为false</param>
        /// <returns>操作是否成功</returns>
        public bool ResetAllStats(bool isResetAchievements = false)
        {
            return _steamWorksManager.ResetAllStats(isResetAchievements);
        }
    }
}