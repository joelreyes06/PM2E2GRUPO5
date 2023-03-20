using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PM2E2GRUPO5.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO5.Controllers
{
    public class SitioController
    {
    private static readonly string URL_SITIOS = "http://192.168.0.16/examenmovilgrupo5/";
    private static HttpClient client = new HttpClient();

        public static async Task<List<SitiosFirma>> GetSitiosAsync()
        {
            List<SitiosFirma> sitios = new List<SitiosFirma>();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(URL_SITIOS + "Listar.php");

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        sitios = JsonConvert.DeserializeObject<List<SitiosFirma>>(content);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return sitios;
        }

        public async Task<List<SitiosFirma>> GetAllAsync()
        {
            var httpClient = new HttpClient();
            var json = await httpClient.GetStringAsync(URL_SITIOS + "Listar.php");
            var sitios = JsonConvert.DeserializeObject<List<SitiosFirma>>(json);
            return sitios;
        }

        public async static Task<bool> DeleteSite(string id)
    {
        try
        {
            var uri = new Uri(URL_SITIOS + "Eliminar.php?id=" + id);
            var result = await client.GetAsync(uri);
            if (result.IsSuccessStatusCode)
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Eliminar: " + ex.Message);
        }
        return false;
    }

        public async static Task<bool> CreateSite(SitiosFirma sitio)
        {
            try
            {
                Uri requestUri = new Uri(URL_SITIOS + "Crear.php");
                var client = new HttpClient();
                var jsonObject = JsonConvert.SerializeObject(sitio);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return false;
        }

        public async static Task<bool> UpdateSitio(SitiosFirma sitio)
        {
            try
            {
                Uri requestUri = new Uri(URL_SITIOS + "Actualizar.php");
                var jsonObject = JsonConvert.SerializeObject(sitio);
                var content = new StringContent(jsonObject, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        /*public async Task<string> ActualizarSitioAsync(SitiosFirma sitio)
        {
            // var url = "http://192.168.1.10/examenmovilgrupo5/actualizar.php";

            var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("id", sitio.Id.ToString()),
                new KeyValuePair<string, string>("descripcion", sitio.Descripcion),
                new KeyValuePair<string, string>("latitud", sitio.Latitud),
                new KeyValuePair<string, string>("longitud", sitio.Longitud),
                new KeyValuePair<string, string>("firmadigital", Convert.ToBase64String(sitio.FirmaDigital)),
                new KeyValuePair<string, string>("trazado", sitio.firma)
            });

            // var response = await client.PostAsync((url), content);
            var response = await client.PostAsync((URL_SITIOS + "Actualizar.php"), content);
            var responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }*/

    }
}
