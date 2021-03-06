﻿using Newtonsoft.Json;
using System;
using System.Data.Linq.SqlClient;
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

        public object GetSocietyInformation()
        {
            var DC = new DataClassesDataContext();
            var SocietyInfoObj = (from ob in DC.tblSocieties
                                  select ob).Single();
            return JsonConvert.SerializeObject(SocietyInfoObj);
        }

        // 0 for All, 1 for Current, -1 for Past Members
        public object GetAllResidentsDetails(int FlagMemType = 0, int ResidentID = 0)
        {
            var DC = new DataClassesDataContext();
            IQueryable<object> ObjAllResidents;
            object ResidentObj;
            if (ResidentID == 0)
            {
                if (FlagMemType == 0)
                {
                    ObjAllResidents = (from ob in DC.tblResidents
                                       join obP in DC.tblPositions on ob.PositionID equals obP.PositionID
                                       join obFH in DC.tblFlatHolders on ob.FlatHolderID equals obFH.FlatHolderID
                                       join obR in DC.tblResidents on obFH.ResidentID equals obR.ResidentID
                                       where ob.IsDeleted == false
                                       select new
                                       {
                                           ob.ResidentID,
                                           ob.ResidentName,
                                           ob.PositionID,
                                           obP.PositionName,
                                           ob.Occupation,
                                           ob.IsActive,
                                           ob.Image,
                                           ob.FlatNo,
                                           ob.ContactNo1,
                                           ob.ContactNo2,
                                           ob.CreatedOn,
                                           ob.DOB,
                                           ob.Email,
                                           ob.FlatHolderID,
                                           FlatHolderName = obR.ResidentName,
                                           ob.Gender
                                       });
                }
                else if (FlagMemType == 1)
                {
                    ObjAllResidents = (from ob in DC.tblResidents
                                       join obP in DC.tblPositions on ob.PositionID equals obP.PositionID
                                       join obFH in DC.tblFlatHolders on ob.FlatHolderID equals obFH.FlatHolderID
                                       join obR in DC.tblResidents on obFH.ResidentID equals obR.ResidentID
                                       where ob.IsActive == true && ob.IsDeleted == false
                                       select new
                                       {
                                           ob.ResidentID,
                                           ob.ResidentName,
                                           ob.PositionID,
                                           obP.PositionName,
                                           ob.Occupation,
                                           ob.IsActive,
                                           ob.Image,
                                           ob.FlatNo,
                                           ob.ContactNo1,
                                           ob.ContactNo2,
                                           ob.CreatedOn,
                                           ob.DOB,
                                           ob.Email,
                                           ob.FlatHolderID,
                                           FlatHolderName = obR.ResidentName,
                                           ob.Gender
                                       });
                }
                else
                {
                    ObjAllResidents = (from ob in DC.tblResidents
                                       join obP in DC.tblPositions on ob.PositionID equals obP.PositionID
                                       join obFH in DC.tblFlatHolders on ob.FlatHolderID equals obFH.FlatHolderID
                                       join obR in DC.tblResidents on obFH.ResidentID equals obR.ResidentID
                                       where ob.IsActive == false && ob.IsDeleted == false
                                       select new
                                       {
                                           ob.ResidentID,
                                           ob.ResidentName,
                                           ob.PositionID,
                                           obP.PositionName,
                                           ob.Occupation,
                                           ob.IsActive,
                                           ob.Image,
                                           ob.FlatNo,
                                           ob.ContactNo1,
                                           ob.ContactNo2,
                                           ob.CreatedOn,
                                           ob.DOB,
                                           ob.Email,
                                           ob.FlatHolderID,
                                           FlatHolderName = obR.ResidentName,
                                           ob.Gender
                                       });
                }
                return JsonConvert.SerializeObject(ObjAllResidents);
            }
            else
            {
                ResidentObj = (from ob in DC.tblResidents
                               join obP in DC.tblPositions on ob.PositionID equals obP.PositionID
                               join obFH in DC.tblFlatHolders on ob.FlatHolderID equals obFH.FlatHolderID
                               join obR in DC.tblResidents on obFH.ResidentID equals obR.ResidentID
                               where ob.ResidentID == ResidentID
                               select new
                               {
                                   ob.ResidentID,
                                   ob.ResidentName,
                                   ob.PositionID,
                                   obP.PositionName,
                                   ob.Occupation,
                                   ob.IsActive,
                                   ob.Image,
                                   ob.FlatNo,
                                   ob.ContactNo1,
                                   ob.ContactNo2,
                                   ob.CreatedOn,
                                   ob.DOB,
                                   ob.Email,
                                   ob.FlatHolderID,
                                   FlatHolderName = obR.ResidentName,
                                   ob.Gender
                               }).Single();

                return JsonConvert.SerializeObject(ResidentObj);
            }

        }

        public object ResidentSearch(string Name = "", string FlatNo = "0")
        {
            var DC = new DataClassesDataContext();
            if (FlatNo != "0" && Name != "")
            {
                var ObjAllResidents = (from ob in DC.tblResidents
                                       join obP in DC.tblPositions on ob.PositionID equals obP.PositionID
                                       join obFH in DC.tblFlatHolders on ob.FlatHolderID equals obFH.FlatHolderID
                                       join obR in DC.tblResidents on obFH.ResidentID equals obR.ResidentID
                                       where ob.IsDeleted == false && SqlMethods.Like(ob.ResidentName.ToLower(), "%" + Name.ToLower() + "%")
                                       || ob.FlatNo == FlatNo
                                       select new
                                       {
                                           ob.ResidentID,
                                           ob.ResidentName,
                                           ob.PositionID,
                                           obP.PositionName,
                                           ob.Occupation,
                                           ob.IsActive,
                                           ob.Image,
                                           ob.FlatNo,
                                           ob.ContactNo1,
                                           ob.ContactNo2,
                                           ob.CreatedOn,
                                           ob.DOB,
                                           ob.Email,
                                           ob.FlatHolderID,
                                           FlatHolderName = obR.ResidentName
                                       });
                return JsonConvert.SerializeObject(ObjAllResidents);
            }
            else if (FlatNo == "0")
            {
                var ObjAllResidents = (from ob in DC.tblResidents
                                       join obP in DC.tblPositions on ob.PositionID equals obP.PositionID
                                       join obFH in DC.tblFlatHolders on ob.FlatHolderID equals obFH.FlatHolderID
                                       join obR in DC.tblResidents on obFH.ResidentID equals obR.ResidentID
                                       where ob.IsDeleted == false && SqlMethods.Like(ob.ResidentName.ToLower(), "%" + Name.ToLower() + "%")
                                       select new
                                       {
                                           ob.ResidentID,
                                           ob.ResidentName,
                                           ob.PositionID,
                                           obP.PositionName,
                                           ob.Occupation,
                                           ob.IsActive,
                                           ob.Image,
                                           ob.FlatNo,
                                           ob.ContactNo1,
                                           ob.ContactNo2,
                                           ob.CreatedOn,
                                           ob.DOB,
                                           ob.Email,
                                           ob.FlatHolderID,
                                           FlatHolderName = obR.ResidentName
                                       });
                return JsonConvert.SerializeObject(ObjAllResidents);
            }
            else
            {
                var ObjAllResidents = (from ob in DC.tblResidents
                                       join obP in DC.tblPositions on ob.PositionID equals obP.PositionID
                                       join obFH in DC.tblFlatHolders on ob.FlatHolderID equals obFH.FlatHolderID
                                       join obR in DC.tblResidents on obFH.ResidentID equals obR.ResidentID
                                       where ob.IsDeleted == false && ob.FlatNo == FlatNo
                                       select new
                                       {
                                           ob.ResidentID,
                                           ob.ResidentName,
                                           ob.PositionID,
                                           obP.PositionName,
                                           ob.Occupation,
                                           ob.IsActive,
                                           ob.Image,
                                           ob.FlatNo,
                                           ob.ContactNo1,
                                           ob.ContactNo2,
                                           ob.CreatedOn,
                                           ob.DOB,
                                           ob.Email,
                                           ob.FlatHolderID,
                                           FlatHolderName = obR.ResidentName
                                       });
                return JsonConvert.SerializeObject(ObjAllResidents);
            }
        }

        public object ResidentDelete(int ResidentID)
        {
            var DC = new DataClassesDataContext();
            var ResidentObj = (from ob in DC.tblResidents
                               where ob.ResidentID == ResidentID
                               select ob).Single();
            ResidentObj.IsDeleted = true;
            DC.SubmitChanges();
            return "True";
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
                                  where ob.IsActive == true
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
                                      obAT.AssetTypeName,
                                      ob.Description
                                  });
                return JsonConvert.SerializeObject(AssetsData);
            }
            else
            {
                var AssetObj = (from ob in DC.tblAssets
                                join obAT in DC.tblAssetTypes on ob.AssetTypeID equals obAT.AssetTypeID
                                where ob.AssetID == AssetID
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
                                    obAT.AssetTypeName,
                                    ob.Description
                                }).Single();
                return JsonConvert.SerializeObject(AssetObj);
            }
        }

        public object AssetSearch(string Name = "", string Type = "")
        {
            var DC = new DataClassesDataContext();
            if (Name != "" && Type != "")
            {
                var AssetsData = (from ob in DC.tblAssets
                                  join obAT in DC.tblAssetTypes on ob.AssetTypeID equals obAT.AssetTypeID
                                  where ob.IsActive == true && SqlMethods.Like(ob.AssetName.ToLower(), "%" + Name.ToLower() + "%")
                                  || obAT.AssetTypeName.ToLower() == Type.ToLower()
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
            else if (Name == "")
            {
                var AssetsData = (from ob in DC.tblAssets
                                  join obAT in DC.tblAssetTypes on ob.AssetTypeID equals obAT.AssetTypeID
                                  where ob.IsActive == true && obAT.AssetTypeName.ToLower() == Type.ToLower()
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
                var AssetsData = (from ob in DC.tblAssets
                                  join obAT in DC.tblAssetTypes on ob.AssetTypeID equals obAT.AssetTypeID
                                  where ob.IsActive == true && SqlMethods.Like(ob.AssetName, "%" + Name + "%")
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
        }

        public object AssetDelete(int AssetID)
        {
            var DC = new DataClassesDataContext();
            var AssetObj = (from ob in DC.tblAssets
                            where ob.AssetID == AssetID
                            select ob).Single();
            AssetObj.IsActive = false;
            DC.SubmitChanges();
            return "True";
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
            IQueryable<object> NoticesData;
            object NoticeObj;
            if (NoticeID == 0)
            {
                if (Priority == 0)
                {
                    NoticesData = (from ob in DC.tblNotices
                                   join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                   where ob.IsActive == true
                                   orderby ob.CreatedOn descending
                                   orderby ob.Priority ascending
                                   select new
                                   {
                                       ob.NoticeID,
                                       ob.Title,
                                       ob.Description,
                                       ob.Priority,
                                       ob.CreatedOn,
                                       CreatedByName = obR.ResidentName,
                                       ob.IsActive
                                   });
                }
                else
                {
                    NoticesData = (from ob in DC.tblNotices
                                   join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                   where ob.IsActive == true && ob.Priority == Priority
                                   orderby ob.CreatedOn descending
                                   orderby ob.Priority ascending
                                   select new
                                   {
                                       ob.NoticeID,
                                       ob.Title,
                                       ob.Description,
                                       ob.Priority,
                                       ob.CreatedOn,
                                       CreatedByName = obR.ResidentName,
                                       ob.IsActive
                                   });
                }

                if (FromDate != "0" && ToDate != "0")
                {
                    NoticesData = (from ob in DC.tblNotices
                                   join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                   where ob.CreatedOn >= Convert.ToDateTime(FromDate) && ob.CreatedOn <= Convert.ToDateTime(ToDate)
                                   && ob.IsActive == true
                                   orderby ob.CreatedOn descending
                                   orderby ob.Priority ascending
                                   select new
                                   {
                                       ob.NoticeID,
                                       ob.Title,
                                       ob.Description,
                                       ob.Priority,
                                       ob.CreatedOn,
                                       CreatedByName = obR.ResidentName,
                                       ob.IsActive
                                   });
                }

                return JsonConvert.SerializeObject(NoticesData);
            }
            else
            {
                NoticeObj = (from ob in DC.tblNotices
                             join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                             where ob.NoticeID == NoticeID && ob.IsActive == true
                             orderby ob.CreatedOn descending
                             orderby ob.Priority ascending
                             select new
                             {
                                 ob.NoticeID,
                                 ob.Title,
                                 ob.Description,
                                 ob.Priority,
                                 ob.CreatedOn,
                                 CreatedByName = obR.ResidentName,
                                 ob.IsActive
                             }).Single();

                return JsonConvert.SerializeObject(NoticeObj);
            }
        }

        public object NoticesSearch(string SearchTerm)
        {
            var DC = new DataClassesDataContext();
            var NoticesData = (from ob in DC.tblNotices
                               join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                               where ob.IsActive == true && SqlMethods.Like(ob.Title, "%" + SearchTerm + "%")
                               orderby ob.Priority ascending
                               select new
                               {
                                   ob.NoticeID,
                                   ob.Title,
                                   ob.Description,
                                   ob.Priority,
                                   ob.CreatedOn,
                                   CreatedByName = obR.ResidentName,
                                   ob.IsActive
                               });
            return JsonConvert.SerializeObject(NoticesData);
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

        public object VendorSearch(string Name = "", string Type = "")
        {
            var DC = new DataClassesDataContext();
            if (Name != "" && Type != "")
            {
                var VendorsData = (from ob in DC.tblVendors
                                   where ob.IsActive == true
                                   && SqlMethods.Like(ob.VendorName.ToLower(), "%" + Name.ToLower() + "%")
                                   || ob.VendorType.ToLower() == Type.ToLower()
                                   select ob);
                return JsonConvert.SerializeObject(VendorsData);
            }
            else if (Name == "")
            {
                var VendorsData = (from ob in DC.tblVendors
                                   where ob.IsActive == true
                                   && ob.VendorType.ToLower() == Type.ToLower()
                                   select ob);
                return JsonConvert.SerializeObject(VendorsData);
            }
            else
            {
                var VendorsData = (from ob in DC.tblVendors
                                   where ob.IsActive == true
                                   && SqlMethods.Like(ob.VendorName.ToLower(), "%" + Name.ToLower() + "%")
                                   select ob);
                return JsonConvert.SerializeObject(VendorsData);
            }
        }

        public object VendorDelete(int VendorID)
        {
            var DC = new DataClassesDataContext();
            var VendorObj = (from ob in DC.tblVendors
                             where ob.VendorID == VendorID
                             select ob).Single();
            VendorObj.IsActive = false;
            DC.SubmitChanges();
            return "True";
        }

        /* Vendors */

        /* Events */

        public object ViewAllEvents(string FromDate = "0", string ToDate = "0", int Priority = 0, int EventID = 0)
        {
            var DC = new DataClassesDataContext();
            IQueryable<object> EventsData;
            object EventObj;
            if (EventID == 0)
            {
                if (Priority == 0)
                {
                    EventsData = (from ob in DC.tblEvents
                                  join obET in DC.tblEventTypes on ob.EventTypeID equals obET.EventTypeID
                                  join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                  where ob.IsActive == true
                                  select new
                                  {
                                      ob.EventID,
                                      ob.EventName,
                                      ob.EventTypeID,
                                      ob.CreatedBy,
                                      ob.Description,
                                      ob.EndTime,
                                      ob.IsActive,
                                      ob.Priority,
                                      ob.StartTime,
                                      ob.Subject,
                                      ob.Venue,
                                      obET.EventTypeName,
                                      CreatedByName = obR.ResidentName
                                  });
                }
                else
                {
                    EventsData = (from ob in DC.tblEvents
                                  join obET in DC.tblEventTypes on ob.EventTypeID equals obET.EventTypeID
                                  join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                  where ob.Priority == Priority && ob.IsActive == true
                                  select new
                                  {
                                      ob.EventID,
                                      ob.EventName,
                                      ob.EventTypeID,
                                      ob.CreatedBy,
                                      ob.Description,
                                      ob.EndTime,
                                      ob.IsActive,
                                      ob.Priority,
                                      ob.StartTime,
                                      ob.Subject,
                                      ob.Venue,
                                      obET.EventTypeName,
                                      CreatedByName = obR.ResidentName
                                  });
                }

                if (FromDate != "0" && ToDate != "0")
                {
                    EventsData = (from ob in DC.tblEvents
                                  join obET in DC.tblEventTypes on ob.EventTypeID equals obET.EventTypeID
                                  join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                  where ob.IsActive == true && ob.StartTime >= Convert.ToDateTime(FromDate) && ob.StartTime <= Convert.ToDateTime(ToDate)
                                  select new
                                  {
                                      ob.EventID,
                                      ob.EventName,
                                      ob.EventTypeID,
                                      ob.CreatedBy,
                                      ob.Description,
                                      ob.EndTime,
                                      ob.IsActive,
                                      ob.Priority,
                                      ob.StartTime,
                                      ob.Subject,
                                      ob.Venue,
                                      obET.EventTypeName,
                                      CreatedByName = obR.ResidentName
                                  });
                }

                return JsonConvert.SerializeObject(EventsData);
            }
            else
            {
                EventObj = (from ob in DC.tblEvents
                            join obET in DC.tblEventTypes on ob.EventTypeID equals obET.EventTypeID
                            join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                            where ob.EventID == EventID
                            select new
                            {
                                ob.EventID,
                                ob.EventName,
                                ob.EventTypeID,
                                ob.CreatedBy,
                                ob.Description,
                                ob.EndTime,
                                ob.IsActive,
                                ob.Priority,
                                ob.StartTime,
                                ob.Subject,
                                ob.Venue,
                                obET.EventTypeName,
                                CreatedByName = obR.ResidentName
                            }).Single();

                return JsonConvert.SerializeObject(EventObj);
            }
        }

        public object EventSearch(string Name = "", string Type = "")
        {
            var DC = new DataClassesDataContext();

            if (Name != "" && Type != "")
            {
                var EventsData = (from ob in DC.tblEvents
                                  join obET in DC.tblEventTypes on ob.EventTypeID equals obET.EventTypeID
                                  join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                  where ob.IsActive == true && SqlMethods.Like(ob.EventName.ToLower(), "%" + Name.ToLower() + "%")
                                  || obET.EventTypeName.ToLower() == Type.ToLower()
                                  select ob);

                return JsonConvert.SerializeObject(EventsData);
            }
            else if (Name == "")
            {
                var EventsData = (from ob in DC.tblEvents
                                  join obET in DC.tblEventTypes on ob.EventTypeID equals obET.EventTypeID
                                  join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                  where ob.IsActive == true && obET.EventTypeName.ToLower() == Type.ToLower()
                                  select ob);

                return JsonConvert.SerializeObject(EventsData);
            }
            else
            {
                var EventsData = (from ob in DC.tblEvents
                                  join obET in DC.tblEventTypes on ob.EventTypeID equals obET.EventTypeID
                                  join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                  where ob.IsActive == true && SqlMethods.Like(ob.EventName.ToLower(), "%" + Name.ToLower() + "%")
                                  select ob);

                return JsonConvert.SerializeObject(EventsData);
            }
        }

        public object EventDelete(int EventID)
        {
            var DC = new DataClassesDataContext();
            var EventObj = (from ob in DC.tblEvents
                            where ob.EventID == EventID
                            select ob).Single();
            EventObj.IsActive = false;
            DC.SubmitChanges();
            return "True";
        }

        /* Events */

        /* FacilityBookings */

        public object GetAllFacilities(int FacilityID = 0)
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
                tblFacility FacilityObj = (from ob in DC.tblFacilities
                                           where ob.FacilityID == FacilityID
                                           select ob).Single();
                return JsonConvert.SerializeObject(FacilityObj);
            }

        }

        public object FacilitiesBookingSearch(string Facility = "", string Date = "")
        {
            var DC = new DataClassesDataContext();
            if (Facility != "" && Date != "")
            {
                IQueryable<object> ProposalsData = (from ob in DC.tblBookings
                                                    join obF in DC.tblFacilities on ob.FacilityID equals obF.FacilityID
                                                    where ob.IsActive == true
                                                    && SqlMethods.Like(obF.FacilityName.ToLower(), "%" + Facility.ToLower() + "%")
                                                    && ob.StartTime.Date == Convert.ToDateTime(Date).Date && ob.ApprovedBy == null
                                                    select new
                                                    {
                                                        ob.FacilityID,
                                                        ob.BookingID,
                                                        obF.FacilityName,
                                                        ob.FlatNo,
                                                        ob.IsApproved,
                                                        ob.Status,
                                                        obF.RatePerHour,
                                                        ob.Description,
                                                        ob.StartTime,
                                                        ob.EndTime,
                                                        ob.Reason
                                                    });
                return JsonConvert.SerializeObject(ProposalsData);
            }
            else if (Facility == "")
            {
                IQueryable<object> ProposalsData = (from ob in DC.tblBookings
                                                    join obF in DC.tblFacilities on ob.FacilityID equals obF.FacilityID
                                                    where ob.IsActive == true
                                                    && ob.StartTime.Date == Convert.ToDateTime(Date).Date && ob.ApprovedBy == null
                                                    select new
                                                    {
                                                        ob.FacilityID,
                                                        ob.BookingID,
                                                        obF.FacilityName,
                                                        ob.FlatNo,
                                                        ob.IsApproved,
                                                        ob.Status,
                                                        obF.RatePerHour,
                                                        ob.Description,
                                                        ob.StartTime,
                                                        ob.EndTime,
                                                        ob.Reason
                                                    });
                return JsonConvert.SerializeObject(ProposalsData);
            }
            else
            {
                IQueryable<object> ProposalsData = (from ob in DC.tblBookings
                                                    join obF in DC.tblFacilities on ob.FacilityID equals obF.FacilityID
                                                    where ob.IsActive == true &&
                                                    SqlMethods.Like(obF.FacilityName.ToLower(), "%" + Facility.ToLower() + "%") && ob.ApprovedBy == null
                                                    select new
                                                    {
                                                        ob.FacilityID,
                                                        ob.BookingID,
                                                        obF.FacilityName,
                                                        ob.FlatNo,
                                                        ob.IsApproved,
                                                        ob.Status,
                                                        obF.RatePerHour,
                                                        ob.Description,
                                                        ob.StartTime,
                                                        ob.EndTime,
                                                        ob.Reason
                                                    });
                return JsonConvert.SerializeObject(ProposalsData);
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
                                                       where ob.IsDeleted == false
                                                       select new
                                                       {
                                                           ob.MemberID,
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
                object MemberObj = (from ob in DC.tblStaffMembers
                                    join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                    where ob.MemberID == MemberID && ob.IsDeleted == false
                                    select new
                                    {
                                        ob.MemberID,
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

        public object StaffSearch(string Name = "", string Type = "")
        {
            var DC = new DataClassesDataContext();
            if (Name != "" && Type != "")
            {
                IQueryable<object> StaffMembersData = (from ob in DC.tblStaffMembers
                                                       join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                                       where ob.IsDeleted == false && SqlMethods.Like(ob.MemberName.ToLower(), "%" + Name.ToLower() + "%")
                                                       || ob.MemberType.ToLower() == Type.ToLower()
                                                       select new
                                                       {
                                                           ob.MemberID,
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
            else if (Name == "")
            {
                IQueryable<object> StaffMembersData = (from ob in DC.tblStaffMembers
                                                       join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                                       where ob.IsDeleted == false && ob.MemberType.ToLower() == Type.ToLower()
                                                       select new
                                                       {
                                                           ob.MemberID,
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
                IQueryable<object> StaffMembersData = (from ob in DC.tblStaffMembers
                                                       join obR in DC.tblResidents on ob.CreatedBy equals obR.ResidentID
                                                       where ob.IsDeleted == false && SqlMethods.Like(ob.MemberName.ToLower(), "%" + Name.ToLower() + "%")
                                                       select new
                                                       {
                                                           ob.MemberID,
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
        }

        public object StaffDelete(int StaffID)
        {
            var DC = new DataClassesDataContext();
            var StaffObj = (from ob in DC.tblStaffMembers
                            where ob.MemberID == StaffID
                            select ob).Single();
            StaffObj.IsDeleted = true;
            DC.SubmitChanges();
            return "True";
        }

        /* Staff Members */

        /* Polls */

        public object GetAllPolls(int PollID = 0, bool IsActive = false)
        {
            var DC = new DataClassesDataContext();
            if (PollID == 0)
            {
                IQueryable<object> PollsData;
                if (IsActive == false)
                {
                    PollsData = (from ob in DC.tblPolls
                                 where ob.IsDeleted == false && ob.EndTime < DateTime.Now
                                 select new
                                 {
                                     ob.PollID,
                                     ob.PollTitle,
                                     ob.PollType,
                                     ob.CreatedBy,
                                     ob.CreatedOn,
                                     ob.EndTime,
                                     PollOptions = (from obPO in DC.tblPollOptions
                                                    where obPO.PollID == ob.PollID
                                                    select obPO)
                                 });
                }
                else
                {
                    PollsData = (from ob in DC.tblPolls
                                 where ob.IsActive == true && ob.IsDeleted == false
                                 select new
                                 {
                                     ob.PollID,
                                     ob.PollTitle,
                                     ob.PollType,
                                     ob.CreatedBy,
                                     ob.CreatedOn,
                                     ob.EndTime,
                                     PollOptions = (from obPO in DC.tblPollOptions
                                                    where obPO.PollID == ob.PollID
                                                    select obPO)
                                 });
                }

                return JsonConvert.SerializeObject(PollsData);
            }
            else
            {
                var PollObj = (from ob in DC.tblPolls
                               where ob.PollID == PollID
                               select new
                               {
                                   ob.PollID,
                                   ob.PollTitle,
                                   ob.PollType,
                                   ob.CreatedBy,
                                   ob.CreatedOn,
                                   ob.EndTime,
                                   PollOptions = (from obPO in DC.tblPollOptions
                                                  where obPO.PollID == ob.PollID
                                                  select obPO)
                               });
                return JsonConvert.SerializeObject(PollObj);
            }
        }

        public object GetPollResults(int PollID = 0)
        {
            var DC = new DataClassesDataContext();
            if (PollID == 0)
            {
                var PollResultsData = (from ob in DC.tblPolls
                                       where ob.IsDeleted == false /*&& ob.EndTime <= DateTime.Now*/
                                       select new
                                       {
                                           ob,
                                           PollVoting = (from obPV in DC.tblPollVotings
                                                         join obO in DC.tblPollOptions on obPV.PollOptionID equals obO.PollOptionID
                                                         where obPV.PollID == ob.PollID && ob.PollID == obO.PollID
                                                         group obPV by obPV.PollOptionID
                                                         into grp
                                                         select new
                                                         {
                                                             OptionID = grp.Key,
                                                             OptionName = (from obO1 in DC.tblPollOptions
                                                                           where obO1.PollOptionID == grp.Key
                                                                           select obO1.PollOptionName).Single(),
                                                             VoteCount = grp.ToList().Count()
                                                         })
                                       });

                return JsonConvert.SerializeObject(PollResultsData);
            }
            else
            {
                var PollResultsObj = (from obPV in DC.tblPollVotings
                                      join ob in DC.tblPolls on obPV.PollID equals ob.PollID
                                      join obO in DC.tblPollOptions on obPV.PollOptionID equals obO.PollOptionID
                                      where ob.PollID == PollID && ob.IsDeleted == false && obPV.PollID == ob.PollID && ob.PollID == obO.PollID
                                      select new
                                      {
                                          OptionName = obO.PollOptionName,
                                          VoteCount = (from obPV1 in DC.tblPollVotings
                                                       where obPV1.PollOptionID == obO.PollOptionID
                                                       select obPV1).Count()
                                      }).Distinct();

                return JsonConvert.SerializeObject(PollResultsObj);
            }
        }

        /* Polls */

        /* Complaints */

        /* Complaints */

        /* Documents */

        public object GetAllDocuments()
        {
            var DC = new DataClassesDataContext();
            var DocumentsData = (from ob in DC.tblOwnerDocuments
                                 select ob);

            return JsonConvert.SerializeObject(DocumentsData);
        }

        public object AddDocument(string FileType, string FilePath, string DocumentType)
        {
            var DC = new DataClassesDataContext();
            tblOwnerDocument DocObj = new tblOwnerDocument();
            DocObj.ApprovedBy = 1;
            DocObj.DocumentType = DocumentType;
            DocObj.Filepath = FilePath;
            DocObj.FileType = FileType;
            DocObj.FlatNo = "0";

            DC.tblOwnerDocuments.InsertOnSubmit(DocObj);
            DC.SubmitChanges();

            return "True";
        }

        /* Documents */

        /* Light Switching */

        private string GetIP()
        {
            var DC = new DataClassesDataContext();
            var IP = (from ob in DC.tblAutomations
                      where ob.ID == 1
                      select ob.IPAdd).Single();
            return IP;
        }

        public object SetIP(string IP)
        {
            var DC = new DataClassesDataContext();
            tblAutomation AutoObj = (from ob in DC.tblAutomations
                                     where ob.ID == 1
                                     select ob).Single();
            AutoObj.IPAdd = IP;
            DC.SubmitChanges();
            return true;
        }

        public object SetFloorLights(bool Floor1, bool Floor2, bool Floor3, bool Floor4)
        {
            try
            {
                string IP = GetIP();
                string requestStr = "http://" + IP + "/toggle?Switches=";
                string pins = "";
                if (Floor1)
                    pins += "1";
                else
                    pins += "0";
                if (Floor2)
                    pins += "1";
                else
                    pins += "0";
                if (Floor3)
                    pins += "1";
                else
                    pins += "0";
                if (Floor4)
                    pins += "1";
                else
                    pins += "0";

                requestStr += pins;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestStr);
                request.Method = "Get";
                request.KeepAlive = true;
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string myResponse = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    myResponse = sr.ReadToEnd();
                }

                var DC = new DataClassesDataContext();
                tblAutomation AutoObj = (from ob in DC.tblAutomations
                                         where ob.ID == 1
                                         select ob).Single();

                if (pins == myResponse)
                {

                    AutoObj.Floor1 = Floor1;
                    AutoObj.Floor2 = Floor2;
                    AutoObj.Floor3 = Floor3;
                    AutoObj.Floor4 = Floor4;
                    AutoObj.temp = myResponse;

                    DC.SubmitChanges();
                    return "True";
                }
                else
                {
                    return "False";
                }
            }
            catch(Exception e)
            {
                return "False";
            }
        }

        public object GetSLStatus()
        {
            try
            {
                string IP = GetIP();
                string requestStr = "http://" + IP + "/SLStatus?stat=send";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestStr);
                request.Method = "Get";
                request.KeepAlive = true;
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string myResponse = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    myResponse = sr.ReadToEnd();
                }

                var DC = new DataClassesDataContext();
                tblAutomation AutoObj = (from ob in DC.tblAutomations
                                         where ob.ID == 1
                                         select ob).Single();

                if (myResponse == "1")
                {
                    AutoObj.StreetLight1 = true;
                    AutoObj.StreetLight2 = true;
                }
                else
                {
                    AutoObj.StreetLight1 = true;
                    AutoObj.StreetLight2 = true;
                }

                return "True";
            }
            catch(Exception e)
            {
                return "False";
            }
        }

        public object SetSensor(bool Stat)
        {
            try
            {
                string IP = GetIP();
                string requestStr = "http://" + IP + "/sensor?s=";
                string stat = "";
                if (Stat)
                    stat += "1";
                else
                    stat += "0";

                requestStr += stat;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestStr);
                request.Method = "Get";
                request.KeepAlive = true;
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string myResponse = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    myResponse = sr.ReadToEnd();
                }

                var DC = new DataClassesDataContext();
                tblAutomation AutoObj = (from ob in DC.tblAutomations
                                         where ob.ID == 1
                                         select ob).Single();

                if (myResponse == "0" && stat == "0")
                {
                    AutoObj.Sensor = false;
                    AutoObj.StreetLight1 = false;
                    AutoObj.StreetLight2 = false;
                    DC.SubmitChanges();
                    return "True";
                }
                else if (myResponse == "0" && stat == "1")
                {
                    AutoObj.Sensor = false;
                    AutoObj.StreetLight1 = false;
                    AutoObj.StreetLight2 = false;
                    DC.SubmitChanges();
                    return "False";
                }
                else if (myResponse == "1" && stat == "1")
                {
                    AutoObj.Sensor = true;
                    AutoObj.StreetLight1 = false;
                    AutoObj.StreetLight2 = false;
                    DC.SubmitChanges();
                    return "False";
                }
                else
                {
                    AutoObj.Sensor = false;
                    AutoObj.StreetLight1 = false;
                    AutoObj.StreetLight2 = false;
                    DC.SubmitChanges();
                    return "False";
                }
            }
            catch(Exception e)
            {
                return "False";
            }
        }

        public object SetStreetLight(bool Stat)
        {
            try
            {
                string IP = GetIP();
                string requestStr = "http://" + IP + "/STtoggle?StreetLight=";
                if (Stat)
                    requestStr += "1";
                else
                    requestStr += "0";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestStr);
                request.Method = "Get";
                request.KeepAlive = true;
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string myResponse = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    myResponse = sr.ReadToEnd();
                }

                if (myResponse == "True")
                {
                    var DC = new DataClassesDataContext();
                    tblAutomation AutoObj = (from ob in DC.tblAutomations
                                             where ob.ID == 1
                                             select ob).Single();
                    AutoObj.StreetLight1 = true;
                    AutoObj.StreetLight2 = true;
                    AutoObj.temp = myResponse;

                    DC.SubmitChanges();
                }

                return "True";
            }
            catch(Exception e)
            {
                return "False";
            }
        }

        public object GetTankLevel()
        {
            try
            {
                string IP = GetIP();
                string requestStr = "http://" + IP + "/AutomationCall.aspx?TankLevel=1";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestStr);
                request.Method = "Get";
                request.KeepAlive = true;
                request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string myResponse = "";
                using (System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream()))
                {
                    myResponse = sr.ReadToEnd();
                }

                int startIndex = myResponse.IndexOf('*');
                int len = myResponse.Length;
                myResponse = myResponse.Substring(startIndex + 1, startIndex + 10);
                int endIndex = myResponse.IndexOf('*');
                myResponse = myResponse.Substring(0, endIndex);

                var DC = new DataClassesDataContext();
                tblAutomation AutoObj = (from ob in DC.tblAutomations
                                         where ob.ID == 1
                                         select ob).Single();
                AutoObj.TankLevel = Convert.ToInt32(myResponse);
                AutoObj.temp = myResponse;

                DC.SubmitChanges();

                return myResponse;
            }
            catch(Exception e)
            {
                return "False";
            }
        }

        public object GetAllAutoStates()
        {
            var DC = new DataClassesDataContext();
            var AutomationState = (from ob in DC.tblAutomations
                                   select ob).Single();

            return JsonConvert.SerializeObject(AutomationState);
        }

        /* Light Switching */

        /* Notifications */

        public object SetNotification(string Text, string Type, string FlatNo, string PageLink)
        {
            var DC = new DataClassesDataContext();
            tblNotification NotificationObj = new tblNotification();
            NotificationObj.CreatedOn = DateTime.Now;
            NotificationObj.Text = Text;
            NotificationObj.Type = Type;
            NotificationObj.PageLink = PageLink;
            NotificationObj.IsActive = true;

            DC.tblNotifications.InsertOnSubmit(NotificationObj);

            int NotiID = (from ob in DC.tblNotifications
                          orderby ob.NotificationID descending
                          select ob.NotificationID).First();


            if (FlatNo == "0")
            {
                for (int i = 1; i <= 16; i++)
                {
                    tblNotificationDetail NotiDetailsObj = new tblNotificationDetail();
                    NotiDetailsObj.NotificationID = NotiID;
                    NotiDetailsObj.Recipient = i;
                    NotiDetailsObj.IsRead = false;
                    DC.tblNotificationDetails.InsertOnSubmit(NotiDetailsObj);
                }
            }
            else
            {
                tblNotificationDetail NotiDetailsObj = new tblNotificationDetail();
                NotiDetailsObj.NotificationID = NotiID;
                NotiDetailsObj.Recipient = Convert.ToInt32(FlatNo);
                NotiDetailsObj.IsRead = false;
                DC.tblNotificationDetails.InsertOnSubmit(NotiDetailsObj);
            }
            DC.SubmitChanges();

            return true;
        }

        /* Notifications */
    }
}
