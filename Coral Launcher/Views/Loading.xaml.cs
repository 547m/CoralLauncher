using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;

namespace Coral_Launcher.Views
{
    public partial class Loading : Window
    {
        private static readonly HttpClient client = new HttpClient();
        private const string versionCheckUrl = "https:
        private const string currentVersion = "0.3.2";
        private const string installerUrl = "https:
        private const string installerPath = "CoralSetupReal.exe";

        public Loading()
        {
            InitializeComponent();
            CheckVersionAndProceed();
        }

        private async void CheckVersionAndProceed()
        {
            string latestVersion = await CheckVersionAsync();

            if (latestVersion == currentVersion)
            {
                await Task.Delay(2000);
                var loginWindow = new Views.Login();
                loginWindow.Show();
                Application.Current.MainWindow = loginWindow;
                this.Close();
            }
            else
            {
                await DownloadInstallerThenShutdown();
            }
        }

        private async Task<string> CheckVersionAsync()
        {
            try
            {
                string version = await client.GetStringAsync(versionCheckUrl);
                return version.Trim();
            }
            catch
            {
                MessageBox.Show("Unable to check for the latest version!");
                Application.Current.Shutdown();
                return string.Empty;
            }
        }

        private async Task DownloadInstallerThenShutdown()
        {
            try
            {
                
                ValidBox.Visibility = Visibility.Visible;

                
                using (var response = await client.GetAsync(installerUrl))
                {
                    response.EnsureSuccessStatusCode();

                    using (var fs = new FileStream(installerPath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }

                
                Process.Start(new ProcessStartInfo
                {
                    FileName = installerPath,
                    UseShellExecute = true
                });

                
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to self-update! Try downloading the new launcher from the Discord Server!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown();
            }
        }
    }
}
