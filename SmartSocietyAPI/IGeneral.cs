using System.ServiceModel;
using System.ServiceModel.Web;

namespace SmartSocietyAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IGeneral
    {
        /* Society Setup */

        [OperationContract]
        [WebGet(UriTemplate = "GetSocietyInformation/", ResponseFormat = WebMessageFormat.Json)]
        object GetSocietyInformation();

        // 0 for All, 1 for Current, -1 for Past Members
        [OperationContract]
        [WebGet(UriTemplate = "GetAllResidentsDetails/{FlagMemType,ResidentID}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllResidentsDetails(int FlagMemType = 0, int ResidentID = 0);

        [OperationContract]
        [WebGet(UriTemplate = "ResidentSearch/{Name,FlatNo}", ResponseFormat = WebMessageFormat.Json)]
        object ResidentSearch(string Name = "", string FlatNo = "0");

        [OperationContract]
        [WebGet(UriTemplate = "ResidentDelete/{ResidentID}", ResponseFormat = WebMessageFormat.Json)]
        object ResidentDelete(int ResidentID);

        // 0 for All, 1 for Owners, -1 for On Rent Flats
        [OperationContract]
        [WebGet(UriTemplate = "GetAllFlatDetails/{FlagFlatType,FlatNo}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllFlatDetails(int FlagFlatType = 0, string FlatNo = "0");

        [OperationContract]
        [WebGet(UriTemplate = "ViewAllAssets/{AssetID}", ResponseFormat = WebMessageFormat.Json)]
        object ViewAllAssets(int AssetID = 0);

        [OperationContract]
        [WebGet(UriTemplate = "AssetSearch/{Name,Type}", ResponseFormat = WebMessageFormat.Json)]
        object AssetSearch(string Name = "", string Type = "");

        [OperationContract]
        [WebGet(UriTemplate = "AssetDelete/{AssetID}", ResponseFormat = WebMessageFormat.Json)]
        object AssetDelete(int AssetID);

        /* Society Setup */

        /* Gatekeeping */

        [OperationContract]
        [WebGet(UriTemplate = "ViewGateKeeping/{CheckedInOnly,FromDate,ToDate,FlatNo}", ResponseFormat = WebMessageFormat.Json)]
        object ViewGateKeeping(bool CheckedInOnly = false, string FromDate = "0", string ToDate = "0", string FlatNo = "-1");

        /* Gatekeeping */

        /* Notices */

        [OperationContract]
        [WebGet(UriTemplate = "ViewAllNotices/{FromDate,ToDate,Priority,NoticeID}", ResponseFormat = WebMessageFormat.Json)]
        object ViewAllNotices(string FromDate = "0", string ToDate = "0", int Priority = 0, int NoticeID = 0);

        /* Notices */

        /* Vendors */

        [OperationContract]
        [WebGet(UriTemplate = "ViewAllVendors/{VendorID}", ResponseFormat = WebMessageFormat.Json)]
        object ViewAllVendors(int VendorID = 0);

        [OperationContract]
        [WebGet(UriTemplate = "VendorSearch/{Name,Type}", ResponseFormat = WebMessageFormat.Json)]
        object VendorSearch(string Name = "", string Type = "");

        [OperationContract]
        [WebGet(UriTemplate = "VendorDelete/{VendorID}", ResponseFormat = WebMessageFormat.Json)]
        object VendorDelete(int VendorID);

        /* Vendors */

        /* Events */

        [OperationContract]
        [WebGet(UriTemplate = "ViewAllEvents/{FromDate,ToDate,Priority,EventID}", ResponseFormat = WebMessageFormat.Json)]
        object ViewAllEvents(string FromDate = "0", string ToDate = "0", int Priority = 0, int EventID = 0);

        [OperationContract]
        [WebGet(UriTemplate = "EventSearch/{Name, Type}", ResponseFormat = WebMessageFormat.Json)]
        object EventSearch(string Name = "", string Type = "");

        [OperationContract]
        [WebGet(UriTemplate = "EventDelete/{EventID}", ResponseFormat = WebMessageFormat.Json)]
        object EventDelete(int EventID);

        /* Events */

        /* FacilityBookings */

        [OperationContract]
        [WebGet(UriTemplate = "GetAllFacilities/{FacilityID}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllFacilities(int FacilityID = 0);

        [OperationContract]
        [WebGet(UriTemplate = "FacilitiesBookingSearch/{Facility,Date}", ResponseFormat = WebMessageFormat.Json)]
        object FacilitiesBookingSearch(string Facility = "", string Date = "");

        /* FacilityBookings */

        /* Staff Members */

        [OperationContract]
        [WebGet(UriTemplate = "GetAllStaffMembers/{MemberID}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllStaffMembers(int MemberID = 0);

        [OperationContract]
        [WebGet(UriTemplate = "StaffSearch/{Name, Type}", ResponseFormat = WebMessageFormat.Json)]
        object StaffSearch(string Name = "", string Type = "");

        [OperationContract]
        [WebGet(UriTemplate = "StaffDelete/{StaffID}", ResponseFormat = WebMessageFormat.Json)]
        object StaffDelete(int StaffID);

        /* Staff Members */

        /* Polls */

        [OperationContract]
        [WebGet(UriTemplate = "GetAllPolls/{PollID,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllPolls(int PollID = 0, bool IsActive = false);

        /* Polls */

        /* Complaints */



        /* Complaints */

        /* Payments & Transactions */

        /* Payments & Transactions */

        [OperationContract]
        [WebGet(UriTemplate = "SetIP/{IP}", ResponseFormat = WebMessageFormat.Json)]
        object SetIP(string IP);

        [OperationContract]
        [WebGet(UriTemplate = "SetFloorLights/{Floor1,Floor2,Floor3,Floor4}", ResponseFormat = WebMessageFormat.Json)]
        object SetFloorLights(bool Floor1, bool Floor2, bool Floor3, bool Floor4);

        [OperationContract]
        [WebGet(UriTemplate = "GetSLStatus/", ResponseFormat = WebMessageFormat.Json)]
        object GetSLStatus();

        [OperationContract]
        [WebGet(UriTemplate = "SetSensor/{Stat}", ResponseFormat = WebMessageFormat.Json)]
        object SetSensor(bool Stat);

        [OperationContract]
        [WebGet(UriTemplate = "SetStreetLight/{Stat}", ResponseFormat = WebMessageFormat.Json)]
        object SetStreetLight(bool Stat);

        [OperationContract]
        [WebGet(UriTemplate = "GetTankLevel/", ResponseFormat = WebMessageFormat.Json)]
        object GetTankLevel();

        /* Notifications */

        [OperationContract]
        [WebGet(UriTemplate = "SetNotification/{Text,Type,FlatNo,PageLink}", ResponseFormat = WebMessageFormat.Json)]
        object SetNotification(string Text, string Type, string FlatNo, string PageLink);
        /* Notifications */
    }
}
