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

using ReaderV2.Views.Shared;

namespace ReaderV2.Views
{
    /// <summary>
    /// Desk.xaml 的交互逻辑
    /// </summary>
    public partial class DeskViewer : Page
    {
        public DeskViewer()
        {
            InitializeComponent();
            this.Background = Brushes.White;
        }

        private void DeskLoaded(object sender, RoutedEventArgs e)
        {
            string type = Application.Current.Properties["Type"].ToString();
            if(type == "1" || type == "3")
            {
                MangaViewer mv = new MangaViewer();
                this.Books.Children.Add(mv);
                this.Books.Children[0].Uid = "mv";
            }
            else if (type == "2" || type == "4")
            {
                TextViewer tv = new TextViewer();
                this.Books.Children.Add(tv);
                this.Books.Children[0].Uid = "tv";
            }
        }
    }
}
