using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherApp
{
    public class City
    {

        [JsonProperty("city")]
        public string CityName { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }
    }
}
