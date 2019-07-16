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

        public Fin(int puntos, string tiempo, Usuario usuario_entrar)
        {
            InitializeComponent();
            lb_puntos.Text = puntos.ToString();
            lb_tiempo.Text = tiempo;
            this.usuario_entrar = usuario_entrar;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1(usuario_entrar);
            Hide();
            form1.ShowDialog();
            Close();
        }
    }
}
