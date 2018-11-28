using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi.Data {
    public interface ISwirlRepository {
        // Add entities to the database
        IActionResult AddEntity<T>(T entity);

        IEnumerable<Product> GetAllProducts();

        bool SaveAll();
    }
}
