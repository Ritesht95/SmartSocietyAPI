using System;

namespace SmartSocietyAPI
{
    public partial class AutomationCall : System.Web.UI.Page
    {
        SmartSocietyAPI.General ServerObjectGen = new SmartSocietyAPI.General();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["IP"] != "" && Request.QueryString["IP"] != null)
            {
                if (Convert.ToBoolean(ServerObjectGen.SetIP(Request.QueryString["IP"])) == true)
                {
                    Response.Write("*True*");
                }
                else
                {
                    Response.Write("*False*");
                }
            }
        }
    }
}