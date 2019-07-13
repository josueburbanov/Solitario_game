using System.Collections.Generic;

namespace Solitario_proyecto
{
    public class Carta
    {
        private string palo;
        private string simbolo;
        private int valor;
        private byte[] imagen;
        private string ruta_imagen;
        private bool boca_abajo;
        private List<Carta> cartasDependientes = new List<Carta>();


        public string Palo { get => palo; set => palo = value; }
        public string Simbolo { get => simbolo; set => simbolo = value; }
        public int Valor { get => valor; set => valor = value; }
        public byte[] Imagen { get => imagen; set => imagen = value; }
        public string Ruta_imagen { get => ruta_imagen; set => ruta_imagen = value; }
        public bool Boca_abajo { get => boca_abajo; set => boca_abajo = value; }
        public List<Carta> CartasDependientes { get => cartasDependientes; set => cartasDependientes = value; }
    }
}