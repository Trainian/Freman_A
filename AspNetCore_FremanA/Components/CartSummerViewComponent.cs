using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore_FremanA.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore_FremanA.Components
{
    public class CartSummerViewComponent : ViewComponent
    {
        private Cart cart;

        public CartSummerViewComponent(Cart cartService)
        {
            cart = cartService;
        }

        public IViewComponentResult Invoke()
        {
            return View(cart);
        }
    }
}
