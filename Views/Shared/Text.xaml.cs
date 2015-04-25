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
using System.Data;
using System.Configuration;

using ReaderV2.Models;
using ReaderV2.Helper;
using WpfFlipPageControl;

namespace ReaderV2.Views.Shared
{
    /// <summary>
    /// Manga.xaml 的交互逻辑
    /// </summary>
    public partial class TextViewer : UserControl
    {
        public TextViewer()
        {
            InitializeComponent();

            // 根据app.config文件设置是否翻页
            string turn = ConfigurationManager.AppSettings["isTurnPage"];
            if(turn=="1")
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

        private void TextLoad(object sender, RoutedEventArgs e)
        {
            string bgColor = ConfigurationManager.AppSettings["theme"];
            string fSize = ConfigurationManager.AppSettings["fontSize"];
            string chpID = Application.Current.Properties["Chp"].ToString();
            List<Text> volList = new List<Text>();
            int maxTxt = 300;
            if(fSize == "16")
            {
                maxTxt = 400;
            }
            else if (fSize == "14")
            {
                maxTxt = 500;
            }

            string sql = "SELECT * FROM Text WHERE ChpID=" + chpID + " ORDER BY No";
            DataSet sdr = DBHelper.Query(sql);
            DataRow dr = sdr.Tables[0].Rows[0];
            string txts = dr["Contents"].ToString();
            List<string> txtArray = SplitTxt(txts, maxTxt);
            
            for (int i = 1; i <= txtArray.Count; i++ )
            {
                volList.Add(new Text() { No = i, Contents = txtArray[i-1], BgColor = bgColor, FontSize = fSize });
            }
            this.myBook.ItemsSource = volList;
        }

        private List<string> SplitTxt(string txts, int max) 
        {
            int len = txts.Length;
            int no = (int)(Math.Ceiling((double)len / (double)max));
            List<string> txta = new List<string>();

            for(int i=0; i<no; i++)
            {
                if(i==(no-1))
                {
                    txta.Add(txts);
                    break;
                }
                txta.Add(txts.Substring(0, max));
                txts = txts.Remove(0,max);
            }
            return txta;
        }

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
