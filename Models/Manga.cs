using System;
using System.Windows.Media.Imaging;

namespace ReaderV2.Models
{
	/// <summary>
	/// Manga:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Manga
	{
		public Manga()
		{}
		#region Model
		private int _id;
		private int _no;
		private int _bokid;
		private int _chpid;
		private byte[] _contents;
		/// <summary>
		/// 
		/// </summary>
		public int ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int No
		{
			set{ _no=value;}
			get{return _no;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int BokID
		{
			set{ _bokid=value;}
			get{return _bokid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int ChpID
		{
			set{ _chpid=value;}
			get{return _chpid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public byte[] Contents
		{
			set{ _contents=value;}
			get{return _contents;}
		}
		#endregion Model


        #region Extra attributes
        private BitmapImage img_des;
        private string showMark;

        public BitmapImage Img_des
        {
            get { return img_des; }
            set { img_des = value; }
        }
        public string ShowMark
        {
            get { return showMark; }
            set { showMark = value; }
        }
        #endregion Extra attributes

	}
}

