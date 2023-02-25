using APILanding.Models.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APILanding.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class SalesController : ControllerBase
   {
      private readonly ModelDbContext _context;
      
      public SalesController(ModelDbContext context)
      {
         this._context = context;
      }

   }
}
