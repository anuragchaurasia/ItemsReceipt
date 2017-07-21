using DataLayer.Helper;
using EntityLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataLayer.DataHelper
{
    public class ProductHelper
    {
        UnitOfWork.UnitOfWork uow = new UnitOfWork.UnitOfWork();

        public bool AddProduct(ProductEL productEL)
        {
            bool isProductAdded = false;
            try
            {
                Product product = new Product();
                product = MapperUtility.MapTo(productEL, product);
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    uow.ProductRepository.Insert(product);
                    uow.Save();
                    isProductAdded = true;
                }
            }
            catch
            {
                isProductAdded = false;
            }

            return isProductAdded;
        }

        public bool UpdateProductAvailabilityPrice(ProductEL productEL)
        {
            bool isProductAvailabilityUpdated = false;
            try
            {
                Product product = new Product();
                
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    product = uow.ProductRepository.GetById(productEL.ProductID);
                    product.IsAvailable = productEL.IsAvailable;
                    product.Price = productEL.Price;
                    uow.ProductRepository.Update(product);
                    uow.Save();
                    isProductAvailabilityUpdated = true;
                }
            }
            catch
            {
                isProductAvailabilityUpdated = false;
            }

            return isProductAvailabilityUpdated;
        }

        public List<ProductEL> GetProductsByReciept(int recieptID)
        {
            List<ProductEL> lstProducts = new List<ProductEL>();

            try
            {
                using (uow = new UnitOfWork.UnitOfWork())
                {
                    lstProducts = uow.ProductRepository.Get().Join(uow.RecieptRepository.Get(), pro => pro.RecieptID, rec => rec.RecieptID, (pro, rec) => new { pro, rec })
                           .Where(sp => sp.pro.RecieptID == recieptID).Select(lst => new ProductEL
                           {
                               Name = lst.pro.Name,
                               AddedOn = lst.pro.AddedOn,
                               RecieptID = lst.pro.RecieptID,
                               ProductID = lst.pro.ProductID,
                               Quantity = lst.pro.Quantity,
                               UpdatedOn = lst.pro.UpdatedOn
                           }).ToList();
                    return lstProducts;
                }
            }
            catch
            {

            }

            return lstProducts;
        }
    }
}
