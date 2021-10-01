using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WA_EGroceryShop_v2.Services.Interface
{
    public interface IBackgroundEmailSender
    {
        Task DoWork();
    }
}
