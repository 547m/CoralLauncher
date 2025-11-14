using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;

namespace Coral_Launcher
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
            AppDomain.CurrentDomain.ProcessExit += OnProcessExit;
            this.Closed += MainWindow_Closed;

            NavigateWithTransition(new Views.Home()); 
        }

        private void NavigateWithTransition(object newContent)
        {
            var fadeOutAnimation = new DoubleAnimation
            {
                From = 1.0,
                To = 0.0,
                Duration = TimeSpan.FromSeconds(0.3),
            };

            fadeOutAnimation.Completed += (sender, e) =>
            {
                MainContent.Content = newContent; 
                var fadeInAnimation = new DoubleAnimation
                {
                    From = 0.0,
                    To = 1.0,
                    Duration = TimeSpan.FromSeconds(0.3),
                };

                MainContent.BeginAnimation(UIElement.OpacityProperty, fadeInAnimation); 
            };

            MainContent.BeginAnimation(UIElement.OpacityProperty, fadeOutAnimation); 
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateWithTransition(new Views.Home()); 
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            NavigateWithTransition(new Views.Pages.Settings()); 
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            KillFortniteProcess();
        }

        private void OnProcessExit(object? sender, EventArgs e)
        {
            KillFortniteProcess(); 
        }

        private void KillFortniteProcess()
        {
            foreach (var process in Process.GetProcessesByName("FortniteClient-Win64-Shipping"))
            {
                try
                {
                    process.Kill();
                    process.WaitForExit();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to kill fortnite: {ex.Message}");
                }
            }
        }
    }
}
