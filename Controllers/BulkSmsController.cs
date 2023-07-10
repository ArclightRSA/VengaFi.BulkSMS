using BulkSMS.Models.CrmDto;
using BulkSMS.Models.DTO;
using BulkSMS.Models.DTO.BulkSmsDto;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Configuration;

#nullable disable

namespace BulkSMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BulkSmsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public BulkSmsController(IConfiguration iConfig)
        {
            _configuration = iConfig;
        }

        // POST api/<BulkSmsController>
        [SwaggerOperation(Summary = "BulkSmsGateway - CRM message Handler for BulkSMS")]
        [SwaggerResponse(201, "Message relayed to BulkSms", typeof(SMSDto))]
        [SwaggerResponse(403, "Forbidden - Possbible Insufficient Credits")]
        [HttpPost("SendBulkSms")]
        public async Task<IActionResult> SendBulkSms(CrmSMSDto request)
        {
            try
            {
                // Set BulkSms API Address
                var VengaBulkSmsUri = _configuration.GetValue<string>("BulkSms:URI");

                // Injection - Get Username and password from appsettings.json
                var userName = _configuration.GetValue<string>("BulkSms:Username");
                var userPassword = _configuration.GetValue<string>("BulkSms:Password");

                // Create Basic Authentication String
                var authenticationString = $"{userName}:{userPassword}";
                var base64String = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));

                // Start API Request
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(VengaBulkSmsUri);
                    client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64String}");

                    // Deciphyer request into workable values
                    SmsContactsDto VengSmsClient = new SmsContactsDto()
                    {
                        FirstName = request.extraData.entity.firstName,
                        LastName = request.extraData.entity.lastName,
                        ContactNumber = request.extraData.entity.contacts[0].phone,
                        Message = request.extraData.message
                    };

                    /*
                        List<ToDto> sendToList = new List<ToDto>();
                        ToDto sentTo = new ToDto()
                        {
                            address = "27832692628",
                            fields = new List<string>()
                            {
                                $"{VengSmsClient.FirstName} {VengSmsClient.LastName}",
                                VengSmsClient.Message
                            }
                        };
                        sendToList.Add(sentTo);
                    */


                    // BulkSMS Template - Set Values
                    SMSDto FormulatedSMS = new SMSDto()
                    {
                        body = "Hi {F0######}, {F1######}",
                        to = new List<ToDto>() {
                            new ToDto(){
                                address = VengSmsClient.ContactNumber,
                                fields = new List<string>()
                                {
                                    $"{VengSmsClient.FirstName} {VengSmsClient.LastName}",
                                    VengSmsClient.Message
                                }
                            }
                        }
                    };

                    // Post BulkSms Values
                    var response = await client.PostAsJsonAsync("messages", FormulatedSMS);
                    var contents = await response.Content.ReadAsStringAsync();

                    //var jobId = BackgroundJob.Enqueue(() => SendSms(VengSmsClient));


                    // Return OK if succeeded or set status code and contents
                    if (response.IsSuccessStatusCode)
                        return Ok();
                    else
                        return StatusCode(Convert.ToInt16(response.StatusCode), contents);

                }
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [NonAction]
        public void SendSms(SmsContactsDto SmsDetails)
        {
            // Implement any logic you want - not in the controller but in some repository.
            Console.WriteLine($"SMS Sent to {SmsDetails.FirstName},  {SmsDetails.LastName} with contact number: {SmsDetails.ContactNumber} - Message body: {SmsDetails.Message}");
        }
    }
}
