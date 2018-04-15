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
                                             where (ob.Email == Username || ob.PhoneNo == Username)
                                             && ob.Password == enpass
                                             && ob.IsBlocked == false
                                             select ob);
            if (ObjLogin.Count() == 1)
            {
                var TempObj = ObjLogin.Single();
                if (TempObj.MemberType == "Security Guard")
                {
                    return JsonConvert.SerializeObject(TempObj);
                }
                else
                {
                    var TempObj1 = (from ob in DC.tblLogins
                                    join obR in DC.tblResidents on ob.MemberID equals obR.ResidentID
                                    join obFH in DC.tblFlatHolders on obR.ResidentID equals obFH.ResidentID
                                    where ob.LoginID == TempObj.LoginID && obFH.IsActive == true
                                    select new
                                    {
                                        ob.LoginID,
                                        ob.LoginName,
                                        ob.MemberID,
                                        obR.ResidentName,
                                        obFH.FlatHolderID,
                                        ob.PhoneNo,
                                        ob.Email,
                                        ob.FlatNo,
                                        ob.MemberType,
                                        ob.VerificationCode,
                                        obR.Gender
                                    }).Single();
                    return JsonConvert.SerializeObject(TempObj1);
                }

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

        public object GetPositionData()
        {
            var DC = new DataClassesDataContext();
            var PositionsData = (from ob in DC.tblPositions
                                 select ob);
            return JsonConvert.SerializeObject(PositionsData);
        }

        public object SetResident(string Name, string DOB, string FlatNo, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID, string Gender)
        {
            var DC = new DataClassesDataContext();
            tblResident ResidentObj = new tblResident();
            ResidentObj.ResidentName = Name;
            ResidentObj.Gender = Gender;
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

        public object EditResident(int ResidentID, string Name, string DOB, string FlatNo, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID, bool IsActive, string Gender)
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
            ResidentObj.Gender = Gender;

            DC.SubmitChanges();
            return "True";
        }

        /* Society Setup */

        /* Notices */

        public object GetNotices(string FlatNo)
        {
            var DC = new DataClassesDataContext();
            bool FlatType = (from ob in DC.tblFlats
                             where ob.FlatNo == FlatNo
                             select ob.OnRent).Single();
            if (FlatType)
            {
                var NoticeData = (from ob in DC.tblNotices
                                  where ob.Recipient == 0
                                  || ob.Recipient == -2
                                  || ob.Recipient == Convert.ToInt32(FlatNo)
                                  orderby ob.CreatedOn descending
                                  orderby ob.Priority ascending
                                  select ob);
                return JsonConvert.SerializeObject(NoticeData);
            }
            else
            {
                var NoticeData = (from ob in DC.tblNotices
                                  where ob.Recipient == 0
                                  || ob.Recipient == -1
                                  || ob.Recipient == Convert.ToInt32(FlatNo)
                                  orderby ob.CreatedOn descending
                                  orderby ob.Priority ascending
                                  select ob);
                return JsonConvert.SerializeObject(NoticeData);
            }
        }

        /* Notices */

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
            VisitorObj.VehicleNumber = (VehicleNo != "0") ? VehicleNo : null;
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
            VendorObj.Ratings = (decimal)((ratings + Rating) / (++VendorObj.RatingsNum));

            DC.SubmitChanges();

            return "True";
        }

        /* Vendors */

        /* Events */

        /* Events */

        /* FacilityBookings */

        public object ProposeFacilityBooking(int FacilityID, string FlatNo, string StartTime, string EndTime, string Purpose, string Description)
        {
            var DC = new DataClassesDataContext();
            tblBooking BookingObj = new tblBooking();
            BookingObj.FacilityID = FacilityID;
            BookingObj.FlatNo = FlatNo;
            BookingObj.StartTime = Convert.ToDateTime(StartTime);
            BookingObj.EndTime = Convert.ToDateTime(EndTime);
            BookingObj.Status = "Panel Review Pending";
            BookingObj.Purpose = Purpose;
            BookingObj.Description = Description;
            BookingObj.IsApproved = false;
            BookingObj.IsActive = true;

            DC.tblBookings.InsertOnSubmit(BookingObj);
            DC.SubmitChanges();

            return "True";
        }

        /* FacilityBookings */

        /* Polls */

        public object SetPollVote(string FlatNo, int PollOptionID, int PollID)
        {
            var DC = new DataClassesDataContext();
            tblPollVoting PollVotingObj = new tblPollVoting();
            PollVotingObj.PollID = PollID;
            PollVotingObj.PollOptionID = PollOptionID;
            PollVotingObj.FlatNo = FlatNo;

            DC.tblPollVotings.InsertOnSubmit(PollVotingObj);
            DC.SubmitChanges();

            return "True";
        }

        /* Polls */

        /* Complaints */

        public object AddComplaint(string ComplaintType, string Subject, string Description, int Priority)
        {
            var DC = new DataClassesDataContext();
            tblComplaint ComplaintObj = new tblComplaint();
            ComplaintObj.ComplaintType = ComplaintType;
            ComplaintObj.Subject = Subject;
            ComplaintObj.Description = Description;
            ComplaintObj.Priority = Priority;
            ComplaintObj.CreatedOn = DateTime.Now;
            ComplaintObj.IsActive = true;

            DC.tblComplaints.InsertOnSubmit(ComplaintObj);
            DC.SubmitChanges();

            return "True";
        }

        /* Complaints */

        /* Payments & Transactions */

        public object GetAllPaymentDues(string FlatNo)
        {
            var DC = new DataClassesDataContext();
            var FlatData = (from ob in DC.tblFlats
                            where ob.FlatNo == FlatNo
                            select ob).Single();
            if (FlatData.OnRent == true)
            {
                var PaymentDuesData = (from ob in DC.tblPayments
                                       from obT in DC.tblTransactions
                                       where ob.PaymentID != obT.PaymentID && obT.FlatNo != FlatNo &&
                                       (ob.PaymentFor == 0 || ob.PaymentFor == -2 || ob.PaymentFor == Convert.ToInt32(FlatNo))
                                       select ob);
                return JsonConvert.SerializeObject(PaymentDuesData);
            }
            else
            {
                var PaymentDuesData = (from ob in DC.tblPayments
                                       from obT in DC.tblTransactions
                                       where ob.PaymentID != obT.PaymentID && obT.FlatNo != FlatNo &&
                                       (ob.PaymentFor == 0 || ob.PaymentFor == -1 || ob.PaymentFor == Convert.ToInt32(FlatNo))
                                       select ob);
                return JsonConvert.SerializeObject(PaymentDuesData);
            }
        }

        public object SetPayment(string FlatNo, int PaymentID, string PaymentMode, string ChequeNo, decimal Amount, decimal Penalty)
        {
            var DC = new DataClassesDataContext();
            tblTransaction TransactionObj = new tblTransaction();
            TransactionObj.PaymentID = PaymentID;
            TransactionObj.PaymentMode = PaymentMode;
            TransactionObj.Penalty = Convert.ToDecimal(Penalty);
            TransactionObj.TransactionType = "Income";
            TransactionObj.TransactionOn = DateTime.Now;
            TransactionObj.FlatNo = FlatNo;
            TransactionObj.Amount = Amount;
            if (ChequeNo != "")
            {
                TransactionObj.ChequeNo = ChequeNo;
            }

            DC.tblTransactions.InsertOnSubmit(TransactionObj);

            tblIncome IncomeObj = new tblIncome();
            IncomeObj.Amount = Amount;
            IncomeObj.Description = "Maintenance from " + FlatNo + " with " + Penalty + " Penalty";
            IncomeObj.IncomeName = "Maintenance from " + FlatNo;
            IncomeObj.PaymentMode = PaymentMode;
            IncomeObj.IncomeType = "Maintenance";
            IncomeObj.IsDeleted = false;

            DC.tblIncomes.InsertOnSubmit(IncomeObj);

            DC.SubmitChanges();

            return "True";
        }
        
        /* Payments & Transactions */
    }
}
