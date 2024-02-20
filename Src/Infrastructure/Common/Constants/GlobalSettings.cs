using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Constants
{
    public class GlobalSettings
    {
        public const string Version = "4.0.0";
        public const string ReleasesUrl = "https://gitee.com/yulongmohen/Timer/releases";
        public const string ReleasesDownload = "https://gitee.com/yulongmohen/Timer/releases/download/{0}/Timer_install_{0}.exe";
        public const string UpdateAppExeName = "NewVersion.exe";
        public const string LocationUrl = "https://restapi.amap.com/v3/ip?key=a4487f08f5b08a537ce955b30d50342f";
        public const string WeatherInfoUrl = "https://restapi.amap.com/v3/weather/weatherInfo?key=a4487f08f5b08a537ce955b30d50342f&city={0}";
    }
}