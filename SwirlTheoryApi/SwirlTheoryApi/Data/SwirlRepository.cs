using System.Collection.Generic;
using System.Linq;
using SwirlTheoryApi.Data.Entities;
using Microsoft.Extensions.Logging;

namespace SwirlTheoryApi.Data {
    public class SwirlRepository {
        private readonly ShoppingContext _ctx;
        private readonly ILogger<SwirlRepository> _logger;

        public SwirlRepository(ShoppingContext ctx, ILogger<SwirlRepositrory> logger) {
            _ctx = ctx;
            // NOTE: See config.json for logging level settings
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts() {
            try {
                return _ctx.Products
                    .OrderBy(p => p.Title)
                    .ToList();
            } catch (Exception ex) {
                _logger.LogError($"Failed in GetAllProducts(): {ex}");
            }
           
        }

        public IEnumerable<Product> GetProductsByCategory(string category) {
            try {
                return _ctx.Products
                    .Where(p => p.Category == category)
                    .ToList();
            } catch (Exception ex) {
                _logger.LogError($"Failed in GetProductsByCategory: {ex}");
            }
           
        }

        public bool SaveAll() {
            return _ctx.SaveChanges() > 0;
        }
    }
}
