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
    public class FulfillmentController : Controller
    {
        const string myShopifyUrl = "virtualbeans.myshopify.com";

        private string _shopAccessToken;
        private IShopifyService _shopifyService;

        public FulfillmentController(IShopifyService shopifyService)
        {
            _shopifyService = shopifyService;
            _shopAccessToken = _shopifyService.ShopAccessToken;
        }


        [HttpGet("{orderId}", Name = "GetFullfillmentsByOrderId")]
        public async Task<IActionResult> GetFullfillmentsByOrderId(long orderId)
        {
            var service = new FulfillmentService(myShopifyUrl, _shopAccessToken);
            var fulfillments = await service.ListAsync(orderId);
            return Ok(fulfillments);
        }


        [HttpPost( Name = "CreateFulfillment")]
        public async Task<IActionResult> CreateFulfillment(FulfillmentDto fulfillmentDto)

        {
            var service = new FulfillmentService(myShopifyUrl, _shopAccessToken);
            var locationService = new LocationService(myShopifyUrl, _shopAccessToken);
            var locations = await locationService.ListAsync();

            var locationId = locations.FirstOrDefault().Id;

            var fulfillment = new Fulfillment()
            {
                TrackingCompany = "Jack Black's Pack, Stack and Track",
                TrackingUrl = "https://example.com/123456789",
                TrackingNumber = "123456789",
                LocationId = locationId
            };

            fulfillment = await service.CreateAsync(fulfillmentDto.OrderId, fulfillment);
            return Ok(fulfillment);
        }


    }
}
