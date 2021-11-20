using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppS6PBlacio
{
    public partial class MainPage : ContentPage
    {
        private const string Url = "http://186.101.162.131/moviles/post.php";
        private readonly HttpClient client = new HttpClient();
        private ObservableCollection<AppS6PBlacio.Datos> _post;
        private int id = 0;
        private string nombre = null;
        private string apellido = null;
        private int edad = 0;

        public MainPage()
        {
            InitializeComponent();
            get();
        }

        public async void get()
        {
            var content = await client.GetStringAsync(Url);
            List<AppS6PBlacio.Datos> post = JsonConvert.DeserializeObject<List<AppS6PBlacio.Datos>>(content);
            _post = new ObservableCollection<AppS6PBlacio.Datos>(post);

            MyListView.ItemsSource = _post;

        }

        private async void btnGet_Clicked(object sender, EventArgs e)
        {
            var content = await client.GetStringAsync(Url);
            List<AppS6PBlacio.Datos> post = JsonConvert.DeserializeObject<List<AppS6PBlacio.Datos>>(content);
            _post = new ObservableCollection<AppS6PBlacio.Datos>(post);

            MyListView.ItemsSource = _post;
        }

        private async void btnPost_Clicked(object sender, EventArgs e)
        {
           await Navigation.PushAsync(new viewInsertarB());
        }

        private async void btnPut_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (id == 0 & nombre == null & apellido == null & edad == 0)
                {
                    await DisplayAlert("Error", "Selecione un estudiante", "Ok");
                }
                else
                {
                    await Navigation.PushAsync(new viewEditar(id, nombre, apellido, edad));
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async void btnDelete_Clicked(object sender, EventArgs e)
        {
            if (id == 0 & nombre == null & apellido == null & edad == 0)
            {
                await DisplayAlert("Error", "Selecione un estudiante", "Ok");
            }
            else
            {
                bool answer = await DisplayAlert("Eliminar", "Eliminar estudiante: " + nombre + " " + apellido + " ID " + id, "Yes", "No");
                if(answer == true){
                    try
                    {
                        if (id == 0)
                        {
                            await DisplayAlert("Error", "Error de ID", "Ok");
                            await Navigation.PushAsync(new MainPage());
                        }
                        else
                        {
                            HttpClient client = new HttpClient();
                            await client.DeleteAsync("http://186.101.162.131/moviles/post.php?codigo=" + id);
                            await DisplayAlert("Eliminado", "Eliminado con exito", "Ok");
                            await Navigation.PushAsync(new MainPage());

                        }
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", ex.Message, "Ok");
                    }
                }
                else
                {

                }
            }
        }

        private void MyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var select = ((ListView)sender).SelectedItem as AppS6PBlacio.Datos;
            if (select == null)
                return;
            id = select.codigo;
            nombre = select.nombre;
            apellido = select.apellido;
            edad = select.edad;
        }
    }
}
