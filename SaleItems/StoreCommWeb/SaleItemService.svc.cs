using DataLayer;
using DataLayer.DataHelper;
using DataLayer.Helper;
using EntityLayer;
using EntityLayer.DTO;
using EntityLayer.Request;
using EntityLayer.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

namespace SaleItemsWeb
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SaleItemService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select SaleItemService.svc or SaleItemService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SaleItemService : ISaleItemService
    {
        UserHelper userHelper = new UserHelper();
        ReceiptHelper receiptHelper = new ReceiptHelper();
        ProductHelper productHelper = new ProductHelper();
        ReceiptOrderHelper orderReceiptHelper = new ReceiptOrderHelper();

        public RegisterStoreResponse Register(RegisterStoreRequest request)
        {
            RegisterStoreResponse registerStoreResponse = new RegisterStoreResponse();
            registerStoreResponse.Message = "Store not registered successfully.";
            if (String.IsNullOrEmpty(request.UserInfo.Username) || String.IsNullOrEmpty(request.UserInfo.Password)
                || String.IsNullOrEmpty(request.UserInfo.Email))
            {
                registerStoreResponse.Message = "Please pass all mandatory fields.";
                return registerStoreResponse;
            }
            if (new Helper().IsEmailExist(request.UserInfo.Email) || new Helper().IsUserNameExist(request.UserInfo.Username))
            {
                registerStoreResponse.Message = "Username of Email already exist.";
                return registerStoreResponse;
            }

            UserEL userEL = new UserEL();
            userEL = MapperUtility.MapTo(request.UserInfo, userEL);
            if (userHelper.RegisterUser(userEL))
            {
                registerStoreResponse.Message = "Registration successfully completed.";
                registerStoreResponse.IsLoggedIn = registerStoreResponse.IsSuccess = true;
                return registerStoreResponse;
            }
            else
            {
                registerStoreResponse.Message = "Some error occured.";
                return registerStoreResponse;
            }
        }

        public UserLoginResponse Login(UserLoginRequest request)
        {
            UserLoginResponse userLoginResponse = new UserLoginResponse();
            userLoginResponse.Message = "Incorrect Userid or Password. Please try again.";
            if (String.IsNullOrEmpty(request.UserNameOREmail) || String.IsNullOrEmpty(request.PasswordHash))
            {
                userLoginResponse.Message = "Please pass all mandatory fields.";
                return userLoginResponse;
            }

            UserEL userEL = new UserEL();
            userEL.Username = request.UserNameOREmail;
            userEL.Password = request.PasswordHash;
            UserEL userData = userHelper.LoginUser(userEL);
            if (userData.Username != null)
            {
                userLoginResponse.Message = "User logged in successfully.";
                userLoginResponse.Token = userData.Token;
                userLoginResponse.UserID = userData.StoreUserID;
                userLoginResponse.FullName = userData.StoreName;
                userLoginResponse.IsLoggedIn = userLoginResponse.IsSuccess = true;
                userLoginResponse.PushToken = userData.DeviceID;
                return userLoginResponse;
            }
            else
            {
                userLoginResponse.IsLoggedIn = userLoginResponse.IsSuccess = false;
                return userLoginResponse;
            }
        }

        public AddRecieptResponse AddReciept(AddRecieptRequest request)
        {
            AddRecieptResponse addRecieptResponse = new AddRecieptResponse();
            addRecieptResponse.Message = "Reciept not added successfully.";

            if (String.IsNullOrEmpty(request.AuthToken))
            {
                addRecieptResponse.Message = "Please pass all mandatory fields.";
                return addRecieptResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(request.AuthToken);

            if (authToken == null)
            {
                addRecieptResponse.Message = "Unauthorizes user.";
                return addRecieptResponse;
            }

            if (String.IsNullOrEmpty(request.recieptDTO.Name))
            {
                addRecieptResponse.Message = "Please pass reciept name.";
                return addRecieptResponse;
            }

            RecieptEL recieptEL = new RecieptEL();
            recieptEL = MapperUtility.MapTo(request.recieptDTO, recieptEL);
            if (receiptHelper.AddReceipt(recieptEL))
            {
                addRecieptResponse.Message = "Reciept added successfully.";
                addRecieptResponse.IsSuccess = true;
                return addRecieptResponse;
            }
            else
            {
                addRecieptResponse.Message = "Some error occured.";
                return addRecieptResponse;
            }
        }

        public AddOrderRecieptResponse AddOrderReciept(AddOrderRecieptRequest request)
        {
            AddOrderRecieptResponse addOrderRecieptResponse = new AddOrderRecieptResponse();
            addOrderRecieptResponse.Message = "Reciept order not added successfully.";

            if (String.IsNullOrEmpty(request.AuthToken))
            {
                addOrderRecieptResponse.Message = "Please pass all mandatory fields.";
                return addOrderRecieptResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(request.AuthToken);

            if (authToken == null)
            {
                addOrderRecieptResponse.Message = "Unauthorizes user.";
                return addOrderRecieptResponse;
            }

            if (request.ReceiverStoreID == 0 || request.RecieptID == 0)
            {
                addOrderRecieptResponse.Message = "Please pass all values.";
                return addOrderRecieptResponse;
            }

            ReceiptOrderEL recieptOrderEL = new ReceiptOrderEL();
            request.SenderStoreID = Convert.ToInt32(authToken.FkUserID);
            recieptOrderEL = MapperUtility.MapTo(request, recieptOrderEL);

            if (orderReceiptHelper.IsOrderRecieptAvailable(recieptOrderEL))
            {
                addOrderRecieptResponse.Message = "This reciept already sent to this store.";
                return addOrderRecieptResponse;
            }

            if (orderReceiptHelper.AddOrderReceipt(recieptOrderEL))
            {
                addOrderRecieptResponse.Message = "Reciept order added successfully.";
                addOrderRecieptResponse.IsSuccess = true;
                RecieptEL recEL = new RecieptEL();

                recEL.Price = "0.00";
                recEL.Status = ((int)ReceiptStatus.ProcessedToVendor).ToString();
                recEL.RecieptID = request.RecieptID;
                receiptHelper.UpdateOrderReceipt(recEL);
                return addOrderRecieptResponse;
            }
            else
            {
                addOrderRecieptResponse.Message = "Some error occured.";
                return addOrderRecieptResponse;
            }
        }

        public AddRecieptResponse UpdateReciept(AddRecieptRequest request)
        {
            AddRecieptResponse addRecieptResponse = new AddRecieptResponse();
            addRecieptResponse.Message = "Reciept not edited successfully.";

            if (String.IsNullOrEmpty(request.AuthToken))
            {
                addRecieptResponse.Message = "Please pass all mandatory fields.";
                return addRecieptResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(request.AuthToken);

            if (authToken == null)
            {
                addRecieptResponse.Message = "Unauthorizes user.";
                return addRecieptResponse;
            }

            if (String.IsNullOrEmpty(request.recieptDTO.Name))
            {
                addRecieptResponse.Message = "Please pass reciept name.";
                return addRecieptResponse;
            }

            RecieptEL recieptEL = new RecieptEL();
            recieptEL = MapperUtility.MapTo(request.recieptDTO, recieptEL);
            if (receiptHelper.EditReceipt(recieptEL))
            {
                addRecieptResponse.Message = "Reciept edited successfully.";
                addRecieptResponse.IsSuccess = true;
                return addRecieptResponse;
            }
            else
            {
                addRecieptResponse.Message = "Some error occured.";
                return addRecieptResponse;
            }
        }

        public AddRecieptResponse UpdateRecieptStatus(AddRecieptRequest request)
        {
            AddRecieptResponse addRecieptResponse = new AddRecieptResponse();
            addRecieptResponse.Message = "Reciept status not updated successfully.";

            if (String.IsNullOrEmpty(request.AuthToken))
            {
                addRecieptResponse.Message = "Please pass all mandatory fields.";
                return addRecieptResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(request.AuthToken);

            if (authToken == null)
            {
                addRecieptResponse.Message = "Unauthorizes user.";
                return addRecieptResponse;
            }

            RecieptEL recieptEL = new RecieptEL();
            recieptEL.Status = request.recieptDTO.Status;
            recieptEL.Price = request.recieptDTO.Price;
            if (receiptHelper.UpdateOrderReceipt(recieptEL))
            {
                addRecieptResponse.Message = "Reciept status updated successfully.";
                addRecieptResponse.IsSuccess = true;
                return addRecieptResponse;
            }
            else
            {
                addRecieptResponse.Message = "Some error occured.";
                return addRecieptResponse;
            }
        }

        public AddProductResponse AddProduct(AddProductRequest request)
        {
            AddProductResponse addProductResponse = new AddProductResponse();
            addProductResponse.Message = "Product not added successfully.";
            if (String.IsNullOrEmpty(request.AuthToken))
            {
                addProductResponse.Message = "Please pass all mandatory fields.";
                return addProductResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(request.AuthToken);

            if (authToken == null)
            {
                addProductResponse.Message = "Unauthorizes user.";
                return addProductResponse;
            }

            if (String.IsNullOrEmpty(request.productInfo.Name) || String.IsNullOrEmpty(request.productInfo.Quantity))
            {
                addProductResponse.Message = "Please pass all mandatory fields.";
                return addProductResponse;
            }

            ProductEL productEL = new ProductEL();
            productEL = MapperUtility.MapTo(request.productInfo, productEL);
            if (productHelper.AddProduct(productEL))
            {
                addProductResponse.Message = "Product added successfully.";
                addProductResponse.IsSuccess = true;
                return addProductResponse;
            }
            else
            {
                addProductResponse.Message = "Some error occured.";
                return addProductResponse;
            }


        }

        public UpdateProductAvailabilityResponse UpdateProductAvailability(UpdateProductAvailabilityRequest request)
        {
            UpdateProductAvailabilityResponse updateProductAvailabilityResponse = new UpdateProductAvailabilityResponse();
            updateProductAvailabilityResponse.Message = "Reciept availability not added successfully.";
            if (String.IsNullOrEmpty(request.AuthToken))
            {
                updateProductAvailabilityResponse.Message = "Please pass all mandatory fields.";
                return updateProductAvailabilityResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(request.AuthToken);

            if (authToken == null)
            {
                updateProductAvailabilityResponse.Message = "Unauthorizes user.";
                return updateProductAvailabilityResponse;
            }

            List<OrderCandidateEL> lstOrderCandidates = new List<OrderCandidateEL>();
            foreach (ProductDTO product in request.productInfo)
            {
                OrderCandidateEL orderCandidateEL = new OrderCandidateEL();
                orderCandidateEL.IsAvailable = product.IsAvailable;
                orderCandidateEL.Price = product.Price;
                orderCandidateEL.StoreID = authToken.FkUserID;
                orderCandidateEL.ProductID = product.ProductID;
                lstOrderCandidates.Add(orderCandidateEL);
            }

            if (orderReceiptHelper.UpdateOrderCandidateByStoreProduct(lstOrderCandidates))
            {
                updateProductAvailabilityResponse.Message = "Product availability updated successfully.";
                updateProductAvailabilityResponse.IsSuccess = true;

                
                return updateProductAvailabilityResponse;
            }
            else
            {
                updateProductAvailabilityResponse.Message = "Some error occured.";
                return updateProductAvailabilityResponse;
            }
        }

        public GetRecieptResponse GetStoreReciepts(string AuthToken, string UserID)
        {
            GetRecieptResponse getRecieptResponse = new GetRecieptResponse();
            getRecieptResponse.IsSuccess = false;
            getRecieptResponse.Message = "Error in getting reciepts. Please try again";

            if (String.IsNullOrEmpty(AuthToken))
            {
                getRecieptResponse.Message = "Please pass all mandatory fields.";
                return getRecieptResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(AuthToken);

            if (authToken == null)
            {
                getRecieptResponse.Message = "Unauthorizes user.";
                return getRecieptResponse;
            }

            List<RecieptEL> lstReciept = receiptHelper.GetRecieptsByUserID(Convert.ToInt32(UserID));
            List<RecieptDTO> lstRecieptDTO = new List<RecieptDTO>();
            if (lstReciept.Count > 0)
            {
                foreach (RecieptEL item in lstReciept)
                {
                    RecieptDTO recieptDTO = new RecieptDTO();
                    recieptDTO = MapperUtility.MapTo(item, recieptDTO);
                    lstRecieptDTO.Add(recieptDTO);
                }
                getRecieptResponse.reciepts = lstRecieptDTO;
                getRecieptResponse.IsSuccess = true;
                getRecieptResponse.Message = "Reciepts returned successfully.";
            }
            else
            {
                getRecieptResponse.Message = "No Reciepts found.";
            }

            return getRecieptResponse;
        }

        public GetRecieptResponse GetOrderedRecieptsForStore(string AuthToken, string UserID)
        {
            GetRecieptResponse getRecieptResponse = new GetRecieptResponse();
            getRecieptResponse.IsSuccess = false;
            getRecieptResponse.Message = "Error in getting ordered reciepts. Please try again";

            if (String.IsNullOrEmpty(AuthToken))
            {
                getRecieptResponse.Message = "Please pass all mandatory fields.";
                return getRecieptResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(AuthToken);

            if (authToken == null)
            {
                getRecieptResponse.Message = "Unauthorizes user.";
                return getRecieptResponse;
            }

            List<RecieptEL> lstReciept = orderReceiptHelper.GetOrderedRecieptsForStore(Convert.ToInt32(UserID));
            List<RecieptDTO> lstRecieptDTO = new List<RecieptDTO>();
            if (lstReciept.Count > 0)
            {
                foreach (RecieptEL item in lstReciept)
                {
                    RecieptDTO recieptDTO = new RecieptDTO();
                    recieptDTO = MapperUtility.MapTo(item, recieptDTO);
                    lstRecieptDTO.Add(recieptDTO);
                }
                getRecieptResponse.reciepts = lstRecieptDTO;
                getRecieptResponse.IsSuccess = true;
                getRecieptResponse.Message = "Ordered reciepts returned successfully.";
            }
            else
            {
                getRecieptResponse.Message = "No Reciepts found.";
            }

            return getRecieptResponse;
        }

        public GetQuotedRecieptResponse GetQuotedRecieptsForCustomer(string AuthToken, string UserID)
        {
            GetQuotedRecieptResponse getRecieptResponse = new GetQuotedRecieptResponse();
            getRecieptResponse.IsSuccess = false;
            getRecieptResponse.Message = "Error in getting ordered reciepts. Please try again";

            if (String.IsNullOrEmpty(AuthToken))
            {
                getRecieptResponse.Message = "Please pass all mandatory fields.";
                return getRecieptResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(AuthToken);

            if (authToken == null)
            {
                getRecieptResponse.Message = "Unauthorizes user.";
                return getRecieptResponse;
            }

            List<RecieptEL> lstReciept = orderReceiptHelper.GetQuotedRecieptsForCustomer(Convert.ToInt32(UserID));

            List<UserEL> lstStores = orderReceiptHelper.GetQuotedStoresForCustomer(Convert.ToInt32(UserID));

            List<RecieptDTO> lstRecieptDTO = new List<RecieptDTO>();
            if (lstReciept.Count > 0)
            {
                foreach (RecieptEL item in lstReciept)
                {
                    RecieptDTO recieptDTO = new RecieptDTO();
                    recieptDTO = MapperUtility.MapTo(item, recieptDTO);
                    lstRecieptDTO.Add(recieptDTO);
                }
            }
            else
            {
                getRecieptResponse.noReceiptMessage = "No Reciepts found.";
            }

            List<UserDTO> lstUserDTO = new List<UserDTO>();
            if (lstStores.Count > 0)
            {
                foreach (UserEL item in lstStores)
                {
                    UserDTO userDTO = new UserDTO();
                    userDTO = MapperUtility.MapTo(item, userDTO);
                    lstUserDTO.Add(userDTO);
                }
            }
            else
            {
                getRecieptResponse.noUserMessage = "No Stores found.";
            }

            getRecieptResponse.reciepts = lstRecieptDTO;
            getRecieptResponse.users = lstUserDTO;
            getRecieptResponse.IsSuccess = true;
            getRecieptResponse.Message = "Ordered reciepts returned successfully.";

            return getRecieptResponse;
        }

        public GetProductResponse GetProductsByReciept(string AuthToken, string RecieptID)
        {
            GetProductResponse getProductResponse = new GetProductResponse();
            getProductResponse.IsSuccess = false;
            getProductResponse.Message = "Error in getting reciepts. Please try again";

            if (String.IsNullOrEmpty(AuthToken))
            {
                getProductResponse.Message = "Please pass all mandatory fields.";
                return getProductResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(AuthToken);

            if (authToken == null)
            {
                getProductResponse.Message = "Unauthorizes user.";
                return getProductResponse;
            }

            List<ProductEL> lstProduct = productHelper.GetProductsByReciept(Convert.ToInt32(RecieptID));
            List<ProductDTO> lstProductDTO = new List<ProductDTO>();
            if (lstProduct.Count > 0)
            {
                foreach (ProductEL item in lstProduct)
                {
                    ProductDTO productDTO = new ProductDTO();
                    productDTO = MapperUtility.MapTo(item, productDTO);
                    lstProductDTO.Add(productDTO);
                }
                getProductResponse.products = lstProductDTO;
                getProductResponse.IsSuccess = true;
                getProductResponse.Message = "Product returned successfully.";
            }
            else
            {
                getProductResponse.Message = "No products found.";
            }

            return getProductResponse;
        }

        public GetAllUsersResponse GetAllUsers(string AuthToken)
        {
            GetAllUsersResponse getAllUsersResponse = new GetAllUsersResponse();
            getAllUsersResponse.IsSuccess = false;
            getAllUsersResponse.Message = "Error in getting reciepts. Please try again";

            if (String.IsNullOrEmpty(AuthToken))
            {
                getAllUsersResponse.Message = "Please pass all mandatory fields.";
                return getAllUsersResponse;
            }

            AuthenticationToken authToken = new Helper().GetAuthenticationToken(AuthToken);

            if (authToken == null)
            {
                getAllUsersResponse.Message = "Unauthorizes user.";
                return getAllUsersResponse;
            }

            List<UserEL> lstUsers = userHelper.GetAllUsers();
            List<UserDTO> lstUserDTO = new List<UserDTO>();
            if (lstUsers.Count > 0)
            {
                foreach (UserEL item in lstUsers)
                {
                    UserDTO userDTO = new UserDTO();
                    userDTO = MapperUtility.MapTo(item, userDTO);
                    lstUserDTO.Add(userDTO);
                }
                getAllUsersResponse.userDetails = lstUserDTO;
                getAllUsersResponse.IsSuccess = true;
                getAllUsersResponse.Message = "Users returned successfully.";
            }
            else
            {
                getAllUsersResponse.Message = "No Reciepts found.";
            }

            return getAllUsersResponse;
        }

        public UpdateDeviceTokenResponse UpdatePushToken(UpdateDeviceTokenRequest userRequest)
        {
            UpdateDeviceTokenResponse updateTokenResponse = new UpdateDeviceTokenResponse();
            updateTokenResponse.IsSuccess = false;
            updateTokenResponse.Message = "Update device push token unsuccessful";
            try
            {
                if (!string.IsNullOrEmpty(userRequest.AuthToken)
                     && !string.IsNullOrEmpty(userRequest.DevicePushToken)
                     && !string.IsNullOrEmpty(userRequest.DeviceType)
                    )
                {
                    UserEL userPushData = new UserEL();
                    userPushData.DeviceID = userRequest.DevicePushToken;
                    userPushData.DeviceType = userRequest.DeviceType;
                    userPushData.AuthToken = userRequest.AuthToken;
                    userHelper.UpdateUserToken(userPushData);
                }
                else
                {
                    updateTokenResponse.Message = "Please pass value of all mandatory fields";
                }
            }
            catch (Exception ex)
            {
                updateTokenResponse.Message = "An error occurred while update device push token.";

            }
            return updateTokenResponse;

        }

        public AndroidFCMPushNotificationResponse SendNotification(AndroidFCMPushNotificationRequest androidFCMPushNotificationRequest)
        {
            AndroidFCMPushNotificationResponse result = new AndroidFCMPushNotificationResponse();
            try
            {
                result.Successful = false;
                result.Error = null;

                var value = androidFCMPushNotificationRequest.message;
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json;charset=UTF-8";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", androidFCMPushNotificationRequest.serverApiKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", androidFCMPushNotificationRequest.senderId));

                //string postData = "collapse_key=score_update&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + androidFCMPushNotificationRequest.deviceId + "";

                var data = new
                {
                    to = androidFCMPushNotificationRequest.deviceId,
                    notification = new
                    {
                        body = androidFCMPushNotificationRequest.message,
                        title = androidFCMPushNotificationRequest.title

                    }
                };
                string jsonss = Newtonsoft.Json.JsonConvert.SerializeObject(data);

                Byte[] byteArray = Encoding.UTF8.GetBytes(jsonss);
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                result.Response = sResponseFromServer;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
            }

            return result;
        }
    }
}
