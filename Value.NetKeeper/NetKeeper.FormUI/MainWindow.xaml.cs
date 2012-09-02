using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NetKeeper.Bll.Infrastructure;
using NetKeeper.Bll;
using System.Text.RegularExpressions;
using System.Threading;
using NetKeeper.Bll.Model;

namespace NetKeeper.FormUI
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        INetKeeperService netKeeperService;

        public MainWindow()
        {
            InitializeComponent();
            netKeeperService = new NetKeeperService();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            updateCatalogList();
        }

        private void btnAddCatalog_Click(object sender, RoutedEventArgs e)
        {
            var catalogName = this.txtCatalogName.Text;
            var result = netKeeperService.AddCatalog(catalogName);
            if (result)
            {
                MessageBox.Show("添加成功");
                this.txtCatalogName.Text = String.Empty;
                updateCatalogList();
            }
            else
                MessageBox.Show("分类已存在");
        }

        private void btnAddWeb_Click(object sender, RoutedEventArgs e)
        {
            var webName = this.txtWebName.Text.Trim();
            var webUrl = this.txtWebUrl.Text.Trim();
            var webNote = this.txtWebNote.Text.Trim();
            var catalogID = this.cboCatalogName.SelectedValue;

            if (catalogID == null)
            {
                MessageBox.Show("请先选择分类");
                return;
            }
            if (String.IsNullOrEmpty(webName))
            {
                MessageBox.Show("网站名不能为空");
                return;
            }
            if (String.IsNullOrEmpty(webUrl))
            {
                MessageBox.Show("网站路由不能为空");
                return;
            }

            netKeeperService.AddWeb(catalogID.ToString(), webName, webUrl, webNote);

            this.txtWebName.Text = String.Empty;
            this.txtWebUrl.Text = String.Empty;
            this.txtWebNote.Text = String.Empty;
            this.updateCatalogList();

            MessageBox.Show("添加成功");

        }

        private void updateCatalogList()
        {
            var catalogs = netKeeperService.GetCatalog();

            this.tvNetKeeper.ItemsSource = catalogs;

            this.cboCatalogName.ItemsSource = catalogs;
            this.cboCatalogName.DisplayMemberPath = "Name";
            this.cboCatalogName.SelectedValuePath = "ID";
        }

        private void txtWebUrl_TextChanged(object sender, TextChangedEventArgs e)
        {
            var url = this.txtWebUrl.Text.Trim();
            if (validateUrl(url))
            {
                Thread thread = new Thread(() => openUrl(url));
                thread.Start();
                //// 方法2
                //Thread open = new Thread(new ParameterizedThreadStart(openUrl));
                //open.Start(url);
            }
            else
                MessageBox.Show("请输入正确网址");
        }

        private void openUrl(String url)
        {
            this.Dispatcher.Invoke(new Action(() => { this.webBrwoserAdd.Navigate(url); }));
        }

        private Boolean validateUrl(String Url)
        {
            String pattern = @"(http|https|ftp)://(\w+.){3,}(net|com|cn|org|cc|tv|[0-9]{1,3})";

            var result = Regex.IsMatch(Url, pattern, RegexOptions.IgnoreCase);
            return result;
        }

        private void tvNetKeeper_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var item = tvNetKeeper.SelectedItem as NetKeeperNode;
            if (item != null && item.Child == null)
            {
                var webInfo = netKeeperService.GetWebInfo(item.PID, item.ID);
                this.txtWebNoteCheck.Text = webInfo.Note;
                this.txtUrl.Text = webInfo.Url;
                this.webBrowserCheck.Navigate(webInfo.Url);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var item = tvNetKeeper.SelectedItem as NetKeeperNode;
            if (item != null && item.Child == null)
            {
                var webNote = this.txtWebNoteCheck.Text.Trim();
                netKeeperService.ChangeNote(item.PID, item.ID, webNote);
                MessageBox.Show("保存成功!");
            }
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetDataObject(this.txtUrl.Text);
        }
    }
}
