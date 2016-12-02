using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaaclifeCal.Class
{
    public class C005_ReportLogin
    {
        private ReportDocument _reportDoc = new ReportDocument();
        private string _serverName = string.Empty;
        private string _databaseName = string.Empty;
        private string _userID = string.Empty;
        private string _password = string.Empty;

        public C005_ReportLogin(ReportDocument report)
        {
            _reportDoc = report;
            Login();
        }

        public C005_ReportLogin(ReportDocument report, C004_DatabaseInfo dbInfo)
        {
            _reportDoc = report;
            _serverName = dbInfo.ServerName;
            _databaseName = dbInfo.DatabaseName;
            _userID = dbInfo.UserID;
            _password = dbInfo.Password;
            Login();
        }

        public C005_ReportLogin(ReportDocument report, string serverName, string databaseName, string userID, string password)
        {
            _reportDoc = report;
            _serverName = serverName;
            _databaseName = databaseName;
            _userID = userID;
            _password = password;
            Login();
        }

        public void Login()
        {
            try
            {
                CrystalDecisions.Shared.TableLogOnInfo _myLogin;

                foreach (Table _myTable in ReportDoc.Database.Tables)
                {
                    _myLogin = _myTable.LogOnInfo;

                    _myLogin.ConnectionInfo.DatabaseName = _databaseName;
                    _myLogin.ConnectionInfo.ServerName = _serverName;
                    _myLogin.ConnectionInfo.UserID = _userID;
                    _myLogin.ConnectionInfo.Password = _password;

                    _myTable.ApplyLogOnInfo(_myLogin);
                }
            }
            catch { }
        }

        public void AddParam(string nameParam, object value)
        {
            try
            {
                ParameterValues _currParam = new ParameterValues();
                ParameterDiscreteValue _paramValue = new ParameterDiscreteValue();

                _paramValue.Value = value;
                _currParam = ReportDoc.DataDefinition.ParameterFields[nameParam].CurrentValues;
                _currParam.Add(_paramValue);

                ReportDoc.DataDefinition.ParameterFields[nameParam].ApplyCurrentValues(_currParam);
            }
            catch { }
        }

        #region Property

        public ReportDocument ReportDoc
        {
            get { return _reportDoc; }
            set { _reportDoc = value; }
        }

        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        public string ServerName
        {
            get { return _serverName; }
            set { _serverName = value; }
        }

        public string UserID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #endregion

    }
}