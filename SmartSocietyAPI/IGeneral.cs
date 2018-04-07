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
        [WebGet(UriTemplate = "GetAllResidentsDetails/{FlagMemType}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllResidentsDetails(int FlagMemType = 0);

        // 0 for All, 1 for Owners, -1 for On Rent Flats
        [OperationContract]
        [WebGet(UriTemplate = "GetAllFlatDetails/{FlagFlatType}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllFlatDetails(int FlagFlatType = 0);

        /* Society Setup */

        /* Gatekeeping */

        [OperationContract]
        [WebGet(UriTemplate = "ViewGateKeeping/{CheckedInOnly,FromDate,ToDate}", ResponseFormat = WebMessageFormat.Json)]
        object ViewGateKeeping(bool CheckedInOnly = false, string FromDate = "0", string ToDate = "0", string FlatNo="-1");

        /* Gatekeeping */

        /* Notices */

        [OperationContract]
        [WebGet(UriTemplate = "ViewAllNotices/{FromDate,ToDate,Priority}", ResponseFormat = WebMessageFormat.Json)]
        object ViewAllNotices(string FromDate = "0", string ToDate = "0", int Priority = 0);

        /* Notices */

        /* Vendors */

        [OperationContract]
        [WebGet(UriTemplate = "ViewAllVendors/", ResponseFormat = WebMessageFormat.Json)]
        object ViewAllVendors();

        /* Vendors */
    }
}
