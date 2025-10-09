using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FaysalFunds.Application.Services
{
    public class SmsService
    {
        private readonly HttpClient _httpClient;

        public SmsService()
        {
            _httpClient = new HttpClient();
        }

        public async Task SendSmsAsync(string mobileNumber, string message)
        {
            string url = "https://secure.m3techservice.com/GenericService/webservice_4_0.asmx";
            string userId = "OTP@FAML";
            string password = "O#$@10#p$896S%P";
            string msgHeader = "9182";
            string msgId = Guid.NewGuid().ToString();

            string soapBody = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
                               xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
                               xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                  <soap:Body>
                    <SendSMS xmlns=""http://tempuri.org/"">
                      <UserId>{userId}</UserId>
                      <Password>{password}</Password>
                      <MobileNo>{mobileNumber}</MobileNo>
                      <MsgId>{msgId}</MsgId>
                      <SMS>{message}</SMS>
                      <MsgHeader>{msgHeader}</MsgHeader>
                      <SMSType>0</SMSType>
                      <HandsetPort>0</HandsetPort>
                      <SMSChannel>0</SMSChannel>
                      <Telco>0</Telco>
                    </SendSMS>
                  </soap:Body>
                </soap:Envelope>";

            var content = new StringContent(soapBody, Encoding.UTF8, "text/xml");
            content.Headers.Add("SOAPAction", "\"http://tempuri.org/SendSMS\"");

            try
            {
                var response = await _httpClient.PostAsync(url, content);
                string responseString = await response.Content.ReadAsStringAsync();

                Console.WriteLine("SMS API Response:");
                Console.WriteLine(responseString);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to send SMS via M3Tech.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending SMS: " + ex.Message);
                // You can add logging or rethrow the exception here
            }
        }
    }
}
