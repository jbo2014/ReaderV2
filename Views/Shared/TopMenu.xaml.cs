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
                Tys.Add("1","漫画");
                Tys.Add("2","原著小说");
                Tys.Add("3","少年绘本");
                Tys.Add("4","百科");
                this.Title.Text = "神界《" + tit + "》" + Tys[Typ];
            }
            else
            {
                this.Title.Text = "神界漫画《四大名著》数字全集";
            }
        }

        private void RemoveWin(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }

        private void BackPrePage(object sender, MouseButtonEventArgs e)
        {
            //if (NavigationService.GetNavigationService(this).CanGoBack)// && this.Parent.InheritanceParent.GetType().Name!="Index")
            //{
            //    NavigationService.GetNavigationService(this).GoBack();
            //}
            //else 
            //{
            //    Application.Current.MainWindow.Close();
            //}
            if (NavigationService.GetNavigationService(this).Content.GetType().Name == "VolumeViewer")
            {
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Index.xaml", UriKind.Relative));
            }
            else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "IndexViewer" && (Application.Current.Properties["Type"] != null && Application.Current.Properties["Type"].ToString() == "4"))
            {
                Application.Current.Properties["Type"] = null;
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Index.xaml", UriKind.Relative));
            }
            else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "CoverViewer")
            {
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Volume.xaml", UriKind.Relative));
            }
            else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "ChapterViewer")
            {
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Cover.xaml", UriKind.Relative));
            }
            //else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "DeskViewer")
            else if (NavigationService.GetNavigationService(this).Content.GetType().Name == "TextViewer" || NavigationService.GetNavigationService(this).Content.GetType().Name == "MangaViewer")
            {
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Chapter.xaml", UriKind.Relative));
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
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Volume.xaml", UriKind.Relative));
            }
            else
            {
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Index.xaml", UriKind.Relative));
            }
        }

    }
}
