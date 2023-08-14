using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaIdentidad
{
//    IdUsuario int identity,
//   Nombres varchar(100),
//Apellidos varchar(100),
//Correo varchar(100),
//Clave varchar(150),
//Restablecer bit default 1,
//Activo bit default 1,
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Correo { get; set; }
        public string Clave { get; set; }
        public bool Restablecer { get; set; }
        public bool Activo { get; set; }
    }
}
