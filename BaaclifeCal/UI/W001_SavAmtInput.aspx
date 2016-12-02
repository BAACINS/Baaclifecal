<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="W001_SavAmtInput.aspx.cs" Inherits="BaaclifeCal.MainInput" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Src="~/UC/Calendar.ascx" TagPrefix="uc1" TagName="Calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <script type="text/javascript">
        function TestCodeBehind()
        {
            PageMethods.btnCalSavAmt_Click();

        }
        
</script>
    <table class="upperbody">
        <tr>
            <td><%--ส่วน Input กับปุ่ม--%> 
               <table>
               <tr>
                   <td>
                       <%--<asp:button id="btnCalSavAmt" runat="server" cssclass="button" 
                           text="ตารางคำนวณ การส่งฝาก ธกส เพิ่มรัก2 12/10" OnClick="btnCalSavAmt_Click" EnableTheming="True"/>--%>

                       <asp:imagebutton id="btnCalSavAmt" runat="server" ImageUrl="~/Images/button1.png" OnClick="btnCalSavAmt_Click"></asp:imagebutton>
                   </td>
               </tr>
               <tr>
                   <td>
                       <%--<asp:button id="btnCalSurrender" runat="server" cssclass="button" text="ตารางคำนวณ &#010; มูลค่าเวนคืนกรมธรรม์" OnClick="btnCalSurrender_Click" />--%>
                       <asp:imagebutton id="btnCalSurrender" runat="server" ImageUrl="~/Images/button2.png" OnClick="btnCalSurrender_Click"></asp:imagebutton>
                   </td>
               </tr>
               <tr>
                   <td>
                       <%--<asp:button id="btnCalPaidUp" runat="server" cssclass="button" text="ตารางคำนวณ มูลค่าใช้เงินสำเร็จ" OnClick="btnCalPaidUp_Click" />--%>
                       <asp:imagebutton id="btnCalPaidUp" runat="server" ImageUrl="~/Images/button3.png" OnClick="btnCalPaidUp_Click"></asp:imagebutton>
                       
                   </td>
               </tr>
                   <tr>
                   <td>
                       <%--<asp:button id="btnPrint" runat="server" cssclass="button" text="พิมพ์ตารางคำนวณมูลค่ากรมธรรม์" OnClick="btnPrint_Click"/>--%>
                       <asp:imagebutton id="btnPrint" runat="server" ImageUrl="~/Images/button4.png" OnClick="btnPrint_Click"></asp:imagebutton>
                   </td><%----%>
               </tr>
                   <tr>
                   <td>
                       
                   </td><%----%>
               </tr>
         </table> 
            </td>
            <td>
               <table class="features-table">
            <tr>
                <td>ระบุเพศ</td>
                <td>
                    <asp:RadioButton ID="rdbMale" GroupName="gender" runat="server" 
                        Checked="true" Text="ชาย" value="01" AutoPostBack="True" OnCheckedChanged="gender_CheckedChanged"/>
                    <asp:RadioButton ID="rdbFemale" GroupName="gender" runat="server" 
                        Text="หญิง" value="02" AutoPostBack="True" OnCheckedChanged="gender_CheckedChanged" />
                </td>
            </tr>
            <tr>
                <td>ระบุวันเดือนปีเกิด</td>
                <td>
                    <table id="UCcalendar" runat="server">
                            <tr>
                                
                                <td>
                                    <uc1:Calendar id="ucDOB" runat="server"/>
                                </td>
                               <td style="width: 45px; font-weight:bold; color:blue; ">
                                   <asp:label id="lblAge" runat="server"></asp:label>&nbsp;ปี</td>
                            </tr>
                        </table>
                </td> 
            </tr>
            <tr>
                <td></td>
                <td>
                       <asp:linkbutton runat="server" style="color:black;font-size:20px;" OnClick="lnk_Click">(คู่มือการใช้ปฏิทิน)
                          </asp:linkbutton>
                       </td>
            </tr>
            <tr>
                <td>ระบุจำนวนส่งฝากรายเดือน</td>
                <td>
                     <asp:dropdownlist id="ddlPaytype" runat="server" CssClass="dropDownList" OnSelectedIndexChanged="ddlPaytype_SelectedIndexChanged" AutoPostBack="True"></asp:dropdownlist>
                     &nbsp;&nbsp;&nbsp;บาท
                </td>
            </tr>
            <tr>
                <td colspan="2"><h3>ความคุ้มครอง</h3></td>
            </tr>
            <tr>
                <td>กรณีเสียชีวิตจากการเจ็บป่วย รับเงินทุนสงเคราะห์</td>
                <td>
                    <asp:textbox runat="server" id="txtDead" Cssclass="textBox"></asp:textbox> 
                    &nbsp&nbsp&nbsp บาท
                </td>
            </tr>
            <tr>
                <td>กรณีเสียชีวิตจากอุบัติเหตุ รับเงินทุนสงเคราะห์</td>
                <td>
                    <asp:textbox runat="server" id="txtAccident" Cssclass="textBox"></asp:textbox>
                    &nbsp&nbsp&nbsp บาท
                </td>
            </tr>               
            
            <tr>
                <td colspan="2"><h3>ผลประโยชน์ที่ได้รับเมื่อครบกำหนดสัญญา</h3></td>
            </tr>
            <tr>
                <td>เงินครบกำหนดสัญญา</td>
                <td>
                    <asp:textbox runat="server" id="txtEocAmt" Cssclass="textBox"></asp:textbox>
                    &nbsp&nbsp&nbsp บาท
                </td>             
            </tr>
            <tr>
                <td>เงินสมนาคุณ</td>
                <td>
                    <asp:textbox runat="server" id="txtReward" Cssclass="textBox"></asp:textbox>
                    &nbsp&nbsp&nbsp บาท
                </td> 
            </tr>
            <tr>
                <td>รวมเงินครบกำหนดพร้อมเงินสมนาคุณ</td>
                <td>
                    <asp:textbox runat="server" id="txtTotalAmt" Cssclass="textBox"></asp:textbox>
                    &nbsp&nbsp&nbsp บาท
                </td> 
            </tr>
        </table>
            </td>
        </tr>
    </table>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <%--    <div style="margin: 0 auto;max-width: 80%;">
     <div>

    </div>--%>
    <asp:panel id="pnlShowDetail" runat="server" class="lowerbody">
    <asp:gridview id="gvShowDetail" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" 
        AutoGenerateColumns="False" OnRowCreated="gvShowDetail_RowCreated" Visible="False" style="width:100%;">
        <Columns>
            <asp:BoundField DataField="payType">
               <ItemStyle HorizontalAlign="Center" Width="40px"/>
            </asp:BoundField>
            <asp:BoundField DataField="instalment" DataFormatString="{0:N0}">
               <ItemStyle HorizontalAlign="Center" Width="5px"/>
            </asp:BoundField>
            <asp:BoundField DataField="instalmentAmt" DataFormatString="{0:N0}">
               <ItemStyle HorizontalAlign="Center" Width="5px"/>
            </asp:BoundField>
            <asp:BoundField DataField="discount" DataFormatString="{0:N0}">
               <ItemStyle HorizontalAlign="Center" Width="5px"/>
            </asp:BoundField>
            <asp:BoundField DataField="totInstalmentAmt" DataFormatString="{0:N0}">
               <ItemStyle HorizontalAlign="Center" Width="5px"/>
            </asp:BoundField>
            <asp:BoundField DataField="contactAmt" DataFormatString="{0:N0}">
               <ItemStyle HorizontalAlign="Center" Width="5px"/>
            </asp:BoundField>
            <asp:BoundField DataField="totContactAmt" DataFormatString="{0:N0}">
               <ItemStyle HorizontalAlign="Center" Width="5px"/>
            </asp:BoundField>
            <asp:BoundField DataField="netGain" DataFormatString="{0:N0}">
               <ItemStyle HorizontalAlign="Center" Width="5px"/>
            </asp:BoundField>
        </Columns>

        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
    </asp:gridview>
        <div id="divSurrender" runat="server" visible="false"> 
            <table class="detail-table">

                <tr>
                    <th colspan="2">ตารางคำนวณมูลค่าเวนคืนกรมธรรม์</th>
                    <tr>
                        <td colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>การขอเวนคืนกรมธรรม์</b>&nbsp;&nbsp;&nbsp;คือ การที่ผู้ฝากเงินสงเคราะห์ขอยกเลิกกรมธรรม์
                            โดยผู้ฝากเงินสงเคราะห์จะได้รับเงินคืนตามตารางมูลค่าเวนคืนแนบท้ายกรมธรรม์ตามระยะเวลาที่ส่งฝากกรมธรรม์และส่งผลให้กรมธรรม์สิ้นสุดความคุ้มครอง และขาดผลบังคับทันที</td>
                    </tr>
                    <tr>
                        <td align="right">ระบุจำนวนปีที่ส่งฝาก &nbsp;&nbsp;&nbsp;&nbsp; :</td>
                        <td>
                            <asp:DropDownList ID="ddlSavYear" runat="server" AutoPostBack="True" cssclass="dropDownList" OnSelectedIndexChanged="ddlSavYear_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp; ปี </td>
                    </tr>
                    <tr>
                        <td align="right">จำนวนมูลค่าเวนคืนกรมธรรม์ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</td>
                        <td>
                            <asp:TextBox ID="txtSurennderAmt" runat="server" cssclass="textBox"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp; บาท </td>
                    </tr>
                </tr>

            </table>
        </div>

        <div id="divPaidup" runat="server" visible="false"> 
            <table class="detail-table">

                <tr>
                    <th colspan="2">ตารางคำนวณมูลค่าใช้เงินสำเร็จ </th>
                    <tr>
                        <td colspan="2">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>การขอรับความคุ้มครองตามมูลค่าใช้เงินสำเร็จ</b>&nbsp;&nbsp;&nbsp; จะได้ในกรณีกรมธรรม์ขาดผลบังคับ
                            ความคุ้มครองตามจำนวนเงินทุนสงเคราะห์ของผู้ฝากเงินสงเคราะห์ จะถูกลดลงเท่ากับมูลค่าใช้
                            เงินสำเร็จตามตารางท้ายกรมธรรม์
                            <br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;ผู้ฝากเงินสงเคราะห์ชีวิต จะได้รับเงินมูลค่าใช้เงินสำเร็จกรณีเสียชีวิตหรือครบกำหนด
                                สัญญา และความคุ้มครองอื่นๆได้แก่ ความคุ้มครองจากการเสียชีวิตจากอุบัติเหตุ และกรณีชดเชยรายได้จากการประสบอุบัติเหตุนอนพักรักษาตัวในโรงพยาบาล จะเป็นอัน<b><u>สิ้นสุดทันที</u></b>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">ระบุจำนวนปีที่ส่งฝาก&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</td>
                        <td>
                            <asp:DropDownList ID="ddlSavYearP" runat="server" AutoPostBack="True" cssclass="dropDownList" OnSelectedIndexChanged="ddlSavYearP_SelectedIndexChanged">
                            </asp:DropDownList>
                            &nbsp;&nbsp;&nbsp; ปี </td>
                    </tr>
                    <tr>
                        <td align="right">จำนวนมูลค่าใช้เงินสำเร็จ&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;:</td>
                        <td>
                            <asp:TextBox ID="txtPaidUp" runat="server" cssclass="textBox"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp; บาท</td>
                    </tr>
                </tr>
            </table>
        </div>
        </asp:panel>
</asp:Content>
