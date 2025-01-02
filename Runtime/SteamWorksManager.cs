#if (UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX || UNITY_STANDALONE_OSX || STEAMWORKS_WIN || STEAMWORKS_LIN_OSX || UNITY_EDITOR) && !UNITY_WEBGL
using System;
using GameFrameX.Runtime;
using Steamworks;
using UnityEngine;

namespace GameFrameX.SteamWorks.Runtime
{
    /// <summary>
    /// SteamWorks管理器
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    public sealed partial class SteamWorksManager : GameFrameworkModule, ISteamWorksManager
    {
        private CSteamID GetSteamId()
        {
            return SteamUser.GetSteamID();
        }

        /// <summary>
        /// 获取SteamID
        /// </summary>
        /// <returns></returns>
        public ulong GetSteamID()
        {
            return GetSteamId().m_SteamID;
        }

        /// <summary>
        /// 获取AccountID
        /// </summary>
        /// <returns></returns>
        public ulong GetAccountID()
        {
            return GetSteamId().GetAccountID().m_AccountID;
        }

        /// <summary>
        /// 获取玩家名称
        /// </summary>
        /// <returns></returns>
        public string GetPersonaName()
        {
            return SteamFriends.GetPersonaName();
        }

        /// <summary>
        /// 获取中等头像
        /// </summary>
        /// <returns></returns>
        public Texture2D GetMediumFriendAvatar()
        {
            var result = SteamFriends.GetMediumFriendAvatar(GetSteamId());
            if (ParseAvatar(result, out var texture2D))
            {
                return texture2D;
            }

            return default;
        }

        private static bool ParseAvatar(int result, out Texture2D texture2D)
        {
            texture2D = Texture2D.whiteTexture;
            if (result > 0 && SteamUtils.GetImageSize(result, out uint width, out uint height))
            {
                int bufferSize = (int)(width * height * 4);
                byte[] avatarBuffer = new byte[bufferSize];
                SteamUtils.GetImageRGBA(result, avatarBuffer, bufferSize);
                Texture2D texture = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
                texture.LoadRawTextureData(avatarBuffer);
                texture.Apply();
                texture2D = texture;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 获取中等头像
        /// </summary>
        /// <returns></returns>
        public Texture2D GetSmallFriendAvatar()
        {
            var result = SteamFriends.GetSmallFriendAvatar(GetSteamId());
            if (ParseAvatar(result, out var texture2D))
            {
                return texture2D;
            }

            return default;
        }

        /// <summary>
        /// 获取大头像
        /// </summary>
        /// <returns></returns>
        public Texture2D GetLargeFriendAvatar()
        {
            var result = SteamFriends.GetLargeFriendAvatar(GetSteamId());
            if (ParseAvatar(result, out var texture2D))
            {
                return texture2D;
            }

            return default;
        }

        private bool _mBInitialized;

        // Our GameID
        private CGameID _mGameID;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="mAppId">程序ID</param>
        public void Init(uint mAppId)
        {
            if (!Packsize.Test())
            {
                Debug.LogError("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
            }

            if (!DllCheck.Test())
            {
                Debug.LogError("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
            }

            if (!SteamAPI.IsSteamRunning())
            {
                try
                {
                    bool result = SteamAPI.RestartAppIfNecessary(new AppId_t(mAppId));
                    if (result)
                    {
                        ApplicationHelper.Quit();
                        return;
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e);
                }
            }

            try
            {
                // If Steam is not running or the game wasn't started through Steam, SteamAPI_RestartAppIfNecessary starts the
                // Steam client and also launches this game again if the User owns it. This can act as a rudimentary form of DRM.
                // Note that this will run which ever version you have installed in steam. Which may not be the precise executable
                // we were currently running.

                // Once you get a Steam AppID assigned by Valve, you need to replace AppId_t.Invalid with it and
                // remove steam_appid.txt from the game depot. eg: "(AppId_t)480" or "new AppId_t(480)".
                // See the Valve documentation for more information: https://partner.steamgames.com/doc/sdk/api#initialization_and_shutdown
                if (SteamAPI.RestartAppIfNecessary(new AppId_t(mAppId)))
                {
                    Log.Info("[Steamworks.NET] Shutting down because RestartAppIfNecessary returned true. Steam will restart the application.");

                    ApplicationHelper.Quit();
                    return;
                }
            }
            catch (System.DllNotFoundException e)
            {
                // We catch this exception here, as it will be the first occurrence of it.
                Debug.LogError("[Steamworks.NET] Could not load [lib]steam_api.dll/so/dylib. It's likely not in the correct location. Refer to the README for more details.\n" + e);

                ApplicationHelper.Quit();
                return;
            }

            _mBInitialized = SteamAPI.Init();
            if (!_mBInitialized)
            {
                Debug.LogError("[Steamworks.NET] SteamAPI_Init() failed. Refer to Valve's documentation or the comment above this line for more information.");

                return;
            }
        }

        private Callback<UserStatsReceived_t> _mUserStatsReceived;
        Callback<UserStatsStored_t> _mUserStatsStored;
        Callback<UserAchievementStored_t> _mUserAchievementStored;
        private bool _mRequestedStats;
        private bool _mStatsValid;

        /// <summary>
        /// 设置成就ID列表
        /// </summary>
        /// <param name="idList">成就ID数组</param>
        public void SetAchievementIds(string[] idList)
        {
            _mAchievements = new AchievementData[idList.Length];
            for (int i = 0; i < idList.Length; i++)
            {
                _mAchievements[i] = new AchievementData(idList[i]);
            }
        }

        private AchievementData[] _mAchievements;

        /// <summary>
        /// 获取成就列表
        /// </summary>
        /// <returns></returns>
        public AchievementData[] GetAchievements()
        {
            return _mAchievements;
        }

        //-----------------------------------------------------------------------------
        // Purpose: We have stats data from Steam. It is authoritative, so update
        //			our data with those results now.
        //-----------------------------------------------------------------------------
        private void OnUserStatsReceived(UserStatsReceived_t pCallback)
        {
            if (!_mBInitialized)
            {
                return;
            }

            Log.Info("Received stats and achievements from Steam" + pCallback.m_steamIDUser.m_SteamID);
            // we may get callbacks for other games' stats arriving, ignore them
            if ((ulong)_mGameID == pCallback.m_nGameID)
            {
                if (EResult.k_EResultOK == pCallback.m_eResult)
                {
                    _mStatsValid = true;

                    // load achievements
                    foreach (var ach in _mAchievements)
                    {
                        var achievement = SteamUserStats.GetAchievement(ach.Id, out ach.MAchieved);
                        if (achievement)
                        {
                            ach.Name = SteamUserStats.GetAchievementDisplayAttribute(ach.Id, "name");
                            ach.Description = SteamUserStats.GetAchievementDisplayAttribute(ach.Id, "desc");
                        }
                        else
                        {
                            Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.Id + "\nIs it registered in the Steam Partner site?");
                        }
                    }
                }
                else
                {
                    Log.Info("RequestStats - failed, " + pCallback.m_eResult);
                }
            }
        }

        //-----------------------------------------------------------------------------
        // Purpose: Our stats data was stored!
        //-----------------------------------------------------------------------------
        private void OnUserStatsStored(UserStatsStored_t pCallback)
        {
            // we may get callbacks for other games' stats arriving, ignore them
            if ((ulong)_mGameID == pCallback.m_nGameID)
            {
                if (EResult.k_EResultOK == pCallback.m_eResult)
                {
                    Log.Info("StoreStats - success");
                }
                else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
                {
                    // One or more stats we set broke a constraint. They've been reverted,
                    // and we should re-iterate the values now to keep in sync.
                    Log.Info("StoreStats - some failed to validate");
                    // Fake up a callback here so that we re-load the values.
                    UserStatsReceived_t callback = new UserStatsReceived_t();
                    callback.m_eResult = EResult.k_EResultOK;
                    callback.m_nGameID = (ulong)_mGameID;
                    OnUserStatsReceived(callback);
                }
                else
                {
                    Log.Info("StoreStats - failed, " + pCallback.m_eResult);
                }
            }
        }

        //-----------------------------------------------------------------------------
        // Purpose: An achievement was stored
        //-----------------------------------------------------------------------------
        private void OnAchievementStored(UserAchievementStored_t pCallback)
        {
            // We may get callbacks for other games' stats arriving, ignore them
            if ((ulong)_mGameID == pCallback.m_nGameID)
            {
                if (0 == pCallback.m_nMaxProgress)
                {
                    Log.Info("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
                }
                else
                {
                    Log.Info("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
                }
            }
        }


        /// <summary>
        /// 每帧更新方法。
        /// </summary>
        /// <param name="elapseSeconds">自上次调用以来经过的时间（秒）。</param>
        /// <param name="realElapseSeconds">实际经过的时间（秒）。</param>
        protected override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (!_mBInitialized)
            {
                _mRequestedStats = true;
                return;
            }

            // Run Steam client callbacks
            SteamAPI.RunCallbacks();

            if (!_mRequestedStats && _mAchievements != null && _mAchievements.Length > 0)
            {
                // If yes, request our stats
                bool bSuccess = SteamUserStats.RequestCurrentStats();

                // This function should only return false if we weren't logged in, and we already checked that.
                // But handle it being false again anyway, just ask again later.
                _mRequestedStats = bSuccess;
            }
        }

        private SteamAPIWarningMessageHook_t _mSteamAPIWarningMessageHook;

        [AOT.MonoPInvokeCallback(typeof(SteamAPIWarningMessageHook_t))]
        private static void SteamAPIDebugTextHook(int nSeverity, System.Text.StringBuilder pchDebugText)
        {
            Debug.LogWarning(pchDebugText);
        }

        /// <summary>
        /// 启用
        /// </summary>
        public void Enable()
        {
            if (!_mBInitialized)
            {
                return;
            }

            _mGameID = new CGameID(SteamUtils.GetAppID());
            if (_mSteamAPIWarningMessageHook == null)
            {
                // Set up our callback to receive warning messages from Steam.
                // You must launch with "-debug_steamapi" in the launch args to receive warnings.
                _mSteamAPIWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamAPIDebugTextHook);
                SteamClient.SetWarningMessageHook(_mSteamAPIWarningMessageHook);
            }

            _mUserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
            _mUserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
            _mUserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);
        }

        /// <summary>
        /// 关闭Steam API。
        /// </summary>
        protected override void Shutdown()
        {
            if (_mBInitialized == false)
            {
                return;
            }

            _mUserStatsReceived.Dispose();
            _mUserStatsStored.Dispose();
            _mUserAchievementStored.Dispose();
            SteamAPI.Shutdown();
        }
    }
}
#endif