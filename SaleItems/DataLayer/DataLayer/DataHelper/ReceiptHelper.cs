using DataLayer.Helper;
using EntityLayer;
using EntityLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.DataHelper
{
    public class ReceiptHelper
    {
        UnitOfWork.UnitOfWork uow = new UnitOfWork.UnitOfWork();

        public bool AddReceipt(RecieptEL recieptEL)
        {
            bool isRecieptAdded = false;
            try
            {
                Reciept reciept = new Reciept();
                reciept = MapperUtility.MapTo(recieptEL, reciept);
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    uow.RecieptRepository.Insert(reciept);
                    uow.Save();
                    isRecieptAdded = true;
                }
            }
            catch
            {
                isRecieptAdded = false;
            }

            return isRecieptAdded;
        }

        public bool EditReceipt(RecieptEL recieptEL)
        {
            bool isRecieptEdited = false;
            try
            {
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    Reciept reciept = uow.RecieptRepository.GetById(recieptEL.RecieptID);
                    reciept = MapperUtility.MapTo(recieptEL, reciept);
                    uow.RecieptRepository.Update(reciept);
                    uow.Save();
                    isRecieptEdited = true;
                }
            }
            catch
            {
                isRecieptEdited = false;
            }

            return isRecieptEdited;
        }

        public bool UpdateOrderReceipt(RecieptEL recieptEL)
        {
            bool isRecieptEdited = false;
            try
            {
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    Reciept reciept = uow.RecieptRepository.GetById(recieptEL.RecieptID);
                    reciept.Status = recieptEL.Status;
                    reciept.Price = recieptEL.Price;
                    uow.RecieptRepository.Update(reciept);
                    uow.Save();
                    isRecieptEdited = true;
                }
            }
            catch
            {
                isRecieptEdited = false;
            }

            return isRecieptEdited;
        }

        public bool DeleteReceipt(int receiptID)
        {
            bool isRecieptDeleted = false;
            try
            {
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    Reciept reciept = uow.RecieptRepository.GetById(receiptID);
                    uow.RecieptRepository.Delete(reciept);
                    uow.Save();
                    isRecieptDeleted = true;
                }
            }
            catch
            {
                isRecieptDeleted = false;
            }

            return isRecieptDeleted;
        }

        public List<RecieptEL> GetRecieptsByUserID(int userID)
        {
            List<RecieptEL> lstReciepts = new List<RecieptEL>();

            try
            {
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    lstReciepts = uow.RecieptRepository.Get().Join(uow.StoreUserRepository.Get(), rec => rec.StoreID, use => use.StoreUserID, (rec, use) => new { rec, use })
                           .Where(sp => sp.rec.StoreID == userID).Select(lst => new RecieptEL
                           {
                               Name = lst.rec.Name,
                               CreatedOn = lst.rec.CreatedOn,
                               RecieptID = lst.rec.RecieptID,
                               StoreID = lst.rec.StoreID
                           }).ToList();
                    return lstReciepts;
                }
            }
            catch
            {

            }

            return lstReciepts;
        }

    }
}
