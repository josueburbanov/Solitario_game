using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solitario_proyecto
{
    public class Usuario
    {
        private string nombre;
        private string nombre_usuario;
        private string contrasenia;
        private string id;

        public string Nombre { get => nombre; set => nombre = value; }
        public string Nombre_usuario { get => nombre_usuario; set => nombre_usuario = value; }
        public string Contrasenia { get => contrasenia; set => contrasenia = value; }
        public string Id { get => id; set => id = value; }
        public string Correo { get; internal set; }
    }
}
