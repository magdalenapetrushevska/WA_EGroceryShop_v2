
using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.DomainModels;

namespace WA_EGroceryShop_v2.Services.Interface
{
    public interface IOrderService
    {
        List<Order> getAllOrders();

        Order getOrderDetails(BaseEntity model);
    }
}
