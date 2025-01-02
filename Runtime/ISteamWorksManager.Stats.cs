#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX || UNITY_EDITOR)
namespace GameFrameX.SteamWorks.Runtime
{
    /// <summary>
    /// SteamWorks接口
    /// </summary>
    public partial interface ISteamWorksManager
    {
        /// <summary>
        /// 获取指定名称的整型统计数据。
        /// </summary>
        /// <param name="name">统计数据的名称。</param>
        /// <param name="data">输出参数，存储获取到的统计数据。</param>
        /// <returns>如果成功获取统计数据，则返回 true；否则返回 false。</returns>
        bool GetStat(string name, out int data);

        /// <summary>
        /// 获取指定名称的浮点型统计数据。
        /// </summary>
        /// <param name="name">统计数据的名称。</param>
        /// <param name="data">输出参数，存储获取到的统计数据。</param>
        /// <returns>如果成功获取统计数据，则返回 true；否则返回 false。</returns>
        bool GetStat(string name, out float data);

        /// <summary>
        /// 保存所有统计数据。
        /// </summary>
        /// <returns>如果成功保存统计数据，则返回 true；否则返回 false。</returns>
        bool StoreStats();

        /// <summary>
        /// 设置指定名称的整型统计数据。
        /// </summary>
        /// <param name="name">统计数据的名称。</param>
        /// <param name="data">要设置的统计数据值。</param>
        /// <returns>如果成功设置统计数据，则返回 true；否则返回 false。</returns>
        bool SetStat(string name, int data);

        /// <summary>
        /// 更新指定名称的平均速率统计数据。
        /// </summary>
        /// <param name="name">统计数据的名称。</param>
        /// <param name="count">当前计数。</param>
        /// <param name="sessionLength">会话长度（秒）。</param>
        /// <returns>如果成功更新统计数据，则返回 true；否则返回 false。</returns>
        bool UpdateAvgRateStat(string name, float count, double sessionLength);

        /// <summary>
        /// 重置所有统计数据
        /// </summary>
        /// <param name="isResetAchievements">是否同时重置成就，默认为false</param>
        /// <returns>操作是否成功</returns>
        bool ResetAllStats(bool isResetAchievements);
    }
}
#endif