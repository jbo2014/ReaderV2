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
using System.Configuration;

using ReaderV2.Helper;

namespace ReaderV2.Views
{
    /// <summary>
    /// Cover.xaml 的交互逻辑
    /// </summary>
    public partial class CoverViewer : Page
    {
        public CoverViewer()
        {
            InitializeComponent();
            this.Background = Brushes.White;
        }

        private void ImgLoaded(object sender, RoutedEventArgs e)
        {
            string volID = Application.Current.Properties["Vol"].ToString();

            byte[] sdr = (byte[])DBHelper.GetSingle("SELECT Img FROM Cover WHERE VolID=" + volID);
            ImageCollection ic = new ImageCollection();
            this.covImg.Source = ic.GetImgSource(sdr);
        }

        private void Url2Catalog(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("Views/Chapter.xaml", UriKind.Relative));
        }
    }
}
