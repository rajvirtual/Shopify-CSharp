using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shopify_CSharp.DTOs;
using Shopify_CSharp.Services;
using ShopifySharp;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Shopify_CSharp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        const string myShopifyUrl = "virtualbeans.myshopify.com";
        
        private string _shopAccessToken;
        private IShopifyService _shopifyService;

        public OrderController(IShopifyService shopifyService)
        {
            _shopifyService = shopifyService;
            _shopAccessToken = _shopifyService.ShopAccessToken;
        }

        [HttpGet(Name = "GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var service = new OrderService(myShopifyUrl, _shopAccessToken);
            var orders = await service.ListAsync();
            return Ok(orders);
        }

        [HttpGet("{orderId}", Name = "GetOrderById")]
        public async Task<IActionResult> GetOrderById(long orderId)
        {
            var service = new OrderService(myShopifyUrl, _shopAccessToken);
            var order = await service.GetAsync(orderId);//3818745888944
            return Ok(order);
        }


        [HttpPost(Name = "CreateOrder")]
        public async Task<IActionResult> CreateOrder(OrderDto orderDto)
        {
            var service = new OrderService(myShopifyUrl, _shopAccessToken);
            var order = new Order()
            {
                CreatedAt = DateTime.UtcNow,
                BillingAddress = new Address()
                {
                    Address1 = "123 4th Street",
                    City = "Minneapolis",
                    Province = "Minnesota",
                    ProvinceCode = "MN",
                    Zip = "55401",
                    Phone = "555-555-5555",
                    FirstName = "John",
                    LastName = "Doe",
                    Company = "Tomorrow Corporation",
                    Country = "United States",
                    CountryCode = "US",
                    Default = true,
                },
                LineItems = new List<LineItem>()
                {
                    new LineItem()
                    {
                        Name = "Test Line Item",
                        Title = "Test Line Item Title",
                        Price = 10
                    }
                },
                FinancialStatus = "paid",
                TotalPrice = 5.00M,
                
                Email = Guid.NewGuid().ToString() + "@example.com",
                Note = "Test note about the customer.",
            };

            order = await service.CreateAsync(order);

            return Ok(order);
        }



    }
}
