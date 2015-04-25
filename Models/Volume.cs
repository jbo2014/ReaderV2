using System;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;

namespace ReaderV2.Models
{
    /// <summary>
    /// Volume:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Volume
    {
        public Volume()
        { }
        #region Model
        private int _id;
        private int _no;
        private int _bokid;
        private string _title;
        private byte[] _img;
        private byte[] _img1;
        private byte[] _img2;
        /// <summary>
        /// 
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int No
        {
            set { _no = value; }
            get { return _no; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int BokID
        {
            set { _bokid = value; }
            get { return _bokid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Img
        {
            set { _img = value; }
            get { return _img; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Img1
        {
            set { _img1 = value; }
            get { return _img1; }
        }
        /// <summary>
        /// 
        /// </summary>
        public byte[] Img2
        {
            set { _img2 = value; }
            get { return _img2; }
        }
        #endregion Model


        #region Extra attributes
        private BitmapImage img_des;
        private BitmapImage img1_des;
        private BitmapImage img2_des;
        private Thickness margin;
        private int left;
        private int top;
        private int row;
        private int col;

        public BitmapImage Img_des
        {
            get { return img_des; }
            set { img_des = value; }
        }
        public BitmapImage Img1_des
        {
            get { return img1_des; }
            set { img1_des = value; }
        }
        public BitmapImage Img2_des
        {
            get { return img2_des; }
            set { img2_des = value; }
        }
        public Thickness Margin
        {
            get { return margin; }
            set { margin = value; }
        }
        public int Left
        {
            get { return left; }
            set { left = value; }
        }
        public int Top
        {
            get { return top; }
            set { top = value; }
        }
        public int Row
        {
            get { return row; }
            set { row = value; }
        }
        public int Col
        {
            get { return col; }
            set { col = value; }
        }
        #endregion Extra attributes

    }

    public partial class Leaf
    {
        public Leaf(int _no, ObservableCollection<Volume> _vol) 
        {
            no = _no;
            volPage = _vol;
        }

        private int no;
        private ObservableCollection<Volume> volPage;

        public int No
        {
            get { return no; }
            set { no = value; }
        }
        public ObservableCollection<Volume> VolPage
        {
            get { return volPage; }
            set { volPage = value; }
        }
    }
}

