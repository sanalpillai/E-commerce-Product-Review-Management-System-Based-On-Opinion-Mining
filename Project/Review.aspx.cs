using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HtmlAgilityPack;
using java.io;
using edu.stanford.nlp.tagger.maxent;
using edu.stanford.nlp.ling;
using javac = com.sun.tools.javac.util;
using java.util;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;

public partial class _Default : System.Web.UI.Page
{
    String posstring;
    string newString;
    string ConnString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString;
    SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnString"].ConnectionString);
    public const string Model =
     @"E:\Sanal\####SEM8\Product_Aspect_Ranking\Bin\english-caseless-left3words-distsim.tagger";

    static List<string> D = new List<string>();
    static string allReview = "";
    List<string> lstAk = new List<string>();
    List<TermsLable> lstLable = new List<TermsLable>();
    List<double> lstVaD = new List<double>();
    public static List<Dictionary<string, double>> aspectVaD = new List<Dictionary<string, double>>();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ProductInfo.ProductName = "";
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        ProductInfo.ProductName = DropDownList2.SelectedItem.Text;
        //Old Code
        #region
        string htmlCode;
        List<string> lst = new List<string>();

        string uri = String.Format("http://reviewing.net/s/prd/{0}", DropDownList2.SelectedItem);
        var getHtmlWeb = new HtmlWeb();
        var document = getHtmlWeb.Load(uri);
        Literal Literal1 = new Literal();
        Literal1.ID = "Literal1";
        foreach (HtmlNode para in document.DocumentNode.SelectNodes("//div[@class='review-summary']"))
        {
            Literal1.Text += String.Format(@"<p>{0}</p>", para.InnerText);
            D.Add((string)(para.InnerText));
        }
        // Panel1.Controls.Add(Literal1);
        Literal1.Text = Literal1.Text.Replace("<p>", " ");
        txtPara.Text = Literal1.Text.Replace("</p>", " ");
        allReview = "";
        allReview = txtPara.Text;
        #endregion

