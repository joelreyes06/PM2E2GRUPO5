using Nancy.Json;
using Plugin.AudioRecorder;
using PM2E2GRUPO5.Models;
using PM2E2GRUPO5.Controllers;
using SignaturePad.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PM2E2GRUPO5
{
    public partial class MainPage : ContentPage
    {

        bool audiobandera = false;

        private readonly AudioRecorderService audioRecorderService = new AudioRecorderService();

        public MainPage()
        {
            InitializeComponent();
            checkInternet();
            getLocation();

            audiobandera = false;

        }

        private async void btnUbicaciones_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.PageListaUbicaciones());

        }

        private async void btngrabarvoz_Clicked(object sender, EventArgs e)
        {

            var microPhone = await Permissions.RequestAsync<Permissions.Microphone>();
            var storageRead = await Permissions.RequestAsync<Permissions.StorageRead>();
            var storageWrite = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (microPhone != PermissionStatus.Granted && 
                storageRead != PermissionStatus.Granted && 
                storageWrite != PermissionStatus.Granted)
            {
                return;
            }

            ondaespacio.IsVisible = false; // Configurar
            btnGuardar.IsEnabled = false;
            btngrabarvoz.IsVisible = false;
            btndetenervoz.IsVisible = true;

            await audioRecorderService.StartRecording();

            audiobandera = true;

        }

        private async void btndetenervoz_Clicked(object sender, EventArgs e)
        {

            ondaespacio.IsVisible = true;
            btnGuardar.IsEnabled = true;
            btngrabarvoz.IsVisible = true;
            btndetenervoz.IsVisible = false;

            await audioRecorderService.StopRecording();
            ondaTexto.Text = "Audio Grabado";

        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            bool validacion = false;

            if (latitud.Text == null || longitud.Text == null)
            {
                validacion = true;
                await DisplayAlert("Error", "Coordenadas de su ubicación requeridas para guardar.", "OK");
            }

            if (descripcion.Text == null || descripcion.Text.Trim() == "")
            {
                validacion = true;
                await DisplayAlert("Error", "Se necesita una breve descripción de la ubicación.", "OK");
            }

            if (!validacion)
            {
                byte[] ImageBytes = null;
                byte[] AudioBytes = null;
                var firma = PadView.Strokes;

                try
                {
                    var image = await PadView.GetImageStreamAsync(SignatureImageFormat.Png);

                    //imagen a base 64
                    var mStream = (MemoryStream)image;
                    byte[] data = mStream.ToArray();
                    string base64Val = Convert.ToBase64String(data);
                    ImageBytes = Convert.FromBase64String(base64Val);
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Aviso", "Se requiere la firma", "OK");
                    return;
                }

                try
                {
                    var audio = audioRecorderService.GetAudioFileStream();


                    AudioBytes = File.ReadAllBytes(audioRecorderService.GetAudioFilePath());
                }
                catch (Exception ex)
                {
                    if (audiobandera)
                    {
                        await DisplayAlert("Aviso", "No has hablado fuerte al grabar tu nota de voz", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Aviso", "No has grabado una nota de voz", "OK");
                    }

                    return;
                }

                try
                {
                    var serializer = new JavaScriptSerializer();

                    var b = serializer.Serialize(firma);

                    SitiosFirma sitio = new SitiosFirma
                    {
                        Descripcion = descripcion.Text,
                        Latitud = latitud.Text,
                        Longitud = longitud.Text,
                        FirmaDigital = ImageBytes,
                        AudioFile = AudioBytes,
                        firma = b
                    };

                    await SitioController.CreateSite(sitio);
                    await DisplayAlert("Aviso", "Sitio ingresado con exito: " + sitio.Descripcion, "OK");
                    PadView.Clear();
                    descripcion.Text = null;

                }
                catch (Exception ex)
                {
                    await DisplayAlert("Aviso", "" + ex, "OK");
                }


            }

        }

        #region Location
        private void cleanLocation()
        {
            latitud.Text = null;
            longitud.Text = null;
        }

        public async void getLocation()
        {
            try
            {
                var location = await Geolocation.GetLocationAsync();

                if (location != null)
                {
                    latitud.Text = "" + location.Latitude;
                    longitud.Text = "" + location.Longitude;
                }
                else
                {
                    await DisplayAlert("Aviso", "El GPS está desactivado o no se pudo reconocer", "OK");
                    cleanLocation();
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await DisplayAlert("Aviso", "Este dispositivo no soporta la versión de GPS utilizada", "OK");
                cleanLocation();
            }
            catch (FeatureNotEnabledException fneEx)
            {
                await DisplayAlert("Aviso", "La ubicación está desactivada", "OK");
                cleanLocation();

            }
            catch (PermissionException pEx)
            {
                await DisplayAlert("Aviso", "La aplicación no puede acceder a su ubicación.\n\n" +
                    "Habilite los permisos de ubicación en los ajustes del dispositivo", "OK");
                cleanLocation();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Aviso", "No se ha podido obtener la localización (error de gps)", "OK");
                cleanLocation();
            }
        }
        #endregion

        #region Internet
        public async void checkInternet()
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                await DisplayAlert("Aviso", "No tiene acceso a Internet.\n" +
                    "Necesita teber acceso a internet para continuar.", "OK");
            }

        }
        #endregion


    }
}
