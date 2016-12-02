using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BaaclifeCal.Class
{
    public class C004_DatabaseInfo
    {
        private static volatile C004_DatabaseInfo _instance;
        private static object _objLock = new Object();
        private string _server = string.Empty;
        private string _database = string.Empty;
        private string _userid = string.Empty;
        private string _password = string.Empty;

        #region Property Info

        public string ServerName
        {
            get { return _server; }
            set { _server = value; }
        }

        public string DatabaseName
        {
            get { return _database; }
            set { _database = value; }
        }

        public string UserID
        {
            get { return _userid; }
            set { _userid = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #endregion

        #region Constructor
        private C004_DatabaseInfo()
        {
            try
            {
                _server = ConfigurationSettings.AppSettings["DBServer"].ToString();
                _database = ConfigurationSettings.AppSettings["DBName"].ToString();
                _userid = ConfigurationSettings.AppSettings["DBUserID"].ToString();
                _password = ConfigurationSettings.AppSettings["DBPwd"].ToString();
            }
            catch { }
        }
        #endregion

        #region Property Instance

        public static C004_DatabaseInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_objLock)
                    {
                        if (_instance == null)
                            _instance = new C004_DatabaseInfo();
                    }
                }

                return _instance;
            }
        }

        #endregion

        #region Public method

        public string GetConnectionString()
        {
            string _strConn = string.Empty;

            try
            {
                if (_userid != "" && _password != "")
                {
                    string _tmpConn = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};max pool size=1000";
                    _strConn = string.Format(_tmpConn, new string[] { _server, _database, _userid, _password });
                }
                else
                {
                    string _tmpConn = "Data Source={0};Initial Catalog={1};Integrated Security=True;max pool size=1000";
                    _strConn = string.Format(_tmpConn, new string[] { _server, _database });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _strConn;
        }
        #endregion
    }
}