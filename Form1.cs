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
    public partial class Form1 : Form
    {
        Tablero tab;
        public Form1()
        {
            //Método generado automaticamente
            InitializeComponent();
            //Mi objeto tablero
            tab = new Tablero(8, 8, 6);
            
        }

        public void pintarTablero()
        {
            //Matriz de imágenes
            PictureBox[,] matPictureBox = new PictureBox[tab.cantidadFil, tab.cantidadCol];
            
            int y = 25;
            for (int i = 0; i < tab.cantidadFil; i++)
            {
                int x = 25;
                for (int j = 0; j < tab.cantidadCol; j++)
                {
                    PictureBox pictureAux = new PictureBox();
                    pictureAux.Image = seleccionarRecursoCaramelo(tab.valores[i,j]);
                    pictureAux.Location = new System.Drawing.Point(x, y);
                    pictureAux.Name = $"pictureBox{i}{j}";
                    pictureAux.Size = new System.Drawing.Size(50, 50);
                    pictureAux.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    matPictureBox[i, j] = pictureAux;
                    //Mostrar en form
                    Controls.Add(pictureAux);
                    x += 50;
                }
                y += 50;
            }          

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pintarTablero();
        }

        private Image seleccionarRecursoCaramelo(int recurso)
        {
            switch(recurso)
            {
                case 0:
                    return global::Candy.Properties.Resources._0;
                case 1:
                    return global::Candy.Properties.Resources._1;
                case 2:
                    return global::Candy.Properties.Resources._2;
                case 3:
                    return global::Candy.Properties.Resources._3;
                case 4:
                    return global::Candy.Properties.Resources._4;
                default:
                    return global::Candy.Properties.Resources._5;
            }
        }
    }
}
