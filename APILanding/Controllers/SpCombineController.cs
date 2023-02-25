using APILanding.Models.Data;
using APILanding.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System.Data;

namespace APILanding.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SpCombineController : ControllerBase
	{
		string con = "Data Source= DESKTOP-HJ8UUA1\\SQLEXPRESS01;Database=dbiBos; Trusted_Connection =True; Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;";
		private DataTable dt = new DataTable();
		public readonly ModelDbContext _context;
		public SpCombineController(ModelDbContext context)
		{
			this._context = context;
		}
		//[HttpGet]
		//[Route("CreateListofItem")]
		//public async Task<IActionResult> CreateListofItem(TblItem obj)
		//{

		//	DataTable dt = new DataTable();
		//	try
		//	{
		//		using (SqlConnection connection = new SqlConnection(con))
		//		{
		//			string sql = "[dbo].[sprCreateListOfItem]";

		//			using (SqlCommand sqlCmd = new SqlCommand(sql, connection))
		//			{
		//				sqlCmd.CommandType = CommandType.StoredProcedure;
		//				sqlCmd.Parameters.AddWithValue("@strItemName", obj.StrItemName);
		//				sqlCmd.Parameters.AddWithValue("@numStockQuantity", obj.NumStockQuantity);
		//				sqlCmd.Parameters.AddWithValue("@IsActive", obj.IsActive);
		//				//string jsonString = JsonSerializer.Serialize(obj);

		//				//sqlCmd.Parameters.AddWithValue("@jsonString", jsonString);
		//				connection.Open();

		//				using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
		//				{
		//					sqlAdapter.Fill(dt);
		//				}
		//				connection.Close();
		//			}
		//		}
		//		return Ok(JsonConvert.SerializeObject(dt));
		//	}
		//	catch (Exception)
		//	{
		//		throw;
		//	}
		//}
		[HttpPost]
		[Route("CreatePartner")]
		public async Task<IActionResult> CreateListofItem(string PartnerName, long PartnertypeId, bool isActive)
		{

			DataTable dt = new DataTable();
			try
			{
				using (SqlConnection connection = new SqlConnection(con))
				{
					string sql = "dbo.spCreatePartner";

					using (SqlCommand sqlCmd = new SqlCommand(sql, connection))
					{
						sqlCmd.CommandType = CommandType.StoredProcedure;
						sqlCmd.Parameters.AddWithValue("@strPartnerName", PartnerName);
						sqlCmd.Parameters.AddWithValue("@intPartnerTypeId", PartnertypeId);
						sqlCmd.Parameters.AddWithValue("@isActive", isActive);
						//string jsonString = System.Text.Json.JsonSerializer.Serialize(obj.parlist);

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
		[HttpPost]
		[Route("CreatePartnerType")]
		public async Task<IActionResult> CreatePartnerType(string PartnerTypeName, bool isActive)
		{
			DataTable dt = new DataTable();
			try
			{
				using (SqlConnection connection = new SqlConnection(con))
				{
					string sql = "dbo.spCreatePartnerType";

					using (SqlCommand sqlCmd = new SqlCommand(sql, connection))
					{
						sqlCmd.CommandType = CommandType.StoredProcedure;
						sqlCmd.Parameters.AddWithValue("@strPartnerTypeName", PartnerTypeName);
						sqlCmd.Parameters.AddWithValue("@isActive", isActive);
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

		[HttpPost]
		[Route("CreateEditItems")]
		public async Task<IActionResult> CreateEditItems(CommonITemDTO obj)
		{
			dt = new DataTable();
			try
			{
				using (var connection = new SqlConnection(con))
				{
					string sql = "dbo.sprCreatedEditItemofList";
					using (SqlCommand sqlCmd = new SqlCommand(sql, connection))
					{
						sqlCmd.CommandType = CommandType.StoredProcedure;
						//sqlCmd.Parameters.AddWithValue("@strPartName", obj.PartName);
						string jsonString = System.Text.Json.JsonSerializer.Serialize(obj.itemList);
						sqlCmd.Parameters.AddWithValue("@jsonString", jsonString);
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
			catch (Exception ex)
			{
				throw ex;
			}
		}

		//[HttpPost]
		//[Route("CreateEditItem")]
		//public Task<IActionResult> CreateEditItem(CommonITemDTO obj)
		//{
		//	try
		//	{
		//		DataTable dt = new DataTable();
		//		using (SqlConnection connection = new SqlConnection(con))
		//		{
		//			string sql = "dbo.sprCreatedEditItemofList";
		//			using (SqlCommand sqlCmd = new SqlCommand(sql, connection))
		//			{
		//				sqlCmd.CommandType = CommandType.StoredProcedure;
		//				sqlCmd.Parameters.AddWithValue("@strPartName", obj.PartName);
		//				string jsonString = System.Text.Json.JsonSerializer.Serialize(obj.itemList);
		//				sqlCmd.Parameters.AddWithValue("@jsonString", jsonString);
		//				connection.Open();
		//				using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(sqlCmd))
		//				{
		//					sqlAdapter.Fill(dt);
		//				}
		//				connection.Close();
		//			}

		//		}

		//	}
		//	catch (Exception)
		//	{
		//		throw;
		//	}

		//}

	}
}
