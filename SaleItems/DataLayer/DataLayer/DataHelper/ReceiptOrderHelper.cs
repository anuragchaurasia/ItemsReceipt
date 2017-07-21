using DataLayer.Helper;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.DataHelper
{
    public class ReceiptOrderHelper
    {
        UnitOfWork.UnitOfWork uow = new UnitOfWork.UnitOfWork();
        ReceiptHelper recHelper = new ReceiptHelper();

        public bool AddOrderReceipt(ReceiptOrderEL receiptOrderEL)
        {
            bool isOrderReceiptAdded = false;
            try
            {
                RecieptOrder recieptOrder = new RecieptOrder();
                recieptOrder = MapperUtility.MapTo(receiptOrderEL, recieptOrder);
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    uow.RecieptOrderRepository.Insert(recieptOrder);
                    uow.Save();
                    isOrderReceiptAdded = true;
                    int orderRecieptID = recieptOrder.RecieptOrderID;
                    List<Product> lstProduct = uow.ProductRepository.Get().Where(x => x.RecieptID == receiptOrderEL.RecieptID).ToList();
                    List<OrderCandidate> lstOrderCandidate = new List<OrderCandidate>();
                    foreach (Product product in lstProduct)
                    {
                        OrderCandidate orderCandidate = new OrderCandidate();
                        orderCandidate.CreatedOn = DateTime.Now.ToString();
                        orderCandidate.IsAvailable = false;
                        orderCandidate.Price = "0.00";
                        orderCandidate.ProductID = product.ProductID;
                        orderCandidate.StoreID = receiptOrderEL.ReceiverStoreID;
                        orderCandidate.UpdatedOn = null;
                        orderCandidate.RecieptOrderID = orderRecieptID;
                        lstOrderCandidate.Add(orderCandidate);
                    }
                    uow.OrderCandidateRepository.InsertBulk(lstOrderCandidate);
                    uow.Save();
                }
            }
            catch
            {
                isOrderReceiptAdded = false;
            }

            return isOrderReceiptAdded;
        }

        public bool AddOrderCandidate(OrderCandidateEL orderCandidateEL)
        {
            bool isOrderCandidateAdded = false;
            try
            {
                OrderCandidate orderCandidate = new OrderCandidate();
                orderCandidate = MapperUtility.MapTo(orderCandidateEL, orderCandidate);
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    uow.OrderCandidateRepository.Insert(orderCandidate);
                    uow.Save();
                    isOrderCandidateAdded = true;
                }
            }
            catch
            {

            }
            return isOrderCandidateAdded;
        }

        public bool IsOrderRecieptAvailable(ReceiptOrderEL receiptOrderEL)
        {
            bool isOrderRecieptAvailable = false;
            try
            {
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    isOrderRecieptAvailable = uow.RecieptOrderRepository.Get().Any(x => x.ReceiverStoreID == receiptOrderEL.ReceiverStoreID &&
                        x.SenderStoreID == receiptOrderEL.SenderStoreID && x.RecieptID == receiptOrderEL.RecieptID);
                    //isOrderRecieptAvailable = true;
                }
            }
            catch
            {

            }
            return isOrderRecieptAvailable;
        }

        public bool UpdateOrderCandidateByStoreProduct(List<OrderCandidateEL> orderCandidateELList)
        {
            bool isOrderCandidatePriceUpdated = false;
            try
            {
                List<OrderCandidate> lstOrders = new List<OrderCandidate>();
                double subtotal = 0.00;
                int orderRecieptID = 0;
                using (uow = new UnitOfWork.UnitOfWork())
                {

                    foreach (OrderCandidateEL orderCandidateEL in orderCandidateELList)
                    {
                        OrderCandidate orderCandidate = uow.OrderCandidateRepository.Get().Where(x => x.StoreID == orderCandidateEL.StoreID && x.ProductID == orderCandidateEL.ProductID).FirstOrDefault();
                        orderCandidate.Price = orderCandidateEL.Price;
                        orderCandidate.IsAvailable = orderCandidateEL.IsAvailable;
                        orderCandidate.UpdatedOn = DateTime.Now.ToString();
                        subtotal = subtotal + Convert.ToDouble(orderCandidateEL.Price);
                        orderRecieptID = Convert.ToInt32(orderCandidate.RecieptOrderID);
                        lstOrders.Add(orderCandidate);
                    }
                    uow.OrderCandidateRepository.UpdateBulk(lstOrders);
                    isOrderCandidatePriceUpdated = true;

                }
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    RecieptOrder recOrder = uow.RecieptOrderRepository.Get().Where(x => x.RecieptOrderID == orderRecieptID).FirstOrDefault();
                    if (recOrder != null)
                    {
                        recOrder.Subtotal = subtotal.ToString();
                        uow.RecieptOrderRepository.Update(recOrder);
                        uow.Save();
                        RecieptEL recEL = new RecieptEL();

                        recEL.Status = ((int)ReceiptStatus.Quoted).ToString();
                        recEL.RecieptID = Convert.ToInt32(recOrder.RecieptID);

                        recHelper.UpdateOrderReceipt(recEL);
                    }
                }
            }
            catch
            {

            }
            return isOrderCandidatePriceUpdated;
        }

        public List<RecieptEL> GetOrderedRecieptsForStore(int userID)
        {
            List<RecieptEL> lstReciepts = new List<RecieptEL>();

            try
            {
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    lstReciepts = uow.RecieptRepository.Get().Join(uow.RecieptOrderRepository.Get(), rec => rec.RecieptID, use => use.RecieptID, (rec, use) => new { rec, use })
                           .Where(sp => sp.use.ReceiverStoreID == userID).Select(lst => new RecieptEL
                           {
                               Name = lst.rec.Name,
                               CreatedOn = lst.rec.CreatedOn,
                               RecieptID = lst.rec.RecieptID,
                               StoreID = lst.rec.StoreID,
                               Status = lst.rec.Status,
                               Price = lst.rec.Price
                           }).ToList();
                    return lstReciepts;
                }
            }
            catch
            {

            }

            return lstReciepts;
        }

        public List<RecieptEL> GetQuotedRecieptsForCustomer(int userID)
        {
            List<RecieptEL> lstReciepts = new List<RecieptEL>();

            try
            {
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    lstReciepts = uow.RecieptRepository.Get()
                        .Where(sp => sp.Status == ((int)ReceiptStatus.Quoted).ToString() && sp.StoreID == userID).OrderByDescending(y => y.CreatedOn)
                           .Select(lst => new RecieptEL
                           {
                               Name = lst.Name,
                               CreatedOn = lst.CreatedOn,
                               RecieptID = lst.RecieptID,
                               StoreID = lst.StoreID,
                               Status = lst.Status,
                               Price = lst.Price
                           }).ToList();
                    return lstReciepts;
                }
            }
            catch
            {

            }

            return lstReciepts;
        }

        public List<UserEL> GetQuotedStoresForCustomer(int userID)
        {
            List<UserEL> lstReciepts = new List<UserEL>();

            try
            {
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    lstReciepts = uow.RecieptRepository.Get().Join(uow.RecieptOrderRepository.Get(), rec => rec.RecieptID, use => use.RecieptID, (rec, use) => new { rec, use })
                        .Join(uow.StoreUserRepository.Get(), str => str.use.ReceiverStoreID, sre => sre.StoreUserID, (str, sre) => new { str, sre })
                           .Where(sp => sp.str.use.SenderStoreID == userID && sp.str.rec.Status == ((int)ReceiptStatus.Quoted).ToString() && sp.str.use.Subtotal != null).OrderByDescending(y => y.str.use.OrderTime)
                           .Select(lst => new UserEL
                           {
                               StoreName = lst.sre.StoreName,
                               DeviceID = lst.sre.DeviceID,
                               DeviceType = lst.sre.DeviceType,
                               Email = lst.sre.Email,
                               Active = lst.sre.Active,
                               StoreUserID = lst.sre.StoreUserID,
                               Address = lst.sre.Address,
                               Username = lst.sre.Username
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
