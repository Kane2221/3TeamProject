using System.Configuration;

namespace _3TeamProject.Extensions.Util
{
    public class ConfigurationUtility
    {
        /// <summary>
        /// 取得Web.config的AppSettings設定
        /// </summary>
        /// <param name="appSettingKey">讀取的appSettingKey</param>
        /// <returns></returns>
        public static string GetAppSetting(string appSettingKey)
        {
            string result = string.Empty;

            if (!string.IsNullOrWhiteSpace(System.Configuration.ConfigurationManager.AppSettings[appSettingKey]))
            {
                result = System.Configuration.ConfigurationManager.AppSettings[appSettingKey].Trim();
            }

            return result;

        }

    }
}
