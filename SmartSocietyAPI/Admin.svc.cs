﻿using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

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

        // Generate a random number between two numbers
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        // Generate a random string with a given size  
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        // Generate a random password  
        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
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

        public string CheckLogin(string Username, string Password)
        {
            var DC = new DataClassesDataContext();
            var ObjLogin = (from ob in DC.tblAdminLogins
                            where ob.AdminLoginName == Username && ob.Password == Encryptdata(Password)
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
                ObjUser.Password = Encryptdata(Password);
                DC.SubmitChanges();
                return "True";
            }
            else
            {
                return "False";
            }
        }

        public object SetSocietyInformation(string Name, string Address, string PostalCode, string LogoImage, string ContactNo, string PresidentName, string Builder, string Email, string Fax, string RegistrationNo, string CampusArea, string SocietyType, string LatLong)
        {
            var DC = new DataClassesDataContext();
            tblSociety SocietyInfoObj = new tblSociety();
            SocietyInfoObj.Name = Name;
            SocietyInfoObj.Address = Address;
            SocietyInfoObj.PostalCode = PostalCode;
            SocietyInfoObj.LogoImage = LogoImage;
            SocietyInfoObj.ContactNo = ContactNo;
            SocietyInfoObj.PresidentName = PresidentName;
            SocietyInfoObj.Builder = Builder;
            SocietyInfoObj.Email = Email;
            SocietyInfoObj.Fax = Fax;
            SocietyInfoObj.RegistrationNo = RegistrationNo;
            SocietyInfoObj.CampusArea = CampusArea;
            SocietyInfoObj.SocietyType = SocietyType;
            SocietyInfoObj.LatLong = LatLong;

            DC.tblSocieties.InsertOnSubmit(SocietyInfoObj);
            DC.SubmitChanges();

            return true;
        }

        public object EditSocietyInformation(string Name, string Address, string PostalCode, string LogoImage, string ContactNo, string PresidentName, string Builder, string Email, string Fax, string RegistrationNo, string CampusArea, string SocietyType, string LatLong)
        {
            var DC = new DataClassesDataContext();
            tblSociety SocietyInfoObj = (from ob in DC.tblSocieties
                                         select ob).Single();
            SocietyInfoObj.Name = Name;
            SocietyInfoObj.Address = Address;
            SocietyInfoObj.PostalCode = PostalCode;
            SocietyInfoObj.LogoImage = LogoImage;
            SocietyInfoObj.ContactNo = ContactNo;
            SocietyInfoObj.PresidentName = PresidentName;
            SocietyInfoObj.Builder = Builder;
            SocietyInfoObj.Email = Email;
            SocietyInfoObj.Fax = Fax;
            SocietyInfoObj.RegistrationNo = RegistrationNo;
            SocietyInfoObj.CampusArea = CampusArea;
            SocietyInfoObj.SocietyType = SocietyType;
            SocietyInfoObj.LatLong = LatLong;

            DC.tblSocieties.InsertOnSubmit(SocietyInfoObj);
            DC.SubmitChanges();

            return true;
        }

        public object GetSocietyInformation()
        {
            var DC = new DataClassesDataContext();
            var SocietyInfoObj = (from ob in DC.tblSocieties
                                  select ob).Single();
            return JsonConvert.SerializeObject(SocietyInfoObj);
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

        public object AddAssetType(string AssetTypeName)
        {
            var DC = new DataClassesDataContext();
            tblAssetType AssetTypeObj = new tblAssetType();
            AssetTypeObj.AssetTypeName = AssetTypeName;

            DC.tblAssetTypes.InsertOnSubmit(AssetTypeObj);
            DC.SubmitChanges();

            return true;
        }

        public object EditAssetType(int AssetTypeID, string AssetTypeName)
        {
            var DC = new DataClassesDataContext();
            var AssetTypeObj = (from ob in DC.tblAssetTypes
                                where ob.AssetTypeID == AssetTypeID
                                select ob).Single();
            AssetTypeObj.AssetTypeName = AssetTypeName;
            DC.SubmitChanges();

            return true;
        }

        public object AddAsset(string AssetName, int AssetTypeID, string Image, string InvoiceDoc, int AssetValue, string PurchasedOn, string Status)
        {
            var DC = new DataClassesDataContext();
            tblAsset AssetObj = new tblAsset();

            AssetObj.AssetName = AssetName;
            AssetObj.AssetTypeID = AssetTypeID;
            AssetObj.Image = Image;
            AssetObj.InvoiceDoc = InvoiceDoc;
            AssetObj.AssetValue = AssetValue;
            AssetObj.PurchasedOn = Convert.ToDateTime(PurchasedOn);
            AssetObj.Status = Status;
            AssetObj.IsActive = true;

            DC.tblAssets.InsertOnSubmit(AssetObj);
            DC.SubmitChanges();

            return true;
        }

        public object EditAsset(int AssetID, string AssetName, int AssetTypeID, string Image, string InvoiceDoc, int AssetValue, string PurchasedOn, string Status, bool IsActive)
        {
            var DC = new DataClassesDataContext();
            tblAsset AssetObj = (from ob in DC.tblAssets
                                 where ob.AssetID==AssetID
                                 select ob).Single();

            AssetObj.AssetName = AssetName;
            AssetObj.AssetTypeID = AssetTypeID;
            AssetObj.Image = Image;
            AssetObj.InvoiceDoc = InvoiceDoc;
            AssetObj.AssetValue = AssetValue;
            AssetObj.PurchasedOn = Convert.ToDateTime(PurchasedOn);
            AssetObj.Status = Status;
            AssetObj.IsActive = IsActive;
            
            DC.SubmitChanges();

            return true;
        }

        public object AddStaffMember(string Name, string MemberType, string DOB, string Contact1, string Contact2, string Image, string Address, string DOJ, string DOL, string CreatedBy)
        {
            var DC = new DataClassesDataContext();
            tblStaffMember StaffObj = new tblStaffMember();
            tblLogin returningObj = new tblLogin();
            int SGFlag = 0;

            StaffObj.MemberName = Name;
            StaffObj.MemberType = MemberType;
            StaffObj.DOB = Convert.ToDateTime(DOB);
            StaffObj.ContactNo1 = Contact1;
            StaffObj.ContactNo2 = Contact2;
            StaffObj.Image = Image;
            StaffObj.Address = Address;
            StaffObj.DOJ = Convert.ToDateTime(DOJ);
            StaffObj.DOL = Convert.ToDateTime(DOL);
            StaffObj.CreatedBy = CreatedBy;

            DC.tblStaffMembers.InsertOnSubmit(StaffObj);
            
            if(MemberType=="Security Guard")
            {
                int MemberID = (from ob in DC.tblStaffMembers
                                select ob).OrderByDescending(ob => ob.MemberID).Single().MemberID;

                tblLogin LoginObj = new tblLogin();
                string loginName = (Name.Replace(' ', Convert.ToChar("")).Length >= 4) ? Name.Replace(' ', Convert.ToChar("")).Substring(0, 4) + MemberID : Name.Replace(' ', Convert.ToChar("")) + MemberID;
                LoginObj.LoginName = loginName;
                LoginObj.PhoneNo = Contact1;
                LoginObj.Email = null;
                LoginObj.Password = RandomPassword();
                LoginObj.VerificationCode = null;
                LoginObj.FlatID = -1;
                LoginObj.MemberType = "Security Guard";
                LoginObj.IsBlocked = false;

                DC.tblLogins.InsertOnSubmit(LoginObj);

                SGFlag = 1;
                
                returningObj.PhoneNo = Contact1;
                returningObj.Password = LoginObj.Password;
            }

            DC.SubmitChanges();
            if (SGFlag == 1)
            {
                return JsonConvert.SerializeObject(returningObj);
            }
            else
            {
                return true;
            }            
        }

        public object EditStaffMember(int MemberID, string Name, string MemberType, string DOB, string Contact1, string Contact2, string Image, string Address, string DOJ, string DOL, string CreatedBy, bool IsActive)
        {
            var DC = new DataClassesDataContext();
            tblStaffMember StaffObj = (from ob in DC.tblStaffMembers
                                       where ob.MemberID==MemberID
                                       select ob).Single();

            StaffObj.MemberName = Name;
            StaffObj.MemberType = MemberType;
            StaffObj.DOB = Convert.ToDateTime(DOB);
            StaffObj.ContactNo1 = Contact1;
            StaffObj.ContactNo2 = Contact2;
            StaffObj.Image = Image;
            StaffObj.Address = Address;
            StaffObj.DOJ = Convert.ToDateTime(DOJ);
            StaffObj.DOL = Convert.ToDateTime(DOL);
            StaffObj.CreatedBy = CreatedBy;
            StaffObj.IsActive = IsActive;
            
            if (MemberType == "Security Guard")
            {
                tblLogin LoginObj = (from ob in DC.tblLogins
                                     where ob.MemberID==MemberID
                                     select ob).Single();
                LoginObj.PhoneNo = Contact1;
                LoginObj.IsBlocked = !IsActive;

                DC.tblLogins.InsertOnSubmit(LoginObj);
            }

            DC.SubmitChanges();

            return true;
        }
    }
}
