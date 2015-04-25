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

using WpfFlipPageControl;
using ReaderV2.Models;

namespace ReaderV2.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void abc(object sender, RoutedEventArgs e)
        {
            List<Volume> v = new List<Volume>() 
            {
                new Volume(){ ID=1, BokID=178 },
                new Volume(){ ID=2, BokID=2999 },
                new Volume(){ ID=3, BokID=32345 },
                new Volume(){ ID=4, BokID=47654 },
                new Volume(){ ID=5, BokID=5456 },
            };

            this.myBook.ItemsSource = v;
            //this.myBook.DisplayMemberPath = "BokID";

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
            //this.myBook.AnimateToNextPage(this.cbFromTop.SelectedIndex == 0, 700);
            this.myBook.AnimateToNextPage(false, 700);
            this.myBook.Focus();
        }

        private void AutoPreviousClick(object sender, RoutedEventArgs e)
        {
            //this.myBook.AnimateToPreviousPage(this.cbFromTop.SelectedIndex == 0, 700);
            this.myBook.AnimateToPreviousPage(false, 700);
            this.myBook.Focus();
        }
    }
}
