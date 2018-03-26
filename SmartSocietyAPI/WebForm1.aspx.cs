using System;
using System.Text;

namespace SmartSocietyAPI
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        private string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SmartSocietyAPI.Client ServiceObject = new SmartSocietyAPI.Client();
            Response.Write(Encryptdata("12345"));
            Response.Write(ServiceObject.CheckLogin("rdtailor@gmail.com", "12345"));
        }
    }
}