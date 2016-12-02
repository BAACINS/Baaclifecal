using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BaaclifeCal.Class
{
    public class C001_Calculator
    {
        CultureInfo en = System.Globalization.CultureInfo.GetCultureInfo("en-GB");
        CultureInfo th = System.Globalization.CultureInfo.GetCultureInfo("th-TH");
        public string CalAge(DateTime birthDate, DateTime deathDate)
        {

            int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            DateTime fromDate;
            DateTime toDate;
            fromDate = birthDate;
            toDate = deathDate;
            int year;
            int month;
            int day;
            if (birthDate >= deathDate)
                return string.Empty;
            //===============Day===================
            int increment = 0;
            if (fromDate.Day > toDate.Day)
            {
                increment = monthDay[fromDate.Month - 1];
            }

            if (increment == -1)
            {
                if (DateTime.IsLeapYear(fromDate.Year))
                {
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }
            }

            if (increment != 0)
            {
                day = (toDate.Day + increment) - fromDate.Day;
                increment = 1;
            }
            else
            {
                day = toDate.Day - fromDate.Day;
            }
            //==============Month=====================
            if ((fromDate.Month + increment) > toDate.Month)
            {
                month = (toDate.Month + 12) - (fromDate.Month + increment);
                increment = 1;
            }
            else
            {
                month = (toDate.Month) - (fromDate.Month + increment);
                increment = 0;
            }
            //=============Year======================
            year = toDate.Year - (fromDate.Year + increment);
            string strDate = string.Empty;
            if (year > 0)
                strDate += year.ToString() + " ปี ";
            if (month > 0)
                strDate += month.ToString() + " เดือน ";
            if (day > 0)
                strDate += day.ToString() + " วัน ";

            return strDate;
        }

        public string CalAgeYear(DateTime birthDate, DateTime deathDate)
        {
            int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            DateTime fromDate;
            DateTime toDate;
            fromDate = birthDate;
            toDate = deathDate;
            int year;
            int month;
            int day;
            if (birthDate >= deathDate)
                return string.Empty;
            //===============Day===================
            int increment = 0;
            if (fromDate.Day > toDate.Day)
            {
                increment = monthDay[fromDate.Month - 1];
            }

            if (increment == -1)
            {
                if (DateTime.IsLeapYear(fromDate.Year))
                {
                    increment = 29;
                }
                else
                {
                    increment = 28;
                }
            }

            if (increment != 0)
            {
                day = (toDate.Day + increment) - fromDate.Day;
                increment = 1;
            }
            else
            {
                day = toDate.Day - fromDate.Day;
            }
            //==============Month=====================
            if ((fromDate.Month + increment) > toDate.Month)
            {
                month = (toDate.Month + 12) - (fromDate.Month + increment);
                increment = 1;
            }
            else
            {
                month = (toDate.Month) - (fromDate.Month + increment);
                increment = 0;
            }
            //=============Year======================
            year = toDate.Year - (fromDate.Year + increment);
            string strDate = string.Empty;
            if (month >= 6 && day >= 1)
            {
                year++;
            }
            strDate = year.ToString();
            return strDate;
        }
        /// <summary>
        /// Convert Formet date
        /// </summary>
        /// <param name="strDate"></param>
        /// <param name="type">0 AD,1 BE</param>
        /// <returns></returns>
        public DateTime SetFormatdate(string strDate, int type)// 0 AD,1 BE
        {
            try
            {
                //string strFormatDate = strDate.Substring(6, 4)+"/"
                //    + strDate.Substring(3, 2)+"/"
                //    + strDate.Substring(0, 2);
                DateTime _date = DateTime.Parse(strDate, th);
                string[] _strFormatDate = _date.ToString().Split('/');
                int _Year = int.Parse(_strFormatDate[2].Substring(0, 4));
                //_date = _date.AddYears(-543);
                if (type == 0)
                {
                    //date = DateTime.Parse(strDate, CultureInfo.GetCultureInfo("en-GB"));
                    if (_Year > 2400)
                        _date = _date.AddYears(-543);
                    else if (_Year < 1600)
                        _date = _date.AddYears(543);
                }
                else
                {
                    //date = DateTime.Parse(strDate, CultureInfo.GetCultureInfo("th-TH")); 
                    if (_Year < 2400)
                        _date = _date.AddYears(543);
                    else if (_Year > 3000)
                        _date = _date.AddYears(-543);
                }
                return _date;
            }
            catch
            {
                throw;
            }
        }

        public Boolean VerifyPeopleID(String PID)//เชคเลขบัตรประชาชน
        {
            string digit = null;
            //ตรวจสอบว่าทุก ๆ ตัวอักษรเป็นตัวเลข
            if (PID.ToCharArray().All(c => char.IsNumber(c)) == false)
                return false;
            //ตรวจสอบว่าข้อมูลมีทั้งหมด 13 ตัวอักษร
            if (PID.Trim().Length != 13)
                return false;
            int sumValue = 0;
            for (int i = 0; i < PID.Length - 1; i++)
                sumValue += int.Parse(PID[i].ToString()) * (13 - i);
            int v = 11 - (sumValue % 11);
            if (v.ToString().Length == 2)
            {
                digit = v.ToString().Substring(1, 1);
            }
            else
            {
                digit = v.ToString();
            }
            return PID[12].ToString() == digit;
        }

        public DataTable LINQResultToDataTable<T>(IEnumerable<T> Linqlist)
        {
            DataTable dt = new DataTable();


            PropertyInfo[] columns = null;

            if (Linqlist == null) return dt;

            foreach (T Record in Linqlist)
            {

                if (columns == null)
                {
                    columns = ((Type)Record.GetType()).GetProperties();
                    foreach (PropertyInfo GetProperty in columns)
                    {
                        Type IcolType = GetProperty.PropertyType;

                        if ((IcolType.IsGenericType) && (IcolType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            IcolType = IcolType.GetGenericArguments()[0];
                        }

                        dt.Columns.Add(new DataColumn(GetProperty.Name, IcolType));
                    }
                }

                DataRow dr = dt.NewRow();

                foreach (PropertyInfo p in columns)
                {
                    dr[p.Name] = p.GetValue(Record, null) == null ? DBNull.Value : p.GetValue
                    (Record, null);
                }

                dt.Rows.Add(dr);
            }
            return dt;
        }

        public Decimal ConvertStringToDecimal(string strInput)
        {
            decimal decOutput;
            if (Decimal.TryParse(strInput, out decOutput))
            {
                return decOutput;
            }
            else
            {
                return 0;
            }
        }
    }
}