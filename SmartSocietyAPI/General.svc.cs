using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace SmartSocietyAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class General : IGeneral
    {
        private Boolean Mail(string Email, string subject, string body)
        {
            try
            {
                MailMessage Msg = new MailMessage("riteshdlab@gmail.com", Email);
                Msg.Subject = subject;
                Msg.Body = body;
                Msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                //smtp.Host = "relay-hosting.secureserver.net";
                smtp.Port = 587;
                //smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                //smtp.EnableSsl = false;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                NetworkCredential MyCredentials = new NetworkCredential("riteshdlab@gmail.com", "Ritesh202$");
                smtp.Credentials = MyCredentials;
                smtp.Send(Msg);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }        
    }
}
