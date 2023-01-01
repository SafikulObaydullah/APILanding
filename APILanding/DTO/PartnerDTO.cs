namespace APILanding.DTO
{
	public class CommonPartnerDTO
	{
		public List<PartnerDTO> parlist { get; set; }
	}
	public class PartnerDTO
	{
		public string? PartnerName { get; set; }
		public long? PartnerTypeId { get; set; }
		public bool? isActive { get; set; }
	}

}
