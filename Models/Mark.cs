using System;
using System.Collections.ObjectModel;

namespace ReaderV2.Models
{
    /// <summary>
    /// Mark:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Mark
    {
        public Mark()
        { }
        #region Model
        private int _id;
        private int _bokid;
        private int _volid;
        private int _chpid;
        private int? _page;
        private string _time;
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
        public int BokID
        {
            set { _bokid = value; }
            get { return _bokid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int VolID
        {
            set { _volid = value; }
            get { return _volid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ChpID
        {
            set { _chpid = value; }
            get { return _chpid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Page
        {
            set { _page = value; }
            get { return _page; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Time
        {
            set { _time = value; }
            get { return _time; }
        }
        #endregion Model

        #region  Extra attributes
        private string title;
        private string fontSize;
        private string strPage;

        public string StrPage
        {
            get { return strPage; }
            set { strPage = value; }
        }
        public string FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        #endregion
    }

    public partial class MLeaf
    {
        public MLeaf(int _no, ObservableCollection<Mark> _vol)
        {
            no = _no;
            volPage = _vol;
        }

        private int no;
        private ObservableCollection<Mark> volPage;

        public int No
        {
            get { return no; }
            set { no = value; }
        }
        public ObservableCollection<Mark> VolPage
        {
            get { return volPage; }
            set { volPage = value; }
        }
    }
}



