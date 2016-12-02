using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using BaaclifeCal.Database;

namespace BaaclifeCal.Class
{
    public class C002_GetDataDDL
    {
        dbAccountDataContext dbAcc = new dbAccountDataContext();

        public DataTable LINQToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            //Column name
            PropertyInfo[] objProp = null;

            if (varlist == null)
            {
                return dtReturn;
            }

            foreach (T rec in varlist)
            {
                if (objProp == null)
                {
                    objProp = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in objProp)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in objProp)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);

                }

                dtReturn.Rows.Add(dr);
            }

            return dtReturn;

        }

        public DataTable GetSavAmt()
        {

            try
            {
                var dtAcc = from savingAmount in dbAcc.savingAmounts
                            //เลือกเบี้ยที่อยู่ระหว่าง 300 - 1000 บาท
                            where savingAmount.savCode.CompareTo("03") >= 0 && savingAmount.savCode.CompareTo("10") <= 0
                            orderby savingAmount.savCode ascending
                            select savingAmount;

                DataTable _dt = LINQToDataTable(dtAcc);

                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}