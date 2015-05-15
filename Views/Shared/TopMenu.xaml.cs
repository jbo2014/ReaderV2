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

using ReaderV2.Helper;
using ReaderV2.Views;


namespace ReaderV2.Views.Shared
{
    /// <summary>
    /// TopMenu.xaml 的交互逻辑
    /// </summary>
    public partial class TopMenu : UserControl
    {
        public TopMenu()
        {
            InitializeComponent();
        }

        private void TopLoaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties["Book"] != null && Application.Current.Properties["Type"] != null)
            {
                string Typ = Application.Current.Properties["Type"].ToString();
                string Bok = Application.Current.Properties["Book"].ToString();
                string tit = DBHelper.GetSingle("select b.Title from book as b left join Type as t on b.id=t.bokid where t.id="+Bok).ToString();

                Dictionary<string, string> Tys = new Dictionary<string,string>();
                Tys.Add("1"," 漫 画");
                Tys.Add("2"," 原 著 小 说");
                Tys.Add("3"," 少 年 绘 本");
                Tys.Add("4"," 百 科");
                this.Title.Text = "神 界 《 " + StrInstSpace(tit, " ") + " 》" + Tys[Typ];
            }
            else
            {
                this.Title.Text = "神 界 漫 画 《 四 大 名 著 》 数 字 全 集";
            }
        }

        private string StrInstSpace(string str, string spa) 
        {
            string tmp = str;
            int lsp = spa.Length;
            int lst = str.Length;
            int i = 1;
            while (i < lst + (lst - 1) * lsp) 
            {
                tmp = tmp.Insert(i, spa);
                i += lsp + 1;
            }
            return tmp;
        }

        private void RemoveWin(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }

        private void BackPrePage(object sender, MouseButtonEventArgs e)
        {
            if (NavigationService.GetNavigationService(this).Content.GetType().Name == "VolumeViewer")
            {
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Index.xaml", UriKind.Relative));
            }
            else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "IndexViewer" && (Application.Current.Properties["Type"] != null && Application.Current.Properties["Type"].ToString() == "4"))
            {
                Application.Current.Properties["Type"] = null;
                //NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Index.xaml", UriKind.Relative));
                App.DoEvents();
                NavigationService.GetNavigationService(this).Content = new IndexViewer();
            }
            else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "CoverViewer")
            {
                Application.Current.Properties["Vol"] = null;
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Volume.xaml", UriKind.Relative));
            }
            else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "ChapterViewer")
            {
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Cover.xaml", UriKind.Relative));
            }
            //else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "DeskViewer")
            else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "TextViewer" || NavigationService.GetNavigationService(this).Content.GetType().Name == "MangaViewer")
            {
                Application.Current.Properties["Chp"] = null;
                if(Application.Current.Properties["Page"]!=null)
                    Application.Current.Properties["Page"] = null;
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Chapter.xaml", UriKind.Relative));
            }
            else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "MarkViewer")
            {
                NavigationService.GetNavigationService(this).GoBack();
            }
            else 
            {
                Application.Current.MainWindow.Close();
            }
        }

        //直接跳到百科
        private void Fly2Wiki(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Properties["Type"] = "4";
            if (Application.Current.Properties.Contains("Book") && Application.Current.Properties["Book"]!=null)
            {
                if (NavigationService.GetNavigationService(this).Content.GetType().Name == "VolumeViewer")
                {
                    NavigationService.GetNavigationService(this).Content = new VolumeViewer();
                }
                else
                {
                    NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Volume.xaml", UriKind.Relative));
                }
            }
            else
            {
                //NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Index.xaml", UriKind.Relative));
                NavigationService.GetNavigationService(this).Content = new IndexViewer();
            }
        }

    }
}
