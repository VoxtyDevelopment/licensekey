using CitizenFX.Core;
using CitizenFX.Core.Native;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Server
{
    internal class Server : BaseScript
    {
        private static HttpClient client = new HttpClient();
        private string licenseKey = ConfigLoader.LicenseKey;

        public Server()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            EventHandlers["onResourceStart"] += new Action<string>(OnResourceStart);
        }

        private async void OnResourceStart(string resourceName)
        {
            if (resourceName == API.GetCurrentResourceName())
            {
                bool isValidLicense = await CheckLicenseKeyInAPI(licenseKey);

                if (isValidLicense)
                {
                    Debug.WriteLine("License key successfully validated. Enjoy your product.");
                }
                else
                {
                    Debug.WriteLine("Exiting startup due to invalid license key.");
                    API.StopResource(resourceName);
                }
            }
        }

        private async Task<bool> CheckLicenseKeyInAPI(string key)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { license_key = key }), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("API_URL", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    dynamic result = JsonConvert.DeserializeObject(responseData);
                    return result.valid;
                }
                else
                {
                    if (response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.BadRequest)
                    {
                    }
                    else
                    {
                        Debug.WriteLine($"The API may be down or undergoing maintenance. Please contact the Vox Development Administration for more info.");

                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine($"HTTP request error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error: {ex.Message}");
            }
            return false;
        }
    }
}