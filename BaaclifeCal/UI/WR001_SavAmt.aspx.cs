using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BaaclifeCal.UI
{
    public partial class WR001_SavAmt : System.Web.UI.Page
    {
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    DataTable dtSavAmt = (DataTable)Session["SavAmt"];

                    var month = from a in dtSavAmt.AsEnumerable()
                                where a.Field<string>("benefitCode") == "01"
                                select new
                                {
                                    payType = "รายเดือน",
                                    instalment = "120",
                                    instalmentAmt = Convert.ToString(a.Field<Int32>("savAmt")),
                                    discount = "-",
                                    totInstalmentAmt = Convert.ToString(a.Field<Int32>("savAmt")),
                                    contactAmt = Convert.ToString(a.Field<Int32>("netMonth")),
                                    totContactAmt = Convert.ToString(a.Field<Int32>("fundAmt1")
                                                         +a.Field<Int32>("reward")),
                                    netGain = Convert.ToString((a.Field<Int32>("fundAmt1")
                                                        + a.Field<Int32>("reward"))
                                                        - a.Field<Int32>("netMonth"))
                                };

                    DataTable _dtMonth = new DataTable();
                    _dtMonth = LINQToDataTable(month);

                    var quarter = from a in dtSavAmt.AsEnumerable()
                                where a.Field<string>("benefitCode") == "01"
                                select new
                                {
                                    payType = "ราย 3 เดือน",
                                    instalment = "40",
                                    instalmentAmt = Convert.ToString(a.Field<Int32>("savAmt") * 3),
                                    discount = "-",
                                    totInstalmentAmt = Convert.ToString(a.Field<Int32>("savAmt") * 3),
                                    contactAmt = Convert.ToString(a.Field<Int32>("netQuarter")),
                                    totContactAmt = Convert.ToString(a.Field<Int32>("fundAmt1")
                                                        + a.Field<Int32>("reward")),
                                    netGain = Convert.ToString((a.Field<Int32>("fundAmt1")
                                                        + a.Field<Int32>("reward"))
                                                        - a.Field<Int32>("netQuarter"))
                                };


                    DataTable _dtQuarter = new DataTable();
                    _dtQuarter = LINQToDataTable(quarter); 

                    var semiannual = from a in dtSavAmt.AsEnumerable()
                                 where a.Field<string>("benefitCode") == "01"
                                 select new
                                 {
                                     payType = "ราย 6 เดือน",
                                     instalment = "20",
                                     instalmentAmt = Convert.ToString(a.Field<Int32>("savAmt") * 6),
                                     discount = Convert.ToString(a.Field<decimal>("discountSemiAnnual")),
                                     totInstalmentAmt = Convert.ToString((a.Field<Int32>("savAmt") *6)
                                                        - a.Field<decimal>("discountSemiAnnual")),
                                     contactAmt = Convert.ToString(a.Field<decimal>("netSemiAnnual")),
                                     totContactAmt = Convert.ToString(a.Field<Int32>("fundAmt1")
                                                        + a.Field<Int32>("reward")),
                                     netGain = Convert.ToString((a.Field<Int32>("fundAmt1")
                                                        + a.Field<Int32>("reward"))
                                                        - a.Field<decimal>("netSemiAnnual"))
                                 };

                    DataTable _dtSemiAnnual = new DataTable();
                    _dtSemiAnnual = LINQToDataTable(semiannual);

                    var annual = from a in dtSavAmt.AsEnumerable()
                                 where a.Field<string>("benefitCode") == "01"
                                 select new
                                 {
                                     payType = "ราย 12 เดือน",
                                     instalment = "10",
                                     instalmentAmt = Convert.ToString(a.Field<Int32>("savAmt") * 10),
                                     discount = Convert.ToString(a.Field<decimal>("discountAnnual")),
                                     totInstalmentAmt = Convert.ToString((a.Field<Int32>("savAmt")*10)
                                                        -  a.Field<decimal>("discountAnnual")),
                                     contactAmt = Convert.ToString(a.Field<decimal>("netAnnual")),
                                     totContactAmt = Convert.ToString(a.Field<Int32>("fundAmt1")
                                                        + a.Field<Int32>("reward")),
                                     netGain = Convert.ToString((a.Field<Int32>("fundAmt1") 
                                                        + a.Field<Int32>("reward")) 
                                                        - a.Field<decimal>("netAnnual"))
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

                    //if (dtSavAmt.Rows.Count > 0)
                    //{
                    //    gvShowDetail.DataSource = dtSavAmt;
                    //    gvShowDetail.DataBind();
                    //}

                }
                catch (Exception ex)
                {
                    throw ex;
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "js", "alert('กรุณาระบุวันเดิอนปีเกิด/จำนวนส่งฝากรายเดือน');", true);
                    //Response.Redirect("W001_SavAmtInput.aspx");
                }
            }
        }

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
                    HeaderCell.Text = "จำนวนส่งฝากตลอดชีวิต";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.VerticalAlign = VerticalAlign.Middle;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "เงินครบกำหนดพร้อมเงินสมนาคุณ";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "ส่วนต่างผลตอบแทน";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.RowSpan = 2;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
                    HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    //Adding Branch Office Column
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "รับ";
                    //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    //HeaderCell.ColumnSpan = 4;
                    //HeaderCell.CssClass = "HeaderStyle";
                    //HeaderRow.Cells.Add(HeaderCell);

                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "ปฏิเสธ";
                    //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    //HeaderCell.ColumnSpan = 2;
                    //HeaderCell.RowSpan = 2;
                    //HeaderCell.CssClass = "HeaderStyle";
                    //HeaderRow.Cells.Add(HeaderCell);

                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "ยกเลิก/บอกล้าง";
                    //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    //HeaderCell.ColumnSpan = 2;
                    //HeaderCell.RowSpan = 2;
                    //HeaderCell.CssClass = "HeaderStyle";
                    //HeaderRow.Cells.Add(HeaderCell);

                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "คงเหลือ";
                    //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    //HeaderCell.ColumnSpan = 2;
                    //HeaderCell.RowSpan = 2;
                    //HeaderCell.CssClass = "HeaderStyle";
                    //HeaderRow.Cells.Add(HeaderCell);

                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "โปรโมชั่น";
                    //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    //HeaderCell.ColumnSpan = 2;
                    //HeaderCell.RowSpan = 2;
                    //HeaderCell.CssClass = "HeaderStyle";
                    //HeaderRow.Cells.Add(HeaderCell);

                    //ProductGrid.Controls[0].Controls.AddAt(1, HeaderRow);

                    //ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
                    //HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    //Adding Branch Office Column
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "งวดแรก";
                    //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    //HeaderCell.ColumnSpan = 2;
                    //HeaderCell.CssClass = "HeaderStyle";
                    //HeaderRow.Cells.Add(HeaderCell);

                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "งวดต่อไป";
                    //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    //HeaderCell.ColumnSpan = 2;
                    //HeaderCell.CssClass = "HeaderStyle";
                    //HeaderRow.Cells.Add(HeaderCell);

                    //ProductGrid.Controls[0].Controls.AddAt(2, HeaderRow);

                    //ProductGrid.Controls[0].Controls.AddAt(0, HeaderRow);
                    //HeaderRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                    //งวดแรก
                    //HeaderCell = new TableCell();
                    //HeaderCell.Text = "งวดที่ส่งฝาก";
                    //HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    //HeaderCell.ColumnSpan = 1;
                    //HeaderCell.CssClass = "HeaderStyle";
                    //HeaderRow.Cells.Add(HeaderCell);

                    //งวดต่อไป
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "จำนวนเงินส่งฝาก";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    HeaderCell = new TableCell();
                    HeaderCell.Text = "ส่วนลด";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    //ปฏิเสธ
                    HeaderCell = new TableCell();
                    HeaderCell.Text = "จำนวนส่งฝากสุทธิ";
                    HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                    HeaderCell.ColumnSpan = 1;
                    HeaderCell.CssClass = "HeaderStyle";
                    HeaderRow.Cells.Add(HeaderCell);

                    

                    ProductGrid.Controls[0].Controls.AddAt(1, HeaderRow);


                }
            }
            catch (Exception ex)
            {

            }
        }

    }
}