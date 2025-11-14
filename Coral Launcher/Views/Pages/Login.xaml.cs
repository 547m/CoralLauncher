using CoralLauncher.Core;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using DiscordRPC;
using Newtonsoft.Json;
using Coral_Launcher.Services;

namespace Coral_Launcher.Views
{
    public partial class Login : Window
    {
        private static readonly HttpClient client = new HttpClient(); 
        private readonly ProcessMonitor processMonitor = new ProcessMonitor();
        public static string Username { get; private set; } 

        public Login()
        {
            InitializeComponent();
            _ = CheckServerStatus(); 
            processMonitor.StartMonitoring();
        }

        private async Task CheckServerStatus()
        {
            string serverIp = "https:
            try
            {
                client.Timeout = TimeSpan.FromSeconds(3);
                var response = await client.GetAsync(serverIp);

                if (!response.IsSuccessStatusCode)
                {
                    ShowErrorAndExit("Coral Servers Are Offline. Check Discord server for more details!");
                }
                else
                {
                    
                    await CheckVpnStatus();
                }
            }
            catch (Exception)
            {
                ShowErrorAndExit("Coral Servers Are Offline. Check Discord server for more details!");
            }
        }

        private async Task CheckVpnStatus()
        {
            string ipInfoUrl = "https:
            try
            {
                var response = await client.GetAsync(ipInfoUrl);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ipInfo = JsonConvert.DeserializeObject<IpInfoResponse>(content);

                    
                    if (IsCloudflareOrg(ipInfo?.Org))
                    {
                        MessageBox.Show("Please do not use an VPN for Coral!");
                        Application.Current.Shutdown();
                    }
                    
                    else if (IsVpnOrg(ipInfo?.Org))
                    {
                        MessageBox.Show("Please do not use an VPN for Coral!");
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        AutoLogin(); 
                    }
                }
                else
                {
                    ShowErrorAndExit("Unable to detect check for anticheat");
                }
            }
            catch (Exception)
            {
                ShowErrorAndExit("Unable to check status for anticheat check");
            }
        }

        private bool IsCloudflareOrg(string org)
        {
            
            return org?.Contains("Cloudflare") == true;
        }

        private bool IsVpnOrg(string org)
        {
            
            
            return org?.ToLower().Contains("vpn") == true;
        }

        private async void AutoLogin()
        {
            string token = Config.ReadFromConfig("Login", "Token");

            if (!string.IsNullOrEmpty(token))
            {
                await AuthenticateWithToken(token);
            }
        }

        private async Task AuthenticateWithToken(string token)
        {
            try
            {
                bool isBanned = await CheckBanStatus(token); 
                if (isBanned)
                {
                    BanBox.Visibility = Visibility.Visible;
                    await Task.Delay(2000);
                    BanBox.Visibility = Visibility.Collapsed;
                    Application.Current.Shutdown();
                    return;
                }

                var response = await client.GetAsync($"http:

                if (response != null && response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var user = JsonConvert.DeserializeObject<UserResponse>(content);

                    if (user != null && !string.IsNullOrEmpty(user.Username))
                    {
                        Username = user.Username;
                        Config.WriteToConfig("Login", "Token", token); 

                        ValidBox.Visibility = Visibility.Visible;
                        await Task.Delay(1500);

                        new MainWindow().Show();
                        Close();
                    }
                    else
                    {
                        ErrorBox.Visibility = Visibility.Visible;
                        await Task.Delay(1500);
                        ErrorBox.Visibility = Visibility.Collapsed;
                    }
                }
                else
                {
                    ErrorBox.Visibility = Visibility.Visible;
                    await Task.Delay(2000);
                    ErrorBox.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred! {ex.Message}\n\nStack Trace:\n{ex.StackTrace}");
            }
        }

        private async Task<bool> CheckBanStatus(string token)
        {
            string url = $"http:
            try
            {
                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var banStatus = JsonConvert.DeserializeObject<BanCheckResponse>(content);

                    return banStatus?.Message == "This account is banned.";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred while checking ban status.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return false;
        }


        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string token = TokenInput.Text.Trim();

            if (string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Please enter your login token.");
                return;
            }

            await AuthenticateWithToken(token);
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string url = "https:
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch (Exception)
            {
                MessageBox.Show("An error occurred", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowErrorAndExit(string message)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(message);
                Application.Current.Shutdown();
            });
        }
    }

    public class BanCheckResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class IpInfoResponse
    {
        public string Ip { get; set; }
        public string Org { get; set; }
    }

    public class UserResponse
    {
        public string Username { get; set; }
    }
}
