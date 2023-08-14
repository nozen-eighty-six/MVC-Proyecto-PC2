using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaIdentidad
{
//    CREATE TABLE CATEGORIA(
//IdCategoria int identity,
//Descripcion varchar(100),
//Activo bit default 1,
//FechaRegistro datetime default getdate(),-->Fecha actual
//Constraint pk_idCategoria primary key(IdCategoria)

//);
    public class Categoria
    {
        public int IdCategoria { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        
    }
}
