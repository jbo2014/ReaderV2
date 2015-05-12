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
using System.Collections.ObjectModel;

using ReaderV2.Models;
using ReaderV2.Helper;
using WpfFlipPageControl;

namespace ReaderV2.Views
{
    /// <summary>
    /// Manga.xaml 的交互逻辑
    /// </summary>
    public partial class MarkViewer : Page
    {
        public MarkViewer()
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

        private void MarkLoad(object sender, RoutedEventArgs e)
        {
            List<Mark> volList = new List<Mark>();
            
            //string sql = "SELECT Mark.ID,Mark.Page,Mark.Time,Chapter.Title FROM Mark LEFT JOIN Chapter ON Mark.ChpID=Chapter.ID ORDER BY Time";
            string sql = "SELECT Mark.ID,Mark.Page,Mark.Time,Chapter.Title FROM Mark LEFT JOIN Chapter ON Mark.ChpID=Chapter.ID ORDER BY Mark.Time DESC";
            DataSet sdr = DBHelper.Query(sql);
            if (sdr.Tables.Count<=0)
            {
                return;
            }

            foreach (DataRow dr in sdr.Tables[0].Rows)
            {
                volList.Add(new Mark() { ID = Convert.ToInt32(dr["ID"]), StrPage = "第" + dr["Page"].ToString() + "页", Title = dr["Title"].ToString(), Time = Switch.StampToDateTime(dr["Time"].ToString()).ToString() });
            }
            this.myBook.ItemsSource = AdjustOrder(volList);
        }

        //将书签按页分
        private ObservableCollection<MLeaf> AdjustOrder(List<Mark> vl)
        {
            int pag = Convert.ToInt32(Math.Ceiling((Double)vl.Count / 3D));

            ObservableCollection<MLeaf> ovs = new ObservableCollection<MLeaf>();
            int t = 0;
            for (int i = 0; i < pag; i++)
            {
                ObservableCollection<Mark> ov = new ObservableCollection<Mark>();//选项数组               
                for (int j = 0; j < 3; j++)
                {
                    ov.Add(vl[t]);
                    ++t;
                    if (t == vl.Count)
                        break;
                }
                ovs.Add(new MLeaf(i, ov));
            }
            return ovs;
        }

        //按书签跳转
        private void Url2Conts(object sender, MouseButtonEventArgs e)
        {
            TextBlock src = sender as TextBlock;
            string sql = "SELECT Mark.BokID,Mark.VolID,Mark.ChpID,Mark.Page,Type.Type FROM Mark LEFT JOIN Type ON Mark.BokID=Type.ID WHERE Mark.ID=" + src.Uid;
            DataSet sdr = DBHelper.Query(sql);
            if (sdr.Tables.Count <= 0)
                return;
            DataRow dr = sdr.Tables[0].Rows[0];
            Application.Current.Properties["Book"] = dr["BokID"];
            Application.Current.Properties["Type"] = dr["Type"];
            Application.Current.Properties["Vol"] = dr["VolID"];
            Application.Current.Properties["Chp"] = dr["ChpID"];
            Application.Current.Properties["Page"] = dr["Page"];
            if (dr["Type"].ToString() == "1" || dr["Type"].ToString() == "3")
                NavigationService.Navigate(new Uri("Views/Manga.xaml", UriKind.Relative));
            else if (dr["Type"].ToString() == "2" || dr["Type"].ToString() == "4")
                NavigationService.Navigate(new Uri("Views/Text.xaml", UriKind.Relative));
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
