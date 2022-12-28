using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data;
using APILanding.Models.Data.Entity;
using System.Text.Json;
using Newtonsoft.Json;

namespace APILanding.Controllers
{
   [Route("api/[controller]")]
   [ApiController]
   public class SpCombineController : ControllerBase
   {
      public SpCombineController()
      {

      }
      [HttpGet]
      [Route("CreateListofItem")]
      public async Task<IActionResult> CreateListofItem(TblItem obj)
      {
         DataTable dt = new DataTable();
         try
         {
            string con = "Data Source= DESKTOP-GRS0037;Database=GEODB; Trusted_Connection =True; Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;";
            using (SqlConnection connection = new SqlConnection(con))
            {
               string sql = "[dbo].[sprCreateListOfItem]";

               using (SqlCommand sqlCmd = new SqlCommand(sql, connection))
               {
                  sqlCmd.CommandType = CommandType.StoredProcedure;
                  sqlCmd.Parameters.AddWithValue("@strItemName", obj.StrItemName);
                  sqlCmd.Parameters.AddWithValue("@numStockQuantity", obj.NumStockQuantity);
                  sqlCmd.Parameters.AddWithValue("@IsActive", obj.IsActive);
                  //string jsonString = JsonSerializer.Serialize(obj);

                  //sqlCmd.Parameters.AddWithValue("@jsonString", jsonString);
                  connection.Open();

                  using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
                  {
                     sqlAdapter.Fill(dt);
                  }
                  connection.Close();
               }
            }
            return Ok(JsonConvert.SerializeObject(dt));
         }
         catch (Exception)
         {
            throw;
         }
      }
   }
}
