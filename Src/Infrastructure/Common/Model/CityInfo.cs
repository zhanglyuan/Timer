using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class CityInfo
    {
        /// <summary>
        /// 返回状态码 值为200请求成功
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 当前城市信息
        /// </summary>
        [JsonProperty("location")]
        public List<Location> Location { get; set; }

        /// <summary>
        ///  原始数据来源
        /// </summary>
        [JsonProperty("refer")]
        public Refer Refer { get; set; }
    }

    public class Location
    {
        /// <summary>
        /// 城市名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 城市id
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// 地区/城市纬度
        /// </summary>
        [JsonProperty("lat")]
        public double Lat { get; set; }

        /// <summary>
        /// 地区/城市经度
        /// </summary>
        [JsonProperty("lon")]
        public double Lon { get; set; }

        /// <summary>
        /// 地区/城市的上级行政区划名称
        /// </summary>
        [JsonProperty("adm2")]
        public string Adm2 { get; set; }

        /// <summary>
        /// 地区/城市所属一级行政区域
        /// </summary>
        [JsonProperty("adm1")]
        public string Adm1 { get; set; }

        /// <summary>
        /// 地区/城市所属国家名称
        /// </summary>
        [JsonProperty("country")]
        public string Country { get; set; }

        /// <summary>
        /// 地区/城市所在时区
        /// </summary>
        [JsonProperty("tz")]
        public string TZ { get; set; }

        /// <summary>
        /// 地区/城市目前与UTC时间偏移的小时数
        /// </summary>
        [JsonProperty("utcOffset")]
        public string UtcOffset { get; set; }

        /// <summary>
        /// 地区/城市是否当前处于夏令时。1 表示当前处于夏令时，0 表示当前不是夏令时。
        /// </summary>
        [JsonProperty("isDst")]
        public string IsDst { get; set; }

        /// <summary>
        /// 地区/城市的属性
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        ///  地区评分
        /// </summary>
        [JsonProperty("rank")]
        public string Rank { get; set; }

        /// <summary>
        ///  该地区的天气预报网页链接，便于嵌入你的网站或应用
        /// </summary>
        [JsonProperty("fxLink")]
        public string FxLink { get; set; }
    }
}