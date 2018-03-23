using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Default2 : System.Web.UI.Page
{
    SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["connString"].ConnectionString);
    private static DataTable dt;
    protected void Page_Load(object sender, EventArgs e)
    {

        string query = "select Feature,COUNT(*) as countOfColoumn , COUNT(PositivePolarity)as PositivePolarity,COUNT(NegativePolarity)as NegativePolarity from FO_Table  where value is not null group by feature";
        SqlDataAdapter da = new SqlDataAdapter(query, conn);
        dt = new DataTable();
        da.Fill(dt);

        ////chage by N   
        SqlCommand cmdtruncate = new SqlCommand("truncate table tempData", conn);
        conn.Open();
        cmdtruncate.ExecuteNonQuery();
        conn.Close();
        float total_polarity = 0;
        int a = 0;
        int b = 0;
        float c = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            a = Convert.ToInt32(dt.Rows[i]["PositivePolarity"]);
            b = Convert.ToInt32(dt.Rows[i]["NegativePolarity"]);
            c = a + b;
            if (c > 0)
            {
                float d = 0;
                d = a / c;
                total_polarity = d * 10;
                double twoDec = Math.Round(total_polarity, 1);
                SqlCommand cmd = new SqlCommand("Insert into tempData values(@Feature, @P_Polarity, @N_Polarity,@total)", conn);
                cmd.Parameters.AddWithValue("@Feature", dt.Rows[i]["Feature"]);
                cmd.Parameters.AddWithValue("@P_Polarity", dt.Rows[i]["PositivePolarity"]);
                cmd.Parameters.AddWithValue("@N_Polarity", dt.Rows[i]["NegativePolarity"]);
                cmd.Parameters.AddWithValue("@total", twoDec);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            else
            {
                SqlCommand cmd = new SqlCommand("Insert into tempData values(@Feature, @P_Polarity, @N_Polarity,@total)", conn);
                cmd.Parameters.AddWithValue("@Feature", dt.Rows[i]["Feature"]);
                cmd.Parameters.AddWithValue("@P_Polarity", dt.Rows[i]["PositivePolarity"]);
                cmd.Parameters.AddWithValue("@N_Polarity", dt.Rows[i]["NegativePolarity"]);
                cmd.Parameters.AddWithValue("@total", 0);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

        }
        ////
        //GridView1.DataSource = dt;
        //GridView1.DataBind();
        string queryData = "select * from tempData";
        SqlDataAdapter _adp = new SqlDataAdapter(queryData, conn);
        DataTable _dt = new DataTable();
        _adp.Fill(_dt);
        if (dt.Rows.Count > 0)
        {
            GridView3.DataSource = _dt;
            GridView3.DataBind();

            //Chart2.Series[0].XValueMember = "Feature";
            //Chart2.Series[0].YValueMembers = "total";
            //Chart2.Series[0].YValuesPerPoint = 2;
        
            Chart2.DataSource = _dt;          
            Chart2.DataBind();
        }

        int xAxisValue = 0;
        int yAxisPositiveValue = 0;
        int yAxisNegativeValue = 0;
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            xAxisValue += Convert.ToInt32(dt.Rows[i]["countOfColoumn"]);
            yAxisPositiveValue += Convert.ToInt32(dt.Rows[i]["PositivePolarity"]);
            yAxisNegativeValue += Convert.ToInt32(dt.Rows[i]["NegativePolarity"]);
        }

        double total = (yAxisPositiveValue + yAxisNegativeValue);
        double yPos = Convert.ToDouble(Convert.ToDouble(yAxisPositiveValue) / total) * 100;
        double yNeg = Convert.ToDouble(Convert.ToDouble(yAxisNegativeValue) / total) * 100;

        Chart1.Series["Series1"].IsValueShownAsLabel = true;
        Chart1.Series["Series1"].Points.AddXY(xAxisValue, yAxisPositiveValue);
        Chart1.Series["Series1"].Points.AddXY(xAxisValue, yAxisNegativeValue);
        Chart1.DataBind();
        Chart1.Series["Series1"].Points[0].LabelToolTip = "Positive Polarity";
        Chart1.Series["Series1"].Points[1].LabelToolTip = "Negative Polarity";

        lblShow.Text = "Total positive percentage of " + ProductInfo.ProductName + " is " + Convert.ToInt32(yPos) + "%";
        //Response.Redirect("ResultData.aspx");
    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        dt.DefaultView.Sort = e.SortExpression;
        GridView3.DataSource = dt;
        GridView3.DataBind();
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            SqlDataAdapter da = new SqlDataAdapter(new SqlCommand("select * from ConstructiveMaster where cWord='" + e.Row.Cells[0].Text + "'", conn));
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                //e.Row.Cells[3].Text = "Constructive";
            }
            else
            {
                //e.Row.Cells[3].Text = "-";
            }

        }
    }
}