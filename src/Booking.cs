using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Flurl.Http;

namespace RestfulBooker
{
    public class Booking
    {
        public static string AuthToken { get; set; }
        public static string BookingId { get; set; }

        #region Request and Response Methods
        // AuthResponse myDeserializedClass = JsonConvert.DeserializeObject<AuthResponse>(myJsonResponse); 
        public class AuthResponse
        {
            public string token { get; set; }
        }

        #endregion

        #region Auth Token Generation
        static async Task Auth_Token()
        {

            var url = "https://restful-booker.herokuapp.com/auth";
            string myJson = "{\"username\":\"admin\",\"password\":\"password123\"}";
            JObject body = JsonConvert.DeserializeObject<JObject>(myJson);
            var response = await url.AllowAnyHttpStatus().PostJsonAsync(body);
            AuthResponse myDeserializedClass = JsonConvert.DeserializeObject<AuthResponse>(response.Content.ReadAsStringAsync().Result);
            AuthToken = myDeserializedClass.token.ToString();

        }
        #endregion

        #region SetUp Method 
        [SetUp]
        public async Task SetupAsync()
        {
            await Auth_Token();
        }
        #endregion

        #region Test Cases or Methods for Booker API
        [Test]
        [Category("Post")]
        public async Task NewBooking()
        {
            try
            {
                string body = "{\"firstname\":\"siva\",\"lastname\":\"Brown\",\"totalprice\":111,\"depositpaid\":true,\"bookingdates\":{\"checkin\":\"2020-08-26\",\"checkout\":\"2019-08-27\"},\"additionalneeds\":\"Breakfast\"}";
                JObject jbody = JsonConvert.DeserializeObject<JObject>(body);
                var url = "https://restful-booker.herokuapp.com/booking";
                var response = await url.AllowAnyHttpStatus()
                .WithHeader("Authorization", $"Bearer {AuthToken}")
                .PostJsonAsync(jbody);
            }
            catch (Exception ex)
            {

            }
        }
        #endregion
    }
}