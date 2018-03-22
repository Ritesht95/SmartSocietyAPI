using System.ServiceModel;
using System.ServiceModel.Web;

namespace SmartSocietyAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IAdmin
    {        
        [OperationContract]
        [WebGet(UriTemplate = "CheckLogin/{Username,Password}", ResponseFormat = WebMessageFormat.Json)]
        string CheckLogin(string Username,string Password);

        [OperationContract]
        [WebGet(UriTemplate = "ForgotPassword/{Username}", ResponseFormat = WebMessageFormat.Json)]
        string ForgotPassword(string Username);

        [OperationContract]
        [WebGet(UriTemplate = "ResetPassword/{Username,Password}", ResponseFormat = WebMessageFormat.Json)]
        string ResetPassword(string Username, string Password);

        [OperationContract]
        [WebGet(UriTemplate = "SetFlatHolder/{StartDate,Name,DOB,FlatID,Occupation,Contact1,Contact2,Email,Image,PositionID,FlatHolderID,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object SetFlatHolder(string StartDate, string Name, string DOB, string FlatID, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID, bool IsActive);        
    }
}
