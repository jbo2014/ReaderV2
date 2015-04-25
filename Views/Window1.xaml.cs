using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Threading;


namespace ReaderV2.Views
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1Viewer : NavigationWindow
    {
        public Window1Viewer()
        {
            InitializeComponent();

            //if (this.Content != null && (this.Content.GetType().Name == "MangaViewer" || this.Content.GetType().Name == "TextViewer"))
            //    this.MouseUp += CloseFootMenu;
        }

        private void NavigationWindow_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (Content != null && !_allowDirectNavigation)
            {
                e.Cancel = true;
                _navArgs = e;
                this.IsHitTestVisible = false;
                DoubleAnimation da = new DoubleAnimation(0.3d, new Duration(TimeSpan.FromMilliseconds(300)));
                da.Completed += FadeOutCompleted;
                this.BeginAnimation(OpacityProperty, da);
            }
            _allowDirectNavigation = false;
        }

        private void FadeOutCompleted(object sender, EventArgs e)
        {
            (sender as AnimationClock).Completed -= FadeOutCompleted;

            this.IsHitTestVisible = true;

            _allowDirectNavigation = true;
            switch (_navArgs.NavigationMode)
            {
                case NavigationMode.New:
                    if (_navArgs.Uri == null)
                    {
                        NavigationService.Navigate(_navArgs.Content);
                    }
                    else
                    {
                        NavigationService.Navigate(_navArgs.Uri);
                    }
                    break;
                case NavigationMode.Back:
                    NavigationService.GoBack();
                    break;

                case NavigationMode.Forward:
                    NavigationService.GoForward();
                    break;
                case NavigationMode.Refresh:
                    NavigationService.Refresh();
                    break;
            }

            Dispatcher.BeginInvoke(DispatcherPriority.Loaded,
                (ThreadStart)delegate()
            {
                DoubleAnimation da = new DoubleAnimation(1.0d, new Duration(TimeSpan.FromMilliseconds(200)));
                this.BeginAnimation(OpacityProperty, da);
            });
        }

        //private void RemoveWin(object sender, System.Windows.Input.MouseButtonEventArgs e)
        //{
        //    DragMove();
        //}

        private bool _allowDirectNavigation = false;
        private NavigatingCancelEventArgs _navArgs = null;

        private void OnWindowStateChanged(object sender, EventArgs e)
        {
            if(WindowState != WindowState.Normal)
            {
                WindowState = WindowState.Normal;
            }
        }

        private void CloseFootMenu(object sender, MouseButtonEventArgs e)
        {
            if (this.Content != null)
            {
                if (this.Content.GetType().Name == "MangaViewer")
                {
                    MangaViewer cv = (MangaViewer)this.Content;
                    cv.Foot.mPro.IsChecked = false;
                    cv.Foot.mSet.IsChecked = false;
                    cv.Foot.mMak.IsChecked = false;
                }
                else if (this.Content.GetType().Name == "TextViewer")
                {
                    TextViewer cv = (TextViewer)this.Content;
                    cv.Foot.mPro.IsChecked = false;
                    cv.Foot.mSet.IsChecked = false;
                    cv.Foot.mMak.IsChecked = false;
                }
            }
        }
    }
}
