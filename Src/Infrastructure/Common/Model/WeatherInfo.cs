using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class WeatherInfo : BaseMsg
    {
        [JsonProperty("lives")]
        public List<Lives> Lives { get; set; }
    }

    public class Lives
    {
        /// <summary>
        /// 省份
        /// </summary>
        [JsonProperty("province")]
        public string Province { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// 城市编码
        /// </summary>
        [JsonProperty("adcode")]
        public string Adcode { get; set; }

        /// <summary>
        /// 天气
        /// </summary>
        [JsonProperty("weather")]
        public string weather { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        [JsonProperty("temperature")]
        public string Temperature { get; set; }

        /// <summary>
        /// 风向
        /// </summary>
        [JsonProperty("winddirection")]
        public string Winddirection { get; set; }

        /// <summary>
        /// 风力
        /// </summary>
        [JsonProperty("windpower")]
        public string Windpower { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        [JsonProperty("humidity")]
        public string Humidity { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("reporttime")]
        public string Reporttime { get; set; }

        /// <summary>
        /// 温度浮动
        /// </summary>
        [JsonProperty("temperature_float")]
        public string Temperature_float { get; set; }

        /// <summary>
        /// 湿度浮动
        /// </summary>
        [JsonProperty("humidity_float")]
        public string humidity_float { get; set; }
    }
}