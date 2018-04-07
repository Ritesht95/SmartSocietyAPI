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

        /* Society Setup */

        // 0 for All, 1 for Current, -1 for Past Members
        public object GetAllResidentsDetails(int FlagMemType=0) 
        {
            var DC = new DataClassesDataContext();
            IQueryable<tblResident> ObjAllResidents;
            if (FlagMemType == 0)
            {
                ObjAllResidents = (from ob in DC.tblResidents
                                   select ob);
            }
            else if (FlagMemType == 1)
            {
                ObjAllResidents = (from ob in DC.tblResidents
                                   where ob.IsActive==true
                                   select ob);
            }
            else
            {
                ObjAllResidents = (from ob in DC.tblResidents
                                   where ob.IsActive == false
                                   select ob);
            }
            return JsonConvert.SerializeObject(ObjAllResidents);
        }

        // 0 for All, 1 for Owners, -1 for On Rent Flats
        public object GetAllFlatDetails(int FlagFlatType = 0) 
        {
            var DC = new DataClassesDataContext();
            IQueryable<tblFlat> ObjAllFlats;
            if (FlagFlatType == 0)
            {
                ObjAllFlats = (from ob in DC.tblFlats
                                   select ob);
            }
            else if (FlagFlatType == 1)
            {
                ObjAllFlats = (from ob in DC.tblFlats
                                   where ob.OnRent == true
                                   select ob);
            }
            else
            {
                ObjAllFlats = (from ob in DC.tblFlats
                                   where ob.OnRent == false
                                   select ob);
            }
            return JsonConvert.SerializeObject(ObjAllFlats);
        }

        /* Society Setup */

        /* Gatekeeping */

        public object ViewGateKeeping(bool CheckedInOnly = false, string FromDate = "0", string ToDate = "0", string FlatNo = "-1")
        {
            var DC = new DataClassesDataContext();
            IQueryable<tblVisitor> VisitorData;
            if (CheckedInOnly)
            {
                if (FromDate == "0" && ToDate == "0")
                {
                    VisitorData = (from ob in DC.tblVisitors
                                   where ob.OutTime == null
                                   select ob);
                }
                else
                {
                    VisitorData = (from ob in DC.tblVisitors
                                   where ob.InTime >= Convert.ToDateTime(FromDate) && ob.InTime <= Convert.ToDateTime(ToDate)
                                   && ob.OutTime == null
                                   select ob);
                }

                if (FlatNo != "-1")
                {
                    VisitorData = (from ob in VisitorData
                                   where ob.FlatNo == FlatNo
                                   select ob);
                }
            }
            else
            {
                if (FromDate == "0" && ToDate == "0")
                {
                    VisitorData = (from ob in DC.tblVisitors
                                   select ob);
                }
                else
                {
                    VisitorData = (from ob in DC.tblVisitors
                                   where ob.InTime >= Convert.ToDateTime(FromDate) && ob.InTime <= Convert.ToDateTime(ToDate)
                                   select ob);
                }

                if (FlatNo != "-1")
                {
                    VisitorData = (from ob in VisitorData
                                   where ob.FlatNo == FlatNo
                                   select ob);
                }
            }
            return JsonConvert.SerializeObject(VisitorData);
        }

        /* Gatekeeping */

        /* Notices */

        public object ViewAllNotices(string FromDate = "0", string ToDate = "0", int Priority = 0)
        {
            var DC = new DataClassesDataContext();
            IQueryable<tblNotice> NoticesData;
            if (Priority == 0)
            {
                NoticesData = (from ob in DC.tblNotices
                               where ob.IsActive == true
                               select ob);
            }
            else
            {
                NoticesData= (from ob in DC.tblNotices
                              where ob.IsActive == true && ob.Priority==Priority
                              select ob);
            }

            if(FromDate!="0" && ToDate != "0")
            {
                NoticesData = (from ob in NoticesData
                               where ob.CreatedOn >= Convert.ToDateTime(FromDate) && ob.CreatedOn <= Convert.ToDateTime(ToDate)
                               select ob);
            }

            return JsonConvert.SerializeObject(NoticesData);
        }

        /* Notices */
    }
}
