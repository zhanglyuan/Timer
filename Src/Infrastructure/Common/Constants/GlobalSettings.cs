using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public class GlobalSettings
    {
        public const string Version = "4.1.0";
        public const string ReleasesUrl = "https://gitee.com/yulongmohen/Timer/releases";
        public const string ReleasesDownload = "https://gitee.com/yulongmohen/Timer/releases/download/{0}/Timer_install_{0}.exe";
        public const string UpdateAppExeName = "NewVersion.exe";

        public const string DefaultSvgUrl = "https://icons.qweather.com/assets/icons/999.svg";

        private const string GaoDeKey = "a4487f08f5b08a537ce955b30d50342f";
        private const string HeFengKey = "3a695878fae14993af153ef23a8cb4a0";

        /// <summary>
        /// 高德查询城市
        /// </summary>
        public const string IPLocationUrl = $"https://restapi.amap.com/v3/ip?key={GaoDeKey}";

        /// <summary>
        /// 和风查询城市信息
        /// </summary>
        public const string CityInfoUrl = $"https://geoapi.qweather.com/v2/city/lookup?key={HeFengKey}&range=cn&number=1&location=";

        /// <summary>
        /// 和风查询天气
        /// </summary>
        public const string WeatherInfoUrl = $"https://devapi.qweather.com/v7/weather/now?key={HeFengKey}&location=";

        /// <summary>
        /// svg
        /// </summary>
        public const string NowSvgUrl = "https://icons.qweather.com/assets/icons/{0}.svg";
    }
}