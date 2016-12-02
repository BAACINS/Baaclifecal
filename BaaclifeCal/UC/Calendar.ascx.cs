using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UnmatchPayment.UC
{
    public partial class WebUserControl1 : System.Web.UI.UserControl
    {
        public event EventHandler TextCalendarChange;

        #region Property

        /// <summary>
        /// get or set date value in format dd/MM/yyyy
        /// </summary>
        [Browsable(false)]
        public string TextDate
        {
            get { return this.txtCalendar.Text; }
            set { this.txtCalendar.Text = value; }
        }

        /// <summary>
        /// get textbox clientid of calendar 
        /// </summary>
        [Browsable(false)]
        public string TextBoxClientID
        {
            get { return this.txtCalendar.ClientID; }
        }

        /// <summary>
        /// set enable or disable control
        /// </summary>
        [Browsable(true)]
        public bool Enable
        {
            set
            {
                this.imgCalendar.Visible = value;
                this.imgClear.Visible = value;
                this.CalendarExtender1.Enabled = value;
            }
        }

        /// <summary>
        /// set textbox width
        /// </summary>
        [Browsable(true)]
        public Unit TextBoxWidth
        {
            set { this.txtCalendar.Width = value; }
        }

        /// <summary>
        /// set stylesheest textbox
        /// </summary>
        [Browsable(true)]
        public string TextBoxCssClass
        {
            set { this.txtCalendar.CssClass = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    //this.txtCalendar.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    this.txtCalendar.Attributes.Add("readonly", "readonly");
                    //this.CalendarExtender1.
                }
                catch { }
            }
        }

        protected void txtCalendar_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (this.TextCalendarChange != null)
                    this.TextCalendarChange(sender, e);
            }
            catch { }
        }

        protected void imgClear_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtCalendar.Text = string.Empty;
            }
            catch { }
        }
    }
}