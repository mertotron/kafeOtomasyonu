using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;


namespace kafeOtomasyonu.Controllers
{
    internal class SerialController
    {
        public async Task<bool> CheckSerialKey(string userName, string password, int userId)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // JSON objesini oluştur
                var json = $"{{\"username\":\"{userName}\",\"password\":\"{password}\",\"userId\":{userId}}}";
                //json içeriğini belirt
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("http://localhost:5000/route/serialcontrol", content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
