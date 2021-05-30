using System.Collections.Generic;
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
    public class ProductController : Controller
    {
        const string myShopifyUrl = "virtualbeans.myshopify.com";
        private string _shopAccessToken;
        private IShopifyService _shopifyService;

        public ProductController(IShopifyService shopifyService)
        {
            _shopifyService = shopifyService;
            _shopAccessToken = _shopifyService.ShopAccessToken;
        }

        [HttpGet(Name = "GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var service = new ProductService(myShopifyUrl, _shopAccessToken);
            var products = await service.ListAsync();
            return Ok(products);
        }


        [HttpGet("{productId}", Name = "GetProductById")]
        public async Task<IActionResult> GetProductById(long productId)
        {
            var service = new ProductService(myShopifyUrl, _shopAccessToken);
            var product = await service.GetAsync(productId);
            return Ok(product);
        }

        [HttpPost(Name = "CreateProduct")]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)

        {
            var product = new Product()
            {
                Title = "Test Product Walter",
                Vendor = "Burton",
                BodyHtml = "<strong>Good snowboard!</strong>",
                ProductType = "Snowboard",
                //Make sure to give your product the correct variant option
                Options = new List<ProductOption>
     {
         new ProductOption
         {
             Name = "Color"
         }
     },
                //And then create a collection of variants or assign the "Variants" property
                //to an already defined collection.
                Variants = new List<ProductVariant>
     {
         new ProductVariant
         {
             Option1 = "Black",
         },
         new ProductVariant
         {
             Option1 = "Green",
         },
     }
            };
            var service = new ProductService(myShopifyUrl, _shopAccessToken);
            await service.CreateAsync(product);
            return Ok(product);
        }
    }
}
