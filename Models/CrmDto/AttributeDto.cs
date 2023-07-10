namespace BulkSMS.Models.CrmDto
{
    public class AttributeDto
    {
        public int? id { get; set; }
        public int? clientId { get; set; }
        public int? customAttributeId { get; set; }
        public string? name { get; set; }
        public string? key { get; set; }
        public string? value { get; set; }
        public bool? clientZoneVisible { get; set; }
    }
}
