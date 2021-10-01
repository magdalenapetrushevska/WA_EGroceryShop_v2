using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WA_EGroceryShop_v2.Domain.DomainModels;

namespace WA_EGroceryShop_v2.Services.Interface
{
    public  interface IEmailService
    {
        Task SendEmailAsync(List<EmailMessage> allMails);
    }
}
