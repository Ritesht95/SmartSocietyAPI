﻿using System.ServiceModel;
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
        [WebGet(UriTemplate = "SetSocietyInformation/{Name, Address, PostalCode, LogoImage, ContactNo, PresidentName, Builder, Email, Fax, RegistrationNo, CampusArea, SocietyType, LatLong}", ResponseFormat = WebMessageFormat.Json)]
        object SetSocietyInformation(string Name, string Address, string PostalCode, string LogoImage, string ContactNo, string PresidentName, string Builder, string Email, string Fax, string RegistrationNo, string CampusArea, string SocietyType, string LatLong);

        [OperationContract]
        [WebGet(UriTemplate = "EditSocietyInformation/{Name, Address, PostalCode, LogoImage, ContactNo, PresidentName, Builder, Email, Fax, RegistrationNo, CampusArea, SocietyType, LatLong}", ResponseFormat = WebMessageFormat.Json)]
        object EditSocietyInformation(string Name, string Address, string PostalCode, string LogoImage, string ContactNo, string PresidentName, string Builder, string Email, string Fax, string RegistrationNo, string CampusArea, string SocietyType, string LatLong);

        [OperationContract]
        [WebGet(UriTemplate = "GetSocietyInformation/", ResponseFormat = WebMessageFormat.Json)]
        object GetSocietyInformation();

        [OperationContract]
        [WebGet(UriTemplate = "SetFlatHolder/{StartDate,Name,DOB,FlatID,Occupation,Contact1,Contact2,Email,Image,PositionID,FlatHolderID,IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object SetFlatHolder(string StartDate, string Name, string DOB, string FlatID, string Occupation, string Contact1, string Contact2, string Email, string Image, int PositionID, int FlatHolderID, bool IsActive);

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
        object AddStaffMember(string Name, string MemberType, string DOB, string Contact1, string Contact2, string Image, string Address, string DOJ, string DOL, string CreatedBy);

        [OperationContract]
        [WebGet(UriTemplate = "EditStaffMember/{MemberID, Name, MemberType, DOB, Contact1, Contact2, Image, Address, DOJ, DOL, CreatedBy, IsActive}", ResponseFormat = WebMessageFormat.Json)]
        object EditStaffMember(int MemberID, string Name, string MemberType, string DOB, string Contact1, string Contact2, string Image, string Address, string DOJ, string DOL, string CreatedBy, bool IsActive);
    }
}