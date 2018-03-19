using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace SmartSocietyAPI
{
    [ServiceContract]
    public interface IClient
    {
        [OperationContract]
        [WebGet(UriTemplate = "CheckLogin/{Username,Password}", ResponseFormat = WebMessageFormat.Json)]
        string CheckLogin(string Username, string Password);

        [OperationContract]
        [WebGet(UriTemplate = "ForgotPassword/{Username}", ResponseFormat = WebMessageFormat.Json)]
        string ForgotPassword(string Username);

        [OperationContract]
        [WebGet(UriTemplate = "ResetPassword/{Username,VerificationCode,Password}", ResponseFormat = WebMessageFormat.Json)]
        string ResetPassword(string Username, string VerificationCode, string Password);
    }
}
