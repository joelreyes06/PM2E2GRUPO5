using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM2E2GRUPO5.Models
{
    public class SitiosFirma
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }
        [JsonProperty("latitud")]
        public string Latitud { get; set; }
        [JsonProperty("longitud")]
        public string Longitud { get; set; }
        [JsonProperty("firmadigital")]
        public byte[] FirmaDigital { get; set; }
        [JsonProperty("audiofile")]
        public byte[] AudioFile { get; set; }
        [JsonProperty("trazado")]
        public string firma { get; set; }
    }
}
