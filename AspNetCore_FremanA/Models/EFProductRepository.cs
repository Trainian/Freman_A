using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCore_FremanA.Models
{
    public class EFProductRepository : IProductRepository
    {
        public ApplicationDbContext context { get; set; }

        public EFProductRepository(ApplicationDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Product> Products => context.Products;
    }
}
