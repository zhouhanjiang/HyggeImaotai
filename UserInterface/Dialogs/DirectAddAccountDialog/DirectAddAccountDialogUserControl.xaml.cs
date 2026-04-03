using System;
using System.Text.RegularExpressions;
using System.Windows;
using HyggeIMaoTai.Domain;
using HyggeIMaoTai.Entity;
using HyggeIMaoTai.Repository;
using HyggeIMaoTai.UserInterface.Component;
using HyggeIMaoTai.UserInterface.UserControls;
using MaterialDesignThemes.Wpf;
using NLog;

namespace HyggeIMaoTai.UserInterface.Dialogs.DirectAddAccountDialog
{
    /// <summary>
    /// Interaction logic for SampleDialog.xaml
    /// </summary>
    public partial class DirectAddAccountDialogUserControl
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly UserEntity _dataContext;

 
        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="isUpadte"></param>
        public DirectAddAccountDialogUserControl(UserEntity dataContext, bool isUpadte = false)
        {
            InitializeComponent();
            DataContext = dataContext;
            _dataContext = (dataContext as UserEntity)!;
           
            TitleBlock.Text = isUpadte ? "更新i茅台用户:" : "添加i茅台用户:";
            LoginButton.Content = isUpadte ? "更新" : "添加";
        }

        /// <summary>
        /// 登录按钮被单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginButton_OnClick(object sender, RoutedEventArgs e)
        {
            // 判断经纬度是否符合规范
            bool latIsInteger = Regex.IsMatch(_dataContext.Lat, @"^\d+$");
            bool latIsFloat = Regex.IsMatch(_dataContext.Lat, @"^\d+(\.\d+)?$");
            if (!latIsFloat && !latIsInteger)
            {
                new MessageBoxCustom("纬度不符合规范", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                return; // 验证失败，不关闭对话框
            }

            bool lngIsInteger = Regex.IsMatch(_dataContext.Lng, @"^\d+$");
            bool lngIsFloat = Regex.IsMatch(_dataContext.Lng, @"^\d+(\.\d+)?$");
            if (!lngIsFloat && !lngIsInteger)
            {
                new MessageBoxCustom("经度不符合规范", MessageType.Warning, MessageButtons.Ok).ShowDialog();
                return; // 验证失败，不关闭对话框
            }

            try
            {
                var foundUserEntity = UserRepository.GetUserByMobile(_dataContext.Mobile);

                if (foundUserEntity != null)
                {
                    UserRepository.UpdateUser(_dataContext);
                    UserManageControl.RefreshData(UserManageControl.UserListViewModel);
                    new MessageBoxCustom("用户信息更新成功！", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }
                else
                {
                    UserRepository.InsertUser(_dataContext);
                    UserManageControl.RefreshData(UserManageControl.UserListViewModel);
                    new MessageBoxCustom("用户添加成功！", MessageType.Success, MessageButtons.Ok).ShowDialog();
                }

                DialogHost.CloseDialogCommand.Execute(null, null);
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"保存用户失败: {ex.Message}");
                new MessageBoxCustom($"保存失败：{ex.Message}", MessageType.Error, MessageButtons.Ok).ShowDialog();
            }
        }
    }
}
