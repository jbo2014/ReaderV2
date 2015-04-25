using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ReaderV2.Models
{
    /// <summary>
    /// Volume:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Index
    {
        public Index()
        { }
        #region Model
        private int _id;
        private int _no;
        private int _bokid;
        private string _title;
        private byte[] _img;
        private int type;
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
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        #endregion Model


        #region Extra attributes
        private int left;
        private int right;
        private int top;
        private int bottom;
        private int width;
        private Thickness margin;
        private BitmapImage img_des;

        public int Left
        {
            get { return left; }
            set { left = value; }
        }
        public int Right
        {
            get { return right; }
            set { right = value; }
        }
        public int Top
        {
            get { return top; }
            set { top = value; }
        }
        public int Bottom
        {
            get { return bottom; }
            set { bottom = value; }
        }
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public Thickness Margin
        {
            get { return margin; }
            set { margin = value; }
        }
        public BitmapImage Img_des
        {
            get { return img_des; }
            set { img_des = value; }
        }
        #endregion Extra attributes

    }
}

