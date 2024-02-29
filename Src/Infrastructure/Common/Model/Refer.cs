using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Refer
    {
        /// <summary>
        /// 原始数据来源，或数据源说明，可能为空
        /// </summary>
        [JsonProperty("status")]
        public string[] Sources { get; set; }

        /// <summary>
        /// 数据许可或版权声明，可能为空
        /// </summary>
        [JsonProperty("license")]
        public string[] License { get; set; }
    }
}