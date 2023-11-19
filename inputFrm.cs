using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Candy
{
    public partial class inputFrm : Form
    {
        public inputFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OperacionesBasicas operBas = new OperacionesBasicas();
            if(jugTextBox.Text == "")
            {
                operBas.mensajeEmergente("Error", "Ingrese por favor su nombre.");
                return;
            }
            String nombre = jugTextBox.Text;
            JuegoPpal.nombreJugador = nombre;
            DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
