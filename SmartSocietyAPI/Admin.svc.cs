using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace SmartSocietyAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Admin : IAdmin 
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

        public string CheckLogin(string Username, string Password)
        {
            var DC = new DataClassesDataContext();
            var ObjLogin = (from ob in DC.tblAdminLogins
                            where ob.AdminLoginName == Username && ob.Password == Password 
                            && ob.IsBlocked==false
                            select ob);
            if (ObjLogin.Count() == 1)
            {
                return JsonConvert.SerializeObject(ObjLogin.Single());
            }
            else
            {
                return "False";
            }
        }

        public string ForgotPassword(string Username)
        {
            var DC = new DataClassesDataContext();
            var ObjForgotPass = (from ob in DC.tblAdminLogins
                            where ob.AdminLoginName == Username && ob.IsBlocked == false
                            select ob);
            if (ObjForgotPass.Count() == 1)
            {
                var ObjData = ObjForgotPass.Single();
                if (Mail(ObjData.Email, "Smart Society: Reset Password", "Link to reset password: http://localhost/ForgetPassword.aspx?Username="+ObjData.AdminLoginName+"<br>Regards,<br>Smart Society(Society Management System)"))
                {
                    return "True";
                }
                else
                {
                    return "MailFalse";
                }
            }
            else
            {
                return "False";
            }
        }

        public string ResetPassword(string Username, string Password)
        {
            var DC = new DataClassesDataContext();
            var ObjReset = (from ob in DC.tblAdminLogins
                            where ob.AdminLoginName == Username && ob.IsBlocked == false
                            select ob);
            if (ObjReset.Count() == 1)
            {
                var ObjUser = ObjReset.Single();
                ObjUser.Password = Password;
                DC.SubmitChanges();
                return "True";
            }
            else
            {
                return "False";
            }
        }

        public object SetFlatHolder(string StartDate, string Name, string DOB, string FlatID, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID, bool IsActive)
        {
            var DC = new DataClassesDataContext();


            tblResident ResidentObj = new tblResident();
            ResidentObj.ResidentName = Name;
            ResidentObj.DOB = Convert.ToDateTime(DOB).Date;
            ResidentObj.FlatID = FlatID;
            ResidentObj.Occupation = Occupation;
            ResidentObj.ContactNo1 = Contact1;
            ResidentObj.ContactNo2 = Contact2;
            ResidentObj.Email = Email;
            ResidentObj.Image = Image;
            ResidentObj.PositionID = PositionID;
            ResidentObj.FlatHolderID = null;
            ResidentObj.IsActive = true;
            ResidentObj.CreatedOn = DateTime.Now;

            DC.tblResidents.InsertOnSubmit(ResidentObj);

            int ResidentID = (from ob in DC.tblResidents
                              where ob.FlatID == FlatID && ob.FlatHolderID == null
                              select ob.ResidentID).Single();

            tblFlatHolder FlatHolderObj = new tblFlatHolder();
            FlatHolderObj.FlatID = FlatID;
            FlatHolderObj.ResidentID = ResidentID;
            FlatHolderObj.StartDate = Convert.ToDateTime(StartDate);
            FlatHolderObj.EndDate = null;
            FlatHolderObj.IsActive = IsActive;

            DC.tblFlatHolders.InsertOnSubmit(FlatHolderObj);
            DC.SubmitChanges();
            return true;
        }
    }
}
