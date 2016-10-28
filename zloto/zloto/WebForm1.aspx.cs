using System;
using System.Xml;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;

namespace zloto
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string link;
        private DataTable goldDataTable;
        Image sortImage = new Image();
        private string _sortDirection;
        string startD;
        string endD;
        decimal priceMin = 100000;
        decimal priceMax = 0;
        decimal startPriceMin;
        decimal startPriceMax;
        DateTime dateMin;
        DateTime dateMax;
        List<decimal> priceList;

        protected void Page_Load(object sender, EventArgs e)
        {
            priceList = new List<decimal>();

            if (!IsPostBack)
            {
                txtStart.Text = DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd");
                txtEnd.Text = DateTime.Today.ToString("yyyy-MM-dd");
            }

            getButton.Click += new EventHandler(this.GetBtn_Click);
            getFilter.Click += new EventHandler(this.FltrBtn_Click);
            resetFilter.Click += new EventHandler(this.RstBtn_Click);
            Table_Get(txtStart.Text, txtEnd.Text);

        }

        void GetBtn_Click(Object sender, EventArgs e)
        {
            startD = txtStart.Text;
            endD = txtEnd.Text;

            DateTime temp;
            DateTime temp2;
            if (DateTime.TryParse(startD, out temp) && DateTime.TryParse(endD, out temp2))
            {

                if (Convert.ToDateTime(startD) > Convert.ToDateTime(endD))
                {
                    errorLabel.Text = "Data początkowa nie może być późniejsza od końcowej.";
                }
                else if ((Convert.ToDateTime(endD) - Convert.ToDateTime(startD)).TotalDays > 93)
                {
                    errorLabel.Text = "Przekroczono limit serwera. Liczba dni między datami może wynosić maksymalnie 93 dni. Danie nie będą wyświetlane poprawnie!";
                }
                else
                {
                    errorLabel.Text = null;
                    Table_Get(startD.ToString(), endD.ToString());
                    priceMinBox.Text = priceMin.ToString();
                    priceMaxBox.Text = priceMax.ToString();
                }
            }
            else
            {
                errorLabel.Text = "Nieprawidłowy format daty lub data jest wcześniejsza niż rok 2013.";
            }
        }

        void FltrBtn_Click(Object sender, EventArgs e)
        {
            Filter();
        }

        void RstBtn_Click(Object sender, EventArgs e)
        {
            priceMinBox.Text = startPriceMin.ToString();
            priceMaxBox.Text = startPriceMax.ToString();
        }

        void Table_Get(string startDate, string endDate)
        {
            link = ("http://api.nbp.pl/api/cenyzlota/" + startDate + "/" + endDate + "?format=xml");

            goldDataTable = new DataTable("GoldPrice");

            for (int i = 0; i < 2; i++)
            {
                string cName;

                if (i == 0)
                {
                    cName = "Data publikacji";
                }
                else
                {
                    cName = "Cena 1 g złota (w próbie 1000)";
                }

                goldDataTable.Columns.Add(cName);
            }

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(link);

                XmlElement root = doc.DocumentElement;
                XmlNodeList nodes = root.SelectNodes(" / ArrayOfCenaZlota/CenaZlota");

                foreach (XmlNode node in nodes)
                {
                    string goldDate = node["Data"].InnerText;
                    string goldPrice = node["Cena"].InnerText.Replace('.', ',');
                    priceList.Add(Convert.ToDecimal(goldPrice));

                    string[] obj = new string[2];
                    obj[0] = goldDate;
                    obj[1] = goldPrice;
                    goldDataTable.Rows.Add(obj);

                    if (Convert.ToDecimal(goldPrice) < priceMin)
                    {
                        priceMin = Convert.ToDecimal(goldPrice);
                        dateMin = Convert.ToDateTime(goldDate);
                    }

                    if (Convert.ToDecimal(goldPrice) > priceMax)
                    {
                        priceMax = Convert.ToDecimal(goldPrice);
                        dateMax = Convert.ToDateTime(goldDate);
                    }
                }

                if (!IsPostBack)
                {
                    priceMinBox.Text = priceMin.ToString();
                    priceMaxBox.Text = priceMax.ToString();
                }


                startPriceMin = priceMin;
                startPriceMax = priceMax;

                minPriceLabel.Text = "Najniższa cena (" + priceMin + ") w dniu " + dateMin.Day + "." + dateMin.Month + "." + dateMin.Year + ".";
                maxPriceLabel.Text = "Najwyższa cena (" + priceMax + ") w dniu " + dateMax.Day + "." + dateMax.Month + "." + dateMax.Year + ".";
                medianLabel.Text = "Mediana z danego okresu wynosi " + Median(priceList) + ".";

                BindGrid();
            }
            catch
            {
                errorLabel.Text = "Błąd w pobieraniu danych z serweru banku. Serwer niedostępny bądź podany zakres obejmuje jedynie dni bez publikacji cen.";
            }
            
        }


        private void BindGrid()
        {
            goldTable.DataSource = goldDataTable;
            goldTable.DataBind();
        }


        private decimal Median(List<decimal> list)
        {
            list.Sort();
            int count = list.Count;

            if (count % 2 == 0)
            {
                decimal a = list[count / 2 - 1];
                decimal b = list[count / 2];
                return Math.Round((decimal)((a + b) / 2), 2);
            }
            else
            {
                return list[count / 2];
            }

        }

        void Filter()
        {
            if (priceMinBox.Text == "")
            {
                priceMinBox.Text = startPriceMin.ToString();
            }

            if (priceMaxBox.Text == "")
            {
                priceMaxBox.Text = startPriceMax.ToString();
            }

            decimal dTemp;
            decimal dTemp2;
            if (decimal.TryParse(priceMinBox.Text, out dTemp) && decimal.TryParse(priceMaxBox.Text, out dTemp2))
            {
                errorLabel.Text = "";
                if (priceMinBox.Text != "" && priceMaxBox.Text != "")
                {
                    int numb = goldTable.Rows.Count;

                    for (int i = 0; i < numb; i++)
                    {
                        if (Convert.ToDecimal(goldTable.Rows[i].Cells[1].Text) > Convert.ToDecimal(priceMaxBox.Text)
                            || Convert.ToDecimal(goldTable.Rows[i].Cells[1].Text) < Convert.ToDecimal(priceMinBox.Text))
                        {
                            goldTable.Rows[i].Visible = false;
                        }
                    }
                }
            }
            else
            {
                errorLabel.Text = "Zły format ceny.";
            }
        }

        public string SortDirection
        {
            get
            {
                if (ViewState["SortDirection"] == null)
                    return string.Empty;
                else
                    return ViewState["SortDirection"].ToString();
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        protected void goldTable_Sorting(object sender, GridViewSortEventArgs e)
        {
            SetSortDirection(SortDirection);
            if (goldDataTable != null)
            {
                goldDataTable.DefaultView.Sort = e.SortExpression + " " + _sortDirection;
                goldTable.DataSource = goldDataTable;
                goldTable.DataBind();
                SortDirection = _sortDirection;
            }

            BindGrid();
            Filter();
        }

        protected void SetSortDirection(string sortDirection)
        {
            if (sortDirection == "ASC")
            {
                _sortDirection = "DESC";
            }
            else
            {
                _sortDirection = "ASC";
            }
        }

        protected void goldTable_DataBound(object sender, EventArgs e)
        {
        }

        protected void goldTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void goldTable_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}