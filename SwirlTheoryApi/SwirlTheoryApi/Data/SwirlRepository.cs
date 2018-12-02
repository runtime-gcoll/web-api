using System.Collections.Generic;
using System.Linq;
using SwirlTheoryApi.Data.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SwirlTheoryApi.Data {
    public class SwirlRepository : ISwirlRepository {
        private readonly ShoppingContext _ctx;
        private readonly ILogger<SwirlRepository> _logger;

        public SwirlRepository(ShoppingContext ctx, ILogger<SwirlRepository> logger) {
            _ctx = ctx;
            // NOTE: See config.json for logging level settings
            _logger = logger;
        }

        //
        // Generic methods
        //

        // Add any kind of entity to it's respective database table
        public void AddEntity(object entity) {
            _ctx.Add(entity);
        }

        public bool SaveAll()
        {
            return _ctx.SaveChanges() > 0;
        }

        //
        // User methods
        //

        public string GetUserIdFromUsername(string username) {
            User user = _ctx.Users
                .Where(u => u.UserName == username)
                .FirstOrDefault();

            return user.Id;
        }

        // ---------------
        // Product methods
        // ---------------

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

        public IEnumerable<Product> SearchProducts(string searchTerm) {
            try {
                return _ctx.Products
                    .Where(p => p.ProductTitle.Contains(searchTerm) || p.ProductDescription.Contains(searchTerm))
                    .ToList();
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in SearchProducts(): {ex}");
                return null;
            }
        }

        public Product GetProductById(int productId) {
            try {
                return _ctx.Products
                    .Where(p => p.ProductId == productId)
                    .FirstOrDefault();
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in GetProductById(): {ex}");
                return null;
            }
        }

        public void UpdateProduct(Product model) {
            try {
                // This should work since the object "model"
                // will have the same primary key as the object in the
                // database that we want to update
                _ctx.Products.Update(model);
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in GetProductById(): {ex}");
            }
        }

        public void DeleteProduct(int productId) {
            try {
                Product prod = _ctx.Products
                    .Where(p => p.ProductId == productId)
                    .FirstOrDefault();
                _ctx.Products.Remove(prod);
            } catch (Exception ex) {
                _logger.LogError($"Failed in DeleteProduct(): {ex}");
            }
        }

        //
        // Address methods
        //
        public IEnumerable<Address> GetAddressesByUserId(string userId) {
            try {
                User user = _ctx.Users
                    .Where(u => u.Id == userId)
                    .FirstOrDefault();
                return _ctx.Addresses
                    .Where(a => a.User == user)
                    .ToList();
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in GetAddressesByUserId(): {ex}");
                return null;
            }
        }

        public void UpdateAddress(Address model) {
            try {
                _ctx.Addresses
                    .Update(model);
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in GetAddressesByUserId(): {ex}");
            }
        }

        public void DeleteAddress(int addressId) {
            try {
                Address addr = _ctx.Addresses
                    .Where(p => p.AddressId == addressId)
                    .FirstOrDefault();
                _ctx.Addresses.Remove(addr);
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in DeleteAddress(): {ex}");
            }
        }

        //
        // BasketRow methods
        //
        public IEnumerable<BasketRow> GetBasketProductsByUserId(string userId) {
            try {
                User user = _ctx.Users
                    .Where(u => u.Id == userId)
                    .FirstOrDefault();
                List<BasketRow> brows = _ctx.BasketRows
                    .Where(br => br.User == user)
                    .ToList();
                List<Product> products = new List<Product>();
                foreach (BasketRow br in brows) {
                    products.Add(br.Product);
                }
                return products;
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in GetAddressesByUserId(): {ex}");
                return null;
            }
        }

        public BasketRow GetBasketRowByUserProduct(string userId, int productId) {
            // Get the User the row is associated with
            User user = _ctx.Users
                    .Where(u => u.Id == userId)
                    .FirstOrDefault();
            // Get the Product we're looking for
            Product prod = _ctx.Products
                .Where(p => p.ProductId == productId)
                .FirstOrDefault();
            return _ctx.BasketRows
                .Where(br => br.User == user)
                .Where(br => br.Product == product)
                .FirstOrDefault();
        }

        public void UpdateBasketRowQuantity(int basketRowId, int quantity) {
            try {
                BasketRow brow = _ctx.BasketRows
                    .Where(br => br.BasketRowId == basketRowId)
                    .FirstOrDefault();

                brow.Quantity = quantity;

                _ctx.BasketRows.Update(brow);
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in UpdateBasketRowQuantity(): {ex}");
            }
        }

        public void DeleteBasketRow(int basketRowId) {
            try {
                BasketRow brow = _ctx.BasketRows
                    .Where(br => br.BasketRowId == basketRowId)
                    .FirstOrDefault();
                _ctx.BasketRows.Remove(brow);
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in DeleteBasketRow(): {ex}");
            }
        }

        //
        // Order methods
        //
        public IEnumerable<Order> GetOrders() {
            try
            {
                return _ctx.Orders
                    .ToList();
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in GetOrders(): {ex}");
                return null;
            }
        }

        public IEnumerable<Order> GetOrdersByUserId(string userId) {
            try {
                User user = _ctx.Users
                    .Where(u => u.Id == userId)
                    .FirstOrDefault();
                return _ctx.Orders
                    .Where(o => o.User == user)
                    .ToList();
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in GetOrdersByUserId(): {ex}");
                return null;
            }
        }

        //
        // PaymentDetails methods
        //
        public IEnumerable<PaymentDetails> GetPaymentDetailsByUserId(string userId) {
            try {
                User user = _ctx.Users
                    .Where(u => u.Id == userId)
                    .FirstOrDefault();
                return _ctx.PaymentDetails
                    .Where(pd => pd.User == user)
                    .ToList();
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in GetPaymentDetailsByUserId(): {ex}");
                return null;
            }
        }

        public void DeletePaymentDetails(int paymentDetailsId) {
            try {
                PaymentDetails payd = _ctx.PaymentDetails
                    .Where(pd => pd.PaymentDetailsId == paymentDetailsId)
                    .FirstOrDefault();
            }
            catch (Exception ex) {
                _logger.LogError($"Failed in CreatePaymentDetails(): {ex}");
            }
        }
    }
}
