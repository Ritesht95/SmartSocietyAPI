using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace SmartSocietyAPI
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IGeneral
    {        
        /*Admin Side(WEBAPP)*/

        [OperationContract]
        [WebGet(UriTemplate = "CheckLogin/{Username,Password}", ResponseFormat = WebMessageFormat.Json)]
        string CheckLogin(string Username,string Password);

        [OperationContract]
        [WebGet(UriTemplate = "ForgotPassword/{Username}", ResponseFormat = WebMessageFormat.Json)]
        string ForgotPassword(string Username);

        [OperationContract]
        [WebGet(UriTemplate = "ResetPassword/{Username,Password}", ResponseFormat = WebMessageFormat.Json)]
        string ResetPassword(string Username, string Password);


        /*Admin Side(WEBAPP)*/

        /*Client Side(Android App)*/

        

        /*Client Side(Android App)*/
    }
}
