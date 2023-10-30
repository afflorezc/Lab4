using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candy
{
    public class Tablero
    {
        public int cantidadFil {  get; set; }
        public int cantidadCol { get; set; }
        public int[,] valores { get; set; }
        public Tablero(int cantidadFil, int cantidadCol, int cantCaramelos)
        {
            this.cantidadFil = cantidadFil;
            this.cantidadCol = cantidadCol;
            this.valores = new int[cantidadFil,cantidadCol];
            iniciarJuego(cantCaramelos);
        }

        private void iniciarJuego(int cantCaramelos)
        {
            Random rand = new Random();
            for (int i = 0; i < valores.GetLength(0); i++)
            {
                for(int j = 0; j < valores.GetLength(1); j++)
                {
                    valores[i,j] = rand.Next(cantCaramelos);
                }
            }
        }
    }
     
}
