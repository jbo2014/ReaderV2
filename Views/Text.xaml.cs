using System;
using System.Collections;
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

namespace ReaderV2.Views
{
    /// <summary>
    /// Manga.xaml 的交互逻辑
    /// </summary>
    public partial class TextViewer : Page
    {
        public TextViewer()
        {
            InitializeComponent();
            this.Background = Brushes.White;

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
            int maxTxt = 0;
            if(fSize == "18")
            {
                maxTxt = 380;
            }
            else if (fSize == "30")
            {
                maxTxt = 187;
            }
            else 
            {
                maxTxt = 285;
            }

            //得到文本并进行分页
            string sql = "SELECT * FROM Text WHERE ChpID=" + chpID + " ORDER BY No";
            DataSet sdr = DBHelper.Query(sql);
            DataRow dr = sdr.Tables[0].Rows[0];
            string txts = dr["Contents"].ToString();
            List<string> txtArray = SplitTxt(txts, maxTxt);

            //获取书签
            sql = "SELECT Page FROM Mark WHERE ChpID=" + chpID;
            DataSet ds = DBHelper.Query(sql);
            ArrayList al = null;
            if (ds.Tables[0].Rows.Count > 0) 
            {
                al = new ArrayList();
                foreach (DataRow dr2 in ds.Tables[0].Rows) 
                {
                    al.Add(Convert.ToInt32(dr2["Page"]));
                }                
            }

            //设置文本颜色
            string fcolor = "Black";
            if (bgColor == "6E6E6E")
                fcolor = "White";
            else
                fcolor = "Black";

            string shmak = "Hidden";
            if (al!=null && al.Count > 0)
            {
                for (int i = 1; i <= txtArray.Count; i++)
                {
                    if (al.Contains(i))
                        shmak = "Visible";
                    else
                        shmak = "Hidden";

                    volList.Add(new Text() { No = i, Contents = txtArray[i - 1], BgColor = "#" + bgColor, FontSize = fSize, FontColor = fcolor, ShowMark = shmak });
                }
            }
            else
            {
                for (int i = 1; i <= txtArray.Count; i++)
                {
                    volList.Add(new Text() { No = i, Contents = txtArray[i - 1], BgColor = "#" + bgColor, FontSize = fSize, FontColor = fcolor, ShowMark = shmak });
                }
            }
            this.myBook.ItemsSource = volList;

            //由书签跳到该页面指定页数
            if (Application.Current.Properties["Page"] != null)
                this.myBook.CurrentSheetIndex = (Convert.ToInt32(Application.Current.Properties["Page"]) - 1) / 2;
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
            CloseFootMenu();
        }
        private void PreviousClick(object sender, RoutedEventArgs args)
        {
            if (this.myBook.CurrentSheetIndex > 0)
                this.myBook.CurrentSheetIndex--;
            CloseFootMenu();
        }

        private void AutoNextClick(object sender, RoutedEventArgs e)
        {
            this.myBook.AnimateToNextPage(false, 700);
            this.myBook.Focus();
            CloseFootMenu();
        }

        private void AutoPreviousClick(object sender, RoutedEventArgs e)
        {
            this.myBook.AnimateToPreviousPage(false, 700);
            this.myBook.Focus();
            CloseFootMenu();
        }


        //点击隐藏左上角书签图标，关闭foot所有弹出菜单
        private void CloseFootMenu()
        {
            xMark.Visibility = Visibility.Hidden;
            if (this.Content != null)
            {
                Foot.mPro.IsChecked = false;
                Foot.mSet.IsChecked = false;
                Foot.mMak.IsChecked = false;
            }
        }
    }
}
