using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Configuration;

using ReaderV2.Helper;
using System.Data.SQLite;


namespace ReaderV2.Services.Shared
{
    public class MangaService
    {
        private string tmp = ConfigurationManager.AppSettings["TmpPath"];
        public bool GetMangasByChpID(int chpID)
        {
            FileStream files = null;
            string sqlStr = "SELECT * FROM Manga WHERE ChpID = @ChpID";
            SQLiteParameter[] parameters = new SQLiteParameter[] { new SQLiteParameter("@ChpID", chpID) };
            SQLiteDataReader dr = null;
            try
            {
                dr = DBHelper.ExecuteReader(sqlStr, parameters);
            }
            catch(SQLiteException se) 
            {
                return false;
            }
            while(dr.Read())
            {
                files = new FileStream("Tmp/"+dr["ID"]+".tmp", FileMode.Create);
                byte[] cont = (byte[])dr["Contents"];
                files.Write(cont, 0, cont.Length);
                files.Close();
            };
            return true;
        }
    }
}
