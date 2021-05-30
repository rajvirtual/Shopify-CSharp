using System;
using Microsoft.Extensions.Configuration;

namespace Shopify_CSharp.Services
{
    public class ShopifyService : IShopifyService
    {
        private IConfiguration Configuration;
        public ShopifyService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public string ShopAccessToken { get { return Configuration["Shopify:ShopAccessToken"]; } }
    }
}
