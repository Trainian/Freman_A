using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore_FremanA.Models;
using Microsoft.AspNetCore.Mvc;
using AspNetCore_FremanA.Models.ViewModels;

namespace AspNetCore_FremanA.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        public int PageSize = 4;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        public IActionResult List(string category, int productPage = 1) =>
            View(new ProductListViewModel()
            {
                Products = repository.Products
                    .Where(p=> category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo()
                {
                    CurrentPage = productPage,
                    ItemsPerRage = PageSize,
                    TotalItems = (int)(category == null ?
                        repository.Products.Count() :
                        repository.Products.Where(e => 
                            e.Category == category)?.Count())
                },
                CurrentCategory = category
            });
        
    }
}
