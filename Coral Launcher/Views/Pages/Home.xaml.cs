using CoralLauncher.Core;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using WindowsAPICodePack.Dialogs;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using DiscordRPC;
using System.Net.Http;
using System.Diagnostics;
using System.Net;
using WpfApp6.Services.Launch;
using WpfApp6.Services;
using Coral_Launcher.Services;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Coral_Launcher.Views
{
    public partial class Home : UserControl
    {
        private const string apiUrl = "https:
        private RPC discordRPC; 
        public Home()
        {
            InitializeComponent();
            DisplayUsername();
            discordRPC = new RPC();
            discordRPC.Initialize();
            GetOnlinePlayerCount();
            
            this.Unloaded += OnPageUnloaded;
        }

        private async void GetOnlinePlayerCount()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    response.EnsureSuccessStatusCode();

                    
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    
                    var data = JsonConvert.DeserializeObject<PlayerCountResponse>(jsonResponse);

                    
                    playerCountTextBlock.Text = data.Amount.ToString();

                    
                    ((Run)playerCountTextBlock.Inlines.FirstInline).Foreground = new SolidColorBrush(Colors.White);
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show($"Error fetching data: {ex.Message}");
            }
        }

        
        public class PlayerCountResponse
        {
            public int Amount { get; set; }
        }


        private void EnableBubbleBuilds_Checked(object sender, RoutedEventArgs e)
        {
            Config.WriteToConfig("Settings", "Bubblebuilds", "Yes");
        }

        private void EnableBubbleBuilds_Unchecked(object sender, RoutedEventArgs e)
        {
            Config.WriteToConfig("Settings", "Bubblebuilds", "No");
        }

        private void Discord_Click(object sender, RoutedEventArgs e)
        {
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https:
                UseShellExecute = true
            });
        }

        private void Daily_Click(object sender, RoutedEventArgs e)
        {
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https:
                UseShellExecute = true
            });
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("You have been logged out!");
            Config.WriteToConfig("Login", "Email", string.Empty);
            Config.WriteToConfig("Login", "Password", string.Empty);
            Config.WriteToConfig("Login", "Path", string.Empty);
            Window.GetWindow(this).Close();
        }

        private void Support_Click(object sender, RoutedEventArgs e)
        {
            
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = "https:
                UseShellExecute = true
            });
        }

        private void OnPageUnloaded(object sender, RoutedEventArgs e)
        {
            discordRPC?.Shutdown();
        }
