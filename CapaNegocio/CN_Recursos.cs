using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.IO;
namespace CapaNegocio
{
    public class CN_Recursos
    {
        public static string GenerarClave()
        {
            //Retornar un código único/ "N" solo caracteres alfanuméricos / una clave de 6 caracteres (que tome de la clave generada)
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);
            return clave;
        }
        public  string ConvertirSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));
                foreach(byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
        public static bool EnviarCorreo(string correo, string asunto, string mensajeAEnviar)
        {
            bool resultado = false;
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(correo);//A quién le envio
                //Desde que cuenta le estoy enviando el correo
                mail.From = new MailAddress("yuushaescalante@gmail.com");
                //El asunto
                mail.Subject = asunto;
                mail.Body = mensajeAEnviar; //Cual es el mensaje a enviar
                mail.IsBodyHtml = true;

                //Nuestro servidor cliente que envia el correo
                var smtp = new SmtpClient()
                {
                    //Correo y contraseña, la contraseña no es de acceso a la cuenta
                    Credentials = new NetworkCredential("yuushaescalante@gmail.com", "pzsmkfignfwbynsh"),

                    Host = "smtp.gmail.com",//Servidor que usa Gmail para enviar los correos
                    Port = 587,
                    EnableSsl = true //Certificado de seguridad
                };
                //Que envie el correo que hemos configurado
                smtp.Send(mail);
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;

            }
            return resultado;
        }
        public static string convertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;
            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                conversion = false;
            }
            return textoBase64;
        }
    }
}
