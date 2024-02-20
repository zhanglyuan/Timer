using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class BaseMsg
    {
        /// <summary>
        /// 返回状态码 值为0或1 1：成功；0：失败
        /// </summary>
        [JsonProperty("status")]
        public string Status { get; set; }

        /// <summary>
        /// 返回结果总数目
        /// </summary>
        [JsonProperty("count")]
        public string Count { get; set; }

        /// <summary>
        /// 返回的状态信息
        /// </summary>
        [JsonProperty("info")]
        public string Info { get; set; }

        /// <summary>
        /// 返回状态说明,10000代表正确
        /// </summary>
        [JsonProperty("infocode")]
        public string Infocode { get; set; }
    }
}