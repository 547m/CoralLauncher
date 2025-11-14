using System;
using System.IO;
using System.Net.Http;
using System.IO.Compression;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using CoralLauncher.Core;
using WindowsAPICodePack.Dialogs;

namespace Coral_Launcher.Views.Pages
{
    
    
    
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
            DisplayUsername();
            UpdateAccountId();
            UpdateDiscordId();
            string savedState = Config.ReadFromConfig("Settings", "BubbleWrapBuilds", "false");
            BubbleWrapToggle.IsChecked = bool.TryParse(savedState, out bool result) && result;
        }

        private void BubbleWrapToggle_Checked(object sender, RoutedEventArgs e)
        {
            Config.WriteToConfig("Settings", "BubbleWrapBuilds", "true");
        }

        private void BubbleWrapToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            Config.WriteToConfig("Settings", "BubbleWrapBuilds", "false");
        }

        private void DisplayUsername()
        {
            string username = Login.Username;

            if (!string.IsNullOrEmpty(username))
            {
                UsernameTextBox.Text = $"{username}";
            }
            else
            {
                UsernameTextBox.Text = "User";
            }
        }

        private async void Version_Click(object sender, RoutedEventArgs e)
        {
            
            string currentVersion = "0.3.2";

            
            string versionUrl = "https:

            
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    
                    string onlineVersion = await client.GetStringAsync(versionUrl);

                    
                    if (onlineVersion.Trim() == currentVersion)
                    {
                        MessageBox.Show("All Good! This is the latest version you are using!");
                    }
                    else
                    {
                        MessageBox.Show("A new launcher is available! Please update the launcher to the newest one available!");
                        Application.Current.Shutdown();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to check for updates: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void Path_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog
                {
                    IsFolderPicker = true,
                    Multiselect = false
                };

                if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    string selectedPath = commonOpenFileDialog.FileName;

                    bool hasFortniteGame = Directory.Exists(System.IO.Path.Combine(selectedPath, "FortniteGame"));
                    bool hasEngine = Directory.Exists(System.IO.Path.Combine(selectedPath, "Engine"));

                    if (hasFortniteGame && hasEngine)
                    {
                        Config.WriteToConfig("Login", "Path", selectedPath);
                        MessageBox.Show("Path successfully updated!");
                    }
                    else
                    {
                        MessageBox.Show("Please select a folder that contains FortniteGame and Engine inside it!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }




        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("You have been logged out!");
            Config.WriteToConfig("Login", "Token", string.Empty);
            Config.WriteToConfig("Login", "Path", string.Empty);
            Window.GetWindow(this).Close();
        }
        private async void UpdateAccountId()
        {
            string username = Login.Username; 

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Username cannot be empty.");
                return;
            }

            
            string apiUrl = $"http:

            try
            {
                
                using (HttpClient client = new HttpClient())
                {
                    
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        
                        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        
                        if (apiResponse.Success)
                        {
                            
                            UsernameTextBox_Copy.Text = apiResponse.AccountId;
                        }
                        else
                        {
                            MessageBox.Show("Failed to retrieve account ID.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve data from the server.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void UpdateDiscordId()
        {
            string username = Login.Username; 

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Username cannot be empty.");
                return;
            }

            
            string apiUrl = $"http:

            try
            {
                
                using (HttpClient client = new HttpClient())
                {
                    
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        
                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        
                        ApiResponse apiResponse = JsonConvert.DeserializeObject<ApiResponse>(jsonResponse);

                        
                        if (apiResponse.Success)
                        {
                            
                            UsernameTextBox_Copy3.Text = apiResponse.DiscordId;
                        }
                        else
                        {
                            MessageBox.Show("Failed to retrieve account ID.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Failed to retrieve data from the server.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        public class ApiResponse
        {
            public bool Success { get; set; }
            public string AccountId { get; set; }

            public string DiscordId { get; set; }
        }


        
        private void AccountsClick(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https:
                UseShellExecute = true
            });
        }
    }
}
