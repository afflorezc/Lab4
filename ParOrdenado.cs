using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Candy
{
    /*
     * Clase abstracta para representar una posición del tablero (matriz) de juego.
     * Sus atributos son la fila y columna correspondientes de la posición o celda en 
     * cuestion
     */
    public class PosicionMatriz
    {
        public int fila;
        public int columna;

        /*
         * Constructor del objeto de tipo celda mediante los enteros que indican la 
         * correspondiente fila y columna que representa la posicion en tablero (matriz)
         */
        public PosicionMatriz(int fila, int columna)
        {
            this.fila = fila;
            this.columna = columna;
        }

        /*
         * Método de comparación de igualdad entre dos objetos de tipo posicion matriz (celda)
         * Los objetos son iguales si sus atributos correspondientes son iguales
         */
        public bool celdasIguales(PosicionMatriz celda2)
        {
            // La igualdad se cumple si y solo si las filas y las columnas coinciden
            if(this.fila == celda2.fila && this.columna == celda2.columna)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    /*
     * Clase que abstrae el comportamiento matemático de un par ordenado que representa una
     * relación entre objetos del tipo posicionMatriz que indicará las jugadas posibles.
     * el par ordenado se compone de una celda de salida x y una celda de llegada y, donde se
     * abstrae la relacion x -> y que indica que la posicion x de la matriz se cambiaría por
     * la posición y de la matriz (x e y son del tipo posicionMatriz o celdas)
     */
    public class ParOrdenado
    {
        public PosicionMatriz x;
        public PosicionMatriz y;

        /*
         * Método constructor que recibe como paramétros dos posiciones en tablero distintas
         * en el orden de la relación x -> y con el significado previamente establecido
         */
        public ParOrdenado(PosicionMatriz x, PosicionMatriz y)
        { 
            this.x = x;
            this.y = y;
        }

        /*
         * Método de comparación de igualdad entre pares ordenados. La igualdad se cumple si
         * cada componente de los pares ordenados se corresponden entre si x1 = x y y1=y2
         */
        public bool esIgual(ParOrdenado parDos)
        {
            // Cada par ordenado se compone de un par de objetos tipo "celda" o posicion en matriz
            // se establce una relacion del tipo x -> y (celda salida -> celda llegada)
            PosicionMatriz celdaSalidaPrimerPar = this.x;
            PosicionMatriz celdaLlegadaPrimerPar = this.y;
            PosicionMatriz celdaSalidaSegundoPar = parDos.x;
            PosicionMatriz celdaLlegadaSegundoPar = parDos.y;

            // los pares ordenados son iguales solo en caso en que cada elemento de salida
            // se corresponda con el otro y del mismo modo cada elemento de llegada
            if (celdaSalidaPrimerPar.celdasIguales(celdaSalidaSegundoPar))
            {
                if (celdaLlegadaPrimerPar.celdasIguales(celdaLlegadaSegundoPar))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
