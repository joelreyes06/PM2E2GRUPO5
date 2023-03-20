using Nancy.Json;
using PM2E2GRUPO5.Models;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PM2E2GRUPO5.Controllers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;

namespace PM2E2GRUPO5.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageModificar : ContentPage
    {
        SitiosFirma _sitio;
        public PageModificar(SitiosFirma sitio)
        {
            InitializeComponent();

            _sitio = sitio;
            latitud.Text = sitio.Latitud;
            longitud.Text = sitio.Longitud;
            descripcion.Text = sitio.Descripcion;
        }

        private void limpiardescripcion_Clicked(object sender, EventArgs e)
        {
            descripcion.Text = null;
        }

        private async void btnactualizar_Clicked(object sender, EventArgs e)
        {
            bool validacion = false;

            if (descripcion.Text == null || descripcion.Text == "")
            {
                validacion = true;
                await DisplayAlert("Aviso", "Se necesita una breve descripción de la ubicación.", "OK");
            }

            if (!validacion)
            {
                byte[] ImageBytes = null;
                var firma = PadView.Strokes;


                try
                {
                    var image = await PadView.GetImageStreamAsync(SignatureImageFormat.Png);

                    var mStream = (MemoryStream)image;
                    byte[] data = mStream.ToArray();
                    string base64Val = Convert.ToBase64String(data);
                    ImageBytes = Convert.FromBase64String(base64Val);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Aviso", "Se requiere la firma nueva", "OK");
                    return;
                }


                try
                {
                    var serializer = new JavaScriptSerializer();
                    var trazado = serializer.Serialize(firma);

                    SitiosFirma sitio = new SitiosFirma
                    {
                        Id = _sitio.Id,
                        Descripcion = descripcion.Text,
                        Latitud = latitud.Text,
                        Longitud = longitud.Text,
                        FirmaDigital = ImageBytes,
                        firma = trazado
                    };

                    await SitioController.UpdateSitio(sitio);
                    await DisplayAlert("Aviso", "Sitio modificado con exito", "Aceptar");
                    await Navigation.PopAsync();
                }
                catch (Exception error)
                {
                    await DisplayAlert("Aviso", "" + error, "OK");
                }
            }
        }

        private async void btneliminar_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Aviso", "¿Seguro que desea eliminar el Registro?", "Confirmar", "Cancelar");
            if (answer)
            {
                await SitioController.DeleteSite("" + _sitio.Id);
                await Navigation.PopAsync();
            }

        }
    }
}