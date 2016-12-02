<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="WR001_SavAmt.aspx.cs" Inherits="BaaclifeCal.UI.WR001_SavAmt" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <asp:panel id="pnlShowDetail" runat="server" >
    <asp:gridview id="gvShowDetail" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" 
        AutoGenerateColumns="False" OnRowCreated="gvShowDetail_RowCreated">
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
        </asp:panel>
</asp:Content>
