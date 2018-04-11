using System.ServiceModel;
using System.ServiceModel.Web;

namespace SmartSocietyAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IGeneral
    {
        /* Society Setup */

        // 0 for All, 1 for Current, -1 for Past Members
        [OperationContract]
        [WebGet(UriTemplate = "GetAllResidentsDetails/{FlagMemType,ResidentID}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllResidentsDetails(int FlagMemType = 0, int ResidentID = 0);

        // 0 for All, 1 for Owners, -1 for On Rent Flats
        [OperationContract]
        [WebGet(UriTemplate = "GetAllFlatDetails/{FlagFlatType,FlatNo}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllFlatDetails(int FlagFlatType = 0, string FlatNo = "0");

        [OperationContract]
        [WebGet(UriTemplate = "ViewAllAssets/{AssetID}", ResponseFormat = WebMessageFormat.Json)]
        object ViewAllAssets(int AssetID=0);

        /* Society Setup */

        /* Gatekeeping */

        [OperationContract]
        [WebGet(UriTemplate = "ViewGateKeeping/{CheckedInOnly,FromDate,ToDate,FlatNo}", ResponseFormat = WebMessageFormat.Json)]
        object ViewGateKeeping(bool CheckedInOnly = false, string FromDate = "0", string ToDate = "0", string FlatNo="-1");

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

        /* Vendors */

        /* Events */

        [OperationContract]
        [WebGet(UriTemplate = "ViewAllEvents/{FromDate,ToDate,Priority,EventID}", ResponseFormat = WebMessageFormat.Json)]
        object ViewAllEvents(string FromDate = "0", string ToDate = "0", int Priority = 0, int EventID=0);

        /* Events */

        /* FacilityBookings */

        [OperationContract]
        [WebGet(UriTemplate = "GetAllFacilities/{FacilityID}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllFacilities(int FacilityID = 0);

        /* FacilityBookings */

        /* Staff Members */

        [OperationContract]
        [WebGet(UriTemplate = "GetAllStaffMembers/{MemberID}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllStaffMembers(int MemberID = 0);

        /* Staff Members */

        /* Polls */

        [OperationContract]
        [WebGet(UriTemplate = "GetAllPolls/{PollID,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllPolls(int PollID = 0, bool IsActive = false);

        /* Polls */
    }
}
