using System;
using System.Windows.Media.Imaging;

namespace ReaderV2.Models
{
	/// <summary>
	/// Chapter:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Chapter
	{
		public Chapter()
		{}
		#region Model
		private int _id;
		private int _no;
		private int _bokid;
		private int _volid;
		private string _title;
		private byte[] _img;
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
		public int VolID
		{
			set{ _volid=value;}
			get{return _volid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Title
		{
			set{ _title=value;}
			get{return _title;}
		}
		/// <summary>
		/// 
		/// </summary>
		public byte[] Img
		{
			set{ _img=value;}
			get{return _img;}
		}
		#endregion Model


        #region Extra attributes
        private BitmapImage img_des;

        public BitmapImage Img_des
        {
            get { return img_des; }
            set { img_des = value; }
        }
        #endregion

    }
}

