using ClosedXML.Excel;
using GemBox.Document;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WA_EGroceryShop_v2.Domain.DomainModels;
using WA_EGroceryShop_v2.Domain.Identity;
using WA_EGroceryShop_v2.Services.Interface;

namespace WA_EGroceryShop_v2.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<ApplicationUser> userManager;


        public OrderController(IOrderService orderService, UserManager<ApplicationUser> userManager)
        {
            ComponentInfo.SetLicense("FREE-LIMITED-KEY");
            this._orderService = orderService;
            this.userManager = userManager;

        }
        public IActionResult Index()
        {
            var allOrders = this._orderService.getAllOrders();
            return View(allOrders);
        }

        public IActionResult GetDetailsForOrder(BaseEntity model)
        {
            return View(this._orderService.getOrderDetails(model));

        }

        public FileContentResult CreateInvoice(Guid id)
        {

            var cuvaj = this._orderService.getAllOrders();
            Order result = null;

            foreach (Order o in cuvaj)
            {
                if (o.Id == id)
                {
                    result = o;
                    break;
                }
            }

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.docx");
            var document = DocumentModel.Load(templatePath);


            document.Content.Replace("{{OrderNumber}}", result.Id.ToString());
            document.Content.Replace("{{UserName}}", result.User.UserName);

            StringBuilder sb = new StringBuilder();

            var totalPrice = 0.0;

            foreach (var item in result.ProductInOrders)
            {
                totalPrice += item.Quantity * item.OrderedProduct.Price;
                sb.AppendLine(item.OrderedProduct.Name + " with quantity of: " + item.Quantity + " and price of: " + item.OrderedProduct.Price + "$");
            }


            document.Content.Replace("{{TicketList}}", sb.ToString());
            document.Content.Replace("{{TotalPrice}}", totalPrice.ToString() + "$");


            var stream = new MemoryStream();

            document.Save(stream, new PdfSaveOptions());

            return File(stream.ToArray(), new PdfSaveOptions().ContentType, "ExportInvoice.pdf");
        }



        public FileContentResult ExportAllOrders()
        {
            string fileName = "Orders.xlsx";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            using (var workbook = new XLWorkbook())
            {
                IXLWorksheet worksheet = workbook.Worksheets.Add("All Orders");

                worksheet.Cell(1, 1).Value = "Order Id";
                worksheet.Cell(1, 2).Value = "Costumer Email";




                var result = this._orderService.getAllOrders();




                for (int i = 1; i <= result.Count(); i++)
                {
                    var item = result[i - 1];

                    worksheet.Cell(i + 1, 1).Value = item.Id.ToString();
                    worksheet.Cell(i + 1, 2).Value = item.User.Email;

                    for (int p = 0; p < item.ProductInOrders.Count(); p++)
                    {
                        worksheet.Cell(1, p + 3).Value = "Ticket-" + (p + 1);
                        worksheet.Cell(i + 1, p + 3).Value = item.ProductInOrders.ElementAt(p).OrderedProduct.Name;
                    }
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, contentType, fileName);
                }

            }
        }


        [Authorize]
        public IActionResult ListOfUserOrders()
        {
            var allOrders = this._orderService.getAllOrders();

            var loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //var najde = userManager.Users.Where(m => m.Id.Equals(loggedInUserId));

            List<Order> userOrders = new List<Order>();

            foreach (var odr in allOrders)
            {
                if (odr.UserId.Equals(loggedInUserId))
                {
                    var prati = this._orderService.getOrderDetails(odr);
                    userOrders.Add(prati);
                }
            }

            return View(userOrders);
        }


    }
}
