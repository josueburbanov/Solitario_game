using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solitario_proyecto
{
    class UltimoMovimiento
    {
        public PictureBox picture_arrastrado;
        public PictureBox picture_caido;
        public Carta carta_arrastrada;
        public Carta carta_caida;
        public int puntos;

        public UltimoMovimiento(PictureBox picture_arrastrado, PictureBox picture_caido, Carta carta_arrastrada, Carta carta_caida, int puntos)
        {
            this.picture_arrastrado = picture_arrastrado;
            this.picture_caido = picture_caido;
            this.carta_arrastrada = carta_arrastrada;
            this.carta_caida = carta_caida;
            this.puntos = puntos;
        }
    }
}
