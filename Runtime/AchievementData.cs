#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX || UNITY_EDITOR)
namespace GameFrameX.SteamWorks.Runtime
{
    /// <summary>
    /// 成就数据类,用于存储成就数据
    /// </summary>
    public sealed class AchievementData
    {
        /// <summary>
        /// 成就的ID
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// 成就的名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 成就的描述
        /// </summary>
        public string Description { get; set; }

        internal bool MAchieved;

        /// <summary>
        /// 成就是否已解锁
        /// </summary>
        public bool Achieved
        {
            get { return MAchieved; }
        }

        public AchievementData(string achievement)
        {
            Id = achievement;
            MAchieved = false;
        }

        /// <summary>
        /// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
        /// </summary>
        /// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
        /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
        /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
        public AchievementData(string achievement, string name, string desc)
        {
            Id = achievement;
            Name = name;
            Description = desc;
            MAchieved = false;
        }
    }
}
#endif