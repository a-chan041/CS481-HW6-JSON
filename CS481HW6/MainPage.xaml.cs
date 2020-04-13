using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Connectivity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using System.Net.Http;
using Json.Net;
using System.Globalization;


namespace CS481HW6
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void GoSearch(object sender, System.EventArgs e)
        {
            // referenced: slide 13 from Wk 9 PPT
            //reference2: https://www.codementor.io/@lutaayahuzaifahidris/xamarin-forms-simple-list-of-items-with-offline-capability-cross-platform-app-btyq0bihv

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Token " + ApiKeys.OwlBotKey);

            var uri = new Uri(
            string.Format($"https://owlbot.info/api/v4/dictionary/"+ $"{input.Text}"));

            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = uri;

            HttpResponseMessage response = await client.SendAsync(request);
            
        }

        private class ApiKeys
        {
            public static string OwlBotKey = "6f7696e6566f5942bca28f2dc6e6696954756417";
        }

        public partial class OwlBotInfo
        {
            [JsonProperty ("type")]
            public string type { get; set; }

            [JsonProperty("definition")]
            public string definition { get; set; }

            [JsonProperty("example")]
            public string example { get; set; }

            [JsonProperty("image_url")]
            public string image_url { get; set; }

            [JsonProperty("emoji")]
            public string emoji { get; set; }

            public static OwlBotInfo FromJson(string json) => JsonConvert.DeserializeObject<OwlBotInfo>(json);
        }
    }

}
