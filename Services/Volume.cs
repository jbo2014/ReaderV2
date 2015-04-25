using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;

using ReaderV2.Helper;
using System.Data.SQLite;


namespace ReaderV2.Services
{
    class Volume
    {
        private string tmp = ConfigurationManager.AppSettings["TmpPath"];

        public bool GetSectionByBokID(int bokID)
        {
            FileStream files = null;
            string sqlStr = "SELECT * FROM Section WHERE BokID = @bokID";
            SQLiteParameter[] parameters = new SQLiteParameter[] { new SQLiteParameter("@bokID", bokID) };
            SQLiteDataReader dr = null;
            try
            {
                dr = DBHelper.ExecuteReader(sqlStr, parameters);
            }
            catch (SQLiteException se)
            {
                return false;
            }
            while (dr.Read())
            {
                files = new FileStream("Tmp/" + dr["ID"] + ".tmp", FileMode.Create);
                byte[] cont = (byte[])dr["Contents"];
                files.Write(cont, 0, cont.Length);
                files.Close();
            };
            return true;
        }

        public bool GetSecStreamByBokID(int bokID)
        {
            FileStream files = null;
            string sqlStr = "SELECT * FROM Section WHERE BokID = @bokID";
            SQLiteParameter[] parameters = new SQLiteParameter[] { new SQLiteParameter("@bokID", bokID) };
            SQLiteDataReader dr = null;
            //Dictionary<string,List> Vols = new Dictionary<string,List>;
            try
            {
                dr = DBHelper.ExecuteReader(sqlStr, parameters);
            }
            catch (SQLiteException se)
            {
                return false;
            }
            while (dr.Read())
            {
                files = new FileStream("Tmp/" + dr["ID"] + ".tmp", FileMode.Create);
                byte[] cont = (byte[])dr["Contents"];
                files.Write(cont, 0, cont.Length);
                files.Close();
            };
            return true;
        }
    }
}
