using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SwirlTheoryApi.Data.Entities;

namespace SwirlTheoryApi.Data {
    public interface ISwirlRepository {
        // Add entities to the database
        void AddEntity(object entity);

        // NOTE: We don't need any User access methods since we can access
        // user data in ALL relevant places using the UserManager class
        string GetUserIdFromUsername(string username);
        User GetUserByUserId(string uid);

        // Product access methods
        IEnumerable<Product> GetAllProducts();
        IEnumerable<Product> SearchProducts(string searchTerm);
        Product GetProductById(int productId);
        void UpdateProduct(Product model);
        void DeleteProduct(int productId);

        // Address access methods
        IEnumerable<Address> GetAddressesByUserId(string userId);
        void UpdateAddress(Address model);
        void DeleteAddress(int addressId);

        // BasketRow access methods
        IEnumerable<BasketRow> GetBasketRowsByUserId(string userId);
        BasketRow GetBasketRowByUserProduct(string userId, int productId);
        void UpdateBasketRow(BasketRow model);
        void DeleteBasketRow(string userId, int productId);

        // Order access methods
        IEnumerable<Order> GetOrders();
        IEnumerable<Order> GetOrdersByUserId(string userId);

        // PaymentDetails access methods
        IEnumerable<PaymentDetails> GetPaymentDetailsByUserId(string userId);
        void DeletePaymentDetails(int paymentDetailsId);

        bool SaveAll();
    }
}