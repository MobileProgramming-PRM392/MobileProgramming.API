using MobileProgramming.Data.Entities;
using MobileProgramming.Data.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileProgramming.Data.Interfaces
{
    public interface IProductImageRepository : IRepository<ProductImage>
    {
        Task<List<string>> GetImageUrlByProductId(int productId);
        Task<List<ProductImage>> GetByProductId(int productId);
        Task<ProductImage?> GetByImageUrl(string url);
    }
}
