namespace BulkSMS.Models.CrmDto
{
    public class ContactDto
    {
        public int? id { get; set; }
        public int? clientId { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? name { get; set; }
        public bool? isBilling { get; set; }
        public bool? isContact { get; set; }
        public List<TypeDto>? types { get; set; }
    }
}
