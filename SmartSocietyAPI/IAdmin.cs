using System.ServiceModel;
using System.ServiceModel.Web;

namespace SmartSocietyAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAdmin
    {
        /*Login And Forgot Password*/

        [OperationContract]
        [WebGet(UriTemplate = "CheckLogin/{Username,Password}", ResponseFormat = WebMessageFormat.Json)]
        string CheckLogin(string Username,string Password);

        [OperationContract]
        [WebGet(UriTemplate = "ForgotPassword/{Username}", ResponseFormat = WebMessageFormat.Json)]
        string ForgotPassword(string Username);

        [OperationContract]
        [WebGet(UriTemplate = "ResetPassword/{Username,Password}", ResponseFormat = WebMessageFormat.Json)]
        string ResetPassword(string Username, string Password);

        /*Login And Forgot Password*/

        /*Society Setup*/

        [OperationContract]
        [WebGet(UriTemplate = "SetSocietyInformation/{Name, Address, PostalCode, LogoImage, ContactNo, PresidentName, Builder, Email, Fax, RegistrationNo, CampusArea, SocietyType, LatLong}", ResponseFormat = WebMessageFormat.Json)]
        object SetSocietyInformation(string Name, string Address, string PostalCode, string LogoImage, string ContactNo, string PresidentName, string Builder, string Email, string Fax, string RegistrationNo, string CampusArea, string SocietyType, string LatLong);

        [OperationContract]
        [WebGet(UriTemplate = "EditSocietyInformation/{Name, Address, PostalCode, LogoImage, ContactNo, PresidentName, Builder, Email, Fax, RegistrationNo, CampusArea, SocietyType, LatLong}", ResponseFormat = WebMessageFormat.Json)]
        object EditSocietyInformation(string Name, string Address, string PostalCode, string LogoImage, string ContactNo, string PresidentName, string Builder, string Email, string Fax, string RegistrationNo, string CampusArea, string SocietyType, string LatLong);

        [OperationContract]
        [WebGet(UriTemplate = "GetSocietyInformation/", ResponseFormat = WebMessageFormat.Json)]
        object GetSocietyInformation();

        [OperationContract]
        [WebGet(UriTemplate = "SetFlatHolder/{StartDate,Name,DOB,FlatNo,Occupation,Contact1,Contact2,Email,Image,PositionID,FlatHolderID,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object SetFlatHolder(string StartDate, string Name, string DOB, string FlatNo, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID, bool IsActive);

        [OperationContract]
        [WebGet(UriTemplate = "AddAssetType/{AssetTypeName}", ResponseFormat = WebMessageFormat.Json)]
        object AddAssetType(string AssetTypeName);

        [OperationContract]
        [WebGet(UriTemplate = "EditAssetType/{AssetTypeID,AssetTypeName}", ResponseFormat = WebMessageFormat.Json)]
        object EditAssetType(int AssetTypeID, string AssetTypeName);

        [OperationContract]
        [WebGet(UriTemplate = "AddAsset/{AssetName, AssetTypeID, Image, InvoiceDoc, AssetValue, PurchasedOn, Status}", ResponseFormat = WebMessageFormat.Json)]
        object AddAsset(string AssetName, int AssetTypeID, string Image, string InvoiceDoc, int AssetValue, string PurchasedOn, string Status);

        [OperationContract]
        [WebGet(UriTemplate = "EditAsset/{AssetID,AssetName, AssetTypeID, Image, InvoiceDoc, AssetValue, PurchasedOn, Status, IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object EditAsset(int AssetID, string AssetName, int AssetTypeID, string Image, string InvoiceDoc, int AssetValue, string PurchasedOn, string Status, bool IsActive);

        [OperationContract]
        [WebGet(UriTemplate = "AddStaffMember/{Name, MemberType, DOB, Contact1, Contact2, Image, Address, DOJ, DOL, CreatedBy}", ResponseFormat = WebMessageFormat.Json)]
        object AddStaffMember(string Name, string MemberType, string DOB, string Contact1, string Contact2, string Image, string Address, string DOJ, string DOL, int CreatedBy);

        [OperationContract]
        [WebGet(UriTemplate = "EditStaffMember/{MemberID, Name, MemberType, DOB, Contact1, Contact2, Image, Address, DOJ, DOL, CreatedBy, IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object EditStaffMember(int MemberID, string Name, string MemberType, string DOB, string Contact1, string Contact2, string Image, string Address, string DOJ, string DOL, int CreatedBy, bool IsActive);

        /*Society Setup*/

        /* Notices */

        [OperationContract]
        [WebGet(UriTemplate = "AddNotice/{Title,Description,Priority,CreatedBy}", ResponseFormat = WebMessageFormat.Json)]
        object AddNotice(string Title, string Description, int Priority, int CreatedBy);

        [OperationContract]
        [WebGet(UriTemplate = "EditNotice/{NoticeID,Title,Description,Priority,CreatedBy,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object EditNotice(int NoticeID, string Title, string Description, int Priority, int CreatedBy, bool IsActive);

        /* Notices */

        /* Vendors */

        [OperationContract]
        [WebGet(UriTemplate = "AddNewVendor/{VendorName,Address,Location}", ResponseFormat = WebMessageFormat.Json)]
        object AddNewVendor(string VendorName, string Address, string Location, string VendorType, string Description);

        [OperationContract]
        [WebGet(UriTemplate = "EditVendor/{VendorID,VendorName,Address,Location,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object EditVendor(int VendorID, string VendorName, string Address, string Location, string VendorType, string Description, bool IsActive);

        /* Vendors */

        /* Events */

        [OperationContract]
        [WebGet(UriTemplate = "GetAllEventTypes/", ResponseFormat = WebMessageFormat.Json)]
        object GetAllEventTypes();

        [OperationContract]
        [WebGet(UriTemplate = "AddEvent/{EventName,EventTypeID,Venue,StartTime,EndTime,Subject,Description,Priority,CreatedBy}", ResponseFormat = WebMessageFormat.Json)]
        object AddEvent(string EventName, int EventTypeID, string Venue, string StartTime, string EndTime, string Subject, string Description, int Priority, int CreatedBy);

        [OperationContract]
        [WebGet(UriTemplate = "EditEvent/{EventID,EventName,EventTypeID,Venue,StartTime,EndTime,Subject,Description,Priority,CreatedBy,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object EditEvent(int EventID, string EventName, int EventTypeID, string Venue, string StartTime, string EndTime, string Subject, string Description, int Priority, int CreatedBy, bool IsActive);

        /* Events */

        /* FacilityBookings */

        [OperationContract]
        [WebGet(UriTemplate = "AddFacility/{FacilityName,RatePerHour}", ResponseFormat = WebMessageFormat.Json)]
        object AddFacility(string FacilityName, int RatePerHour);

        [OperationContract]
        [WebGet(UriTemplate = "EditFacility/{FacilityID,FacilityName,RatePerHour,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object EditFacility(int FacilityID, string FacilityName, int RatePerHour, bool IsActive);

        [OperationContract]
        [WebGet(UriTemplate = "ViewAllBookingProposals/{Approved}", ResponseFormat = WebMessageFormat.Json)]
        object ViewAllBookingProposals(int Approved = 0);

        [OperationContract]
        [WebGet(UriTemplate = "ApprovalOfBooking/{BookingID,Approval,ApprovedBy}", ResponseFormat = WebMessageFormat.Json)]
        object ApprovalOfBooking(int BookingID, bool Approval, int ApprovedBy, decimal RatePerHour, string Reason);

        /* FacilityBookings */

        /* Polls */

        [OperationContract]
        [WebGet(UriTemplate = "AddPoll/{PollTitle,PollType,CreatedBy,EndTime}", ResponseFormat = WebMessageFormat.Json)]
        object AddPoll(string PollTitle, int PollType, int CreatedBy, string EndTime);

        [OperationContract]
        [WebGet(UriTemplate = "EditPoll/{PollID,PollTitle,PollType,CreatedBy,EndTime,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object EditPoll(int PollID, string PollTitle, int PollType, int CreatedBy, string EndTime, bool IsActive);

        [OperationContract]
        [WebGet(UriTemplate = "AddPollOptions/{PollOptionNames}", ResponseFormat = WebMessageFormat.Json)]
        object AddPollOptions(object PollOptionNames, int PollID);

        [OperationContract]
        [WebGet(UriTemplate = "EditPollOptions/{PollOption,PollID}", ResponseFormat = WebMessageFormat.Json)]
        object EditPollOptions(object PollOptions, int PollID);
        /* Polls */

        /* Complaints */

        [OperationContract]
        [WebGet(UriTemplate = "GetAllComplaints/{ComplaintID,NotHandled}", ResponseFormat = WebMessageFormat.Json)]
        object GetAllComplaints(int ComplaintID=0, bool NotHandled = false);

        [OperationContract]
        [WebGet(UriTemplate = "SendComplaintResponse/{ComplaintID,Response,HandledBy}", ResponseFormat = WebMessageFormat.Json)]
        object SendComplaintResponse(int ComplaintID, string Response, int HandledBy);

        /* Complaints */

        /* Payments & Transactions */

        [OperationContract]
        [WebGet(UriTemplate = "AddIncome/{IncomeName,Type,Description,Amount,PaymentMode}", ResponseFormat = WebMessageFormat.Json)]
        object AddIncome(string IncomeName, string Type, string Description, decimal Amount, string PaymentMode);

        /* Payments & Transactions */
    }
}
