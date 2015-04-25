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
using System.Data.SQLite;

using ReaderV2.Helper;
using ReaderV2.Models;

namespace ReaderV2.Views
{
    /// <summary>
    /// Catalog.xaml 的交互逻辑
    /// </summary>
    public partial class ChapterViewer : Page
    {
        public ChapterViewer()
        {
            InitializeComponent();
            this.Background = Brushes.White;

            Image img = this.Foot.mChp.Content as Image;
            img.Source = new BitmapImage(new Uri("/ReaderV2;component/Assets/catalog1.png", UriKind.Relative));
        }

        private void ImgLoaded(object sender, RoutedEventArgs e)
        {
            string volID = Application.Current.Properties["Vol"].ToString();
            ImageCollection ic = new ImageCollection();

            /********************* 左侧大图加载 *******************/
            byte[] dr = (byte[])DBHelper.GetSingle("SELECT Img FROM Catalog WHERE VolID=" + volID);
            this.ChpImg.Source = ic.GetImgSource(dr);

            /********************* 章回目录加载 *******************/
            
            List<Chapter> volList = new List<Chapter>();
            string sql = "SELECT * FROM Chapter WHERE VolID=" + volID + " ORDER BY No";
            SQLiteDataReader sdr = DBHelper.ExecuteReader(sql);
            while (sdr.Read())
            {
                volList.Add(new Chapter() { ID = Convert.ToInt32(sdr["Id"]), Title = sdr["Title"].ToString() });
            }
            this.listChp.ItemsSource = volList;
        }

        private void Url2Content(object sender, MouseButtonEventArgs e)
        {
            TextBlock src = sender as TextBlock;
            Application.Current.Properties["Chp"] = src.Uid;
            if (Application.Current.Properties["Type"].ToString() == "1" || Application.Current.Properties["Type"].ToString() == "3")  //漫画、绘本
            {
                NavigationService.Navigate(new Uri("Views/Manga.xaml", UriKind.Relative));
            }
            else if (Application.Current.Properties["Type"].ToString() == "2" || Application.Current.Properties["Type"].ToString() == "4")  //小说、百科
            {
                NavigationService.Navigate(new Uri("Views/Text.xaml", UriKind.Relative));
            }
        }
    }
}
