using Common.Constants;
using Common.Events;
using Common.Interfaces;
using Common.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
                var location = await GetAPI<Location>(GlobalSettings.LocationUrl);
                if (location == null)
                    return;

                Mediator.EventAggregator.GetEvent<UpdateWeatherEvent>().Publish(Tuple.Create(location, await GetAPI<WeatherInfo>(string.Format(GlobalSettings.WeatherInfoUrl, location?.Adcode))));
            }
            catch (Exception ex)
            {
                Log.Error($"GetLocationWeather {ex.Message}");
            }
        }

        private async Task<T> GetAPI<T>(string url)
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
    }
}