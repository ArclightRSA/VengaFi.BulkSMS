namespace BulkSMS.Models.CrmDto
{
    public class ExtraDataDto
    {
        public string? mailingId { get; set; }
        public string? message { get; set; }
        public EntityDto? entity { get; set; }
        public object? entityBeforeEdit { get; set; }
    }
}
