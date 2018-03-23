using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class _Default : System.Web.UI.Page
{
    SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            FillGrid();
            fillTfidfGrid();
        }
    }
    private void FillGrid()
    {
        SqlDataAdapter daGrid = new SqlDataAdapter(new SqlCommand("select * from Ranking order by fRank desc", conn));
        DataTable dt = new DataTable();
        daGrid.Fill(dt);
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    private void fillTfidfGrid()
    {
        SqlDataAdapter daGrid = new SqlDataAdapter(new SqlCommand("select Feature,Fcount from (select distinct Feature,COUNT(Feature)as Fcount from FO_Table where  PositivePolarity>0 group by Feature)t order by Fcount desc", conn));
        DataTable dt = new DataTable();
        daGrid.Fill(dt);
        GridView2.DataSource = dt;
        GridView2.DataBind();
    }
}