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
using System.IO;
using System.Configuration;

using ReaderV2.Models;
using ReaderV2.Helper;
using WpfFlipPageControl;

namespace ReaderV2.Views
{
    /// <summary>
    /// Manga.xaml 的交互逻辑
    /// </summary>
    public partial class MangaViewer : Page
    {
        public MangaViewer()
        {
            InitializeComponent();
            this.Background = Brushes.White;

            string turn = ConfigurationManager.AppSettings["isTurnPage"];
            if (turn == "1")
            {
                prevBut.Click += AutoPreviousClick;
                nextBut.Click += AutoNextClick;
            }
            else
            {
                prevBut.Click += PreviousClick;
                nextBut.Click += NextClick;
            }
        }

        private void MangaLoad(object sender, RoutedEventArgs e)
        {
            List<Manga> volList = new List<Manga>();

            //string bok = Application.Current.Properties["Book"].ToString();
            string chp = Application.Current.Properties["Chp"].ToString();
            string sql = "SELECT * FROM Manga WHERE ChpID = "+chp+" ORDER BY No";
            SQLiteDataReader sdr = DBHelper.ExecuteReader(sql);
            string shmak = "Hidden";
            while (sdr.Read())
            {
                volList.Add(new Manga() { ID = Convert.ToInt32(sdr["Id"]), No = Convert.ToInt32(sdr["No"]), Img_des = GetImgSource((byte[])sdr["Contents"]), ShowMake = "Visible" });
            }
            this.myBook.ItemsSource = volList;
        }

        public BitmapImage GetImgSource(byte[] codeByte)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;

            byte[] dec = Encrypt.DecryptByte(codeByte);

            bitmap.StreamSource = new MemoryStream(dec);
            bitmap.EndInit();
            return bitmap;
        }


        /***************************************************** 翻页控件 ***************************************************/

        private void NextClick(object sender, RoutedEventArgs args)
        {
            if (this.myBook.CurrentSheetIndex < this.myBook.GetItemsCount() / 2)
                this.myBook.CurrentSheetIndex++;
        }

        private void PreviousClick(object sender, RoutedEventArgs args)
        {
            if (this.myBook.CurrentSheetIndex > 0)
                this.myBook.CurrentSheetIndex--;
        }

        private void AutoNextClick(object sender, RoutedEventArgs e)
        {
            this.myBook.AnimateToNextPage(false, 700);
            this.myBook.Focus();
        }

        private void AutoPreviousClick(object sender, RoutedEventArgs e)
        {
            this.myBook.AnimateToPreviousPage(false, 700);
            this.myBook.Focus();
        }
    }
}
