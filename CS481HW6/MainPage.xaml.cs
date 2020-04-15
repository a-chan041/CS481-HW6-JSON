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
using Xamarin.Essentials;


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
            IsConnected();
        }

        public bool IsConnected() //checking user connectivity
                                  //referenced: https://docs.microsoft.com/en-us/xamarin/essentials/connectivity?tabs=androidconnect
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                // user is connected to internet
                return true;
            }


            DisplayAlert("ERROR", "You are not connected to the Internet", "OK"); //display error if user is not connected to the internet
            return false;
        
        }

        async void GoSearch(object sender, System.EventArgs e)
        {
            // referenced: slide 13 from Wk 9 PPT
            //reference2: https://www.codementor.io/@lutaayahuzaifahidris/xamarin-forms-simple-list-of-items-with-offline-capability-cross-platform-app-btyq0bihv

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Token " + "6f7696e6566f5942bca28f2dc6e6696954756417");

            var uri = new Uri(
            string.Format($"https://owlbot.info/api/v4/dictionary/" + $"{input.Text}")); //open Owlbot page concatenated with word user looks up in search bar

            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Get;
            request.RequestUri = uri;

            HttpResponseMessage response = await client.SendAsync(request);
            WordInfo word = null;

            if (response.IsSuccessStatusCode) //if http page is found
            {

                var content = await response.Content.ReadAsStringAsync();
                word = WordInfo.FromJson(content); //get data from JSON
                BindingContext = word;
            }

            else //if page is not found
            {
               Console.WriteLine("An error occured while loading data");
            }
          
        }

        public partial class WordInfo //WordInfo object
        {
            [JsonProperty("definitions")]
            public OwlBotInfo[] Def { get; set; }


            [JsonProperty("word")]
            public string word { get; set; }


            [JsonProperty("pronunciation")]
            public string pronunciation { get; set; }

            public static WordInfo FromJson(string json) => JsonConvert.DeserializeObject<WordInfo>(json);
        }

        public partial class OwlBotInfo //OwlBotInfo object created from JSON recieved from OwlBotAPI
        {
                [JsonProperty("type")]
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
