namespace BulkSMS.Models.DTO.BulkSmsDto
{
    public class SMSDto
    {
        public string? body { get; set; }
        public List<ToDto>? to { get; set; }
    }
}
