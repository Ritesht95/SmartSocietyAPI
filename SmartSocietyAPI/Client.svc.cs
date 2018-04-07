using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace SmartSocietyAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Client : IClient
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
                //smtp.Port = 587;
                smtp.Port = 25;
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

        private string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        private string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        /* Login & Registration */

        public object CheckLogin(string Username, string Password)
        {
            var DC = new DataClassesDataContext();
            var enpass = Encryptdata(Password);
            IQueryable<tblLogin> ObjLogin = (from ob in DC.tblLogins
                            where (ob.Email == Username || ob.PhoneNo==Username)
                            && ob.Password== enpass
                            && ob.IsBlocked == false
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

        public object ForgotPassword(string Username)
        {
            var DC = new DataClassesDataContext();
            var ObjForgotPass = (from ob in DC.tblLogins
                                 where (ob.PhoneNo == Username || ob.Email == Username)
                                 && ob.IsBlocked == false
                                 select ob);
            if (ObjForgotPass.Count() == 1)
            {
                var ObjData = ObjForgotPass.Single();
                Random Number = new Random();
                string Code = Number.Next(100000, 999999).ToString();
                if (Mail(ObjData.Email, "Smart Society: Reset Password", "Verification Code: " + Code + "<br>Regards,<br>Smart Society(Society Management System)"))
                {
                    ObjData.VerificationCode = Code;
                    DC.SubmitChanges();
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

        public object ResetPassword(string Username, string VerificationCode, string Password)
        {
            var DC = new DataClassesDataContext();
            var ObjReset = (from ob in DC.tblLogins
                            where (ob.PhoneNo == Username || ob.Email == Username)
                            && ob.VerificationCode == VerificationCode
                            && ob.IsBlocked == false
                            select ob);
            if (ObjReset.Count() == 1)
            {
                var ObjUser = ObjReset.Single();
                ObjUser.Password = Encryptdata(Password);
                DC.SubmitChanges();
                return "True";
            }
            else
            {
                return "False";
            }
        }

        /* Login & Registration */

        /* Society Setup */

        public object SetResident(string Name, string DOB, string FlatNo, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID)
        {
            var DC = new DataClassesDataContext();
            tblResident ResidentObj = new tblResident();
            ResidentObj.ResidentName = Name;
            ResidentObj.DOB = Convert.ToDateTime(DOB).Date;
            ResidentObj.FlatNo = FlatNo;
            ResidentObj.Occupation = Occupation;
            ResidentObj.ContactNo1 = Contact1;
            ResidentObj.ContactNo2 = Contact2;
            ResidentObj.Email = Email;
            ResidentObj.Image = Image;
            ResidentObj.PositionID = PositionID;
            ResidentObj.FlatHolderID = FlatHolderID;
            ResidentObj.IsActive = true;
            ResidentObj.CreatedOn = DateTime.Now;

            DC.tblResidents.InsertOnSubmit(ResidentObj);
            DC.SubmitChanges();
            return "True";
        }

        public object EditResident(int ResidentID, string Name, string DOB, string FlatNo, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID, bool IsActive)
        {
            var DC = new DataClassesDataContext();
            var ResidentObj = (from ob in DC.tblResidents
                               where ob.ResidentID == ResidentID
                               select ob).Single();
            ResidentObj.ResidentName = Name;
            ResidentObj.DOB = Convert.ToDateTime(DOB).Date;
            ResidentObj.FlatNo = FlatNo;
            ResidentObj.Occupation = Occupation;
            ResidentObj.ContactNo1 = Contact1;
            ResidentObj.ContactNo2 = Contact2;
            ResidentObj.Email = Email;
            ResidentObj.Image = Image;
            ResidentObj.PositionID = PositionID;
            ResidentObj.FlatHolderID = FlatHolderID;
            ResidentObj.IsActive = IsActive;

            DC.SubmitChanges();
            return "True";
        }

        /* Society Setup */

        /* Gatekeeping */

        public object GateCheckIn(string VisitorName, string FlatNo, string Purpose, string VehicleNo, string MobileNo)
        {
            var DC = new DataClassesDataContext();
            tblVisitor VisitorObj = new tblVisitor();
            VisitorObj.VisitorName = VisitorName;
            VisitorObj.FlatNo = FlatNo;
            VisitorObj.InTime = DateTime.Now;
            VisitorObj.OutTime = null;
            VisitorObj.Purpose = Purpose;
            VisitorObj.VehicleNumber = (VehicleNo!="0")?VehicleNo:null;
            VisitorObj.MobileNo = MobileNo;

            DC.tblVisitors.InsertOnSubmit(VisitorObj);
            DC.SubmitChanges();

            return "True";
        }

        public object GateCheckOut(int VisitorID)
        {
            var DC = new DataClassesDataContext();
            var VisitorObj = (from ob in DC.tblVisitors
                              where ob.VisitorID == VisitorID
                              select ob).Single();
            VisitorObj.OutTime = DateTime.Now;
            DC.SubmitChanges();

            return "True";
        }

        /* Gatekeeping */

        /* Vendors */

        public object AddVendorRating(int VendorID, double Rating)
        {
            var DC = new DataClassesDataContext();
            var VendorObj = (from ob in DC.tblVendors
                             where ob.VendorID == VendorID
                             select ob).Single();
            double ratings = (double)(VendorObj.RatingsNum * VendorObj.Ratings);
            VendorObj.Ratings = (decimal)((ratings + Rating)/(++VendorObj.RatingsNum));

            DC.SubmitChanges();

            return "True";
        }

        /* Vendors */
    }
}
