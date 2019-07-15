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
        Baraja cartas = new Baraja();
        int contador_cartas_boca_abajo = 0;
        int puntos = 0;

        public Form1()
        {
            InitializeComponent();


            //Cartas que arrancan con la posibilidad de ser caídas
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

            pctbx_deck1.AllowDrop = true;
            pctbx_deck1.DragEnter += pctbx_deck_DragEnter;

            pctbx_deck2.AllowDrop = true;
            pctbx_deck2.DragEnter += pctbx_deck_DragEnter;

            pctbx_deck3.AllowDrop = true;
            pctbx_deck3.DragEnter += pctbx_deck_DragEnter;

            pctbx_deck4.AllowDrop = true;
            pctbx_deck4.DragEnter += pctbx_deck_DragEnter;

            //Cartas que arrancan con la posibilidad de ser arrastradas
            pctbx_espacio1.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio2_1.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio3_2.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio4_3.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio5_4.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio6_5.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            pctbx_espacio7_6.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
        }

        private void pctbx_deck_DragEnter(object sender, DragEventArgs e)
        {
            PictureBox picture_arrastrado = e.Data.GetData("System.Windows.Forms.PictureBox") as PictureBox;
            PictureBox picture_caido = sender as PictureBox;
            if (picture_arrastrado == pctbx_baraja_0 || picture_arrastrado == pctbx_baraja_1 || picture_arrastrado == pctbx_baraja_2)
            {

                PictureBox picture_box_vacio = new PictureBox();
                picture_box_vacio.Location = new Point(348, 104);
                picture_box_vacio.Name = "pctbx_" +
                    cartas.Cartas_boca_abajo[cartas.Cartas_boca_abajo.IndexOf((Carta)picture_arrastrado.Tag)].Palo + "_" +
                    cartas.Cartas_boca_abajo[cartas.Cartas_boca_abajo.IndexOf((Carta)picture_arrastrado.Tag)].Simbolo;
                picture_box_vacio.Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
                contador_cartas_boca_abajo++;
                picture_box_vacio.Size = new Size(94, 132);
                picture_box_vacio.SizeMode = PictureBoxSizeMode.StretchImage;
                cartas.Cartas_boca_abajo.Remove((Carta)picture_arrastrado.Tag);
            }

            if (picture_caido.Image == null)
            {
                Carta carta_arrastrada = (Carta)picture_arrastrado.Tag;
                Carta carta_caida = (Carta)picture_caido.Tag;
                if (carta_arrastrada.Valor == 1)
                {
                    picture_arrastrado.Location = new Point(picture_caido.Location.X, picture_caido.Location.Y);
                    picture_arrastrado.BringToFront();
                    revelar_carta_detras_arrastrada(carta_arrastrada);
                }
            }
            else
            {
                Carta carta_arrastrada = (Carta)picture_arrastrado.Tag;
                Carta carta_caida = (Carta)picture_caido.Tag;
                if (carta_arrastrada.Valor == carta_caida.Valor + 1 && carta_arrastrada.Palo.Equals(carta_caida.Palo))
                {
                    picture_arrastrado.Location = new Point(picture_caido.Location.X, picture_caido.Location.Y);
                    picture_arrastrado.BringToFront();
                    revelar_carta_detras_arrastrada(carta_arrastrada);
                }
            }
        }

        private async Task<Baraja> cargar_recursos()
        {
            RestClient cliente = new RestClient("http://localhost:29485/Service1.svc");
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
            PictureBox picture_arrastrado = e.Data.GetData("System.Windows.Forms.PictureBox") as PictureBox;
            PictureBox picture_caido = sender as PictureBox;

            if (picture_caido != picture_arrastrado)
            {
                Carta carta_arrastrada = (Carta)picture_arrastrado.Tag;
                Carta carta_caida = (Carta)picture_caido.Tag;

                if (!carta_arrastrada.Boca_abajo)
                {
                    if (colocar_carta_caida(picture_arrastrado, picture_caido, carta_arrastrada, carta_caida))
                    {
                        revelar_carta_detras_arrastrada(carta_arrastrada);
                        if (carta_arrastrada.CartasDependientes.Count != 0)
                        {
                            Carta carta_aux_arrastrada = carta_arrastrada;
                            foreach (var item in carta_arrastrada.CartasDependientes)
                            {
                                colocar_carta_caida(buscar_picture_box(item), buscar_picture_box(carta_aux_arrastrada), item, carta_aux_arrastrada);
                                carta_aux_arrastrada = item;
                            }
                        }

                    }
                }
            }
        }

        private void revelar_carta_detras_arrastrada(Carta carta_arrastrada)
        {
            PictureBox picture_carta_detras;
            if (cartas.Cartas_primer_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_primer_espacio.Remove(carta_arrastrada);
                if (carta_arrastrada.CartasDependientes.Count != 0)
                    cartas.Cartas_primer_espacio.RemoveRange(0, carta_arrastrada.CartasDependientes.Count);

                if (cartas.Cartas_primer_espacio.Count != 0)
                {
                    picture_carta_detras = buscar_picture_box(cartas.Cartas_primer_espacio.Last());
                    picture_carta_detras.Image = Image.FromFile(cartas.Cartas_primer_espacio.Last().Ruta_imagen);

                    picture_carta_detras.AllowDrop = true;
                    picture_carta_detras.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_carta_detras.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
                    cartas.Cartas_primer_espacio.Last().Boca_abajo = false;
                    picture_carta_detras.Tag = cartas.Cartas_primer_espacio.Last();
                }
                else
                {
                    PictureBox picture_box_vacio = new PictureBox();
                    picture_box_vacio.Location = new Point(102, 336);
                    picture_box_vacio.Name = "pctbx_espacio1_vacio";
                    picture_box_vacio.Size = new Size(94, 132);
                    picture_box_vacio.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box_vacio.BorderStyle = BorderStyle.Fixed3D;
                    picture_box_vacio.TabIndex = 8;
                    picture_box_vacio.TabStop = false;
                }
            }
            else if (cartas.Cartas_segundo_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_segundo_espacio.Remove(carta_arrastrada);
                if (carta_arrastrada.CartasDependientes.Count != 0)
                    cartas.Cartas_segundo_espacio.RemoveRange(0, carta_arrastrada.CartasDependientes.Count);
                if (cartas.Cartas_segundo_espacio.Count != 0)
                {
                    picture_carta_detras = buscar_picture_box(cartas.Cartas_segundo_espacio.Last());
                    picture_carta_detras.Image = Image.FromFile(cartas.Cartas_segundo_espacio.Last().Ruta_imagen);
                    picture_carta_detras.AllowDrop = true;
                    picture_carta_detras.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_carta_detras.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
                    cartas.Cartas_segundo_espacio.Last().Boca_abajo = false;
                    picture_carta_detras.Tag = cartas.Cartas_segundo_espacio.Last();
                }
                else
                {
                    PictureBox picture_box_vacio = new PictureBox();
                    picture_box_vacio.Location = new Point(277, 336);
                    picture_box_vacio.Name = "pctbx_espacio2_vacio";
                    picture_box_vacio.Size = new Size(94, 132);
                    picture_box_vacio.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box_vacio.BorderStyle = BorderStyle.Fixed3D;
                    picture_box_vacio.TabIndex = 9;
                    picture_box_vacio.TabStop = false;
                }
            }
            else if (cartas.Cartas_tercer_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_tercer_espacio.Remove(carta_arrastrada);
                if (carta_arrastrada.CartasDependientes.Count != 0)
                    cartas.Cartas_tercer_espacio.RemoveRange(0, carta_arrastrada.CartasDependientes.Count);
                if (cartas.Cartas_tercer_espacio.Count != 0)
                {
                    picture_carta_detras = buscar_picture_box(cartas.Cartas_tercer_espacio.Last());
                    picture_carta_detras.Image = Image.FromFile(cartas.Cartas_tercer_espacio.Last().Ruta_imagen);
                    picture_carta_detras.AllowDrop = true;
                    picture_carta_detras.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_carta_detras.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
                    cartas.Cartas_tercer_espacio.Last().Boca_abajo = false;
                    picture_carta_detras.Tag = cartas.Cartas_tercer_espacio.Last();
                }
                else
                {
                    PictureBox picture_box_vacio = new PictureBox();
                    picture_box_vacio.Location = new Point(442, 336);
                    picture_box_vacio.Name = "pctbx_espacio3_vacio";
                    picture_box_vacio.Size = new Size(94, 132);
                    picture_box_vacio.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box_vacio.BorderStyle = BorderStyle.Fixed3D;
                    picture_box_vacio.TabIndex = 9;
                    picture_box_vacio.TabStop = false;
                }
            }
            else if (cartas.Cartas_cuarto_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_cuarto_espacio.Remove(carta_arrastrada);
                if (carta_arrastrada.CartasDependientes.Count != 0)
                    cartas.Cartas_cuarto_espacio.RemoveRange(0, carta_arrastrada.CartasDependientes.Count);
                if (cartas.Cartas_cuarto_espacio.Count != 0)
                {
                    picture_carta_detras = buscar_picture_box(cartas.Cartas_cuarto_espacio.Last());
                    picture_carta_detras.Image = Image.FromFile(cartas.Cartas_cuarto_espacio.Last().Ruta_imagen);
                    picture_carta_detras.AllowDrop = true;
                    picture_carta_detras.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_carta_detras.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
                    cartas.Cartas_cuarto_espacio.Last().Boca_abajo = false;
                    picture_carta_detras.Tag = cartas.Cartas_cuarto_espacio.Last();
                }
                else
                {
                    PictureBox picture_box_vacio = new PictureBox();
                    picture_box_vacio.Location = new Point(608, 336);
                    picture_box_vacio.Name = "pctbx_espacio4_vacio";
                    picture_box_vacio.Size = new Size(94, 132);
                    picture_box_vacio.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box_vacio.BorderStyle = BorderStyle.Fixed3D;
                    picture_box_vacio.TabIndex = 9;
                    picture_box_vacio.TabStop = false;
                }
            }
            else if (cartas.Cartas_quinto_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_quinto_espacio.Remove(carta_arrastrada);
                if (carta_arrastrada.CartasDependientes.Count != 0)
                    cartas.Cartas_quinto_espacio.RemoveRange(0, carta_arrastrada.CartasDependientes.Count);
                if (cartas.Cartas_quinto_espacio.Count != 0)
                {
                    picture_carta_detras = buscar_picture_box(cartas.Cartas_quinto_espacio.Last());
                    picture_carta_detras.Image = Image.FromFile(cartas.Cartas_quinto_espacio.Last().Ruta_imagen);
                    picture_carta_detras.AllowDrop = true;
                    picture_carta_detras.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_carta_detras.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
                    cartas.Cartas_quinto_espacio.Last().Boca_abajo = false;
                    picture_carta_detras.Tag = cartas.Cartas_quinto_espacio.Last();
                }
                else
                {
                    PictureBox picture_box_vacio = new PictureBox();
                    picture_box_vacio.Location = new Point(763, 336);
                    picture_box_vacio.Name = "pctbx_espacio5_vacio";
                    picture_box_vacio.Size = new Size(94, 132);
                    picture_box_vacio.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box_vacio.BorderStyle = BorderStyle.Fixed3D;
                    picture_box_vacio.TabIndex = 9;
                    picture_box_vacio.TabStop = false;
                }
            }
            else if (cartas.Cartas_sexto_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_sexto_espacio.Remove(carta_arrastrada);
                if (carta_arrastrada.CartasDependientes.Count != 0)
                    cartas.Cartas_sexto_espacio.RemoveRange(0, carta_arrastrada.CartasDependientes.Count);
                if (cartas.Cartas_sexto_espacio.Count != 0)
                {
                    picture_carta_detras = buscar_picture_box(cartas.Cartas_sexto_espacio.Last());
                    picture_carta_detras.Image = Image.FromFile(cartas.Cartas_sexto_espacio.Last().Ruta_imagen);
                    picture_carta_detras.AllowDrop = true;
                    picture_carta_detras.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_carta_detras.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
                    cartas.Cartas_sexto_espacio.Last().Boca_abajo = false;
                    picture_carta_detras.Tag = cartas.Cartas_sexto_espacio.Last();
                }
                else
                {
                    PictureBox picture_box_vacio = new PictureBox();
                    picture_box_vacio.Location = new Point(763, 336);
                    picture_box_vacio.Name = "pctbx_espacio5_vacio";
                    picture_box_vacio.Size = new Size(94, 132);
                    picture_box_vacio.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box_vacio.BorderStyle = BorderStyle.Fixed3D;
                    picture_box_vacio.TabIndex = 9;
                    picture_box_vacio.TabStop = false;
                }
            }
            else if (cartas.Cartas_septimo_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_septimo_espacio.Remove(carta_arrastrada);
                if (carta_arrastrada.CartasDependientes.Count != 0)
                    cartas.Cartas_septimo_espacio.RemoveRange(0, carta_arrastrada.CartasDependientes.Count);
                if (cartas.Cartas_septimo_espacio.Count != 0)
                {
                    picture_carta_detras = buscar_picture_box(cartas.Cartas_septimo_espacio.Last());
                    picture_carta_detras.Image = Image.FromFile(cartas.Cartas_septimo_espacio.Last().Ruta_imagen);
                    picture_carta_detras.AllowDrop = true;
                    picture_carta_detras.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_carta_detras.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
                    cartas.Cartas_septimo_espacio.Last().Boca_abajo = false;
                    picture_carta_detras.Tag = cartas.Cartas_septimo_espacio.Last();
                }
                else
                {
                    PictureBox picture_box_vacio = new PictureBox();
                    picture_box_vacio.Location = new Point(763, 336);
                    picture_box_vacio.Name = "pctbx_espacio5_vacio";
                    picture_box_vacio.Size = new Size(94, 132);
                    picture_box_vacio.SizeMode = PictureBoxSizeMode.StretchImage;
                    picture_box_vacio.BorderStyle = BorderStyle.Fixed3D;
                    picture_box_vacio.TabIndex = 9;
                    picture_box_vacio.TabStop = false;
                }
            }
        }

        private PictureBox buscar_picture_box(Carta tag)
        {
            foreach (var item in Controls.OfType<PictureBox>())
            {
                if (item.Tag == tag)
                {
                    return item;
                }
            }
            return null;

        }

        private bool colocar_carta_caida(PictureBox picture_arrastrado, PictureBox picture_caido, Carta carta_arrastrada, Carta carta_caida)
        {
            if (carta_arrastrada.Valor + 1 == carta_caida.Valor && chequear_colores(carta_arrastrada, carta_caida))
            {
                picture_arrastrado.Location = new Point(picture_caido.Location.X, picture_caido.Location.Y + 20);
                picture_arrastrado.BringToFront();
                carta_caida.CartasDependientes.Add(carta_arrastrada);
                if (carta_arrastrada.CartasDependientes.Count != 0)
                    carta_caida.CartasDependientes.AddRange(carta_arrastrada.CartasDependientes);
                return true;
            }
            return false;
        }

        private bool chequear_colores(Carta carta_arrastrada, Carta carta_caida)
        {
            bool comparacion = false;
            if (carta_arrastrada.Palo == "corazones")
            {
                comparacion = carta_caida.Palo == "treboles" || carta_caida.Palo == "negros";
            }
            if (carta_arrastrada.Palo == "treboles")
            {
                comparacion = carta_caida.Palo == "corazones" || carta_caida.Palo == "diamantes";
            }
            if (carta_arrastrada.Palo == "negros")
            {
                comparacion = carta_caida.Palo == "corazones" || carta_caida.Palo == "diamantes";
            }
            if (carta_arrastrada.Palo == "diamantes")
            {
                comparacion = carta_caida.Palo == "treboles" || carta_caida.Palo == "negros";
            }
            return true;
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
        private void pctbx_baraja_2_MouseDown(object sender, MouseEventArgs e)
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
            cartas = await cargar_recursos();
            foreach (var item in cartas.Cartas_total)
            {
                File.WriteAllBytes("/Resources", item.Imagen);
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
                pctbx_espacio1.Tag = item;
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
                else if (i == 1)
                {
                    pctbx_espacio2_1.Image = Image.FromFile(rutaImagen);
                    pctbx_espacio2_1.Tag = cartas.Cartas_segundo_espacio[i];
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
                }
                else if (i == 1)
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
            for (int i = 0; i < cartas.Cartas_boca_abajo.Count; i++)
            {
                cartas.Cartas_boca_abajo[i].Simbolo = Regex.Replace(cartas.Cartas_boca_abajo[i].Simbolo, @"\s+", "");
                cartas.Cartas_boca_abajo[i].Palo = Regex.Replace(cartas.Cartas_boca_abajo[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_boca_abajo[i].Simbolo + "_" + cartas.Cartas_boca_abajo[i].Palo + ".png";
                File.WriteAllBytes(rutaImagen, cartas.Cartas_boca_abajo[i].Imagen);
                cartas.Cartas_boca_abajo[i].Ruta_imagen = rutaImagen;
                //item.Imagen = null;
                if (i == 0)
                {
                    pctbx_baraja_0.Image = Image.FromFile(rutaImagen);
                    pctbx_baraja_0.Tag = cartas.Cartas_boca_abajo[i];
                }
                else if (i == 1)
                {
                    pctbx_baraja_1.Image = Image.FromFile(rutaImagen);
                    pctbx_baraja_1.Tag = cartas.Cartas_boca_abajo[i];
                }
                else if (i == 2)
                {
                    pctbx_baraja_2.Image = Image.FromFile(rutaImagen);
                    pctbx_baraja_2.Tag = cartas.Cartas_boca_abajo[i];
                }
            }
        }

        private void pctbx_baraja_Click(object sender, EventArgs e)
        {
            if (contador_cartas_boca_abajo == 24) contador_cartas_boca_abajo = 1;
            pctbx_baraja_0.Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
            contador_cartas_boca_abajo++;
            if (contador_cartas_boca_abajo == 24) contador_cartas_boca_abajo = 1;
            pctbx_baraja_1.Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
            contador_cartas_boca_abajo++;
            if (contador_cartas_boca_abajo == 24) contador_cartas_boca_abajo = 1;
            pctbx_baraja_2.Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
            contador_cartas_boca_abajo--;
        }


    }
}
