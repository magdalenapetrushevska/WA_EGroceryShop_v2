using System;
using System.Collections.Generic;
using System.Text;
using WA_EGroceryShop_v2.Domain.Identity;

namespace WA_EGroceryShop_v2.Repository.Interface
{
    public interface  IUserRepository
    {
        IEnumerable<ApplicationUser> GetAll();
        ApplicationUser Get(string id);
        void Insert(ApplicationUser entity);
        void Update(ApplicationUser entity);
        void Delete(ApplicationUser entity);
    }
}
