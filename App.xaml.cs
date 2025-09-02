using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using VRCX;

namespace VRChatScreenshotLinker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length > 0)
            {
                foreach (var filePath in e.Args)
                {
                    if (File.Exists(filePath) && filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                    {
                        var metadata = ScreenshotHelper.GetScreenshotMetadata(filePath);
                        if (metadata?.World?.Id != null)
                        {
                            string worldUrl = $"https://vrchat.com/home/world/{metadata.World.Id}";
                            try
                            {
                                Process.Start(new ProcessStartInfo
                                {
                                    FileName = worldUrl,
                                    UseShellExecute = true
                                });
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"브라우저 열기 실패:\n{ex.Message}", "오류",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }

                Shutdown();
            }
            else
            {
                base.OnStartup(e);
            }
        }
    }

}
