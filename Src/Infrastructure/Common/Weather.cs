using Common.Constants;
using Common.Events;
using Common.Interfaces;
using Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Common
{
    public class Weather : IWeather
    {
        private readonly HttpClient client = new HttpClient();
#if DEBUG
        private Timer timer = new Timer(20000);
#else
         private Timer timer = new Timer(3600000);//一小时更新一次
#endif

        public Weather()
        {
            timer.Elapsed += UpdateWeather;
            timer.Start();
        }

        private void UpdateWeather(object sender, ElapsedEventArgs e)
        {
            _ = GetLocationWeather();
        }

        public async Task GetLocationWeather()
        {
            try
            {
                var ipLocation = await GetAPIString<IPLocation>(GlobalSettings.IPLocationUrl);
                if (ipLocation == null || string.IsNullOrEmpty(ipLocation.City))
                    return;

                var cityInfo = await GetAPISteam<CityInfo>(GlobalSettings.CityInfoUrl + ipLocation.City);

                if (cityInfo == null || cityInfo.Location.Count == 0 || string.IsNullOrEmpty(cityInfo.Location[0].ID))
                    return;

                var weatherInfo = await GetAPISteam<WeatherInfo>(GlobalSettings.WeatherInfoUrl + cityInfo.Location[0].ID);

                Mediator.EventAggregator.GetEvent<UpdateWeatherEvent>().Publish(Tuple.Create(ipLocation, weatherInfo));
            }
            catch (Exception ex)
            {
                Log.Error($"GetLocationWeather {ex.Message}");
            }
        }

        private async Task<T> GetAPIString<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(data);
            }
            catch (HttpRequestException e)
            {
                Log.Error($"GetAPI {e.Message}");
                return default;
            }
        }

        /// <summary>
        /// 和风天气返回的GZIP的流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<T> GetAPISteam<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(url);
                var stream = await response.Content.ReadAsStreamAsync();
                string strData = string.Empty;
                using (GZipStream gzip = new GZipStream(stream, CompressionMode.Decompress))
                {
                    byte[] buffer = new byte[4096];

                    int bytesRead;
                    while ((bytesRead = gzip.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        strData += Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    };
                }
     
                return JsonConvert.DeserializeObject<T>(strData);
            }
            catch (HttpRequestException e)
            {
                Log.Error($"GetAPISteam {e.Message}");
                return default;
            }
        }
    }
}