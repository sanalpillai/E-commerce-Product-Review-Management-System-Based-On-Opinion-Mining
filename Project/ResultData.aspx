<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="ResultData.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:GridView ID="GridView3" Width="100%" AutoGenerateColumns="false" runat="server"
        CellPadding="4" ForeColor="#333333" GridLines="None" Style="text-align: center">
        <Columns>
            <asp:BoundField DataField="Feature" HeaderText="Feature" SortExpression="Feature" />
            <asp:BoundField DataField="P_Polarity" HeaderText="+ Polarity" SortExpression="P_Polarity" />
            <asp:BoundField DataField="N_Polarity" HeaderText="- Polarity" SortExpression="N_Polarity" />
            <asp:BoundField DataField="total" HeaderText="Rating" SortExpression="total" />
        </Columns>
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        <EditRowStyle BackColor="#999999" />
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#E9E7E2" />
        <SortedAscendingHeaderStyle BackColor="#506C8C" />
        <SortedDescendingCellStyle BackColor="#FFFDF8" />
        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
    </asp:GridView>
</asp:Content>
