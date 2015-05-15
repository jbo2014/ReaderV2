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
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.IO;
using System.Data.SQLite;

using ReaderV2.Helper;
using ReaderV2.Models;

namespace ReaderV2.Views
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class IndexViewer : Page
    {
        ImageCollection collection = null;

        public IndexViewer()
        {
            InitializeComponent();
            Application.Current.Properties["Book"] = null;
        }

        private void IndexLoaded(object sender, RoutedEventArgs e)
        {
            if (Application.Current.Properties.Contains("Start") && Application.Current.Properties["Start"] != null)
            {
                (this.Resources["main"] as Storyboard).Begin();
            }
            else
            {
                (this.Resources["intro"] as Storyboard).Begin();
                Application.Current.Properties["Start"] = "Y";
            }
        }

        private void ImgLoad(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmap = null;
            List<Index> imgList = new List<Index>();
            string equ = "<>";

            //判断是否要查看的是百科
            if(Application.Current.Properties["Type"]!=null && Application.Current.Properties["Type"].ToString()=="4")
            {
                equ = "=";
            }

            SQLiteDataReader sdr = DBHelper.ExecuteReader("SELECT * FROM Type WHERE Type " + equ + " 4 ORDER BY Type,No");
            string ii = ConfigurationManager.AppSettings["IndexImg"];
            int i = 1;
            while(sdr.Read())
            {
                if(i%2 != 0)
                {
                    Index ind0 = new Index();
                    bitmap = this.GetImgSource(ii + "T" + sdr["Type"].ToString() + "_Title.png");
                    ind0.Img_des = bitmap;
                    ind0.Margin = new Thickness(40,16,40,10);
                    ind0.Width = 410;
                    imgList.Add(ind0);
                }
                Index ind = new Index();
                bitmap = this.GetImgSource(ii + sdr["Img"].ToString() + ".png");
                ind.Img_des = bitmap;
                ind.ID = Convert.ToInt32(sdr["ID"]);
                ind.Width = 352;
                ind.Margin = new Thickness(35,20,35,100);
                ind.Type = Convert.ToInt32(sdr["Type"]);
                imgList.Add(ind);
                i++;
            }

            this.listImgs.ItemsSource = imgList;
        }

        public BitmapImage GetImgSource(String page)
        {
            Uri uri = new Uri(System.IO.Path.Combine("file://", page));
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.UriSource = uri;
            bitmap.EndInit();
            return bitmap;
        }

        private void Url2Volume(object sender, MouseButtonEventArgs e) 
        {
            //NavigationService.Navigate(new Uri("Views/Volume.xaml", UriKind.Relative),"0103");
            //Volume vol = new Volume();
            //vol.Vol = "12";
            //NavigationService.Navigate(vol);
            Image src = sender as Image;
            Application.Current.Properties["Book"] = src.Uid;
            Application.Current.Properties["Type"] = src.Tag;
            NavigationService.Navigate(new Uri("Views/Volume.xaml", UriKind.Relative));
		}

        private void PageDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
                return;
            int rows = 6;
            //判断是否要查看的是百科
            if (Application.Current.Properties["Type"] != null && Application.Current.Properties["Type"].ToString() == "4")
            {
                rows = 2;
            }
            Double dy0 = 88D;
            Double range = 740D;
            Double dy = Books.TransformToAncestor(this).Transform(new Point(0, 0)).Y;
            if (dy > (rows - 1) * -range + dy0)
            {
                DoubleAnimation daY = new DoubleAnimation();
                daY.By = -range;
                Duration d = new Duration(TimeSpan.FromMilliseconds(300));
                daY.Duration = d;
                this.tt.BeginAnimation(TranslateTransform.YProperty, daY);
            }
            else if (dy == (rows - 1) * -range + dy0)
            {
                DoubleAnimationUsingKeyFrames dakY = new DoubleAnimationUsingKeyFrames();
                dakY.Duration = new Duration(TimeSpan.FromMilliseconds(800));

                LinearDoubleKeyFrame x_fk1 = new LinearDoubleKeyFrame();
                EasingDoubleKeyFrame x_fk2 = new EasingDoubleKeyFrame();

                BounceEase be = new BounceEase();
                be.Bounces = 3;
                be.Bounciness = 3;
                x_fk2.EasingFunction = be;

                x_fk1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100));
                x_fk1.Value = -200;
                x_fk2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(800));
                x_fk2.Value = 0;

                dakY.KeyFrames.Add(x_fk1);
                dakY.KeyFrames.Add(x_fk2);
                this.tt.BeginAnimation(TranslateTransform.YProperty, dakY);
            }
        }

        private void PageUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
                return;
            Double dy0 = 88D;
            Double range = 740D;
            Double dy = Books.TransformToAncestor(this).Transform(new Point(0, 0)).Y;
            if (dy < dy0) //Books正常下滑
            {
                DoubleAnimation daY = new DoubleAnimation();
                daY.By = range;
                Duration d = new Duration(TimeSpan.FromMilliseconds(300));
                daY.Duration = d;
                this.tt.BeginAnimation(TranslateTransform.YProperty, daY);
            }
            else if (dy == dy0) //Books下滑到顶点的情况
            {
                DoubleAnimationUsingKeyFrames dakY = new DoubleAnimationUsingKeyFrames();
                dakY.Duration = new Duration(TimeSpan.FromMilliseconds(800));
                
                LinearDoubleKeyFrame x_fk1 = new LinearDoubleKeyFrame();
                EasingDoubleKeyFrame x_fk2 = new EasingDoubleKeyFrame();

                BounceEase be = new BounceEase();
                be.Bounces = 3;
                be.Bounciness = 3;
                x_fk2.EasingFunction = be;

                x_fk1.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(100));
                x_fk1.Value = 200;
                x_fk2.KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(800));
                x_fk2.Value = 0;

                dakY.KeyFrames.Add(x_fk1);
                dakY.KeyFrames.Add(x_fk2);
                this.tt.BeginAnimation(TranslateTransform.YProperty,dakY);
            }
        }

    }
}
