using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solitario_proyecto
{
    public partial class Form1 : Form
    {

        
        public Form1()
        {
            InitializeComponent();
            pctbx_boca_abajo.AllowDrop = true;
            pctbx_boca_abajo.DragEnter += pctbx_boca_abajo_DragEnter;
            pctbx_baraja_0.AllowDrop = true;
            pctbx_baraja_0.DragEnter += pctbx_boca_abajo_DragEnter;
            

            //Cartas que arrancan con la posibilidad de ser caídas y arrastradas
            pctbx_espacio1.AllowDrop = true;
            pctbx_espacio1.DragEnter += pctbx_boca_abajo_DragEnter;

            pctbx_espacio2_1.AllowDrop = true;
            pctbx_espacio2_1.DragEnter += pctbx_boca_abajo_DragEnter;

            pctbx_espacio3_2.AllowDrop = true;
            pctbx_espacio3_2.DragEnter += pctbx_boca_abajo_DragEnter;

            pctbx_espacio4_3.AllowDrop = true;
            pctbx_espacio4_3.DragEnter += pctbx_boca_abajo_DragEnter;

            pctbx_espacio5_4.AllowDrop = true;
            pctbx_espacio5_4.DragEnter += pctbx_boca_abajo_DragEnter;

            pctbx_espacio6_5.AllowDrop = true;
            pctbx_espacio6_5.DragEnter += pctbx_boca_abajo_DragEnter;

            pctbx_espacio7_6.AllowDrop = true;
            pctbx_espacio7_6.DragEnter += pctbx_boca_abajo_DragEnter;

            //Cartas que arrancan con la posibilidad de ser arrastradas
            pctbx_espacio1.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio2_1.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio3_2.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio4_3.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio5_4.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio6_5.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio7_6.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);




        }

        private async Task<Baraja> cargar_recursos()
        {
            string url = "http://localhost:29485/Service1.svc";
            RestClient cliente;
            cliente = new RestClient(url);
            var solicitud = new RestRequest(Method.GET);
            solicitud.Resource = "/colocar_cartas";
            solicitud.RequestFormat = DataFormat.Json;            
            var respuesta = await cliente.ExecuteTaskAsync(solicitud);
            var contenidoRespuesta = respuesta.Content;
            if (!respuesta.IsSuccessful)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Baraja>(contenidoRespuesta);

        }


        
        private void pctbx_boca_abajo_DragEnter(object sender, DragEventArgs e)
        {
            var bmp = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
            PictureBox picture_arrastrado = e.Data.GetData("System.Windows.Forms.PictureBox") as PictureBox;
            PictureBox picture_caido = sender as PictureBox;
            if(picture_caido != picture_arrastrado)
            {
                Carta carta_arrastrada = (Carta)picture_arrastrado.Tag;
                Carta carta_caida = (Carta)picture_caido.Tag;
                if (!carta_arrastrada.Boca_abajo)
                {
                    if (carta_arrastrada.Valor + 1 == carta_caida.Valor && chequear_colores(carta_arrastrada, carta_caida))
                    {
                        picture_arrastrado.Location = new Point(picture_caido.Location.X, picture_caido.Location.Y + 20);
                        carta_caida.CartasDependientes.Add(carta_arrastrada);
                    }
                }
            }
        }

        private bool chequear_colores(Carta carta_arrastrada, Carta carta_caida)
        {
            bool comparacion1 = carta_arrastrada.Palo != carta_caida.Palo;
            bool comparacion2 = false;
            bool comparacion3 = false;
            if (carta_arrastrada.Palo == "corazones")
            {
                comparacion2 = carta_caida.Palo == "treboles";
                comparacion3 = carta_caida.Palo == "negros";
            }
            if (carta_arrastrada.Palo == "treboles")
            {
                comparacion2 = carta_caida.Palo == "corazones";
                comparacion3 = carta_caida.Palo == "diamantes";
            }
            if (carta_arrastrada.Palo == "negros")
            {
                comparacion2 = carta_caida.Palo == "corazones";
                comparacion3 = carta_caida.Palo == "diamantes";
            }
            if (carta_arrastrada.Palo == "diamantes")
            {
                comparacion2 = carta_caida.Palo == "treboles";
                comparacion3 = carta_caida.Palo == "negros";
            }
            return comparacion1 || comparacion2 || comparacion3;
        }

        private void enviar_atras(List<PictureBox> deck_of)
        {
            for (int i = deck_of.Count - 1; i >= 0 ; i--)
            {
                deck_of[i].SendToBack();
            }
        }

        private void pctbx_baraja_0_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                var img = sender as PictureBox;
                if (img == null) return;
                if (DoDragDrop(img, DragDropEffects.Move) == DragDropEffects.Move)
                {
                    img.Image = null;
                }
            }
        }

        

        

        private async void button1_Click(object sender, EventArgs e)
        {
            await iniciar_juego();
            
        }

        private async Task iniciar_juego()
        {
            Baraja cartas = await cargar_recursos();
            foreach (var item in cartas.Cartas_total)
            {
                File.WriteAllBytes("/Resources",item.Imagen);
                item.Imagen = null;
            }

            foreach (var item in cartas.Cartas_primer_espacio)
            {
                item.Simbolo = Regex.Replace(item.Simbolo, @"\s+", "");
                item.Palo = Regex.Replace(item.Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + item.Simbolo + "_" + item.Palo + ".png";
                File.WriteAllBytes(rutaImagen, item.Imagen);
                item.Ruta_imagen = rutaImagen;
                //item.Imagen = null;
                pctbx_espacio1.Image = Image.FromFile(rutaImagen);
                pctbx_espacio1.Tag = cartas.Cartas_primer_espacio[0];
            }

            for (int i = 0; i < cartas.Cartas_segundo_espacio.Count; i++)
            {
                cartas.Cartas_segundo_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_segundo_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_segundo_espacio[i].Palo = Regex.Replace(cartas.Cartas_segundo_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_segundo_espacio[i].Simbolo + "_" + cartas.Cartas_segundo_espacio[i].Palo + ".png";
                File.WriteAllBytes(rutaImagen, cartas.Cartas_segundo_espacio[i].Imagen);
                cartas.Cartas_segundo_espacio[i].Ruta_imagen = rutaImagen;
                //item.Imagen = null;
                if (i == 0)
                {
                    pctbx_espacio2.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio2.Tag = cartas.Cartas_segundo_espacio[i];
                    cartas.Cartas_segundo_espacio[i].Boca_abajo = true;
                }
                else if(i == 1)
                {
                    pctbx_espacio2_1.Image = Image.FromFile(rutaImagen);
                    pctbx_espacio2.Tag = cartas.Cartas_segundo_espacio[i];

                }
            }

            for (int i = 0; i < cartas.Cartas_tercer_espacio.Count; i++)
            {
                cartas.Cartas_tercer_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_tercer_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_tercer_espacio[i].Palo = Regex.Replace(cartas.Cartas_tercer_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_tercer_espacio[i].Simbolo + "_" + cartas.Cartas_tercer_espacio[i].Palo + ".png";
                File.WriteAllBytes(rutaImagen, cartas.Cartas_tercer_espacio[i].Imagen);
                cartas.Cartas_tercer_espacio[i].Ruta_imagen = rutaImagen;
                //item.Imagen = null;
                if (i == 0)
                {
                    pctbx_espacio3.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio3.Tag = cartas.Cartas_tercer_espacio[i];
                    cartas.Cartas_tercer_espacio[i].Boca_abajo = true;
                } else if (i == 1)
                {
                    pctbx_espacio3_1.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio3_1.Tag = cartas.Cartas_tercer_espacio[i];
                    cartas.Cartas_tercer_espacio[i].Boca_abajo = true;
                }
                else if (i == 2)
                {
                    pctbx_espacio3_2.Image = Image.FromFile(rutaImagen);
                    pctbx_espacio3_2.Tag = cartas.Cartas_tercer_espacio[i];
                }
                
            }

            for (int i = 0; i < cartas.Cartas_cuarto_espacio.Count; i++)
            {
                cartas.Cartas_cuarto_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_cuarto_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_cuarto_espacio[i].Palo = Regex.Replace(cartas.Cartas_cuarto_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_cuarto_espacio[i].Simbolo + "_" + cartas.Cartas_cuarto_espacio[i].Palo + ".png";
                File.WriteAllBytes(rutaImagen, cartas.Cartas_cuarto_espacio[i].Imagen);
                cartas.Cartas_cuarto_espacio[i].Ruta_imagen = rutaImagen;
                //item.Imagen = null;
                if (i == 0)
                {
                    pctbx_espacio4.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio4.Tag = cartas.Cartas_cuarto_espacio[i];
                    cartas.Cartas_cuarto_espacio[i].Boca_abajo = true;
                }
                else if (i == 1)
                {
                    pctbx_espacio4_1.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio4_1.Tag = cartas.Cartas_cuarto_espacio[i];
                    cartas.Cartas_cuarto_espacio[i].Boca_abajo = true;
                }
                else if (i == 2)
                {
                    pctbx_espacio4_2.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio4_2.Tag = cartas.Cartas_cuarto_espacio[i];
                    cartas.Cartas_cuarto_espacio[i].Boca_abajo = true;
                }
                else if (i == 3)
                {
                    pctbx_espacio4_3.Image = Image.FromFile(rutaImagen);
                    pctbx_espacio4_3.Tag = cartas.Cartas_cuarto_espacio[i];
                }
            }

            for (int i = 0; i < cartas.Cartas_quinto_espacio.Count; i++)
            {
                cartas.Cartas_quinto_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_quinto_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_quinto_espacio[i].Palo = Regex.Replace(cartas.Cartas_quinto_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_quinto_espacio[i].Simbolo + "_" + cartas.Cartas_quinto_espacio[i].Palo + ".png";
                File.WriteAllBytes(rutaImagen, cartas.Cartas_quinto_espacio[i].Imagen);
                cartas.Cartas_quinto_espacio[i].Ruta_imagen = rutaImagen;
                //item.Imagen = null;
                if (i == 0)
                {
                    pctbx_espacio5.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio5.Tag = cartas.Cartas_quinto_espacio[i];
                    cartas.Cartas_quinto_espacio[i].Boca_abajo = true;
                }
                else if (i == 1)
                {
                    pctbx_espacio5_1.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio5_1.Tag = cartas.Cartas_quinto_espacio[i];
                    cartas.Cartas_quinto_espacio[i].Boca_abajo = true;
                }
                else if (i == 2)
                {
                    pctbx_espacio5_2.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio5_2.Tag = cartas.Cartas_quinto_espacio[i];
                    cartas.Cartas_quinto_espacio[i].Boca_abajo = true;
                }
                else if (i == 3)
                {
                    pctbx_espacio5_3.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio5_3.Tag = cartas.Cartas_quinto_espacio[i];
                    cartas.Cartas_quinto_espacio[i].Boca_abajo = true;
                }
                else if (i == 4)
                {
                    pctbx_espacio5_4.Image = Image.FromFile(rutaImagen);
                    pctbx_espacio5_4.Tag = cartas.Cartas_quinto_espacio[i];
                }
            }

            for (int i = 0; i < cartas.Cartas_sexto_espacio.Count; i++)
            {
                cartas.Cartas_sexto_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_sexto_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_sexto_espacio[i].Palo = Regex.Replace(cartas.Cartas_sexto_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_sexto_espacio[i].Simbolo + "_" + cartas.Cartas_sexto_espacio[i].Palo + ".png";
                File.WriteAllBytes(rutaImagen, cartas.Cartas_sexto_espacio[i].Imagen);
                cartas.Cartas_sexto_espacio[i].Ruta_imagen = rutaImagen;
                //item.Imagen = null;
                if (i == 0)
                {
                    pctbx_espacio6.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio6.Tag = cartas.Cartas_sexto_espacio[i];
                    cartas.Cartas_sexto_espacio[i].Boca_abajo = true;
                }
                else if (i == 1)
                {
                    pctbx_espacio6_1.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio6_1.Tag = cartas.Cartas_sexto_espacio[i];
                    cartas.Cartas_sexto_espacio[i].Boca_abajo = true;
                }
                else if (i == 2)
                {
                    pctbx_espacio6_2.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio6_2.Tag = cartas.Cartas_sexto_espacio[i];
                    cartas.Cartas_sexto_espacio[i].Boca_abajo = true;
                }
                else if (i == 3)
                {
                    pctbx_espacio6_3.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio6_3.Tag = cartas.Cartas_sexto_espacio[i];
                    cartas.Cartas_sexto_espacio[i].Boca_abajo = true;
                }
                else if (i == 4)
                {
                    pctbx_espacio6_4.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio6_4.Tag = cartas.Cartas_sexto_espacio[i];
                    cartas.Cartas_sexto_espacio[i].Boca_abajo = true;
                }
                else if (i == 5)
                {
                    pctbx_espacio6_5.Image = Image.FromFile(rutaImagen);
                    pctbx_espacio6_5.Tag = cartas.Cartas_sexto_espacio[i];
                }
            }

            for (int i = 0; i < cartas.Cartas_septimo_espacio.Count; i++)
            {
                cartas.Cartas_septimo_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_septimo_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_septimo_espacio[i].Palo = Regex.Replace(cartas.Cartas_septimo_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_septimo_espacio[i].Simbolo + "_" + cartas.Cartas_septimo_espacio[i].Palo + ".png";
                File.WriteAllBytes(rutaImagen, cartas.Cartas_septimo_espacio[i].Imagen);
                cartas.Cartas_septimo_espacio[i].Ruta_imagen = rutaImagen;
                //item.Imagen = null;
                if (i == 0)
                {
                    pctbx_espacio7.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio7.Tag = cartas.Cartas_septimo_espacio[i];
                    cartas.Cartas_septimo_espacio[i].Boca_abajo = true;
                }
                else if (i == 1)
                {
                    pctbx_espacio7_1.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio7_1.Tag = cartas.Cartas_septimo_espacio[i];
                    cartas.Cartas_septimo_espacio[i].Boca_abajo = true;
                }
                else if (i == 2)
                {
                    pctbx_espacio7_2.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio7_2.Tag = cartas.Cartas_septimo_espacio[i];
                    cartas.Cartas_septimo_espacio[i].Boca_abajo = true;
                }
                else if (i == 3)
                {
                    pctbx_espacio7_3.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio7_3.Tag = cartas.Cartas_septimo_espacio[i];
                    cartas.Cartas_septimo_espacio[i].Boca_abajo = true;
                }
                else if (i == 4)
                {
                    pctbx_espacio7_4.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio7_4.Tag = cartas.Cartas_septimo_espacio[i];
                    cartas.Cartas_septimo_espacio[i].Boca_abajo = true;
                }
                else if (i == 5)
                {
                    pctbx_espacio7_5.Image = Image.FromFile("..\\..\\Resources\\blue_back.png");
                    pctbx_espacio7_5.Tag = cartas.Cartas_septimo_espacio[i];
                    cartas.Cartas_septimo_espacio[i].Boca_abajo = true;
                }
                else if (i == 6)
                {
                    pctbx_espacio7_6.Image = Image.FromFile(rutaImagen);
                    pctbx_espacio7_6.Tag = cartas.Cartas_septimo_espacio[i];
                }
            }

        }

        
    }
}
