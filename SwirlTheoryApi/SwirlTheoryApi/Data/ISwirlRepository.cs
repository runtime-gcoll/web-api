using System.Collections.Generic;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi.Data {
    public interface ISwirlRepository {
        IEnumerable<Product> GetAllProducts();

        bool SaveAll();
    }
}
