using System.Collections.Generic;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi.Data {
    public interface ISwirlRepository {
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> GetProductsByCategory(string category);

        bool SaveChanges();
    }
}
