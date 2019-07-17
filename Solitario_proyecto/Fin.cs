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
    public partial class Fin : Form
    {
        private Usuario usuario_entrar;
        private Form1 form1;

        public Fin(int puntos, string tiempo, Usuario usuario_entrar, Form1 form1)
        {
            InitializeComponent();
            lb_puntos.Text = puntos.ToString();
            lb_tiempo.Text = tiempo;
            this.usuario_entrar = usuario_entrar;
            this.form1 = form1;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            form1.Show();
            form1.bandera_inicio = true;
            await form1.iniciar_juego();
            Hide();
            
        }
    }
}
