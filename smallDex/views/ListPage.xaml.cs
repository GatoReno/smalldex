using Newtonsoft.Json;
using smallDex.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace smallDex.views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {

        public string next;
        public object previous;
        public ListPage()
        {
            InitializeComponent();
            string def = "https://pokeapi.co/api/v2/pokemon/";
            _ = GetPokesFromWeb(def);
            entName.TextChanged += entName_TextChangedEvent;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            lblError.IsVisible = false;
            btnpokmon.IsVisible = false;
            entName.Text= "";

        }
        private void entName_TextChangedEvent(object sender, TextChangedEventArgs e)
        {
           _ = entName_TextChanged();
        }

        private async Task entName_TextChanged()
        {
            var pkn = entName.Text;
            if (string.IsNullOrEmpty(pkn))
            {

            }
            else { 
            var uri = "https://pokeapi.co/api/v2/pokemon/" + pkn;
            HttpClient client = new HttpClient();
            var responseMsg = await  client.GetAsync(uri);

            if (responseMsg.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                lblError.IsVisible = true;
                lblError.Text = "Tu pokeon no existe,intenta otra ves";
                entName.Focus();
            }
            else if (responseMsg.StatusCode == System.Net.HttpStatusCode.OK)
            {

                lblError.IsVisible = false;
                //entName.Text = "";
                btnpokmon.IsVisible = true;
            }
            else {
                lblError.IsVisible = true;
                lblError.Text = "Posible Error";
                entName.Focus();
            }

            }



        }

        private async void GetPokes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var content = e.Item as Result;
            await Navigation.PushAsync(new PokeInfo(content.url));
        }

    
        public async Task GetPokesFromWeb(string ux) {

            try
            {
                HttpClient client = new HttpClient();                 
                var responseMsg = await client.GetAsync(ux);



                switch (responseMsg.StatusCode)
                {
                    case System.Net.HttpStatusCode.InternalServerError:
                        Console.WriteLine("----------------------------------------------_____:Here status 500");

                        //xlabel.Text = "error 500";
                        // Cator.IsVisible = false;
                        break;


                    case System.Net.HttpStatusCode.OK:
                        Console.WriteLine("----------------------------------------------_____:Here status 200");

                        // ylabel.Text = "Ultimas noticas de proyectos";


                        // var json_ = JsonConvert.DeserializeObject<List<VisitasMod>>(xjson);

                        string xjson = await responseMsg.Content.ReadAsStringAsync();
                        //DireccApiCall

                        HttpContent contentD = responseMsg.Content;
                        var xjsonD = await contentD.ReadAsStringAsync();

                        

                            var json_d = JsonConvert.DeserializeObject<PokeCallAll>(xjsonD);
                            ListPoke.ItemsSource = json_d.results;
                            next = json_d.next;
                            previous = json_d.previous;
                                if (previous == null)
                                {
                                    btnprev.IsVisible = false;
                                }
                                else{
                                    btnprev.IsVisible = true;
                                }
                            



                        break;

                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("", "" + ex.ToString(), "ok");
                //  Cator.IsVisible = false;

                //  CatorT.Text = "Ha habido un error";
                return;
            }
        }
        
        private void btnnext_Clicked(object sender, EventArgs e)
        {
            _ = GetPokesFromWeb(next);
        }

        private void btnprev_Clicked(object sender, EventArgs e)
        {
            _ = GetPokesFromWeb(previous.ToString());
        }

        private void btnpokmon_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(entName.Text))
            {
                entName.Focus();
            }
            else {
                var ir = "https://pokeapi.co/api/v2/pokemon/" + entName.Text;
                 Navigation.PushAsync(new PokeInfo(ir));

            }
        }
    }
}