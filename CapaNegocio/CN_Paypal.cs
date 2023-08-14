using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;//Para el manejo de los datos en
//Archivo Web.Config
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CapaIdentidad.Paypal;

namespace CapaNegocio
{
    public class CN_Paypal
    {
        //Si no reconocen  Configuration, es necesario instalar un paquete 
        private static string urlPaypal = ConfigurationManager.AppSettings["UrlPaypal"];
        private static string clienteId = ConfigurationManager.AppSettings["ClientId"];
        private static string secret = ConfigurationManager.AppSettings["Secret"];

        public async Task<Response_Paypal<Response_Checkout>> CrearSolicitud(Checkout_Order orden)
        {
            Response_Paypal<Response_Checkout> response_paypal = new Response_Paypal<Response_Checkout>();
            //Creamos una solicitud HTTP Client
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri(urlPaypal);

                //Token de autorización
                var authToke = Encoding.ASCII.GetBytes($"{clienteId}:{secret}");

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToke));

                //Request lo convertimos
                var json = JsonConvert.SerializeObject(orden);
                //Tipo de dato que necesita nuestra API, indicamos de que tipo de dato es
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                //Ejecutar nuestra API, respuesta de la ejueción de la API
                HttpResponseMessage response = await cliente.PostAsync("/v2/checkout/orders", data);
                //Si la solicitud tuvo Éxito
                response_paypal.Status = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    //Si tuvo éxito, obtengo la respuesta
                    string jsonRespuesta = response.Content.ReadAsStringAsync().Result;
                    //Convertimos en una clase de tipo
                    Response_Checkout checkout = JsonConvert.DeserializeObject<Response_Checkout>(jsonRespuesta);

                    response_paypal.Response = checkout;

                }
                return response_paypal;

            }
        }

        public async Task<Response_Paypal<Response_Capture>> AprobarPago(string token)
        {
            Response_Paypal<Response_Capture> response_paypal = new Response_Paypal<Response_Capture>();
            //Creamos una solicitud HTTP Client
            using (var cliente = new HttpClient())
            {
                cliente.BaseAddress = new Uri(urlPaypal);

                //Token de autorización
                var authToke = Encoding.ASCII.GetBytes($"{clienteId}:{secret}");

                cliente.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authToke));

                //Tipo de dato que necesita nuestra API, indicamos de que tipo de dato es
                var data = new StringContent("{}", Encoding.UTF8, "application/json");

                //Ejecutar nuestra API
                HttpResponseMessage response = await cliente.PostAsync($"/v2/checkout/orders/{token}/capture", data);
                //Si la solicitud tuvo Éxito
                response_paypal.Status = response.IsSuccessStatusCode;

                if (response.IsSuccessStatusCode)
                {
                    //Si tuvo éxito, obtengo la respuesta
                    string jsonRespuesta = response.Content.ReadAsStringAsync().Result;
                    Response_Capture capture = JsonConvert.DeserializeObject<Response_Capture>(jsonRespuesta);

                    response_paypal.Response = capture;

                }
                return response_paypal;

            }
        }
    }
}