        //New Code
        #region
        //if (FileUpload1.HasFile)
        //{
        //    try
        //    {
        //        string FilePath = System.IO.Path.GetFileName(FileUpload1.FileName);
        //        using (System.IO.StreamReader sr = new System.IO.StreamReader(FileUpload1.FileContent))
        //        {
        //            String line = sr.ReadToEnd();
        //            txtPara.Text = line;
        //            allReview = txtPara.Text;
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        #endregion
    }

    private void TagReader(Reader reader)
    {
        var tagger = new MaxentTagger(Model);
        //List obj = (List)MaxentTagger.tokenizeText(reader);
        foreach (ArrayList sentence in MaxentTagger.tokenizeText(reader).toArray())
        {
            var tSentence = tagger.tagSentence(sentence);
            System.Console.WriteLine(Sentence.listToString(tSentence, false));
            posstring = (Sentence.listToString(tSentence, false));
            newString = newString + posstring;
            System.Console.WriteLine();
        }

    }

    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btnPos_Click(object sender, EventArgs e)
    {

        //////word replacement 

        SqlDataAdapter _objadp = new SqlDataAdapter("select * from Word_Master", conn);
        DataTable objdt = new DataTable();
        _objadp.Fill(objdt);
        for (int i = 0; i < objdt.Rows.Count; i++)
        {
            string word = objdt.Rows[i]["word"].ToString();
            string replace_word = objdt.Rows[i]["replace_word"].ToString();

            txtPara.Text = txtPara.Text.Replace(word, " " + replace_word + " ");
        }

        //////
        allReview = txtPara.Text;
        txtreplacement.Text = allReview;
        string[] chars = new string[] { ",", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]", "-" };
        string replacestr = txtPara.Text;
        for (int i = 0; i < chars.Length; i++)
        {
            if (replacestr.Contains(chars[i]))
            {
                replacestr = replacestr.Replace(chars[i], " ");
            }
        }
        TagReader(new StringReader(replacestr));
        txtPara.Text = newString;

        //string replacestr = Regex.Replace(txtPara.Text, "[^a-zA-Z0-9._]+", " ");
        //TagReader(new StringReader(replacestr));

    }
    protected void btnFeature_Click(object sender, EventArgs e)
    {
        truncate();
        int sumPa = 0;
        int sumNa = 0;
        string[] para = txtPara.Text.Split('.');
        para = para.Where((s => (s != "/") && (s != ""))).ToArray();// lamda expression to remove blank paragraph.

        List<string> lstNN = new List<string>();
        List<string> lstJJ = new List<string>();
        for (int i = 0; i < para.Length; i++)
        {
            string line = para[i];
            string[] word = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int j = 0; j < word.Length; j++)
            {
                int startindex = word[j].IndexOf("/");
                if (startindex < 0) { startindex = 0; }
                int lastindex = word[j].Length;
                string NewPos = word[j].Remove(0, startindex);
                string NewWord = word[j].Substring(0, startindex);
                switch (NewPos)
                {
                    case "/NN":
                        {
                            //System.Console.Write("NN");
                            SqlDataAdapter daCheckF = new SqlDataAdapter(new SqlCommand("select * from Feature where Feature='" + NewWord + "'", conn));
                            DataTable dtCheckF = new DataTable();
                            daCheckF.Fill(dtCheckF);
                            if (dtCheckF.Rows.Count > 0)
                            {
                                lstNN.Add(NewWord);
                            }
                            break;
                        }
                    case "/NNP":
                        {
                            //System.Console.Write("NNP");
                            SqlDataAdapter daCheckF = new SqlDataAdapter(new SqlCommand("select * from Feature where Feature='" + NewWord + "'", conn));
                            DataTable dtCheckF = new DataTable();
                            daCheckF.Fill(dtCheckF);
                            if (dtCheckF.Rows.Count > 0)
                            {
                                lstNN.Add(NewWord);
                            }
                            break;
                        }
                    case "/JJ":
                        {
                            System.Console.Write("JJ");
                            lstJJ.Add(NewWord);
                            break;
                        }
                    case "/JJR":
                        {
                            System.Console.Write("JJR");
                            lstJJ.Add(NewWord);

                            break;
                        }

                    case "/JJS":
                        {
                            System.Console.Write("JJS");
                            lstJJ.Add(NewWord);

                            break;
                        }

                    default:
                        { break; }
                }
            }
            for (int jj = 0; lstJJ.Count < lstNN.Count; jj++)
            {
                if (lstNN.Count > lstJJ.Count && lstNN.Count > 0)
                {
                    lstNN.RemoveAt(lstNN.Count - 1);
                }
            }
            for (int nn = 0; lstNN.Count < lstJJ.Count; nn++)
            {
                if (lstJJ.Count > lstNN.Count && lstJJ.Count > 0)
                {
                    lstJJ.RemoveAt(lstJJ.Count - 1);
                }
            }

            for (int k = 0; k < lstNN.Count; k++)
            {
                // int n= lstJJ.Count;
                lstAk.Add(lstNN[k]);
                SqlDataAdapter da = new SqlDataAdapter("insert into FO_Table(Feature,value) values('" + lstNN[k] + "','" + lstJJ[k] + "')", conn);
                conn.Open();
                da.SelectCommand.ExecuteNonQuery();
                conn.Close();
                string query = "select MAX(PosScore)as PosScore,MAX(NegScore)as NegScore from dbo.SentiWordNet where SynsetTerms='" + lstJJ[k] + "'";
                // string query = "select max(PosScore)as PosScore,max(NegScore)as NegScore from SentiWordNet where SynsetTerms='" + lstJJ[k] + "'";
                SqlDataAdapter da1 = new SqlDataAdapter(query, conn);
                conn.Open();
                da1.SelectCommand.ExecuteNonQuery();
                DataTable dt = new DataTable();
                da1.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    double posvalue = 0;
                    double Negvalue = 0;
                    if (dt.Rows[0]["PosScore"].Equals(DBNull.Value))
                    {
                        posvalue = 0;
                    }
                    else
                    {
                        posvalue = Convert.ToDouble(dt.Rows[0]["PosScore"]);
                    }

                    if (dt.Rows[0]["NegScore"].Equals(DBNull.Value))
                    {
                        Negvalue = 0;
                    }
                    else
                    {
                        Negvalue = Convert.ToDouble(dt.Rows[0]["NegScore"]);
                    }
                    if (posvalue > Negvalue)
                    {
                        string feature = lstNN[k];
                        string NewWord = lstJJ[k];
                        conn.Open();
                        SqlDataAdapter da3 = new SqlDataAdapter(new SqlCommand("update FO_Table set PositivePolarity=" + 1 + ",value='" + NewWord + "' where Feature='" + feature + "' and value='" + NewWord + "'", conn));
                        da3.SelectCommand.ExecuteNonQuery();
                        conn.Close();
                    }
                    else if (Negvalue > posvalue || Negvalue != 0)
                    {
                        string feature = lstNN[k];
                        string NewWord = lstJJ[k];
                        conn.Open();
                        SqlDataAdapter da3 = new SqlDataAdapter("update FO_Table set NegativePolarity=" + 1 + ",value='" + NewWord + "' where feature='" + feature + "' and value='" + NewWord + "'", conn);
                        da3.SelectCommand.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            lstNN.Clear();
            lstJJ.Clear();
        }

        var distinctAspects = new List<string>(lstAk.Distinct());
        foreach (var item in distinctAspects.ToList())
        {
            int count = Regex.Matches(allReview.ToLower(), item.ToLower()).Count; //Count Aspect in Review
            int TotalWords = Regex.Matches(allReview, @"[A-Za-z0-9]+").Count;

            SqlCommand cmdInsertCount = new SqlCommand("insert into Aspect_Count(term, tCount)values(@term, @tCount)", conn);
            cmdInsertCount.Parameters.AddWithValue("@term", item);
            cmdInsertCount.Parameters.AddWithValue("@tCount", count);
            conn.Open();
            cmdInsertCount.ExecuteNonQuery();
            conn.Close();


            SqlDataAdapter daGetLablePolarity = new SqlDataAdapter(new SqlCommand("select case when Pa IS null then 0 else Pa end as Pa,case when Na IS null then 0 else Na end as Na from (select SUM(PositivePolarity)as Pa,SUM(NegativePolarity)as Na from dbo.FO_Table where Feature='" + item + "')t", conn));
            DataTable dtGetLablePolarity = new DataTable();
            daGetLablePolarity.Fill(dtGetLablePolarity);
            if (dtGetLablePolarity.Rows.Count > 0)
            {
                TermsLable tl = new TermsLable();
                tl.Term = item;
                tl.Pa = (int)(dtGetLablePolarity.Rows[0]["Pa"]);
                tl.Na = (int)(dtGetLablePolarity.Rows[0]["Na"]);
                lstLable.Add(tl);

                //List<Dictionary<string, int>> ObjPositivelabel = new List<Dictionary<string, int>>();
                //ObjPositivelabel.Add(new Dictionary<string, int>() { { tl.Term, tl.Pa } });
            }


            sumPa = sumPa + (int)(dtGetLablePolarity.Rows[0]["Pa"]);
            sumNa = sumNa + (int)(dtGetLablePolarity.Rows[0]["Na"]);
            double VaD = 0;

            // VaD = count * Math.Log(Convert.ToDouble((Convert.ToDouble(dtGetLablePolarity.Rows[0]["Na"]) == 0 ? 1 : Convert.ToDouble(dtGetLablePolarity.Rows[0]["Na"])) / (Convert.ToDouble(dtGetLablePolarity.Rows[0]["Pa"]) == 0 ? 1 : Convert.ToDouble(dtGetLablePolarity.Rows[0]["Pa"]))), 2);
            VaD = count * (Convert.ToDouble((Convert.ToDouble(dtGetLablePolarity.Rows[0]["Na"]) == 0 ? 1 : Convert.ToDouble(dtGetLablePolarity.Rows[0]["Na"])) / (Convert.ToDouble(dtGetLablePolarity.Rows[0]["Pa"]) == 0 ? 1 : Convert.ToDouble(dtGetLablePolarity.Rows[0]["Pa"]))));

            aspectVaD.Add(new Dictionary<string, double>() { { item.ToString(), VaD } });
            lstVaD.Add(VaD);

        }
        int CountPa;
        int CountNa;

        int a = 0;
        foreach (var AspTrm in distinctAspects.ToList())
        {
            SqlDataAdapter daGetLablePolarity = new SqlDataAdapter(new SqlCommand("select case when Pa IS null then 0 else Pa end as Pa,case when Na IS null then 0 else Na end as Na from (select SUM(PositivePolarity)as Pa,SUM(NegativePolarity)as Na from dbo.FO_Table where Feature='" + AspTrm + "')t", conn));
            DataTable dtGetLablePolarity = new DataTable();
            daGetLablePolarity.Fill(dtGetLablePolarity);
            if (dtGetLablePolarity.Rows.Count > 0)
            {

                int count = Regex.Matches(allReview.ToLower(), AspTrm.ToLower()).Count; //Count Aspect in Review
                int TotalWords = Regex.Matches(allReview, @"[A-Za-z0-9]+").Count;

                //double alph = (double)(sumPa) / (double)(distinctAspects.Count) * (Convert.ToDouble((int)(dtGetLablePolarity.Rows[0]["Pa"])) / (double)(sumPa));
                //double beta = (double)(sumNa) / (double)(distinctAspects.Count) * (Convert.ToDouble((int)(dtGetLablePolarity.Rows[0]["Na"])) / (double)(sumNa));
                double alph = Convert.ToDouble((int)(dtGetLablePolarity.Rows[0]["Pa"])) / Convert.ToDouble(sumPa);
                double beta = Convert.ToDouble((int)(dtGetLablePolarity.Rows[0]["Na"])) / Convert.ToDouble(sumNa);
                if (double.IsNaN(alph) == true)
                    alph = 0.0;

                if (double.IsNaN(beta) == true)
                    beta = 0.0;

                if (lstVaD[a].ToString() == "-Infinity" || lstVaD[a].ToString() == "Infinity")
                {
                    lstVaD[a] = 0;
                }
                double w = 0;
                w = lstVaD[a] - (alph - beta);

                SqlCommand cmdW = new SqlCommand("update FO_Table set Weight=@w where Feature=@f", conn);
                cmdW.Parameters.AddWithValue("@w", w);
                cmdW.Parameters.AddWithValue("@f", AspTrm);
                conn.Open();
                cmdW.ExecuteNonQuery();
                conn.Close();
                a++;
            }
        }


        SqlCommand cmdGetFeature = new SqlCommand("truncate table Ranking;select distinct Feature from FO_Table where Weight>=0", conn);
        conn.Open();
        SqlDataReader dr = cmdGetFeature.ExecuteReader();
        if (dr.HasRows)
        {
            while (dr.Read())
            {
                int Tcount = Regex.Matches(allReview, dr["Feature"].ToString()).Count;
                int TotalWords = Regex.Matches(allReview, @"[A-Za-z0-9]+").Count;
                double weight = Convert.ToDouble(Tcount) / Convert.ToDouble(TotalWords);
                SqlConnection con = new SqlConnection(ConnString);
                SqlCommand cmdInsertRank = new SqlCommand("Insert into Ranking(Feature, fRank,weight)values(@Feature, @fRank,@weight)", con);
                cmdInsertRank.Parameters.AddWithValue("@Feature", dr["Feature"].ToString());
                cmdInsertRank.Parameters.AddWithValue("@fRank", Tcount);
                cmdInsertRank.Parameters.AddWithValue("@weight", weight);
                con.Open();
                cmdInsertRank.ExecuteNonQuery();
                con.Close();
            }
        }
        conn.Close();
        Response.Redirect("Result.aspx");
    }
    protected void truncate()
    {
        SqlCommand cmd = new SqlCommand("truncate table FO_Table;truncate table Aspect_Count", conn);
        conn.Open();
        cmd.ExecuteNonQuery();
        conn.Close();
    }

}
public class TermsLable
{
    public string Term { get; set; }
    public int Pa { get; set; }
    public int Na { get; set; }
}
