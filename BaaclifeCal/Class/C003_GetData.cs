using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using BaaclifeCal.Database;

namespace BaaclifeCal.Class
{
    public class C003_GetData
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

        public DataTable GetFundAmt(string genderCode, string age , string savAmt)
        {
            try
            {

                var dtAcc = from fundAmt in dbAcc.fundAmts
                            where (fundAmt.age.ToString() == age) &&
                                  (fundAmt.savAmt.ToString() == savAmt) &&
                                  (fundAmt.gender.ToString() == genderCode)
                            orderby fundAmt.benefitCode ascending
                            select fundAmt;

                DataTable _dt = LINQToDataTable(dtAcc);
                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            
        }

        public DataTable GetSurrenderAmt(string genderCode, string age)
        {
            try
            {
                var dtAcc = from surrenderAmt in dbAcc.surrenders
                            where (surrenderAmt.age.ToString() == age) &&
                                  (surrenderAmt.gender.ToString() == genderCode)
                            select surrenderAmt;

                DataTable _dt = LINQToDataTable(dtAcc);
                return _dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetPaidupAmt(string genderCode, string age)
        {
            try
            {
                var dtAcc = from paidAmt in dbAcc.paidUps
                            where (paidAmt.age.ToString() == age) &&
                                  (paidAmt.gender.ToString() == genderCode)
                            select paidAmt;

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