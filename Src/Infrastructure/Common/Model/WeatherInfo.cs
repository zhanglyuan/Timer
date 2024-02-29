using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class WeatherInfo
    {
        /// <summary>
        /// 返回状态码 值为200请求成功
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 当前API的最近更新时间
        /// </summary>
        [JsonProperty("updateTime")]
        public string UpdateTime { get; set; }

        /// <summary>
        ///  该地区的天气预报网页链接，便于嵌入你的网站或应用
        /// </summary>
        [JsonProperty("fxLink")]
        public string FxLink { get; set; }

        /// <summary>
        /// 当前天气信息
        /// </summary>
        [JsonProperty("now")]
        public Now NowWeather { get; set; }

        /// <summary>
        ///  原始数据来源
        /// </summary>
        [JsonProperty("refer")]
        public Refer Refer { get; set; }
    }

    public class Now
    {
        /// <summary>
        /// 数据观测时间
        /// </summary>
        [JsonProperty("obsTime")]
        public string ObsTime { get; set; }

        /// <summary>
        /// 温度，默认单位：摄氏度
        /// </summary>
        [JsonProperty("temp")]
        public string Temp { get; set; }

        /// <summary>
        /// 体感温度，默认单位：摄氏度
        /// </summary>
        [JsonProperty("feelsLike")]
        public string FeelsLike { get; set; }

        /// <summary>
        /// 天气状况的图标代码
        /// </summary>
        [JsonProperty("icon")]
        public string Icon { get; set; }

        /// <summary>
        /// 天气状况的文字描述，包括阴晴雨雪等天气状态的描述
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// 风向360角度
        /// </summary>
        [JsonProperty("wind360")]
        public string Wind360 { get; set; }

        /// <summary>
        /// 风力
        /// </summary>
        [JsonProperty("windDir")]
        public string WindDir { get; set; }

        /// <summary>
        /// 风力等级
        /// </summary>
        [JsonProperty("windScale")]
        public string WindScale { get; set; }

        /// <summary>
        /// 风速，公里/小时
        /// </summary>
        [JsonProperty("windSpeed")]
        public string windSpeed { get; set; }

        /// <summary>
        /// 相对湿度，百分比数值
        /// </summary>
        [JsonProperty("humidity")]
        public string Humidity { get; set; }

        /// <summary>
        /// 当前小时累计降水量，默认单位：毫米
        /// </summary>
        [JsonProperty("precip")]
        public string precip { get; set; }

        /// <summary>
        /// 大气压强，默认单位：百帕
        /// </summary>
        [JsonProperty("pressure")]
        public string Pressure { get; set; }

        /// <summary>
        /// 能见度，默认单位：公里
        /// </summary>
        [JsonProperty("vis")]
        public string Vis { get; set; }

        /// <summary>
        /// 云量，百分比数值。可能为空
        /// </summary>
        [JsonProperty("cloud")]
        public string Cloud { get; set; }

        /// <summary>
        /// 露点温度。可能为空
        /// </summary>
        [JsonProperty("dew")]
        public string Dew { get; set; }
    }
}