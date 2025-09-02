using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using VRChatScreenshotLinker.Properties;
using VRCX;

namespace VRChatScreenshotLinker
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            // "항상 위" 설정 불러오기
            bool alwaysOnTop = Settings.Default.AlwaysOnTop;
            TopmostCheckBox.IsChecked = alwaysOnTop;
            Topmost = alwaysOnTop;

            // "브라우저 열기" 설정 불러오기
            OpenBrowserCheckBox.IsChecked = Settings.Default.OpenBrowserOnDrop;
        }

        private void DropZone_DragEnter(object sender, DragEventArgs e)
        {
            DropZoneBorder.BorderBrush = Brushes.DeepSkyBlue;
            DropZoneBorder.Background = new SolidColorBrush(Color.FromArgb(40, 0, 191, 255));
        }

        private void DropZone_DragLeave(object sender, DragEventArgs e)
        {
            DropZoneBorder.BorderBrush = Brushes.Transparent;
            DropZoneBorder.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));
        }

        private void DropZone_Drop(object sender, DragEventArgs e)
        {
            DropZoneBorder.BorderBrush = Brushes.Transparent;
            DropZoneBorder.Background = new SolidColorBrush(Color.FromArgb(0, 255, 255, 255));

            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (var file in files)
            {
                if (!File.Exists(file) || !file.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show($"{Path.GetFileName(file)} 은(는) PNG 파일이 아닙니다.",
                        "오류", MessageBoxButton.OK, MessageBoxImage.Warning);
                    continue;
                }

                var metadata = ScreenshotHelper.GetScreenshotMetadata(file);
                // 기본 정보 표시
                var metadataInfo = new List<KeyValuePair<string, string>>
                {
                    new("File", Path.GetFileName(file)),
                    new("World ID", metadata?.World.Id ?? "(알 수 없음)"),
                    new("World Name", metadata?.World.Name ?? "(알 수 없음)"),
                    new("Instance ID", metadata?.World.InstanceId ?? "(알 수 없음)"),
                    new("Author", $"{metadata?.Author.DisplayName ?? "(알 수 없음)"} ({metadata?.Author.Id ?? "알 수 없음"})"),
                    new("Timestamp", metadata?.Timestamp?.ToString() ?? "(알 수 없음)"),
                    new("Note", metadata?.Note ?? "(없음)")
                };
                MetadataGrid.ItemsSource = metadataInfo;

                if (metadata == null || metadata.World == null || string.IsNullOrEmpty(metadata.World.Id))
                    continue;

                // 체크박스가 선택된 경우에만 브라우저 열기
                if (OpenBrowserCheckBox.IsChecked == true)
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

        private void TopmostCheckBox_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked = TopmostCheckBox.IsChecked == true;
            Topmost = isChecked;
            Settings.Default.AlwaysOnTop = isChecked;
            Settings.Default.Save();
        }

        private void OpenBrowserCheckBox_Click(object sender, RoutedEventArgs e)
        {
            Settings.Default.OpenBrowserOnDrop = OpenBrowserCheckBox.IsChecked == true;
            Settings.Default.Save();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Settings.Default.Save();
        }
    }
}
