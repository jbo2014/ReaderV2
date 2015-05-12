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
            //string sql = "SELECT * FROM Manga WHERE ChpID = "+chp+" ORDER BY No";
            string sql = "SELECT Manga.ID,Manga.No,Manga.Contents,Mark.ID as MkID FROM Manga LEFT JOIN Mark ON Manga.ChpID=Mark.ChpID AND Manga.No=Mark.Page WHERE Manga.ChpID = " + chp + " ORDER BY No";
            SQLiteDataReader sdr = DBHelper.ExecuteReader(sql);
            string shmak = null;
            int i = 0;
            while (sdr.Read())
            {
                //if (i % 2 == 0)
                //    shmak = "Visible";
                //else
                //    shmak = "Hidden";
                if(!string.IsNullOrEmpty(sdr["MkID"].ToString()))
                    shmak = "Visible";
                else
                    shmak = "Hidden";
                volList.Add(new Manga() { ID = Convert.ToInt32(sdr["Id"]), No = Convert.ToInt32(sdr["No"]), Img_des = GetImgSource((byte[])sdr["Contents"]), ShowMark = shmak });
                i++;
            }
            this.myBook.ItemsSource = volList;
            
            if (Application.Current.Properties["Page"] != null)
                this.myBook.CurrentSheetIndex = (Convert.ToInt32(Application.Current.Properties["Page"]) - 1) / 2;
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
            //if (this.myBook.CurrentSheetIndex < this.myBook.GetItemsCount() / 2)
            if (this.myBook.CurrentSheetIndex + 1 <= (this.myBook.GetItemsCount() - 1) / 2)
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
