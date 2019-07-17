using System.Collections.Generic;

namespace Solitario_proyecto
{
    public class Baraja
    {
        private List<Carta> cartas_primer_espacio;
        private List<Carta> cartas_segundo_espacio;
        private List<Carta> cartas_tercer_espacio;
        private List<Carta> cartas_cuarto_espacio;
        private List<Carta> cartas_quinto_espacio;
        private List<Carta> cartas_sexto_espacio;
        private List<Carta> cartas_septimo_espacio;
        private List<Carta> cartas_boca_abajo;
        private List<Carta> cartas_total = new List<Carta>();

        public List<Carta> Cartas_primer_espacio { get => cartas_primer_espacio; set => cartas_primer_espacio = value; }
        public List<Carta> Cartas_segundo_espacio { get => cartas_segundo_espacio; set => cartas_segundo_espacio = value; }
        public List<Carta> Cartas_tercer_espacio { get => cartas_tercer_espacio; set => cartas_tercer_espacio = value; }
        public List<Carta> Cartas_cuarto_espacio { get => cartas_cuarto_espacio; set => cartas_cuarto_espacio = value; }
        public List<Carta> Cartas_quinto_espacio { get => cartas_quinto_espacio; set => cartas_quinto_espacio = value; }
        public List<Carta> Cartas_sexto_espacio { get => cartas_sexto_espacio; set => cartas_sexto_espacio = value; }
        public List<Carta> Cartas_septimo_espacio { get => cartas_septimo_espacio; set => cartas_septimo_espacio = value; }
        public List<Carta> Cartas_boca_abajo { get => cartas_boca_abajo; set => cartas_boca_abajo = value; }
        public List<Carta> Cartas_total { get => cartas_total; set => cartas_total = value; }

        public Baraja DeepCopy()
        {
            Baraja baraja_copy = new Baraja(new List<Carta>(cartas_primer_espacio), new List<Carta>(cartas_segundo_espacio),
                new List<Carta>(cartas_tercer_espacio), new List<Carta>(cartas_cuarto_espacio),
                    new List<Carta>(cartas_quinto_espacio), new List<Carta>(cartas_sexto_espacio),
                    new List<Carta>(cartas_septimo_espacio), new List<Carta>(cartas_boca_abajo));
            return baraja_copy;
        }
        public Baraja(List<Carta> cartas_primer_espacio, List<Carta> cartas_segundo_espacio, List<Carta> cartas_tercer_espacio, List<Carta> cartas_cuarto_espacio, List<Carta> cartas_quinto_espacio, List<Carta> cartas_sexto_espacio, List<Carta> cartas_septimo_espacio, List<Carta> cartas_boca_abajo)
        {
            this.Cartas_primer_espacio = cartas_primer_espacio;
            this.Cartas_segundo_espacio = cartas_segundo_espacio;
            this.Cartas_tercer_espacio = cartas_tercer_espacio;
            this.Cartas_cuarto_espacio = cartas_cuarto_espacio;
            this.Cartas_quinto_espacio = cartas_quinto_espacio;
            this.Cartas_sexto_espacio = cartas_sexto_espacio;
            this.Cartas_septimo_espacio = cartas_septimo_espacio;
            this.Cartas_boca_abajo = cartas_boca_abajo;
            this.cartas_total.AddRange(cartas_primer_espacio);
            this.cartas_total.AddRange(cartas_segundo_espacio);
            this.cartas_total.AddRange(cartas_tercer_espacio);
            this.cartas_total.AddRange(cartas_cuarto_espacio);
            this.cartas_total.AddRange(cartas_quinto_espacio);
            this.cartas_total.AddRange(cartas_sexto_espacio);
            this.cartas_total.AddRange(cartas_septimo_espacio);
            this.cartas_total.AddRange(cartas_boca_abajo);
        }
    }
}