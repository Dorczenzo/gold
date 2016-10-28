<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="zloto.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Ceny złota</title>
    <%--<link href="styles/jquery-ui-1.8.19.custom.css" rel="stylesheet" />--%>
    <link href="styles/jquery-ui.css" rel="stylesheet" />
    <link href="styles/Site.css" rel="stylesheet" />
    <script src="scripts/jquery-1.7.2.min.js"></script>
    <script src="scripts/jquery-ui-1.8.19.custom.min.js"></script>
    <script src="scripts/formatDate.js"></script>
    <style type="text/css">
        .auto-style1 {
            width: 203px;
            height: 33px;
        }
        .auto-style2 {
            width: 203px;
            height: 45px;
        }
        .auto-style3 {
            height: 45px;
        }
        .auto-style4 {
            width: 203px;
            height: 42px;
        }
        .auto-style5 {
            height: 42px;
        }
        .auto-style7 {
            width: 91%;
            margin-left: 40px;
        }
        .auto-style8 {
            margin-left: 40px;
        }
        .auto-style9 {
            width: 90%;
            margin-left: 43px;
        }
        .auto-style10 {
            height: 19px;
        }
        .auto-style12 {
            height: 33px;
        }
    </style>
    </head>
<body>
    <header>.</header>
    <img style="width: 980px; height: 100px; max-height: none; max-width: none; display: block; margin: 0 auto;" alt="logo" src="images/gold_header.jpg" />
    <form class="page" id="form1" runat="server">
    <h1>Porównywarka cen złota</h1>
        <p>&nbsp;</p>
        <div>
                <table class="auto-style7">

                    <tr>
                        <td class="auto-style4">
                            Data początkowa:<asp:TextBox ID="txtStart" runat="server" MaxLength="10">2016-09-29</asp:TextBox>
                        </td>
                        <td class="auto-style5">
                            Dolny zakres cen:<br />
                            <asp:TextBox ID="priceMinBox" runat="server" MaxLength="6"></asp:TextBox>
                        </td>
                    </tr>
                         <tr>
                        <td class="auto-style2">
                            Data końcowa:<asp:TextBox ID="txtEnd" runat="server" MaxLength="10">2016-10-03</asp:TextBox>
                             </td>
                        <td class="auto-style3">
                            Górny zakres cen:<br />
                            <asp:TextBox ID="priceMaxBox" runat="server" MaxLength="6"></asp:TextBox>
                             </td>
                    </tr>
                         <tr>
                        <td class="auto-style1">
            <asp:Button ID="getButton" runat="server" Text="Pobierz dane" />
                             </td>
                        <td class="auto-style12">
                            <asp:Button ID="getFilter" runat="server" Text="Filtruj" Width="62px" />
                             <asp:Button ID="resetFilter" runat="server" Text="Resetuj" Width="62px"/>
                             </td>
                    </tr>
                </table>
            <br />
            <div style="width: 900px; margin-left:auto; margin-right:auto; color: red; text-align: center">
                <asp:Label ID="errorLabel" runat="server" Text=""></asp:Label>
            </div>
            <br />

            <asp:GridView ID="goldTable" runat="server" AllowSorting="True" BackColor="White" BorderColor="#007870" BorderStyle="Solid" BorderWidth="2px" EmptyDataText="------" ForeColor="Black" OnSorting="goldTable_Sorting" Width="900px" CssClass="auto-style8" OnDataBound="goldTable_DataBound" CellPadding="4" HorizontalAlign="Center">
                <HeaderStyle BackColor="#006666" ForeColor="White" Height="30px" HorizontalAlign="Center" VerticalAlign="Middle" Width="450px" Font-Size="14px" />
                <RowStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="450px"/>
            </asp:GridView>


                <br />
                <table class="auto-style9">
                    <tr>
                        <td class="auto-style10">


            <asp:Label ID="minPriceLabel" runat="server" Text="Label">Najniższa cena (163,32) w dniu 2016/13/43.</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style10">
                            <asp:Label ID="maxPriceLabel" runat="server" Text="Label">Najwyższa cena (163,32) w dniu 2016/13/43.</asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="medianLabel" runat="server" Text="Label">Mediana z danego okresu wynosi 163,32.</asp:Label>
                        </td>
                    </tr>
                </table>
            <p>&nbsp;</p>

    </div>
    </form>
</body>
</html>
