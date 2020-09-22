using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Net;
using System.Collections.Generic;

namespace RestfulBooker
{
    public class Booking
    {
        public static string AuthToken { get; set; }
        public static string BookingId { get; set; }

        HttpClient client = new HttpClient();

        #region Request and Response Methods
       
        public class AuthResponse
        {
            public string token { get; set; }
        }

        public class Bookings
        {
            public int bookingid { get; set; }
        }

        public class Bookingdates
        {
            public string checkin { get; set; }
            public string checkout { get; set; }
        }

        public class BokokingDetail
        {
            public string firstname { get; set; }
            public string lastname { get; set; }
            public int totalprice { get; set; }
            public bool depositpaid { get; set; }
            public Bookingdates bookingdates { get; set; }
            public string additionalneeds { get; set; }
        }

        #endregion
        #region SetUp and Tear Down Method 
        [SetUp]
        public async Task SetupAsync()
        {
            // await Auth_Token();
        }

        [TearDown]
        public void AfterTest()
        {
            _ = new HttpClient();
        }

        #endregion
        #region Test Cases or Methods for Booker API
        /// <summary>
        /// Auth Token 
        /// </summary>
        /// <returns>Returns a Token</returns>
        [Test]
        public async Task Auth_Token()
        {
            try
            {
                var url = "https://restful-booker.herokuapp.com/auth";
                string myJson = "{\"username\":\"admin\",\"password\":\"password123\"}";
                var stringContent = new StringContent(myJson, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                var response = await client.PostAsync(url, stringContent);
                AuthResponse myDeserializedClass = JsonConvert.DeserializeObject<AuthResponse>(response.Content.ReadAsStringAsync().Result);
                AuthToken = myDeserializedClass.token.ToString();
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsNotNull(AuthToken);
            }
            catch (Exception ex)
            {
                Assert.Fail("Token is Not Generated : Test Failed" + ex.Message);
            }

        }

        /// <summary>
        /// New Booking
        /// </summary>
        /// <returns>Createa a New Booking with the Details </returns>
        [Test]
        public async Task New_Booking()
        {
            try
            {
                var url = "https://restful-booker.herokuapp.com/booking";
                string body = "{\"firstname\":\"noway\",\"lastname\":\"goway\",\"totalprice\":1234,\"depositpaid\":false,\"bookingdates\":{\"checkin\":\"2018-09-20\",\"checkout\":\"2017-09-20\"},\"additionalneeds\":\"break\"}";
                JObject RequestBody = JsonConvert.DeserializeObject<JObject>(body);
                var stringContent = new StringContent(body, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                HttpResponseMessage response = await client.PostAsync(url, stringContent);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,"New Booking Not Created");
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        /// <summary>
        /// Gets All the Bookings
        /// </summary>
        /// <returns>a list of all the bookings</returns>
        [Test]
        public async Task Get_All_Bookings()
        {
            try
            {
                var url = "https://restful-booker.herokuapp.com/booking";
                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result.ToString());
                List<Bookings> AllBokings = JsonConvert.DeserializeObject<List<Bookings>>(response.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode ,"Expected Status code not Returned");
                foreach (var booking in AllBokings)
                {
                    Assert.IsNotNull(booking.bookingid);
                    Console.WriteLine(booking.bookingid);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test Failed " + ex.Message);
            }
        }

        [Test]
        public async Task Get_a_Booking()
        {
            try
            {
                var url = "https://restful-booker.herokuapp.com/booking/1";
                var client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(url);
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected Status code not Returned");
                Console.WriteLine(response.Content.ReadAsStringAsync().Result.ToString());
                BokokingDetail data = JsonConvert.DeserializeObject<BokokingDetail>(response.Content.ReadAsStringAsync().Result);
                Assert.AreEqual(data.firstname, "Susan");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Test Failed " + ex.Message);
            }
        }

        #endregion
    }
}