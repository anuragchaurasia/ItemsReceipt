using System.Collections.Generic;
using SaleItemMobile.DTO;

namespace SaleItemMobile.SaleEntities
{
    public class UpdateProductEntity : BaseEntity
    {
        public List<ProductDTO> productInfo { get; set; }
    }
}