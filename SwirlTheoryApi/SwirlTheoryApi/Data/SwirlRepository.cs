using System.Collection.Generic;
using System.Linq;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi.Data {
    public class SwirlRepository {
        private readonly ShoppingContext _ctx;

        public SwirlRepository(ShoppingContext ctx) {
            _ctx = ctx;
        }

        public IEnumerable<Product> GetAllProducts() {
            return _ctx.Products
                .OrderBy(p => p.Title)
                .ToList();
        }

        public IEnumerable<Product> GetProductsByCategory(string category) {
            return _ctx.Products
                .Where(p => p.Category == category)
                .ToList();
        }

        public bool SaveAll() {
            return _ctx.SaveChanges() > 0;
        }
    }
}
