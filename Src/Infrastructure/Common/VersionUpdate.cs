using Common.Constants;
using Common.Events;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace Common
{
    public class VersionUpdate : IVersionUpdate
    {
#if DEBUG
        private Timer timer = new Timer(10000);
#else
         private Timer timer = new Timer(300000);
#endif
        private static readonly HttpClient client = new HttpClient();

        public VersionUpdate()
        {
            timer.Elapsed += Update;
            timer.Start();
        }

        private async void Update(object sender, ElapsedEventArgs e)
        {
            string strHtml = await GetNewVersion();
            string pattern = @"data-tag-name=['""]([^'""]+)['""]";
            Match match = Regex.Match(strHtml, pattern);
            if (match.Success)
            {
                string newVersion = match.Groups[1].Value;
                if (Version.TryParse(newVersion, out Version res) && res.CompareTo(Version.Parse(GlobalSettings.Version)) > 0)
                {
                    timer.Stop();
                    Mediator.EventAggregator.GetEvent<UpdateAppEvent>().Publish();
                }
            }
        }

        private async Task<string> GetNewVersion()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(GlobalSettings.ReleasesUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Log.Error($"GetNewVersion {e.Message}");
                return string.Empty;
            }
        }
    }
}