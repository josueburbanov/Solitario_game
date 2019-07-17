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
            Form1 form = new Form1(usuario_entrar, form1.cartas_repo);
            await form.iniciar_juego();
            form.dificultad_facil = form1.dificultad_facil;
            form.ShowDialog();
            Hide();

        }
    }
}
