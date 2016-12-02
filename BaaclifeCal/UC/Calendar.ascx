<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Calendar.ascx.cs" Inherits="UnmatchPayment.UC.WebUserControl1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<script type="text/javascript">
    function checkDate(sender, args) {
        var strCal = document.getElementById('<%=this.txtCalendar.ClientID %>');

        if (sender._selectedDate != null) {
            var year = parseInt(sender._selectedDate.getFullYear());
            //alert(sender._selectedDate.Date);
            if (year < 2000)
                year = year + 543;
            if (year > 2600)
                year = year - 543;
            var month = parseInt(sender._selectedDate.getMonth());
            var day = parseInt(sender._selectedDate.getDate());

            var newDate = new Date(year, month, day + 1);

            sender._selectedDate = newDate;
        }
    }

</script>

<table border="0" cellpadding="0" cellspacing="0" id="UCcalendar">
    <tr >
        <td style="width: 130px;">
            <asp:TextBox ID="txtCalendar" runat="server" AutoPostBack="true" OnTextChanged="txtCalendar_TextChanged"
                CssClass="textBox" Height="23px" Width="174px"></asp:TextBox>
        </td>
        <td style="cursor: pointer; padding: 3px;">
            <asp:Image ID="imgCalendar" runat="server" ImageUrl="I006_calendar.png"
                AlternateText="คลิกเพื่อเลือกวันที่" />
        </td>
        <td style="cursor: pointer; padding: 3px;">
            <asp:ImageButton ID="imgClear" runat="server" ImageUrl="I007_eraser.png"
                AlternateText="คลิกเพื่อลบวันที่" OnClick="imgClear_Click" />
        </td>
    </tr>
</table>
<div style="display: none">
    <asp:CalendarExtender ID="CalendarExtender1" runat="server" Enabled="True" TargetControlID="txtCalendar"
        PopupButtonID="imgCalendar" Format="dd/MM/yyyy" OnClientShowing="checkDate" >
    </asp:CalendarExtender>
</div>