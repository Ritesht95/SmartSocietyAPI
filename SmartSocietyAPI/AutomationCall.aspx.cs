using System;

namespace SmartSocietyAPI
{
    public partial class AutomationCall : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblData.Text = Request.QueryString["Data"].ToString();
            if (Request.QueryString["Data"].ToString()!="" || Request.QueryString["Data"] != null)
            {

            }
        }
    }
}