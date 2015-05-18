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
using System.Configuration;

using ReaderV2.Helper;
using WpfFlipPageControl;
using ReaderV2.Models;
using System.Data.SQLite;

namespace ReaderV2.Views.Shared
{
    /// <summary>
    /// FootMenu.xaml 的交互逻辑
    /// </summary>
    public partial class FootMenu : UserControl
    {
        private string pageType;
        //private Object curViewer;
        private CtrlBook cb;

        public FootMenu()
        {
            InitializeComponent();
        }

        private void FootLoad(object sender, RoutedEventArgs e)
        {
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (this.PageType == "TextViewer" || this.PageType == "MangaViewer")
            {
                //MakCanv.Loaded += MakLoad;

                mChp.Checked += OpenChp;

                mPro.Checked += OpenProSet;
                mPro.Unchecked += CloseProSet;

                mMak.Checked += OpenMakSet;
                mMak.Unchecked += CloseMakSet;

                SdrBar.ValueChanged += JumpPage;

                mSet.Checked += OpenSetSet;
                mSet.Unchecked += CloseSetSet;

                //Text\Manga下允许打开设置面板的翻页设置
                mSet.Checked += OpenSetSet;
                mSet.Unchecked += CloseSetSet;

                string turn = cfa.AppSettings.Settings["isTurnPage"].Value;
                RadioButton rbp = SetCanv.FindName("p" + turn) as RadioButton;
                rbp.RemoveHandler(RadioButton.CheckedEvent, new RoutedEventHandler(ChangSet));
                rbp.IsChecked = true;
                shadeImg.Visibility = Visibility.Visible;
            }
            if (this.PageType == "TextViewer")
            {
                string size = cfa.AppSettings.Settings["fontSize"].Value;
                string theme = cfa.AppSettings.Settings["theme"].Value;
                
                RadioButton rbs = SetCanv.FindName("f" + size) as RadioButton;
                rbs.RemoveHandler(RadioButton.CheckedEvent, new RoutedEventHandler(ChangSet));
                rbs.IsChecked = true;
                RadioButton rbt = SetCanv.FindName("t" + theme) as RadioButton;
                rbt.RemoveHandler(RadioButton.CheckedEvent, new RoutedEventHandler(ChangSet));
                rbt.IsChecked = true;
                shadeImg.Visibility = Visibility.Hidden;
            }
        }

        private void OpenProSet(object sender, RoutedEventArgs e)
        {
            Image img = mPro.Content as Image;
            img.Source = new BitmapImage(new Uri("/ReaderV2;component/Assets/progress1.png", UriKind.Relative));
            this.ProCanv.Visibility = Visibility.Visible;

            SdrBar.Maximum = Convert.ToDouble(this.Cb.GetItemsCount());
            SdrBar.Value = Convert.ToDouble(this.Cb.CurrentSheetIndex * 2 + 1);
            cot.Text = "/" + this.Cb.GetItemsCount().ToString();
        }

        private void OpenSetSet(object sender, RoutedEventArgs e)
        {
            Image img = mSet.Content as Image;
            img.Source = new BitmapImage(new Uri("/ReaderV2;component/Assets/setup1.png", UriKind.Relative));
            this.SetCanv.Visibility = Visibility.Visible;
        }

        //打开书签面板时
        private void OpenMakSet(object sender, RoutedEventArgs e)
        {
            Image img = mMak.Content as Image;
            img.Source = new BitmapImage(new Uri("/ReaderV2;component/Assets/mark1.png", UriKind.Relative));
            this.MakCanv.Visibility = Visibility.Visible;

            if (this.PageType == "MangaViewer")
            {
                Manga mga = this.Cb.Items[this.Cb.CurrentSheetIndex * 2] as Manga;
                if (mga.ShowMark.ToLower() == "visible")
                {
                    Mark.IsChecked = true;
                    MakLab.Text = "删除书签";
                }
                else
                {
                    Mark.IsChecked = false;
                    MakLab.Text = "添加书签";
                }
            }
            else if (this.PageType == "TextViewer") 
            {
                Text txt = this.Cb.Items[this.Cb.CurrentSheetIndex * 2] as Text;
                if (txt.ShowMark.ToLower() == "visible")
                {
                    Mark.IsChecked = true;
                    MakLab.Text = "删除书签";
                }
                else
                {
                    Mark.IsChecked = false;
                    MakLab.Text = "添加书签";
                }
            }

            if (this.PageType == "MarkViewer")
            {
                Jilu.IsChecked = true;
            }
            else
            {
                Jilu.IsChecked = false;
            }
        }

