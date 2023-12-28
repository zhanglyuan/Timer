using Common.Constants;
using Common.Events;
using Common.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
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
        private Timer timer = new Timer(20000);
#else
         private Timer timer = new Timer(300000);
#endif
        private static readonly HttpClient client = new HttpClient();
        private static string downloadUrl = string.Empty;

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
                    downloadUrl = string.Format(GlobalSettings.ReleasesDownload, res);
                    if (!await GetNewFile())
                        timer.Start();
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

        private async Task<bool> GetNewFile()
        {
            try
            {
                Mediator.EventAggregator.GetEvent<UpdateAppStartEvent>().Publish();

                HttpResponseMessage response = await client.GetAsync(downloadUrl);
                if (response.IsSuccessStatusCode)
                {
                    // 创建文件流以保存文件内容
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        // 定义保存文件的路径和文件名
                        string savePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, GlobalSettings.UpdateAppExeName);

                        // 写入文件流到磁盘文件
                        using (var fileStream = new FileStream(savePath, FileMode.Create))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }
                else
                {
                    Mediator.EventAggregator.GetEvent<UpdateAppEndEvent>().Publish(false);
                    return false;
                }
            }
            catch (HttpRequestException e)
            {
                Log.Error($"GetNewFile {e.Message}");
                Mediator.EventAggregator.GetEvent<UpdateAppEndEvent>().Publish(false);
                return false;
            }

            Mediator.EventAggregator.GetEvent<UpdateAppEndEvent>().Publish(true);
            return true;
        }
    }
}