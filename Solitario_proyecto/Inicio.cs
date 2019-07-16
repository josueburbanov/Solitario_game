using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Solitario_proyecto
{
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Usuario usuario_entrar = await check_usuario(textBox1.Text, textBox2.Text);
            if (usuario_entrar != null)
            {
                Form1 form1 = new Form1(usuario_entrar);
                Hide();
                form1.ShowDialog();
                Close();
            }
            else
            {
                MessageBox.Show("Error al autenticarse", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task <Usuario>check_usuario(string usuario, string contrasenia)
        {
            RestClient cliente = new RestClient("http://localhost:29485/Service1.svc");
            var solicitud = new RestRequest(Method.GET);
            solicitud.Resource = "/ingresar/usuario/"+usuario+"/contrasenia/"+contrasenia;
            solicitud.RequestFormat = DataFormat.Json;
            var respuesta = await cliente.ExecuteTaskAsync(solicitud);
            var contenidoRespuesta = respuesta.Content;
            if (!respuesta.IsSuccessful)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Usuario>(contenidoRespuesta);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lb_nombre.Visible = true;
            tx_nombre.Visible = true;
            lb_usuario.Visible = true;
            tx_usuario.Visible = true;
            lb_correo.Visible = true;
            tx_correo.Visible = true;
            lb_contra.Visible = true;
            tx_contra.Visible = true;
            btn_aceptar.Visible = true;

        }

        private async void btn_aceptar_Click(object sender, EventArgs e)
        {
            Usuario user = new Usuario { Nombre = tx_nombre.Text,
                Nombre_usuario = tx_usuario.Text, Contrasenia = tx_contra.Text,
                Correo = tx_correo.Text };
            Usuario usuario_registrado = await registrar_usuario(user);
            if(usuario_registrado != null)
            {
                lb_nombre.Visible = false;
                tx_nombre.Visible = false;
                lb_usuario.Visible = false;
                tx_usuario.Visible = false;
                lb_correo.Visible = false;
                tx_correo.Visible = false;
                lb_contra.Visible = false;
                tx_contra.Visible = false;
                btn_aceptar.Visible = false;
                MessageBox.Show("Registro correcto", "Correcto", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error al registrarse", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private async Task<Usuario> registrar_usuario(Usuario usuario)
        {
            RestClient cliente = new RestClient("http://localhost:29485/Service1.svc");
            var solicitud = new RestRequest(Method.GET);
            solicitud.Resource = "/registrar/usuario/" + usuario.Nombre_usuario + "/nombre/" + usuario.Nombre
                + "/contrasenia/" + usuario.Contrasenia + "/correo/"+usuario.Correo;
            solicitud.RequestFormat = DataFormat.Json;
            var respuesta = await cliente.ExecuteTaskAsync(solicitud);
            var contenidoRespuesta = respuesta.Content;
            if (!respuesta.IsSuccessful)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Usuario>(contenidoRespuesta);
        }
    }
}
