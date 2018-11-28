using System.Collections.Generic;
using System.Linq;
using SwirlTheoryApi.Data.Entities;
using Microsoft.Extensions.Logging;
using System;

namespace SwirlTheoryApi.Data {
    public class SwirlRepository : ISwirlRepository {
        private readonly ShoppingContext _ctx;
        private readonly ILogger<SwirlRepository> _logger;

        public SwirlRepository(ShoppingContext ctx, ILogger<SwirlRepository> logger) {
            _ctx = ctx;
            // NOTE: See config.json for logging level settings
            _logger = logger;
        }

        public IEnumerable<Product> GetAllProducts() {
            try {
                return _ctx.Products
                    .OrderBy(p => p.ProductTitle)
                    .ToList();
            } catch (Exception ex) {
                _logger.LogError($"Failed in GetAllProducts(): {ex}");
                return null;
            }
           
        }

        public bool SaveAll() {
            return _ctx.SaveChanges() > 0;
        }
    }
}
