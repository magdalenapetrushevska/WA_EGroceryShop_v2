using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Repository.Interface;
using WA_EGroceryShop_v2.Services.Interface;

namespace WA_EGroceryShop_v2.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            this._orderRepository = orderRepository;
        }

        public List<Order> getAllOrders()
        {
            return this._orderRepository.getAllOrders();
        }

        public Order getOrderDetails(BaseEntity model)
        {
            return this._orderRepository.getOrderDetails(model);
        }
    }
}
