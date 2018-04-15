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
        [WebGet(UriTemplate = "GetPositionData/", ResponseFormat = WebMessageFormat.Json)]
        object GetPositionData();

        [OperationContract]
        [WebGet(UriTemplate = "SetResident/{Name,DOB,FlatNo,Occupation,Contact1,Contact2,Email,Image,PositionID,FlatHolderID,Gender}", ResponseFormat = WebMessageFormat.Json)]
        object SetResident(string Name, string DOB, string FlatNo, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID, string Gender);

        [OperationContract]
        [WebGet(UriTemplate = "EditResident/{ResidentID,Name,DOB,FlatID,Occupation,Contact1,Contact2,Email,Image,PositionID,FlatHolderID,IsActive,Gender}", ResponseFormat = WebMessageFormat.Json)]
        object EditResident(int ResidentID, string Name, string DOB, string FlatNo, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID, bool IsActive, string Gender);

        /* Society Setup */

        /* Notices */
        
        [OperationContract]
        [WebGet(UriTemplate = "GetNotices/{FlatNo}", ResponseFormat = WebMessageFormat.Json)]
        object GetNotices(string FlatNo);

        /* Notices */

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

        /* Events */
        
        /* Events */

        /* FacilityBookings */

        [OperationContract]
        [WebGet(UriTemplate = "ProposeFacilityBooking/{FacilityID,FlatNo,StartTime,EndTime,Purpose,Description}", ResponseFormat = WebMessageFormat.Json)]
        object ProposeFacilityBooking(int FacilityID, string FlatNo, string StartTime, string EndTime, string Purpose, string Description);

        /* FacilityBookings */

        /* Polls */

        [OperationContract]
        [WebGet(UriTemplate = "SetPollVote/{FlatNo,PollOptionID,PollID}", ResponseFormat = WebMessageFormat.Json)]
        object SetPollVote(string FlatNo, int PollOptionID, int PollID);

        /* Polls */

        /* Complaints */

        [OperationContract]
        [WebGet(UriTemplate = "AddComplaint/{ComplaintType,Subject,Description,Priority}", ResponseFormat = WebMessageFormat.Json)]
        object AddComplaint(string ComplaintType, string Subject, string Description, int Priority);

        /* Complaints */

        /* Payments & Transactions */

        [OperationContract]
        [WebGet(UriTemplate = "GetAllPaymentDues/{FlatNo}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllPaymentDues(string FlatNo);

        [OperationContract]
        [WebGet(UriTemplate = "SetPayment/{FlatNo,PaymentID,PaymentMode,ChequeNo,Amount,Penalty}", ResponseFormat = WebMessageFormat.Json)]
        object SetPayment(string FlatNo, int PaymentID, string PaymentMode, string ChequeNo, decimal Amount, decimal Penalty);

        /* Payments & Transactions */
    }
}
