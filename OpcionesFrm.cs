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
    public partial class OpcionesFrm : Form
    {

        private void guardarBtn_Click(object sender, EventArgs e)
        {
            Dificultad dificultad = new Dificultad();
            if (facilRadBtn.Checked)
            {
                dificultad = Dificultad.Facil;
                
            }
            else if (medioRadBtn.Checked)
            {
                dificultad = Dificultad.Medio;
            }
            else if (dificilRadBtn.Checked)
            {
                dificultad = Dificultad.Dificil;
            }
            else if (supremoRadBtn.Checked)
            {
                dificultad = Dificultad.Supremo;
            }
            JuegoPpal.dificultad = dificultad;
            DialogResult = DialogResult.OK;
            this.Close();
        }

        public OpcionesFrm()
        {
            InitializeComponent();
            facilRadBtn.Checked = true;
        }

        private void cancelarBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
