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
        public object GetAllResidentsDetails(int FlagMemType = 0, int ResidentID = 0)
        {
            var DC = new DataClassesDataContext();
            IQueryable<tblResident> ObjAllResidents;
            tblResident ResidentObj;
            if (ResidentID == 0)
            {
                if (FlagMemType == 0)
                {
                    ObjAllResidents = (from ob in DC.tblResidents
                                       select ob);
                }
                else if (FlagMemType == 1)
                {
                    ObjAllResidents = (from ob in DC.tblResidents
                                       where ob.IsActive == true
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
            else
            {
                ResidentObj = (from ob in DC.tblResidents
                               where ob.ResidentID == ResidentID
                               select ob).Single();
                return JsonConvert.SerializeObject(ResidentObj);
            }

        }

        // 0 for All, 1 for Owners, -1 for On Rent Flats
        public object GetAllFlatDetails(int FlagFlatType = 0, string FlatNo = "0")
        {
            var DC = new DataClassesDataContext();
            IQueryable<object> ObjAllFlats;
            object FlatObj;
            if (FlatNo == "0")
            {
                if (FlagFlatType == 0)
                {
                    ObjAllFlats = (from ob in DC.tblFlats
                                   join obfh in DC.tblFlatHolders on ob.FlatNo equals obfh.FlatNo
                                   join obr in DC.tblResidents on obfh.ResidentID equals obr.ResidentID
                                   select new
                                   {
                                       ob.FlatNo,
                                       ob.OnRent,
                                       ob.TenamentNo,
                                       obr.ResidentName,
                                       RenteeName = (from obre in DC.tblRents
                                                     join obr1 in DC.tblResidents on obre.ResidentID equals obr1.ResidentID
                                                     where obfh.IsActive == true
                                                     select obr1.ResidentName).Single().ToString()
                                   });
                }
                else if (FlagFlatType == 1)
                {
                    ObjAllFlats = (from ob in DC.tblFlats
                                   join obfh in DC.tblFlatHolders on ob.FlatNo equals obfh.FlatNo
                                   join obr in DC.tblResidents on obfh.ResidentID equals obr.ResidentID
                                   where obfh.IsActive == true && ob.OnRent == false
                                   select new
                                   {
                                       ob.FlatNo,
                                       ob.OnRent,
                                       ob.TenamentNo,
                                       obr.ResidentName
                                   });
                }
                else
                {
                    ObjAllFlats = (from ob in DC.tblFlats
                                   join obfh in DC.tblFlatHolders on ob.FlatNo equals obfh.FlatNo
                                   join obr in DC.tblResidents on obfh.ResidentID equals obr.ResidentID
                                   where obfh.IsActive == true && ob.OnRent == true
                                   select new
                                   {
                                       ob.FlatNo,
                                       ob.OnRent,
                                       ob.TenamentNo,
                                       obr.ResidentName,
                                       RenteeName = (from obre in DC.tblRents
                                                     join obr1 in DC.tblResidents on obre.ResidentID equals obr1.ResidentID
                                                     where obfh.IsActive == true
                                                     select obr1.ResidentName).Single().ToString()
                                   });
                }
                return JsonConvert.SerializeObject(ObjAllFlats);
            }
            else
            {
                FlatObj = (from ob in DC.tblFlats
                           join obfh in DC.tblFlatHolders on ob.FlatNo equals obfh.FlatNo
                           join obr in DC.tblResidents on obfh.ResidentID equals obr.ResidentID
                           select new
                           {
                               ob.FlatNo,
                               ob.OnRent,
                               ob.TenamentNo,
                               obr.ResidentName,
                               RenteeName = (from obre in DC.tblRents
                                             join obr1 in DC.tblResidents on obre.ResidentID equals obr1.ResidentID
                                             where obfh.IsActive == true
                                             select obr1.ResidentName).Single().ToString()
                           });

                return JsonConvert.SerializeObject(FlatObj);
            }
        }

        public object ViewAllAssets(int AssetID = 0)
        {
            var DC = new DataClassesDataContext();
            if (AssetID == 0)
            {
                var AssetsData = (from ob in DC.tblAssets
                                  join obAT in DC.tblAssetTypes on ob.AssetTypeID equals obAT.AssetTypeID
                                  select new
                                  {
                                      ob.AssetID,
                                      ob.AssetName,
                                      ob.AssetTypeID,
                                      ob.AssetValue,
                                      ob.Image,
                                      ob.InvoiceDoc,
                                      ob.IsActive,
                                      ob.PurchasedOn,
                                      ob.Status,
                                      obAT.AssetTypeName
                                  });
                return JsonConvert.SerializeObject(AssetsData);
            }
            else
            {
                var AssetObj = (from ob in DC.tblAssets
                                  join obAT in DC.tblAssetTypes on ob.AssetTypeID equals obAT.AssetTypeID
                                  where ob.AssetID==AssetID
                                  select new
                                  {
                                      ob.AssetID,
                                      ob.AssetName,
                                      ob.AssetTypeID,
                                      ob.AssetValue,
                                      ob.Image,
                                      ob.InvoiceDoc,
                                      ob.IsActive,
                                      ob.PurchasedOn,
                                      ob.Status,
                                      obAT.AssetTypeName
                                  });
                return JsonConvert.SerializeObject(AssetObj);
            }
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

        public object ViewAllNotices(string FromDate = "0", string ToDate = "0", int Priority = 0, int NoticeID = 0)
        {
            var DC = new DataClassesDataContext();
            IQueryable<tblNotice> NoticesData;
            tblNotice NoticeObj;
            if (NoticeID == 0)
            {
                if (Priority == 0)
                {
                    NoticesData = (from ob in DC.tblNotices
                                   where ob.IsActive == true
                                   select ob);
                }
                else
                {
                    NoticesData = (from ob in DC.tblNotices
                                   where ob.IsActive == true && ob.Priority == Priority
                                   select ob);
                }

                if (FromDate != "0" && ToDate != "0")
                {
                    NoticesData = (from ob in NoticesData
                                   where ob.CreatedOn >= Convert.ToDateTime(FromDate) && ob.CreatedOn <= Convert.ToDateTime(ToDate)
                                   select ob);
                }

                return JsonConvert.SerializeObject(NoticesData);
            }
            else
            {
                NoticeObj = (from ob in DC.tblNotices
                             where ob.NoticeID == NoticeID
                             select ob).Single();

                return JsonConvert.SerializeObject(NoticeObj);
            }
        }

        /* Notices */

        /* Vendors */

        public object ViewAllVendors(int VendorID = 0)
        {
            var DC = new DataClassesDataContext();
            if (VendorID == 0)
            {
                var VendorsData = (from ob in DC.tblVendors
                                   where ob.IsActive == true
                                   select ob);
                return JsonConvert.SerializeObject(VendorsData);
            }
            else
            {
                var VendorObj = (from ob in DC.tblVendors
                                 where ob.VendorID == VendorID
                                 select ob);
                return JsonConvert.SerializeObject(VendorObj);
            }

        }

        /* Vendors */

        /* Events */

        public object ViewAllEvents(string FromDate = "0", string ToDate = "0", int Priority = 0, int EventID = 0)
        {
            var DC = new DataClassesDataContext();
            IQueryable<tblEvent> EventsData;
            tblEvent EventObj;
            if (EventID == 0)
            {
                if (Priority == 0)
                {
                    EventsData = (from ob in DC.tblEvents
                                  join obET in DC.tblEventTypes on ob.EventTypeID equals obET.EventTypeID
                                  join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                  select ob);
                }
                else
                {
                    EventsData = (from ob in DC.tblEvents
                                  join obET in DC.tblEventTypes on ob.EventTypeID equals obET.EventTypeID
                                  join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                  where ob.Priority == Priority
                                  select ob);
                }

                if (FromDate != "0" && ToDate != "0")
                {
                    EventsData = (from ob in EventsData
                                  where ob.StartTime >= Convert.ToDateTime(FromDate) && ob.StartTime <= Convert.ToDateTime(ToDate)
                                  select ob);
                }

                return JsonConvert.SerializeObject(EventsData);
            }
            else
            {
                EventObj= (from ob in DC.tblEvents
                           join obET in DC.tblEventTypes on ob.EventTypeID equals obET.EventTypeID
                           join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                           where ob.EventID==EventID
                           select ob).Single();
                return JsonConvert.SerializeObject(EventObj);
            }
        }

        /* Events */


        /* FacilityBookings */

        public object GetAllFacilities(int FacilityID=0)
        {
            var DC = new DataClassesDataContext();
            if (FacilityID == 0)
            {
                IQueryable<tblFacility> FacilitiesData = (from ob in DC.tblFacilities
                                                          select ob);
                return JsonConvert.SerializeObject(FacilitiesData);
            }
            else
            {
                tblFacility FacilityObj= (from ob in DC.tblFacilities
                              where ob.FacilityID==FacilityID
                              select ob).Single();
                return JsonConvert.SerializeObject(FacilityObj);
            }
            
        }

        /* FacilityBookings */

        /* Staff Members */

        public object GetAllStaffMembers(int MemberID = 0)
        {
            var DC = new DataClassesDataContext();
            if (MemberID == 0)
            {
                IQueryable<object> StaffMembersData = (from ob in DC.tblStaffMembers
                                                       join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                                       select new
                                                       {
                                                           ob.MemberName,
                                                           ob.MemberType,
                                                           ob.DOB,
                                                           ob.ContactNo1,
                                                           ob.ContactNo2,
                                                           ob.Image,
                                                           ob.IDProofDoc,
                                                           ob.Address,
                                                           ob.DOJ,
                                                           ob.DOL,
                                                           CreatedBy = obR.ResidentName,
                                                           ob.IsActive
                                                       });
                return JsonConvert.SerializeObject(StaffMembersData);
            }
            else
            {
                object MemberObj= (from ob in DC.tblStaffMembers
                                   join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                   where ob.MemberID==MemberID
                                   select new
                                   {
                                       ob.MemberName,
                                       ob.MemberType,
                                       ob.DOB,
                                       ob.ContactNo1,
                                       ob.ContactNo2,
                                       ob.Image,
                                       ob.IDProofDoc,
                                       ob.Address,
                                       ob.DOJ,
                                       ob.DOL,
                                       CreatedBy = obR.ResidentName,
                                       ob.IsActive
                                   });
                return JsonConvert.SerializeObject(MemberObj);
            }
        }

        /* Staff Members */
    }
}
