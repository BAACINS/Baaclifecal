using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BaaclifeCal.Database;
using System.Text;
using BaaclifeCal.Class;
using System.Data;
using System.Globalization;
using System.Reflection;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.Reporting.WebForms;

namespace BaaclifeCal
{
    public partial class MainInput : System.Web.UI.Page
    {
        CultureInfo us = System.Globalization.CultureInfo.GetCultureInfo("en-US");
        CultureInfo th = System.Globalization.CultureInfo.GetCultureInfo("th-TH");

        #region References
        //Using LINQ
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
        C001_Calculator cal = new C001_Calculator();
        C002_GetDataDDL GetDataDDL = new C002_GetDataDDL();
        C003_GetData GetData = new C003_GetData();
        M001_InputData Input = new M001_InputData();
        dbAccountDataContext dbAcc = new dbAccountDataContext();
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetSavAmt();
                GetYear();
            }

            gvShowDetail.Visible = false;
            divPaidup.Visible = false;
            divSurrender.Visible = false;
            CalAge();

            if (ddlPaytype.SelectedValue.ToString() != "00")
                {
                    this.ucDOB.TextCalendarChange += new EventHandler(ddlPaytype_SelectedIndexChanged);
                }
        }

        #region Methods
        /*Convert TH : B.E. format to US :B.C. format */
        public DateTime ConvertToBC(string bcDate)
        {
            string getBCDate = DateTime.Parse(bcDate).ToString("yyyy-MM-dd", us);
            DateTime dtFormat = DateTime.Parse(getBCDate, us);
            return dtFormat;
        }

        public DateTime ConvertToBE(string beDate)
        {
            string getBCDate = DateTime.Parse(beDate).ToString("yyyy-MM-dd", th);
            DateTime dtFormat = DateTime.Parse(getBCDate, th);
            return dtFormat;
        }

        private void CalAge() //คำนวณอายุ
        {
            if (ucDOB.TextDate != string.Empty)
            {
                string a = cal.CalAge(ConvertToBC(ucDOB.TextDate), DateTime.Now);
                lblAge.Text = cal.CalAgeYear(ConvertToBC(ucDOB.TextDate), DateTime.Now);
            }
        }

        private void ClearData()
        {
            txtAccident.Text = "";
            txtDead.Text = "";
            txtEocAmt.Text = "";
            txtReward.Text = "";
            txtTotalAmt.Text = "";
            ddlSavYear.SelectedValue = "00";
            txtSurennderAmt.Text = "";
            ddlSavYearP.SelectedValue = "00";
            txtPaidUp.Text = "";
        }

        /*-----------------   Reports  ------------------------*/

        private void OpenPDF(string downloadAsFilename)
        {
            ReportDocument Rel = new ReportDocument();
            Rel.Load(Server.MapPath("~/Reports/R0001_CompensationClaim.rpt"));
            BinaryReader stream = new BinaryReader(Rel.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat));
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=" + downloadAsFilename);
            Response.AddHeader("content-length", stream.BaseStream.Length.ToString());
            Response.BinaryWrite(stream.ReadBytes(Convert.ToInt32(stream.BaseStream.Length)));
            Response.Flush();
            Response.Close();
        }

        private void GetYear()
        {
            var items = new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            ddlSavYear.DataSource = items;
            ddlSavYear.DataBind();
            ddlSavYear.DataTextField = items.ToString();
            ddlSavYear.Items.Insert(0, new ListItem("-- เลือก --", "00"));
            ddlSavYearP.DataSource = items;
            ddlSavYearP.DataBind();
            ddlSavYearP.DataTextField = items.ToString();
            ddlSavYearP.Items.Insert(0, new ListItem("-- เลือก --", "00"));
        }

        private void GetSavAmt()
        {
            ddlPaytype.DataSource = GetDataDDL.GetSavAmt();
            ddlPaytype.DataTextField = "savAmt";
            ddlPaytype.DataValueField = "savCode";
            ddlPaytype.DataBind();
            ddlPaytype.Items.Insert(0, new ListItem("------ เลือก ------", "00"));
        }

        #endregion

        #region Events

        protected void gender_CheckedChanged(Object sender, EventArgs e)
        {
            ClearData();
            ddlPaytype_SelectedIndexChanged(this, EventArgs.Empty);
        }

        /*-----------------   DDLs  ------------------------*/

        protected void ddlPaytype_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearData();

            if (ddlPaytype.SelectedValue.ToString() != "00")
            {
                if (Convert.ToInt32(lblAge.Text) < 20 || Convert.ToInt32(lblAge.Text) > 60)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js", "alert('อายุต้องอยู่ในช่วง 20 ถึง 60 ปี');", true);
                    return;
                }

                DataTable _dt = new DataTable();
                string gender;
                if (rdbMale.Checked)
                    gender = "01";
                else gender = "02";
                _dt = GetData.GetFundAmt(gender, lblAge.Text, ddlPaytype.SelectedItem.Text);
                Session["SavAmt"] = _dt;
                txtDead.Text = Convert.ToInt32(_dt.Rows[0]["fundAmt1"]).ToString("N0");
                txtAccident.Text = Convert.ToInt32(_dt.Rows[1]["fundAmt1"]).ToString("N0");
                txtEocAmt.Text = Convert.ToInt32(_dt.Rows[0]["fundAmt1"]).ToString("N0");
                txtReward.Text = Convert.ToInt32(_dt.Rows[0]["reward"]).ToString("N0");
                txtTotalAmt.Text = Convert.ToInt32(_dt.Rows[2]["fundAmt1"]).ToString("N0");
            }
            else { ClearData(); }
        }

        protected void ddlSavYearP_SelectedIndexChanged(object sender, EventArgs e)
        {

            DataTable dtSavAmt = (DataTable)Session["SavAmt"];
            DataTable _dtPaidAmt = new DataTable();
            _dtPaidAmt = GetData.GetPaidupAmt(dtSavAmt.Rows[0]["gender"].ToString()
                        , dtSavAmt.Rows[0]["age"].ToString());

            var paidAmt = (from b in _dtPaidAmt.AsEnumerable()
                           where b.Field<string>("year") == ddlSavYearP.SelectedValue.ToString()
                           select (Convert.ToDouble(dtSavAmt.Rows[0]["fundamt1"]) / 1000.00) * b.Field<Int32>("paidAmt")).FirstOrDefault();

            txtPaidUp.Text = Convert.ToDecimal(paidAmt).ToString("N2");
            divPaidup.Visible = true;
            divPaidup.Focus();
        }

        protected void ddlSavYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dtSavAmt = (DataTable)Session["SavAmt"];
            DataTable _dtSurrenderAmt = new DataTable();
            _dtSurrenderAmt = GetData.GetSurrenderAmt(dtSavAmt.Rows[0]["gender"].ToString()
                        , dtSavAmt.Rows[0]["age"].ToString());

            int z;

            var surrenderAmt = (from b in _dtSurrenderAmt.AsEnumerable()
                                where b.Field<string>("year") == ddlSavYear.SelectedValue.ToString()
                                select (Convert.ToDouble(dtSavAmt.Rows[0]["fundamt1"])/1000.00) * b.Field<Int32>("surAmt")).FirstOrDefault();
            txtSurennderAmt.Text = Convert.ToDecimal(surrenderAmt).ToString("N2");
            divSurrender.Visible = true;
            divSurrender.Focus();
        }
        /*-----------------   Buttons  ------------------------*/

        protected void btnCalSavAmt_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblAge.Text == "" || ddlPaytype.SelectedValue.ToString() == "00")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js", "alert('กรุณาระบุวันเดิอนปีเกิด/จำนวนส่งฝากรายเดือน');", true);
                    return;
                }
                else
                {
                    gvShowDetail.Visible = true;
                    gvShowDetail.Focus();
                    divPaidup.Visible = false;
                    divSurrender.Visible = false;

                    DataTable dtSavAmt = (DataTable)Session["SavAmt"];
                    var month = from a in dtSavAmt.AsEnumerable()
                                where a.Field<string>("benefitCode") == "01"
                                select new
                                {
                                    payType = "รายเดือน",
                                    instalment = "120",
                                    instalmentAmt = a.Field<Int32>("savAmt"),
                                    discount = "-",
                                    totInstalmentAmt = a.Field<Int32>("savAmt"),
                                    contactAmt = a.Field<Int32>("netMonth"),
                                    totContactAmt = a.Field<Int32>("fundAmt1")
                                                         + a.Field<Int32>("reward"),
                                    netGain = (a.Field<Int32>("fundAmt1")
                                                        + a.Field<Int32>("reward"))
                                                        - a.Field<Int32>("netMonth")
                                };

                    DataTable _dtMonth = new DataTable();
                    _dtMonth = LINQToDataTable(month);

                    var quarter = from a in dtSavAmt.AsEnumerable()
                                  where a.Field<string>("benefitCode") == "01"
                                  select new
                                  {
                                      payType = "ราย 3 เดือน",
                                      instalment = "40",
                                      instalmentAmt = a.Field<Int32>("savAmt") * 3,
                                      discount = "-",
                                      totInstalmentAmt = a.Field<Int32>("savAmt") * 3,
                                      contactAmt = a.Field<Int32>("netQuarter"),
                                      totContactAmt = a.Field<Int32>("fundAmt1")
                                                          + a.Field<Int32>("reward"),
                                      netGain = (a.Field<Int32>("fundAmt1")
                                                          + a.Field<Int32>("reward"))
                                                          - a.Field<Int32>("netQuarter")
                                  };


                    DataTable _dtQuarter = new DataTable();
                    _dtQuarter = LINQToDataTable(quarter);

                    var semiannual = from a in dtSavAmt.AsEnumerable()
                                     where a.Field<string>("benefitCode") == "01"
                                     select new
                                     {
                                         payType = "ราย 6 เดือน",
                                         instalment = "20",
                                         instalmentAmt = a.Field<Int32>("savAmt") * 6,
                                         discount = a.Field<Int32>("discountSemiAnnual").ToString("N0"),
                                         totInstalmentAmt = (a.Field<Int32>("savAmt") * 6)
                                                            - a.Field<Int32>("discountSemiAnnual"),
                                         contactAmt = a.Field<Int32>("netSemiAnnual"),
                                         totContactAmt = a.Field<Int32>("fundAmt1")
                                                            + a.Field<Int32>("reward"),
                                         netGain = (a.Field<Int32>("fundAmt1")
                                                            + a.Field<Int32>("reward"))
                                                            - a.Field<Int32>("netSemiAnnual")
                                     };

                    DataTable _dtSemiAnnual = new DataTable();
                    _dtSemiAnnual = LINQToDataTable(semiannual);

                    var annual = from a in dtSavAmt.AsEnumerable()
                                 where a.Field<string>("benefitCode") == "01"
                                 select new
                                 {
                                     payType = "ราย 12 เดือน",
                                     instalment = "10",
                                     instalmentAmt = a.Field<Int32>("savAmt") * 12,
                                     discount = a.Field<Int32>("discountAnnual").ToString("N0"),
                                     totInstalmentAmt = (a.Field<Int32>("savAmt") * 12)
                                                        - a.Field<Int32>("discountAnnual"),
                                     contactAmt = a.Field<Int32>("netAnnual"),
                                     totContactAmt = a.Field<Int32>("fundAmt1")
                                                        + a.Field<Int32>("reward"),
                                     netGain = (a.Field<Int32>("fundAmt1")
                                                        + a.Field<Int32>("reward"))
                                                        - a.Field<Int32>("netAnnual")
                                 };

                    DataTable _dtAnnual = new DataTable();
                    _dtAnnual = LINQToDataTable(annual);

                    DataTable dtAllRecord = new DataTable();
                    dtAllRecord.Merge(_dtMonth);
                    dtAllRecord.Merge(_dtQuarter);
                    dtAllRecord.Merge(_dtSemiAnnual);
                    dtAllRecord.Merge(_dtAnnual);

                    gvShowDetail.DataSource = dtAllRecord;
                    gvShowDetail.DataBind();
                }
            }
            catch { }

        }

        protected void btnCalSurrender_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblAge.Text == "" || ddlPaytype.SelectedValue.ToString() == "00")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js", "alert('กรุณาระบุวันเดิอนปีเกิด/จำนวนส่งฝากรายเดือน');", true);
                    return;
                }
                else
                {
                    divSurrender.Visible = true;
                    divSurrender.Focus();
                    gvShowDetail.Visible = false;
                    divPaidup.Visible = false;
                    ddlSavYear_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnCalPaidUp_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblAge.Text == "" || ddlPaytype.SelectedValue.ToString() == "00")
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js", "alert('กรุณาระบุวันเดิอนปีเกิด/จำนวนส่งฝากรายเดือน');", true);
                    return;
                }
                else
                {
                    divPaidup.Visible = true;
                    divPaidup.Focus();
                    divSurrender.Visible = false;
                    gvShowDetail.Visible = false;
                    ddlSavYearP_SelectedIndexChanged(this, EventArgs.Empty);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            DataTable dtSavAmt = (DataTable)Session["SavAmt"];
            DataSet1TableAdapters.SP_fundAmtTableAdapter fundsAmtAdapter;
            fundsAmtAdapter = new DataSet1TableAdapters.SP_fundAmtTableAdapter();


            /*Check subreport*/
            string chkSubRep = string.Empty;

            /*Show both*/
            if (ddlSavYear.SelectedValue.ToString() != "00" && ddlSavYearP.SelectedValue.ToString() != "00")
            {
                chkSubRep = "1";
            }
            /*Show Surrender*/
            else if (ddlSavYear.SelectedValue.ToString() != "00" && ddlSavYearP.SelectedValue.ToString() == "00")
            {
                chkSubRep = "2";
            }
            /*Show Paidup*/
            else if (ddlSavYear.SelectedValue.ToString() == "00" && ddlSavYearP.SelectedValue.ToString() != "00")
            {
                chkSubRep = "3";
            }
            /*Don't show*/
            else { chkSubRep = "0"; }

            DataSet1.SP_fundAmtDataTable a;
            a = fundsAmtAdapter.GetData(dtSavAmt.Rows[0]["gender"].ToString(), dtSavAmt.Rows[0]["age"].ToString(), dtSavAmt.Rows[0]["savamt"].ToString(),chkSubRep);
            DataTable b = new DataTable();
            b = a;

            DataSet1TableAdapters.SP_BenefitTableAdapter benAdp;
            benAdp = new DataSet1TableAdapters.SP_BenefitTableAdapter();
            DataSet1.SP_BenefitDataTable ben;
            ben = benAdp.GetData(dtSavAmt.Rows[0]["gender"].ToString(), dtSavAmt.Rows[0]["age"].ToString(), dtSavAmt.Rows[0]["savamt"].ToString());
            DataTable repBen = new DataTable();
            repBen = ben;

            DataSet1TableAdapters.SP_SurrenderTableAdapter surrenderAdp;
            surrenderAdp = new DataSet1TableAdapters.SP_SurrenderTableAdapter();
            DataSet1.SP_SurrenderDataTable c;
            c = surrenderAdp.GetData(dtSavAmt.Rows[0]["gender"].ToString(), dtSavAmt.Rows[0]["age"].ToString(),ddlSavYear.SelectedValue.ToString(), dtSavAmt.Rows[0]["savamt"].ToString());
            DataTable d = new DataTable();
            d = c;

            DataSet1TableAdapters.SP_PaidUpTableAdapter paidupAdp;
            paidupAdp = new DataSet1TableAdapters.SP_PaidUpTableAdapter();
            DataSet1.SP_PaidUpDataTable f;
            f = paidupAdp.GetData(dtSavAmt.Rows[0]["gender"].ToString(), dtSavAmt.Rows[0]["age"].ToString(), ddlSavYearP.SelectedValue.ToString(), dtSavAmt.Rows[0]["savamt"].ToString());
            DataTable g = new DataTable();
            g = f;

            ReportDocument rpt = new ReportDocument();
            string _pathReport = Server.MapPath("~/R001_fundAmt.rpt");
            rpt.Load(_pathReport);

            rpt.SetDataSource(b);
            rpt.Subreports["sub_benefit"].SetDataSource(repBen);
            rpt.Subreports["sub_surrender"].SetDataSource(d);
            rpt.Subreports["sub_paidup"].SetDataSource(g);

            BinaryReader stream = new BinaryReader(rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat));
            
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment; filename=" + "BaaclifePer2.pdf");
            Response.AddHeader("content-length", stream.BaseStream.Length.ToString());
            Response.BinaryWrite(stream.ReadBytes(Convert.ToInt32(stream.BaseStream.Length)));
            Response.Flush();
            Response.Close();
            rpt.Close();
            rpt.Dispose();

        }

        protected void lnk_Click(object sender, EventArgs e)
        {
            string strFileName = "CalendarManual.pdf";
            string strSourceFile = string.Empty;
            strSourceFile = Server.MapPath("~/Manual/CalendarManual.pdf");
            System.Web.HttpResponse response = System.Web.HttpContext.Current.Response;
            response.ClearContent();
            response.Clear();
            response.ContentType = "text/plain";
            response.AddHeader("Content-Disposition",
                               "attachment; filename=" + strFileName + ";");
            response.TransmitFile(strSourceFile);
            response.Flush();
            response.End();
        }

        /*-----------------   Grid  ------------------------*/

        protected void gvShowDetail_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                // Adding a column manually once the header created
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    int maxCell = e.Row.Cells.Count;
                    for (int i = 0; i < maxCell; i++)
                    {
                        e.Row.Cells[i].Visible = false;
                    }

                    GridView ProductGrid = (GridView)sender;
                    GridViewRow HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    //Adding Year Column
                    TableCell HeaderCell = new TableCell();
                    HeaderCell.Text = "งวดที่ส่งฝาก";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "จำนวนงวด";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    //Adding Head Office Column
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "จำนวนส่งฝากต่องวด";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 3;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "จำนวนส่งฝากตลอดชีวิต(บาท)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.VerticalAlign = VerticalAlign.Middle;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "เงินครบกำหนดพร้อมเงินสมนาคุณ(บาท)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "ส่วนต่างผลตอบแทน(บาท)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
                    HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "จำนวนเงินส่งฝาก(บาท)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "ส่วนลด(บาท)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "จำนวนส่งฝากสุทธิ(บาท)";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);



                    ProductGrid.Controls[0].Controls.AddAt(1, HeaderRow);


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion









    }
}