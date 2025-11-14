using CoralLauncher.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp6.Services.Launch
{
    public static class Game
    {
        public static Process _FortniteProcess;
        public static void Start(string PATH, string args)
        {
            string token = Config.ReadFromConfig("Login", "Token");
            if (string.IsNullOrEmpty(token))
            {
                MessageBox.Show("Token not found.");
                return;
            }
            if (File.Exists(Path.Combine(PATH, "FortniteGame\\Binaries\\Win64\\", "FortniteClient-Win64-Shipping.exe")))
            {
                Game._FortniteProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        Arguments = $"-AUTH_LOGIN=unused -AUTH_PASSWORD={token} -AUTH_TYPE=exchangecode " + args,
                        FileName = Path.Combine(PATH, "FortniteGame\\Binaries\\Win64\\", "FortniteClient-Win64-Shipping.exe")
                    },
                    EnableRaisingEvents = true
                };
                Game._FortniteProcess.Exited += new EventHandler(Game.OnFortniteExit);
                Game._FortniteProcess.Start();


            }

        }

        public static void OnFortniteExit(object sender, EventArgs e)
        {
            Process fortniteProcess = Game._FortniteProcess;
            if (fortniteProcess != null && fortniteProcess.HasExited)
            {
                Game._FortniteProcess = (Process)null;
            }
            FakeAC._FNLauncherProcess?.Kill();
            FakeAC._FNAntiCheatProcess?.Kill();
        }
    }
}