namespace BulkSMS.Models.CrmDto
{
    public class CrmSMSDto
    {
        public string? uuid { get; set; }
        public string? changeType { get; set; }
        public string? entity { get; set; }
        public int? entityId { get; set; }
        public string? eventName { get; set; }
        public ExtraDataDto? extraData { get; set; }
    }
}
