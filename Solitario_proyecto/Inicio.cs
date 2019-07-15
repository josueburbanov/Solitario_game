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
            if(await check_usuario(textBox1.Text, textBox2.Text) != null)
            {
                Form1 form1 = new Form1();
                form1.Visible = true;
                this.Visible = false;
            }
            else
            {
                MessageBox.Show("Error al autenticarse");
            }
        }

        private async Task <Usuario>check_usuario(string usuario, string contrasenia)
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
            return JsonConvert.DeserializeObject<Usuario>(contenidoRespuesta);
        }
    }
}
