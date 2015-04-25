using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Data.SQLite;
using System.Collections.ObjectModel;

using ReaderV2.Helper;
using ReaderV2.Models;

namespace ReaderV2.Views
{
    /// <summary>
    /// Volume.xaml 的交互逻辑
    /// </summary>
    public partial class VolumeViewer : Page
    {
        private string _Vol = "";
        private int pag = 0;

        public VolumeViewer()
        {
            InitializeComponent();
        }

        private void VolumeLoad(object sender, RoutedEventArgs e)
        {
            List<Volume> volList = new List<Volume>();

            string sql = "";
            string bok = Application.Current.Properties["Book"].ToString();
            if (Application.Current.Properties["Type"] != null && Application.Current.Properties["Type"].ToString() == "4")
            {
                sql = "SELECT ID,TITLE,IMG FROM VOLUME WHERE BOKID = (SELECT ID FROM TYPE WHERE BOKID = (SELECT BOKID FROM TYPE WHERE ID = "+bok+") AND TYPE = 4)";
            }
            else
            {
                sql = "SELECT ID,TITLE,IMG FROM VOLUME WHERE BokID = " + bok + " ORDER BY No";
            }
            SQLiteDataReader sdr = DBHelper.ExecuteReader(sql);
            int i = 1;
            while (sdr.Read())
            {
                //if (i % 4 == 1 && i != 1)
                //{
                //    l = 60;
                //}
                volList.Add(new Volume() { ID = Convert.ToInt32(sdr["Id"]), Title = sdr["Title"].ToString(), Img_des = GetImgSource((byte[])sdr["Img"]), Margin = new Thickness(0,30,12,30), Row = i, Col = 1, Top = i*100, Left = i*100 });
                i++;
            }
            this.listSec.ItemsSource = AdjustOrder(volList);
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

        public static byte[] SerializeObj(object data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, data);
            return ms.GetBuffer();
        } 

        private void Url2Cover(object sender, MouseButtonEventArgs e)
        {
            Image src = sender as Image;
            Application.Current.Properties["Vol"] = src.Uid;
            NavigationService.Navigate(new Uri("Views/Cover.xaml", UriKind.Relative));
        }

        private void PageRight(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
                return;
            Double dx0 = 40D-(pag-1)*908D;
            //当前面板的X坐标
            Double dx = Books.TransformToAncestor(this).Transform(new Point(0, 0)).X;
            if (dx > dx0) //Books正常右滑
            {
                DoubleAnimation daX = new DoubleAnimation();
                daX.By = -908D;
                Duration d = new Duration(TimeSpan.FromMilliseconds(300));
                daX.Duration = d;
                this.tt.BeginAnimation(TranslateTransform.XProperty, daX);
            }
        }

        private void PageLeft(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
                return;            
            Double dx0 = 40D;
            //当前面板的X坐标
            Double dx = Books.TransformToAncestor(this).Transform(new Point(0, 0)).X;
            if (dx < dx0) //Books正常右滑
            {
                DoubleAnimation daX = new DoubleAnimation();
                daX.By = 908D;
                Duration d = new Duration(TimeSpan.FromMilliseconds(300));
                daX.Duration = d;
                this.tt.BeginAnimation(TranslateTransform.XProperty, daX);
            }
        }

        public string Vol
        {
          get { return _Vol; }
          set { _Vol = value; }
        }

        private ObservableCollection<Leaf> AdjustOrder(List<Volume> vl) 
        {
            pag = Convert.ToInt32(Math.Ceiling((Double)vl.Count/8D));

            ObservableCollection<Leaf> ovs = new ObservableCollection<Leaf>();
            int t = 0;
            for (int i=0; i < pag; i++) 
            {
                ObservableCollection<Volume> ov = new ObservableCollection<Volume>();//选项数组               
                for (int j = 0; j < 8; j++)
                {
                    ov.Add(vl[t]);
                    ++t;
                    if(t==vl.Count)
                        break;
                }
                ovs.Add(new Leaf(i, ov));
            }
            return ovs;
        }
    }
}
