using System.ServiceModel;
using System.ServiceModel.Web;

namespace SmartSocietyAPI
{
    [ServiceContract]
    public interface IClient
    {
        /* Login & Registration */

        [OperationContract]
        [WebGet(UriTemplate = "CheckLogin/{Username,Password}", ResponseFormat = WebMessageFormat.Json)]
        object CheckLogin(string Username, string Password);

        [OperationContract]
        [WebGet(UriTemplate = "ForgotPassword/{Username}", ResponseFormat = WebMessageFormat.Json)]
        object ForgotPassword(string Username);

        [OperationContract]
        [WebGet(UriTemplate = "ResetPassword/{Username,VerificationCode,Password}", ResponseFormat = WebMessageFormat.Json)]
        object ResetPassword(string Username, string VerificationCode, string Password);

        /* Login & Registration */

        /* Society Setup */

        [OperationContract]
        [WebGet(UriTemplate = "SetResident/{Name,DOB,FlatID,Occupation,Contact1,Contact2,Email,Image,PositionID,FlatHolderID}", ResponseFormat = WebMessageFormat.Json)]
        object SetResident(string Name, string DOB, string FlatNo, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID);

        [OperationContract]
        [WebGet(UriTemplate = "EditResident/{ResidentID,Name,DOB,FlatID,Occupation,Contact1,Contact2,Email,Image,PositionID,FlatHolderID,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object EditResident(int ResidentID, string Name, string DOB, string FlatNo, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID, bool IsActive);

        /* Society Setup */

        /* Gatekeeping */

        [OperationContract]
        [WebGet(UriTemplate = "GateCheckIn/{VisitorName,FlatID,Purpose,VehicleNo,MobileNo}", ResponseFormat = WebMessageFormat.Json)]
        object GateCheckIn(string VisitorName, string FlatNo, string Purpose, string VehicleNo, string MobileNo);

        [OperationContract]
        [WebGet(UriTemplate = "GateCheckOut/{VisitorID}", ResponseFormat = WebMessageFormat.Json)]
        object GateCheckOut(int VisitorID);

        /* Gatekeeping */

        /* Vendors */

        [OperationContract]
        [WebGet(UriTemplate = "AddVendorRating/{VendorID,Rating}", ResponseFormat = WebMessageFormat.Json)]
        object AddVendorRating(int VendorID, double Rating);

        /* Vendors */
    }
}
