using System;
using System.Windows;
using HyggeIMaoTai;
using HyggeIMaoTai.Entity;
using HyggeIMaoTai.Domain;
using HyggeIMaoTai.UserInterface.Component;
using NLog;

namespace HyggeIMaoTai.UserInterface.UserControls
{
    /// <summary>
    /// AppointProjectUserControl.xaml 的交互逻辑
    /// </summary>
    public partial class AppointProjectUserControl : System.Windows.Controls.UserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public AppointProjectUserControl()
        {
            InitializeComponent();
            DataContext = new AppointProjectViewModel();
        }

        private async void RefreshProductButton_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshProductButton.IsEnabled = false;
            try
            {
                App.MtSessionId = string.Empty;
                App.WriteCache("mtSessionId.txt", string.Empty);
                AppointProjectViewModel.ProductList.Clear();
                await IMTService.GetCurrentSessionId();
                Logger.Info("商品列表已刷新");
                new MessageBoxCustom("商品列表已刷新", MessageType.Success, MessageButtons.Ok).ShowDialog();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"刷新商品列表失败: {ex.Message}");
                new MessageBoxCustom($"刷新商品列表失败：{ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
            finally
            {
                RefreshProductButton.IsEnabled = true;
            }
        }
    }
}
