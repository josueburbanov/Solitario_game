using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solitario_proyecto
{
    public partial class Form1 : Form
    {
        Baraja cartas;
        public Baraja cartas_repo;
        public bool bandera_inicio;
        int contador_cartas_boca_abajo = 1;
        int puntos = 0;
        Usuario usuario_entrar = new Usuario();
        int contador_seg = 0;
        int contador_min = 0;
        string min = "";
        string seg = "";
        Timer timer = new Timer { Interval = 1000 };
        List<PictureBox> pctbxs_cartas_baraja = new List<PictureBox>();
        public bool dificultad_facil = false;
        UltimoMovimiento ultimoMovimiento;

        private void Tick(object sender, EventArgs e)
        {
            if (contador_seg == 60)
            {
                contador_seg = 0;
                contador_min++;
            }
            contador_seg++;

            if (contador_min < 10) min = "0" + contador_min;
            else min = contador_min.ToString();
            if (contador_seg < 10) seg = "0" + contador_seg;
            else seg = contador_seg.ToString();

            lb_time.Text = min + ":" + seg;
        }

        public Form1(Usuario usuario_entrar, Baraja cartas_repo)
        {
            InitializeComponent();
            this.usuario_entrar = usuario_entrar;
            cartas = cartas_repo;
            lb_usuario.Text = usuario_entrar.Nombre;
            timer.Tick += Tick;

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

            //Cartas en baraja (3)
            pctbxs_cartas_baraja.Add(pctbx_baraja_0);
            pctbxs_cartas_baraja.Add(pctbx_baraja_1);
            pctbxs_cartas_baraja.Add(pctbx_baraja_2);
        }

        private void pctbx_deck_DragEnter(object sender, DragEventArgs e)
        {
            PictureBox picture_arrastrado = e.Data.GetData("System.Windows.Forms.PictureBox") as PictureBox;
            PictureBox picture_caido = sender as PictureBox;


            bool carta_movida = false;
            if (picture_caido.Image == null)
            {
                Carta carta_arrastrada = (Carta)picture_arrastrado.Tag;
                Carta carta_caida = (Carta)picture_caido.Tag;
                if (carta_arrastrada.Valor == 1)
                {
                    Guardar_ultimo_mov(picture_arrastrado, picture_caido, carta_arrastrada, carta_caida, puntos);
                    picture_arrastrado.Location = new Point(picture_caido.Location.X, picture_caido.Location.Y);
                    picture_arrastrado.BringToFront();
                    if (dificultad_facil)
                    {
                        puntos += 10;
                    }
                    else
                    {
                        puntos += 20;
                    }

                    lb_puntos.Text = puntos.ToString();
                    carta_movida = true;
                    picture_arrastrado.MouseDown -= new MouseEventHandler(pctbx_baraja_0_MouseDown);
                    picture_arrastrado.AllowDrop = true;
                    picture_arrastrado.DragEnter += pctbx_deck_DragEnter;
                    if (!pctbxs_cartas_baraja.Contains(picture_arrastrado)) revelar_carta_detras_arrastrada(carta_arrastrada);
                }
            }
            else
            {
                Carta carta_arrastrada = (Carta)picture_arrastrado.Tag;
                Carta carta_caida = (Carta)picture_caido.Tag;
                if (carta_arrastrada.Valor == carta_caida.Valor + 1 && carta_arrastrada.Palo.Equals(carta_caida.Palo))
                {
                    Guardar_ultimo_mov(picture_arrastrado, picture_caido, carta_arrastrada, carta_caida, puntos);
                    picture_arrastrado.Location = new Point(picture_caido.Location.X, picture_caido.Location.Y);
                    picture_arrastrado.BringToFront();
                    picture_arrastrado.AllowDrop = true;
                    picture_arrastrado.DragEnter += pctbx_deck_DragEnter;
                    if (!pctbxs_cartas_baraja.Contains(picture_arrastrado)) revelar_carta_detras_arrastrada(carta_arrastrada);
                    if (dificultad_facil)
                    {
                        puntos += 10;
                    }
                    else
                    {
                        puntos += 20;
                    }
                    lb_puntos.Text = puntos.ToString();
                    carta_movida = true;
                    picture_arrastrado.MouseDown -= new MouseEventHandler(pctbx_baraja_0_MouseDown);
                }
            }

            generar_nuevo_pctbx_carta_baraja(picture_arrastrado, carta_movida);

        }

        private void Guardar_ultimo_mov(PictureBox picture_arrastrado, PictureBox picture_caido, Carta carta_arrastrada, Carta carta_caida, int puntos)
        {
            if (dificultad_facil)
            {
                puntos = 10;
            }
            else if (puntos == 0)
            {
                puntos = 0;
            }
            else
            {
                puntos = 20;
            }
            ultimoMovimiento = new UltimoMovimiento(picture_arrastrado, picture_caido, carta_arrastrada, carta_caida, puntos);
            picture_arrastrado.Location = new Point(picture_arrastrado.Location.X, picture_arrastrado.Location.Y);
            picture_arrastrado.BringToFront();
            if (dificultad_facil)
            {
                puntos -= 10;
            }
            else
            {
                puntos -= 20;
            }

            lb_puntos.Text = puntos.ToString();
            picture_arrastrado.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
            picture_arrastrado.AllowDrop = false;
            picture_arrastrado.DragEnter -= pctbx_deck_DragEnter;
            if (!pctbxs_cartas_baraja.Contains(picture_arrastrado)) revelar_carta_detras_arrastrada(carta_arrastrada);
        }

        private bool generar_nuevo_pctbx_carta_baraja(PictureBox picture_arrastrado, bool carta_movida)
        {
            if (pctbxs_cartas_baraja.Contains(picture_arrastrado) && carta_movida)
            {
                PictureBox picture_box_nuevo = new PictureBox();
                picture_box_nuevo.Location = new Point(348, 104);
                picture_box_nuevo.Name = "pctbx_" +
                    cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Palo + "_" +
                    cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Simbolo;
                picture_box_nuevo.Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
                picture_box_nuevo.Size = new Size(94, 132);
                picture_box_nuevo.SizeMode = PictureBoxSizeMode.StretchImage;
                Controls.Add(picture_box_nuevo);
                picture_box_nuevo.BringToFront();
                picture_box_nuevo.Tag = cartas.Cartas_boca_abajo[contador_cartas_boca_abajo];
                cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Boca_abajo = false;
                picture_box_nuevo.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);


                cartas.Cartas_boca_abajo.Remove((Carta)picture_arrastrado.Tag);
                pctbxs_cartas_baraja.Remove(picture_arrastrado);
                pctbxs_cartas_baraja.Add(picture_box_nuevo);

                contador_restar(1);
                contador_restar(1);
                pctbxs_cartas_baraja[0].Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
                pctbxs_cartas_baraja[0].Tag = cartas.Cartas_boca_abajo[contador_cartas_boca_abajo];
                contador_sumar(1);
                pctbxs_cartas_baraja[1].Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
                pctbxs_cartas_baraja[1].Tag = cartas.Cartas_boca_abajo[contador_cartas_boca_abajo];
                contador_sumar(1);
                pctbxs_cartas_baraja[2].Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
                pctbxs_cartas_baraja[2].Tag = cartas.Cartas_boca_abajo[contador_cartas_boca_abajo];
                contador_restar(1);

                return true;
            }
            return false;
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

        private async Task<Baraja> barajar_cartas()
        {
            RestClient cliente = new RestClient("http://localhost:29485/Service1.svc");
            var solicitud = new RestRequest(Method.GET);
            solicitud.Resource = "/barajar_cartas";
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

                    if (picture_caido.Tag == null && carta_arrastrada.Valor == 13)
                    {
                        picture_arrastrado.Location = new Point(picture_caido.Location.X, picture_caido.Location.Y);
                        picture_arrastrado.BringToFront();
                        revelar_carta_detras_arrastrada(carta_arrastrada);
                    }
                    if (colocar_carta_caida(picture_arrastrado, picture_caido, carta_arrastrada, carta_caida))
                    {

                        if (!generar_nuevo_pctbx_carta_baraja(picture_arrastrado, true))
                        {
                            revelar_carta_detras_arrastrada(carta_arrastrada);
                        }
                        else
                        {
                            picture_arrastrado.AllowDrop = true;
                            picture_arrastrado.DragEnter += pctbx_boca_abajo_DragEnter;
                            carta_arrastrada.Boca_abajo = false;
                            picture_arrastrado.MouseDown += new MouseEventHandler(pctbx_baraja_0_MouseDown);
                        }
                        if (carta_arrastrada.CartasDependientes.Count != 0)
                        {
                            colocar_cartas_dependientes(carta_arrastrada);
                        }
                        ultimoMovimiento.carta_arrastrada = carta_arrastrada;
                        ultimoMovimiento.carta_caida = carta_caida;
                        ultimoMovimiento.picture_arrastrado = picture_arrastrado;
                        ultimoMovimiento.picture_caido = picture_caido;
                        ultimoMovimiento.puntos = 0;

                    }
                }

            }
        }

        private void colocar_cartas_dependientes(Carta carta_arrastrada)
        {
            Carta carta_aux_arrastrada = carta_arrastrada;
            foreach (var item in carta_arrastrada.CartasDependientes)
            {
                colocar_carta_caida(buscar_picture_box(item), buscar_picture_box(carta_aux_arrastrada), item, carta_aux_arrastrada);
                carta_aux_arrastrada = item;
                if (item.CartasDependientes.Count != 0)
                {
                    colocar_cartas_dependientes(item);
                }
            }
        }

        private List<Carta> buscar_lista(Carta carta_caida)
        {
            if (cartas.Cartas_primer_espacio.Contains(carta_caida)) return cartas.Cartas_primer_espacio;
            if (cartas.Cartas_segundo_espacio.Contains(carta_caida)) return cartas.Cartas_segundo_espacio;
            if (cartas.Cartas_tercer_espacio.Contains(carta_caida)) return cartas.Cartas_tercer_espacio;
            if (cartas.Cartas_cuarto_espacio.Contains(carta_caida)) return cartas.Cartas_cuarto_espacio;
            if (cartas.Cartas_quinto_espacio.Contains(carta_caida)) return cartas.Cartas_quinto_espacio;
            if (cartas.Cartas_sexto_espacio.Contains(carta_caida)) return cartas.Cartas_sexto_espacio;
            if (cartas.Cartas_septimo_espacio.Contains(carta_caida)) return cartas.Cartas_septimo_espacio;
            return null;

        }

        private void revelar_carta_detras_arrastrada(Carta carta_arrastrada)
        {
            PictureBox picture_carta_detras;
            if (cartas.Cartas_primer_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_primer_espacio.Remove(carta_arrastrada);

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
                    picture_box_vacio.AllowDrop = true;
                    picture_box_vacio.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_box_vacio.BackColor = Color.Transparent;
                    Controls.Add(picture_box_vacio);
                }
            }
            else if (cartas.Cartas_segundo_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_segundo_espacio.Remove(carta_arrastrada);
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
                    picture_box_vacio.AllowDrop = true;
                    picture_box_vacio.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_box_vacio.BackColor = Color.Transparent;
                    Controls.Add(picture_box_vacio);
                }
            }
            else if (cartas.Cartas_tercer_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_tercer_espacio.Remove(carta_arrastrada);
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
                    picture_box_vacio.AllowDrop = true;
                    picture_box_vacio.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_box_vacio.BackColor = Color.Transparent;
                    Controls.Add(picture_box_vacio);
                }
            }
            else if (cartas.Cartas_cuarto_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_cuarto_espacio.Remove(carta_arrastrada);
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
                    picture_box_vacio.AllowDrop = true;
                    picture_box_vacio.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_box_vacio.BackColor = Color.Transparent;
                    Controls.Add(picture_box_vacio);
                }
            }
            else if (cartas.Cartas_quinto_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_quinto_espacio.Remove(carta_arrastrada);
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
                    picture_box_vacio.AllowDrop = true;
                    picture_box_vacio.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_box_vacio.BackColor = Color.Transparent;
                    Controls.Add(picture_box_vacio);
                }
            }
            else if (cartas.Cartas_sexto_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_sexto_espacio.Remove(carta_arrastrada);
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
                    picture_box_vacio.AllowDrop = true;
                    picture_box_vacio.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_box_vacio.BackColor = Color.Transparent;
                    Controls.Add(picture_box_vacio);
                }
            }
            else if (cartas.Cartas_septimo_espacio.Contains(carta_arrastrada))
            {
                cartas.Cartas_septimo_espacio.Remove(carta_arrastrada);
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
                    picture_box_vacio.AllowDrop = true;
                    picture_box_vacio.DragEnter += pctbx_boca_abajo_DragEnter;
                    picture_box_vacio.BackColor = Color.Transparent;
                    Controls.Add(picture_box_vacio);
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
            if (carta_arrastrada.Valor == 13 && picture_caido.Image == null) return true;
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
            if (dificultad_facil) return true;
            bool comparacion = false;
            carta_arrastrada.Simbolo = Regex.Replace(carta_arrastrada.Simbolo, @"\s+", "");
            carta_arrastrada.Palo = Regex.Replace(carta_arrastrada.Palo, @"\s+", "");
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
            return comparacion;
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
            if (cartas == null)
            {
                await iniciar_juego();
            }
        }


        public async Task iniciar_juego()
        {
            timer.Start();
            if (cartas == null)
            {
                cartas = await cargar_recursos();
                cartas_repo = cartas.DeepCopy();
                bandera_inicio = true;
            }
            else
            {
                contador_min = 0;
                contador_seg = 0;
                bandera_inicio = false;
                cartas.Cartas_boca_abajo.Shuffle();
                cartas.Cartas_primer_espacio.Shuffle();
                cartas.Cartas_segundo_espacio.Shuffle();
                cartas.Cartas_tercer_espacio.Shuffle();
                cartas.Cartas_cuarto_espacio.Shuffle();
                cartas.Cartas_quinto_espacio.Shuffle();
                cartas.Cartas_sexto_espacio.Shuffle();
                cartas.Cartas_septimo_espacio.Shuffle();
            }

            foreach (var item in cartas.Cartas_primer_espacio)
            {
                item.Simbolo = Regex.Replace(item.Simbolo, @"\s+", "");
                item.Palo = Regex.Replace(item.Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + item.Simbolo + "_" + item.Palo + ".png";
                if (bandera_inicio)
                    File.WriteAllBytes(rutaImagen, item.Imagen);
                item.Ruta_imagen = rutaImagen;
                //item.Imagen = null;
                pctbx_espacio1.Image = Image.FromFile(rutaImagen);
                pctbx_espacio1.Tag = item;
                item.Boca_abajo = false;
            }

            for (int i = 0; i < cartas.Cartas_segundo_espacio.Count; i++)
            {
                cartas.Cartas_segundo_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_segundo_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_segundo_espacio[i].Palo = Regex.Replace(cartas.Cartas_segundo_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_segundo_espacio[i].Simbolo + "_" + cartas.Cartas_segundo_espacio[i].Palo + ".png";
                if (bandera_inicio)
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
                    cartas.Cartas_segundo_espacio[i].Boca_abajo = false;
                }
            }

            for (int i = 0; i < cartas.Cartas_tercer_espacio.Count; i++)
            {
                cartas.Cartas_tercer_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_tercer_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_tercer_espacio[i].Palo = Regex.Replace(cartas.Cartas_tercer_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_tercer_espacio[i].Simbolo + "_" + cartas.Cartas_tercer_espacio[i].Palo + ".png";
                if (bandera_inicio)
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
                    cartas.Cartas_tercer_espacio[i].Boca_abajo = false;
                }

            }

            for (int i = 0; i < cartas.Cartas_cuarto_espacio.Count; i++)
            {
                cartas.Cartas_cuarto_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_cuarto_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_cuarto_espacio[i].Palo = Regex.Replace(cartas.Cartas_cuarto_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_cuarto_espacio[i].Simbolo + "_" + cartas.Cartas_cuarto_espacio[i].Palo + ".png";
                if (bandera_inicio)
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
                    cartas.Cartas_cuarto_espacio[i].Boca_abajo = false;
                }
            }

            for (int i = 0; i < cartas.Cartas_quinto_espacio.Count; i++)
            {
                cartas.Cartas_quinto_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_quinto_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_quinto_espacio[i].Palo = Regex.Replace(cartas.Cartas_quinto_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_quinto_espacio[i].Simbolo + "_" + cartas.Cartas_quinto_espacio[i].Palo + ".png";
                if (bandera_inicio)
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
                    cartas.Cartas_quinto_espacio[i].Boca_abajo = false;
                }
            }

            for (int i = 0; i < cartas.Cartas_sexto_espacio.Count; i++)
            {
                cartas.Cartas_sexto_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_sexto_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_sexto_espacio[i].Palo = Regex.Replace(cartas.Cartas_sexto_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_sexto_espacio[i].Simbolo + "_" + cartas.Cartas_sexto_espacio[i].Palo + ".png";
                if (bandera_inicio)
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
                    cartas.Cartas_sexto_espacio[i].Boca_abajo = false;
                }
            }

            for (int i = 0; i < cartas.Cartas_septimo_espacio.Count; i++)
            {
                cartas.Cartas_septimo_espacio[i].Simbolo = Regex.Replace(cartas.Cartas_septimo_espacio[i].Simbolo, @"\s+", "");
                cartas.Cartas_septimo_espacio[i].Palo = Regex.Replace(cartas.Cartas_septimo_espacio[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_septimo_espacio[i].Simbolo + "_" + cartas.Cartas_septimo_espacio[i].Palo + ".png";
                if (bandera_inicio)
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
                    cartas.Cartas_septimo_espacio[i].Boca_abajo = false;
                }
            }
            for (int i = 0; i < cartas.Cartas_boca_abajo.Count; i++)
            {
                cartas.Cartas_boca_abajo[i].Simbolo = Regex.Replace(cartas.Cartas_boca_abajo[i].Simbolo, @"\s+", "");
                cartas.Cartas_boca_abajo[i].Palo = Regex.Replace(cartas.Cartas_boca_abajo[i].Palo, @"\s+", "");
                string rutaImagen = "..\\..\\Resources\\" + cartas.Cartas_boca_abajo[i].Simbolo + "_" + cartas.Cartas_boca_abajo[i].Palo + ".png";
                if (bandera_inicio)
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
                    break;
                }
            }
        }

        private void pctbx_baraja_Click(object sender, EventArgs e)
        {
            if (cartas == null)
            {
                MessageBox.Show("Primero inicie el juego", "Información", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (contador_cartas_boca_abajo == cartas.Cartas_boca_abajo.Count)
                contador_cartas_boca_abajo = 0;
            pctbxs_cartas_baraja[0].Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
            pctbxs_cartas_baraja[0].Tag = cartas.Cartas_boca_abajo[contador_cartas_boca_abajo];
            contador_cartas_boca_abajo++;
            if (contador_cartas_boca_abajo == cartas.Cartas_boca_abajo.Count)
                contador_cartas_boca_abajo = 0;
            pctbxs_cartas_baraja[1].Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
            pctbxs_cartas_baraja[1].Tag = cartas.Cartas_boca_abajo[contador_cartas_boca_abajo];
            contador_cartas_boca_abajo++;
            if (contador_cartas_boca_abajo == cartas.Cartas_boca_abajo.Count)
                contador_cartas_boca_abajo = 0;
            pctbxs_cartas_baraja[2].Image = Image.FromFile(cartas.Cartas_boca_abajo[contador_cartas_boca_abajo].Ruta_imagen);
            pctbxs_cartas_baraja[2].Tag = cartas.Cartas_boca_abajo[contador_cartas_boca_abajo];
            if (contador_cartas_boca_abajo != 0)
            {
                contador_cartas_boca_abajo--;
            }
            else
            {
                contador_cartas_boca_abajo = cartas.Cartas_boca_abajo.Count - 1;
            }

        }

        private int contador_sumar(int i)
        {
            if (contador_cartas_boca_abajo == cartas.Cartas_boca_abajo.Count)
            {
                contador_cartas_boca_abajo = 0;
            }
            else
            {
                contador_cartas_boca_abajo = contador_cartas_boca_abajo + i;
            }
            return contador_cartas_boca_abajo;
        }

        private int contador_restar(int i)
        {
            if (contador_cartas_boca_abajo == 0)
            {
                contador_cartas_boca_abajo = cartas.Cartas_boca_abajo.Count;
            }
            else
            {
                contador_cartas_boca_abajo = contador_cartas_boca_abajo - i;
            }
            return contador_cartas_boca_abajo;

        }

        private int contador()
        {
            return contador_cartas_boca_abajo;
        }

        private void iniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void terminarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fin form1 = new Fin(puntos, min + ":" + seg, usuario_entrar, this);
            Hide();
            form1.ShowDialog();
            Close();
        }

        private async void reiniciarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(usuario_entrar, cartas_repo);
            await form1.iniciar_juego();
            form1.dificultad_facil = dificultad_facil;
            form1.ShowDialog();
            Hide();
        }

        private async void fácilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            bandera_inicio = true;
            dificultad_facil = true;
            lb_dificultad.Text = "Fácil";
            if (cartas == null)
            {
                await iniciar_juego();
            }
        }

        private async void difícilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lb_dificultad.Text = "Difícil";
            dificultad_facil = false;
            if (cartas == null)
            {
                await iniciar_juego();
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.Z)
            {
                if (ultimoMovimiento != null)
                {
                    DialogResult respuesta = MessageBox.Show("¿Seguro de deshacer movimiento?", "Deshacer", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (respuesta == DialogResult.OK)
                    {

                    }
                }

            }

        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void deshacerMovimientoCtrlZToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ultimoMovimiento != null)
            {
                DialogResult respuesta = MessageBox.Show("¿Seguro de deshacer movimiento?", "Deshacer", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (respuesta == DialogResult.OK)
                {

                }
            }
        }
    }
}