        private void CloseProSet(object sender, RoutedEventArgs e)
        {
            Image img = mPro.Content as Image;
            img.Source = new BitmapImage(new Uri("/ReaderV2;component/Assets/progress0.png", UriKind.Relative));
            this.ProCanv.Visibility = Visibility.Hidden;
        }

        private void CloseSetSet(object sender, RoutedEventArgs e)
        {
            Image img = mSet.Content as Image;
            img.Source = new BitmapImage(new Uri("/ReaderV2;component/Assets/setup0.png", UriKind.Relative));
            this.SetCanv.Visibility = Visibility.Hidden;
        }

        private void CloseMakSet(object sender, RoutedEventArgs e)
        {
            Image img = mMak.Content as Image;
            img.Source = new BitmapImage(new Uri("/ReaderV2;component/Assets/mark0.png", UriKind.Relative));
            this.MakCanv.Visibility = Visibility.Hidden;
        }

        private void OpenChp(object sender, RoutedEventArgs e)
        {
            NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Chapter.xaml", UriKind.Relative));
        }

        //private void abc(object sender, RoutedEventArgs e)
        //{
        //    //App.DoEvents();

        //    if (NavigationService.GetNavigationService(this).Content.GetType().Name == "DeskViewer") 
        //    {
        //        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //        cfa.AppSettings.Settings["theme"].Value = "#5C5B59";
        //        cfa.Save(ConfigurationSaveMode.Modified);　　//这个模式的话是将修改的属性写出到配置文件，即使值和继承值相同。
        //        ConfigurationManager.RefreshSection("appSettings");

        //        DeskViewer dv = (DeskViewer)NavigationService.GetNavigationService(this).Content;
        //        dv.Books.Children.Remove(dv.Books.Children[0]);
        //        if (Application.Current.Properties["Type"].ToString() == "2" || Application.Current.Properties["Type"].ToString() == "4")
        //        {
        //            dv.Books.Children.Add(new TextViewer());
        //        }
        //    }
        //}



        /********************************************* 进度条操作 ****************************************/
        //上一章 v="-1"; 下一章v="+1"
        public void JumpChp(object sender, MouseButtonEventArgs e)
        {
            string v = "+1";
            TextBlock tb = sender as TextBlock;
            if (tb.Uid == "lChp")
            {
                v = "-1";
            }

            string chp = Application.Current.Properties["Chp"].ToString();
            string sql = "select b.id from Chapter as a INNER JOIN Chapter as b on a.VolID=b.VolID and a.No" + v + "=b.No where a.ID=" + chp;
            Application.Current.Properties["Chp"] = DBHelper.GetSingle(sql).ToString();

            if (Application.Current.Properties["Type"].ToString() == "1" || Application.Current.Properties["Type"].ToString() == "3")  //漫画、绘本
            {
                App.DoEvents();
                NavigationService.GetNavigationService(this).Content = new MangaViewer();
            }
            else if (Application.Current.Properties["Type"].ToString() == "2" || Application.Current.Properties["Type"].ToString() == "4")  //小说、百科
            {
                App.DoEvents();
                NavigationService.GetNavigationService(this).Content = new TextViewer();
            }
        }

        //slider改变页数
        private void JumpPage(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.Cb.CurrentSheetIndex = (Convert.ToInt32(SdrBar.Value)-1)/2;
        }



