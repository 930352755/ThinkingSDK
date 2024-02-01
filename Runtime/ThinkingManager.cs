using System.Collections;
using System.Collections.Generic;
using ThinkingAnalytics;
using UnityEngine;

namespace RewardCardSDK.Thinking
{
    public class ThinkingManager
    {

        private static ThinkingManager instance = null;
        public static ThinkingManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ThinkingManager();
                }
                return instance;
            }
        }
        private ThinkingManager() { }

        /// <summary>
        /// 数数初始化
        /// </summary>
        /// <param name="appId">数数需要的项目ID</param>
        /// <param name="serverUrl">数数需要的服务器地址</param>
        /// <param name="userID">服务端传入的ID</param>
        /// <param name="dynamicSuperProperties">这个是每个点自带的统一信息，可空</param>
        public void Initial(string appId, string serverUrl, string userID = "", IDynamicSuperProperties dynamicSuperProperties = null)
        {
            Debug.LogError("数数开始初始化"+Time.time);
            // 手动初始化（动态挂载 ThinkingAnalyticsAPI 脚本）
            new GameObject("ThinkingAnalytics", typeof(ThinkingAnalyticsAPI));
            // 设置实例参数
            ThinkingAnalyticsAPI.TAMode mode = ThinkingAnalyticsAPI.TAMode.NORMAL;
            ThinkingAnalyticsAPI.TATimeZone timeZone = ThinkingAnalyticsAPI.TATimeZone.Local;
            ThinkingAnalyticsAPI.Token token = new ThinkingAnalyticsAPI.Token(appId, serverUrl, mode, timeZone);
            ThinkingAnalyticsAPI.StartThinkingAnalytics(token);
            //设置每个打点前的必要数据
            if (dynamicSuperProperties != null)
            {
                ThinkingAnalyticsAPI.SetDynamicSuperProperties(dynamicSuperProperties);
            }
            //自动触发一些打点
            EnableAutoTrack();
            //记录用户ID
            if (!string.IsNullOrEmpty(userID))
            {
                ThinkingAnalyticsAPI.Login(userID);
            }
            ThinkingAnalyticsAPI.EnableThirdPartySharing(ThinkingAnalytics.Utils.TAThirdPartyShareType.ADJUST);
            Debug.LogError("数数初始化完成" + Time.time);
        }

        /// <summary>
        /// 自动去打一些点。
        /// </summary>
        private void EnableAutoTrack()
        {
            ThinkingAnalyticsAPI.EnableAutoTrack(AUTO_TRACK_EVENTS.ALL);
#if UNITY_ANDROID
            //TODO IOS需要延迟开启自动
            ThinkingAnalyticsAPI.EnableAutoTrack(AUTO_TRACK_EVENTS.ALL);
#endif

        }

    }

}
