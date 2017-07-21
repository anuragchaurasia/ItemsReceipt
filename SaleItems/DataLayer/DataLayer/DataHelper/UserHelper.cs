using DataLayer.Helper;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace DataLayer.DataHelper
{
    public class UserHelper
    {
        UnitOfWork.UnitOfWork uow = new UnitOfWork.UnitOfWork();

        public bool RegisterUser(UserEL user)
        {
            bool isUserRegistered = false;
            try
            {
                StoreUser storeUser = new StoreUser();
                storeUser = MapperUtility.MapTo(user, storeUser);
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    storeUser.Password = EncryptionHelper.Encrypt(storeUser.Password);
                    uow.StoreUserRepository.Insert(storeUser);
                    uow.Save();
                    isUserRegistered = true;
                }
            }
            catch
            {
                isUserRegistered = false;
            }

            return isUserRegistered;
        }

        public UserEL LoginUser(UserEL userElData)
        {
            UserEL user = new UserEL();
            string PasswordHash = EncryptionHelper.Encrypt(userElData.Password);
            using (uow = new UnitOfWork.UnitOfWork())
            {
                StoreUser storeUserData = uow.StoreUserRepository.Get().Where(u => PasswordHash.Equals(u.Password)
                                                                           && userElData.Username.Equals(u.Username)
                                                                      ).FirstOrDefault();
                string token;
                if (storeUserData != null)
                {
                    AuthenticationToken existingToken = uow.AuthenticationTokenRepository.Get().
                                                                   Where(auth => auth.FkUserID.Equals(storeUserData.StoreUserID))
                                                                   .FirstOrDefault();
                    if (existingToken != null)
                    {
                        token = existingToken.Token;
                    }
                    else
                    {
                        AuthenticationToken authToken = new AuthenticationToken();
                        authToken.FkUserID = storeUserData.StoreUserID;
                        token = authToken.Token = Guid.NewGuid().ToString().Replace("-", "");
                        authToken.CreatedDate = System.DateTime.UtcNow;
                        uow.AuthenticationTokenRepository.Insert(authToken);
                        uow.Save();
                    }

                    user = MapperUtility.MapTo(storeUserData, user);
                    user.Token = token;
                }

            }

            return user;
        }

        public List<UserEL> GetAllUsers()
        {
            List<UserEL> lstUsers = new List<UserEL>();
            using (uow = new UnitOfWork.UnitOfWork())
            {
                lstUsers = uow.StoreUserRepository.Get().Select(x => new UserEL
                {
                    StoreName = x.StoreName,
                    Active = x.Active,
                    Address = x.Address,
                    DeviceID = x.DeviceID,
                    Email = x.Email,
                    Username = x.Username,
                    StoreUserID = x.StoreUserID
                }).ToList();
            }
            return lstUsers;
        }

        public bool UpdateUserToken(UserEL user)
        {
            bool isTokenUpdated = false;
            if (user.AuthToken != null)
            {
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    AuthenticationToken authToken = new Helper.Helper().GetAuthenticationToken(user.AuthToken);
                    StoreUser existingUser = null;
                    existingUser = uow.StoreUserRepository.Get().Where(u => u.StoreUserID.Equals(authToken.FkUserID)).FirstOrDefault();

                    #region Get Existing User

                    if (existingUser != null)
                    {
                        existingUser.DeviceID = user.DeviceID;
                        existingUser.DeviceType = user.DeviceType;
                        uow.StoreUserRepository.Update(existingUser);
                        uow.Save();
                        isTokenUpdated = true;
                        return isTokenUpdated;
                    }
                    else
                    {
                        isTokenUpdated = false;
                    }
                    #endregion
                }
            }
            else
            {
                isTokenUpdated = false;
            }
            return isTokenUpdated;
        }
    }
}
