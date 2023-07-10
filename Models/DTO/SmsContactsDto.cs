#nullable disable

namespace BulkSMS.Models.DTO
{
    public class SmsContactsDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}