        /********************************************* 设置项操作 ****************************************/
        private void ChangSet(object sender, RoutedEventArgs e)
        {
            RadioButton rb = e.OriginalSource as RadioButton;
            if (this.PageType == "TextViewer" || this.PageType == "MangaViewer")
            {
                Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                //cfa.AppSettings.Settings[rb.GroupName].Value = rb.Name.Substring(1);
                cfa.AppSettings.Settings.Remove(rb.GroupName);
                cfa.AppSettings.Settings.Add(rb.GroupName, rb.Name.Substring(1));
                cfa.Save(ConfigurationSaveMode.Modified);　　//这个模式的话是将修改的属性写出到配置文件，即使值和继承值相同。
                ConfigurationManager.RefreshSection("appSettings");
            }

            if(this.PageType == "TextViewer")
            {
                NavigationService.GetNavigationService(this).Content = new TextViewer();
            }
            else if (this.PageType == "MangaViewer")
            {
                NavigationService.GetNavigationService(this).Content = new MangaViewer();
            }
        }
        //跳到书签页
        private void Url2Mark(object sender, RoutedEventArgs e)
        {
            if (NavigationService.GetNavigationService(this) != null)
                NavigationService.GetNavigationService(this).Navigate(new Uri("/Views/Mark.xaml", UriKind.Relative));
        }



        /********************************************* 书签项操作 ****************************************/
        private void AddMark(object sender, RoutedEventArgs e)
        {
            bool bol = DBHelper.Exists("SELECT ID FROM Mark WHERE ChpID=" + Application.Current.Properties["Chp"].ToString() + " AND Page=" +(this.Cb.CurrentSheetIndex * 2 + 1));
            if (bol)
                return;
                
            string sql = "INSERT INTO Mark (BokID,VolID,ChpID,Page,Time) VALUES (@BokID,@VolID,@ChpID,@Page,@Time)";
            SQLiteParameter[] sps = new SQLiteParameter[]{
                new SQLiteParameter("BokID", Application.Current.Properties["Book"].ToString()),
                new SQLiteParameter("VolID", Application.Current.Properties["Vol"].ToString()),
                new SQLiteParameter("ChpID", Application.Current.Properties["Chp"].ToString()),
                new SQLiteParameter("Page", this.Cb.CurrentSheetIndex * 2 + 1),
                new SQLiteParameter("Time", Switch.DateTimeToStamp(DateTime.Now))
            };
            DBHelper.ExecuteSql(sql, sps);

            if (this.PageType == "MangaViewer")
            {
                Manga mga = this.Cb.Items[this.Cb.CurrentSheetIndex * 2] as Manga;
                mga.ShowMark = "Visible";

                MangaViewer tv = NavigationService.GetNavigationService(this).Content as MangaViewer;
                tv.xMark.Visibility = Visibility.Visible;
            }
            else if (this.PageType == "TextViewer") 
            {
                Text txt = this.Cb.Items[this.Cb.CurrentSheetIndex * 2] as Text;
                txt.ShowMark = "Visible";

                TextViewer tv = NavigationService.GetNavigationService(this).Content as TextViewer;
                tv.xMark.Visibility = Visibility.Visible;
            }
            
        }

        private void DelMark(object sender, RoutedEventArgs e)
        {
            string sql = "DELETE FROM Mark WHERE ChpID=" + Application.Current.Properties["Chp"].ToString() + " AND Page=" + (this.Cb.CurrentSheetIndex * 2 + 1);
            DBHelper.ExecuteSql(sql);

            if (this.PageType == "MangaViewer")
            {
                Manga mga = this.Cb.Items[this.Cb.CurrentSheetIndex * 2] as Manga;
                mga.ShowMark = "Hidden";
            }
            else if (this.PageType == "TextViewer")
            {
                Text txt = this.Cb.Items[this.Cb.CurrentSheetIndex * 2] as Text;
                txt.ShowMark = "Hidden";
            }
        }




        /********************************************* 辅助函数 ******************************************/
        //得到当前页面类型名
        public string PageType
        {
            get { 
                if(pageType==null && NavigationService.GetNavigationService(this)!=null)
                    return NavigationService.GetNavigationService(this).Content.GetType().Name;
                return pageType;
            }
            set { pageType = value; }
        }

        //得到 TextViewer 或 MangaViewer 的 CtrlBook：mybook
        public CtrlBook Cb
        {
            get {
                if (this.PageType!=null && this.PageType == "TextViewer")
                {
                    TextViewer tv = NavigationService.GetNavigationService(this).Content as TextViewer;
                    cb = tv.myBook;
                }
                else if (this.PageType != null && this.PageType == "MangaViewer")
                {
                    MangaViewer mv = NavigationService.GetNavigationService(this).Content as MangaViewer;
                    cb = mv.myBook;
                }
                return cb; 
            }
            set { cb = value; }
        }
    }
}
