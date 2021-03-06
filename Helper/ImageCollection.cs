using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace ReaderV2.Helper
{
    /// <summary>
    /// Data Model that holds the images of the pages.
    /// </summary>
    public class ImageCollection
    {
        private ObservableCollection<string> images = new ObservableCollection<string>();
        private string location;

        public ImageCollection()
        {
        }

        #region Public Attributes

        public ObservableCollection<string> Images
        {
            get { return this.images; }
        }

        public string Location
        {
            get { return this.location; }
            set
            {
                this.location = value;
                this.Reload();
            }
        }

        public string FirstImage
        {
            get
            {
                if (this.Images.Count != 0)
                {
                    return this.Images[0];
                }
                else
                {
                    return "";
                }
            }
        }

        #endregion

        private void Reload()
        {
            this.images.Clear();

            if (Directory.Exists(this.location))
            {
                DirectoryInfo dir = new DirectoryInfo(this.location);
                foreach (FileInfo file in dir.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    if (this.IsImage(file))
                        this.images.Add(file.FullName);
                }
            }
        }

        private bool IsImage(FileInfo file)
        {
            String ext = file.Extension.ToLower();
            if (ext == ".jpg" || ext == ".gif" || ext == ".png")
            //if (ext == ".tmp")
                return true;
            return false;
        }



        #region Custom Function
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
        #endregion
    }
}
