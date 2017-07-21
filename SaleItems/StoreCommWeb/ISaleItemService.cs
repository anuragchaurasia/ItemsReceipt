using EntityLayer.Request;
using EntityLayer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace SaleItemsWeb
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISaleItemService" in both code and config file together.
    [ServiceContract]
    public interface ISaleItemService
    {
        [OperationContract]
        [WebInvoke(Method = "POST",
                 BodyStyle = WebMessageBodyStyle.Bare,
                 RequestFormat = WebMessageFormat.Json,
                 ResponseFormat = WebMessageFormat.Json,
                 UriTemplate = "/RegisterStore")]
        RegisterStoreResponse Register(RegisterStoreRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                  BodyStyle = WebMessageBodyStyle.Bare,
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  UriTemplate = "/Login")]
        UserLoginResponse Login(UserLoginRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                  BodyStyle = WebMessageBodyStyle.Bare,
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  UriTemplate = "/AddReciept")]
        AddRecieptResponse AddReciept(AddRecieptRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                  BodyStyle = WebMessageBodyStyle.Bare,
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  UriTemplate = "/UpdateReciept")]
        AddRecieptResponse UpdateReciept(AddRecieptRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                  BodyStyle = WebMessageBodyStyle.Bare,
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  UriTemplate = "/UpdateRecieptStatus")]
        AddRecieptResponse UpdateRecieptStatus(AddRecieptRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                  BodyStyle = WebMessageBodyStyle.Bare,
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  UriTemplate = "/AddProduct")]
        AddProductResponse AddProduct(AddProductRequest request);

        [OperationContract]
        [WebInvoke(Method = "POST",
                  BodyStyle = WebMessageBodyStyle.Bare,
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  UriTemplate = "/UpdateProductAvailability")]
        UpdateProductAvailabilityResponse UpdateProductAvailability(UpdateProductAvailabilityRequest request);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetProducts/{AuthToken}/{RecieptID}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        GetProductResponse GetProductsByReciept(string AuthToken, string RecieptID);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetReciepts/{AuthToken}/{UserID}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        GetRecieptResponse GetStoreReciepts(string AuthToken, string UserID);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetOrderedRecieptsForStore/{AuthToken}/{UserID}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        GetRecieptResponse GetOrderedRecieptsForStore(string AuthToken, string UserID);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetQuotedRecieptsForCustomer/{AuthToken}/{UserID}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        GetQuotedRecieptResponse GetQuotedRecieptsForCustomer(string AuthToken, string UserID);

        [OperationContract]
        [WebGet(RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetAllUsers/{AuthToken}",
            BodyStyle = WebMessageBodyStyle.Bare)]
        GetAllUsersResponse GetAllUsers(string AuthToken);

        [OperationContract]
        [WebInvoke(Method = "POST",
                  BodyStyle = WebMessageBodyStyle.Bare,
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  UriTemplate = "/UpdatePushToken")]
        UpdateDeviceTokenResponse UpdatePushToken(UpdateDeviceTokenRequest userRequest);

        [OperationContract]
        [WebInvoke(Method = "POST",
                  BodyStyle = WebMessageBodyStyle.Bare,
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  UriTemplate = "/SendPushNotification")]
        AndroidFCMPushNotificationResponse SendNotification(AndroidFCMPushNotificationRequest androidFCMPushNotificationRequest);

        [OperationContract]
        [WebInvoke(Method = "POST",
                  BodyStyle = WebMessageBodyStyle.Bare,
                  RequestFormat = WebMessageFormat.Json,
                  ResponseFormat = WebMessageFormat.Json,
                  UriTemplate = "/AddOrderReciept")]
        AddOrderRecieptResponse AddOrderReciept(AddOrderRecieptRequest request);

    }
}