private void DisplayUsername()
        {
            string username = Login.Username;

            if (!string.IsNullOrEmpty(username))
            {
                UsernameTextBox.Text = $"Welcome Back, {username}!";
            }
            else
            {
                UsernameTextBox.Text = "Welcome Back, User!";
            }
        }
        private void Download_Click2(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https:
                UseShellExecute = true
            });
        }

        private async Task UpdateLaunchButtonText(string text)
        {
            await Dispatcher.InvokeAsync(() => LaunchButton.Content = text);
        }

        private async Task DownloadFileWithProgressAsync(string url, string destinationPath, Action<int> progressCallback)
        {
            using (HttpClient client = new HttpClient())
            {
                
                using (HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                {
                    response.EnsureSuccessStatusCode(); 

                    
                    long? totalBytes = response.Content.Headers.ContentLength;

                    
                    using (Stream contentStream = await response.Content.ReadAsStreamAsync(),
                                  fileStream = new FileStream(destinationPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                    {
                        byte[] buffer = new byte[8192]; 
                        long totalDownloadedBytes = 0;
                        int bytesRead;

                        
                        while ((bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                        {
                            await fileStream.WriteAsync(buffer, 0, bytesRead);
                            totalDownloadedBytes += bytesRead;

                            
                            if (totalBytes.HasValue)
                            {
                                int progressPercentage = (int)((double)totalDownloadedBytes / totalBytes.Value * 100);
                                progressCallback(progressPercentage); 
                            }
                        }
                    }
                }
            }
        }

        private async void Launch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Path69 = Config.ReadFromConfig("Login", "Path");

                if (Path69 == "NONE" || !Directory.Exists(Path69) ||
                    !File.Exists(System.IO.Path.Combine(Path69, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe")))
                {
                    CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog
                    {
                        IsFolderPicker = true,
                        Multiselect = false
                    };

                    if (commonOpenFileDialog.ShowDialog() == CommonFileDialogResult.Ok)
                    {
                        string selectedPath = commonOpenFileDialog.FileName;

                        if (File.Exists(System.IO.Path.Combine(selectedPath, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe")))
                        {
                            Config.WriteToConfig("Login", "Path", selectedPath);
                            Path69 = selectedPath;
                        }
                        else
                        {
                            MessageBox.Show("Please select a folder that contains FortniteGame and Engine inside it!");
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
                string exePath = Path.Combine(Path69, "FortniteGame\\Binaries\\Win64\\FortniteClient-Win64-Shipping.exe");

                if (File.Exists(exePath))
                {
                    FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(exePath);
                    Version expectedVersion = new Version(4, 25, 0, 0);
                    Version actualVersion = new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);

                    if (actualVersion != expectedVersion)
                    {
                        MessageBox.Show($"Invalid Fortnite version! Please make sure you used the version 12.41!");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Fortnite executable not found!");
                    return;
                }

                
                await Task.Run(() =>
                {
                    try
                    {
                        foreach (var directory in Directory.GetDirectories(Path69))
                        {
                            string dirName = Path.GetFileName(directory);
                            if (!dirName.Equals("Engine", StringComparison.OrdinalIgnoreCase) &&
                                !dirName.Equals("FortniteGame", StringComparison.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    Directory.Delete(directory, true);
                                }
                                catch {  }
                            }
                        }

                        
                        foreach (var file in Directory.GetFiles(Path69))
                        {
                            try
                            {
                                File.Delete(file);
                            }
                            catch {  }
                        }
                    }
                    catch {  }
                });

                
                string eacExePath = Path.Combine(Path69, "Coral_EAC.exe");
                string eacFolderPath = Path.Combine(Path69, "EasyAntiCheat");
                string eacSetupPath = Path.Combine(eacFolderPath, "EasyAntiCheat_EOS_Setup.exe");

                if (!File.Exists(eacExePath) || !Directory.Exists(eacFolderPath))
                {
                    string zipPath = Path.Combine(Path69, "EAC.zip");
                    string downloadUrl = "https:

                    
                    await DownloadFileWithProgressAsync(downloadUrl, zipPath, async (progress) =>
                    {
                        await UpdateLaunchButtonText($"Downloading 1/3 {progress}%");
                    });

                    await UpdateLaunchButtonText("Extracting...");
                    System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, Path69);
                    File.Delete(zipPath);
                }

                string coralSeriesZipPath = Path.Combine(Path69, "FortniteGame", "Content", "Paks", "CoralSeries.zip");
                string coralSeriesUrl = "https:

                await DownloadFileWithProgressAsync(coralSeriesUrl, coralSeriesZipPath, async (progress) =>
                {
                    await UpdateLaunchButtonText($"Downloading 2/3 {progress}%");
                });

                string paksDirectory = Path.Combine(Path69, "FortniteGame", "Content", "Paks");

                string coralSeriesPakPath = Path.Combine(paksDirectory, "CoralAssets.pak");
                string coralSeriesSigPath = Path.Combine(paksDirectory, "CoralAssets.sig");

                if (File.Exists(coralSeriesPakPath))
                {
                    File.Delete(coralSeriesPakPath);
                }

                if (File.Exists(coralSeriesSigPath))
                {
                    File.Delete(coralSeriesSigPath);
                }

                await UpdateLaunchButtonText("Extracting...");
                try
                {
                    System.IO.Compression.ZipFile.ExtractToDirectory(coralSeriesZipPath, paksDirectory);
                }
                catch (Exception)
                {
                }

                File.Delete(coralSeriesZipPath);


                
                string dllDirectory = Path.Combine(Path69, "Engine", "Binaries", "ThirdParty", "NVIDIA", "NVAftermath", "Win64");
                string dllPath = Path.Combine(dllDirectory, "GFSDK_Aftermath_Lib.x64.dll");
                string dllUrl = "https:

                if (!Directory.Exists(dllDirectory))
                {
                    Directory.CreateDirectory(dllDirectory);
                }

                
                await DownloadFileWithProgressAsync(dllUrl, dllPath, async (progress) =>
                {
                    await UpdateLaunchButtonText($"Downloading 3/3 {progress}%");
                });

                
                await UpdateLaunchButtonText("Close Fortnite");


                string bubbleBuildsPakPath = Path.Combine(Path69, "FortniteGame", "Content", "Paks", "z_BubbleBuilds_P.pak");
                string bubbleBuildsSigPath = Path.Combine(Path69, "FortniteGame", "Content", "Paks", "z_BubbleBuilds_P.sig");

                
                bool bubbleBuildsEnabled = bool.TryParse(Config.ReadFromConfig("Settings", "BubbleWrapBuilds", "false"), out bool enabled) && enabled;

                if (bubbleBuildsEnabled)
                {
                    if (File.Exists(bubbleBuildsPakPath) && File.Exists(bubbleBuildsSigPath))
                    {
                        
                    }
                    else
                    {
                        string paksZipUrl = "https:
                        string paksZipPath = Path.Combine(Path69, "Paks.zip");

                        
                        await DownloadFileWithProgressAsync(paksZipUrl, paksZipPath, async (progress) =>
                        {
                            await UpdateLaunchButtonText($"Downloading... {progress}%");
                        });

                        await UpdateLaunchButtonText("Extracting...");
                        System.IO.Compression.ZipFile.ExtractToDirectory(paksZipPath, Path.Combine(Path69, "FortniteGame", "Content", "Paks"));
                        File.Delete(paksZipPath);
                    }
                }
                else
                {
                    
                    if (File.Exists(bubbleBuildsPakPath)) File.Delete(bubbleBuildsPakPath);
                    if (File.Exists(bubbleBuildsSigPath)) File.Delete(bubbleBuildsSigPath);
                }

                string[] requiredFiles = {
            "CoralAssets.pak",
            "CoralAssets.sig",
            "pakchunk0-WindowsClient.pak",
            "pakchunk0-WindowsClient.sig",
            "pakchunk10-WindowsClient.pak",
            "pakchunk10-WindowsClient.sig",
            "pakchunk1000-WindowsClient.pak",
            "pakchunk1000-WindowsClient.sig",
            "pakchunk10_s1-WindowsClient.pak",
            "pakchunk10_s1-WindowsClient.sig",
            "pakchunk10_s10-WindowsClient.pak",
            "pakchunk10_s10-WindowsClient.sig",
            "pakchunk10_s11-WindowsClient.pak",
            "pakchunk10_s11-WindowsClient.sig",
            "pakchunk10_s12-WindowsClient.pak",
            "pakchunk10_s12-WindowsClient.sig",
            "pakchunk10_s13-WindowsClient.pak",
            "pakchunk10_s13-WindowsClient.sig",
            "pakchunk10_s14-WindowsClient.pak",
            "pakchunk10_s14-WindowsClient.sig",
            "pakchunk10_s15-WindowsClient.pak",
            "pakchunk10_s15-WindowsClient.sig",
            "pakchunk10_s16-WindowsClient.pak",
            "pakchunk10_s16-WindowsClient.sig",
            "pakchunk10_s17-WindowsClient.pak",
            "pakchunk10_s17-WindowsClient.sig",
            "pakchunk10_s2-WindowsClient.pak",
            "pakchunk10_s2-WindowsClient.sig",
            "pakchunk10_s3-WindowsClient.pak",
            "pakchunk10_s3-WindowsClient.sig",
            "pakchunk10_s4-WindowsClient.pak",
            "pakchunk10_s4-WindowsClient.sig",
            "pakchunk10_s5-WindowsClient.pak",
            "pakchunk10_s5-WindowsClient.sig",
            "pakchunk10_s6-WindowsClient.pak",
            "pakchunk10_s6-WindowsClient.sig",
            "pakchunk10_s7-WindowsClient.pak",
            "pakchunk10_s7-WindowsClient.sig",
            "pakchunk10_s8-WindowsClient.pak",
            "pakchunk10_s8-WindowsClient.sig",
            "pakchunk10_s9-WindowsClient.pak",
            "pakchunk10_s9-WindowsClient.sig",
            "pakchunk11-WindowsClient.pak",
            "pakchunk11-WindowsClient.sig",
            "pakchunk11_s1-WindowsClient.pak",
            "pakchunk11_s1-WindowsClient.sig",
            "pakchunk2-WindowsClient.pak",
            "pakchunk2-WindowsClient.sig",
            "pakchunk5-WindowsClient.pak",
            "pakchunk5-WindowsClient.sig",
            "pakchunk7-WindowsClient.pak",
            "pakchunk7-WindowsClient.sig",
            "pakchunk8-WindowsClient.pak",
            "pakchunk8-WindowsClient.sig",
            "pakchunk9-WindowsClient.pak",
            "pakchunk9-WindowsClient.sig"
        };

                string paksPath = Path.Combine(Path69, "FortniteGame", "Content", "Paks");

                if (!Directory.Exists(paksPath))
                {
                    MessageBox.Show("Files not found");
                    return;
                }

                string[] existingFiles = Directory.GetFiles(paksPath).Select(Path.GetFileName).ToArray();
                var filesToIgnore = new[] { "z_BubbleBuilds_P.pak", "z_BubbleBuilds_P.sig" };
                existingFiles = existingFiles.Except(filesToIgnore).ToArray();
                var missingFiles = requiredFiles.Except(existingFiles).ToArray();
                var extraFiles = existingFiles.Except(requiredFiles).ToArray();
                if (missingFiles.Length > 0 || extraFiles.Length > 0)
                {
                    string message = "UNVERIFIED FILES DETECTED! \n";
                    if (missingFiles.Length > 0)
                        message += "Missing files: " + string.Join(", ", missingFiles) + "\n";
                    if (extraFiles.Length > 0)
                        message += "Extra Files: " + string.Join(", ", extraFiles);
                    
                    MessageBox.Show(message);
                    return;
                }


                
                await Task.Delay(1000);
                Window.GetWindow(this).WindowState = WindowState.Minimized;

                string username = Login.Username;
                string token = Config.ReadFromConfig("Login", "Token");
                Game.Start(Path69, "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck -AUTH_PASSWORD=" + token);
                FakeAC.Start(Path69, "FortniteClient-Win64-Shipping_BE.exe", "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "r");
                FakeAC.Start(Path69, "FortniteLauncher.exe", "-epicapp=Fortnite -epicenv=Prod -epiclocale=en-us -epicportal -noeac -fromfl=be -fltoken=h1cdhchd10150221h130eB56 -skippatchcheck", "dsf");

                
                LaunchButton.Content = "Close Fortnite";
                LaunchButton.Click -= Launch_Click;
                LaunchButton.Click += CloseFortnite_Click;
            }
            catch (Exception)
            {
                MessageBox.Show("Error Happened!");
                LaunchButton.Content = "Launch Coral";
            }
        }


        private void CloseFortnite_Click(object sender, RoutedEventArgs e)
        {
            string[] processesToKill =
            {
        "FortniteClient-Win64-Shipping",
        "FortniteClient-Win64-Shipping_BE",
        "FortniteClient-Win64-Shipping_EAC",
        "FortniteLauncher",
        "Coral_EAC"
    };

            foreach (string processName in processesToKill)
            {
                foreach (var process in Process.GetProcessesByName(processName))
                {
                    try
                    {
                        process.Kill();
                    }
                    catch { }
                }
            }

            
            LaunchButton.Content = "Launch Coral";
            LaunchButton.Click -= CloseFortnite_Click;
            LaunchButton.Click += Launch_Click;
        }
    }
}