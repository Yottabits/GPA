using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace Slack.Models.CanteenModels
{
    public class CanteenRepository : ICanteenRepository
    {
        public Canteen Get(DateTime day, CanteensEnum canteen = CanteensEnum.AmAdenauerring)
        {
            string url = String.Format("https://www.iwi.hs-karlsruhe.de/Intranetaccess/REST/canteen/{0}/{1}", (int)canteen, day.ToString("yyyy-MM-dd"));
            Canteen m = null;
            try
            {
                using (var client = new HttpClient())
                {
                    string response = client.GetStringAsync(url).Result;
                    m = JsonConvert.DeserializeObject<Canteen>(response);
                }
            }
            catch (Exception e)
            {
                //TODO: Logging
            }

            return m;
        }
    }
}
