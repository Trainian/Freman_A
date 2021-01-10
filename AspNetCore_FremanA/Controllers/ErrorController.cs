using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore_FremanA.Controllers
{
    public class ErrorController : Controller
    {
        public ViewResult Error() => View();
    }
}
