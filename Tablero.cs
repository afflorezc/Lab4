using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Candy
{

    /*
     * Clase que abstrae el tablero de juego como una matriz de enteros
     * Se asignan valores aleatorios a sus posiciones de acuerdo a diferentes
     * condiciones. Los atributos básicos son la cantidad de filas, columnas, la matriz
     * númerica y el juego se inicializa con la cantidad de caramelos posibles a asignar
     * con otras condiciones basadas en la dificultad
     * contiene los métodos para el manejo de todas las operaciones del juego como son
     * la comprobación de jugadas posibles, el intercambio y actualizacion de posiciones
     * eliminadas
     */
    public class Tablero
    {
        public int cantidadFil { get; set; }
        public int cantidadCol { get; set; }

        public int cantCaramelos { get; set; }
        public int[,] valores { get; set; }

        public int minJugadas { get; set; }

        public List<ParOrdenado> jugadasPosibles;

        public List<PosicionMatriz> filaToEliminar;

        public List<PosicionMatriz> colToEliminar;

        /* constructor básico de la clase, al inicializar el objeto se asignan valores
         * aleatorios a las entradas de la matriz mediante el metodo iniciarJuego
         */
        public Tablero(int cantidadFil, int cantidadCol, int cantCaramelos, 
                                 int minJugadas)
        {
            this.cantidadFil = cantidadFil;
            this.cantidadCol = cantidadCol;
            this.valores = new int[cantidadFil, cantidadCol];
            this.cantCaramelos = cantCaramelos;
            this.minJugadas = minJugadas;
            this.jugadasPosibles = new List<ParOrdenado>(); 
            this.filaToEliminar = new List<PosicionMatriz>();
            this.colToEliminar = new List<PosicionMatriz>();
            iniciarJuego(cantCaramelos, minJugadas);
        }

        /*
         * Se hace una inicialización del juego de acuerdo a la dificultad asignada que
         * establece una cantidad minima de jugadas posibles al inicio del juego
         * de modo que una dificultad mayor implica menor número de jugadas posibles
         * Los paramétros son la cantidad de caramelos y el valor minimo de jugadas iniciales
         */
        private void iniciarJuego(int cantCaramelos, int minJugadas)
        {
            // De acuerdo a la dificultad se garantiza un minimo de jugadas iniciales
            // posible pero no más de 4 jugadas más de ese minimo
            int numJugPos = 0;
            do
            {
                if(jugadasPosibles.Count > 0)
                {
                    jugadasPosibles.Clear();
                }
                asignarValores(cantCaramelos);
                encontrarJugadas();
                // La relación de intercambio es simetrica, el cambio de una celda hacia la 
                // derecha es equivalente a mover la celda de la derecha hacia la izquierda
                // del mismo modo para movimientos verticales
                numJugPos = jugadasPosibles.Count/2;
            } while (numJugPos < minJugadas || numJugPos > minJugadas+5);
            Console.WriteLine($"numero de jugadas: {numJugPos}");
            for(int i=0; i < cantidadFil; i++)
            {
                for(int j=0;j < cantidadCol; j++)
                {
                    Console.Write($"{valores[i, j]} ");
                }
                Console.WriteLine("\n");
            }
            foreach(ParOrdenado jugada in jugadasPosibles)
            {
                PosicionMatriz celdaSal = jugada.x;
                PosicionMatriz celdaLleg = jugada.y;
                Console.WriteLine($"jugada identificada desde posicion ({celdaSal.fila}," +
                              $"{celdaSal.columna}) hacia ({celdaLleg.fila}, " +
                              $"{celdaLleg.columna})");
            }
        }
        /*
         * Método que intercambia un par de celdas dadas por el par ordenado dado en el 
         * paramétro que representa una jugada de intercambio en el tablero
         */
        public void intercambiarCeldas(ParOrdenado jugada)
        {
            PosicionMatriz celdaUno = jugada.x;
            PosicionMatriz celdaDos = jugada.y;
            int aux = valores[celdaUno.fila, celdaUno.columna];
            valores[celdaUno.fila, celdaUno.columna]
                               = valores[celdaDos.fila, celdaDos.columna];
            valores[celdaDos.fila, celdaDos.columna] = aux;
        }
        /*
         * Método que elimina los valores asignados a las celdas que se han registrado
         * previamente que componen una terna, cuarteta u otra posibilidad en una fila
         * Se asigna un valor de -1 a la posición en la matriz para referencia posterior
         */
        private void eliminarFila()
        {
            foreach (PosicionMatriz celda in filaToEliminar)
            {
                valores[celda.fila, celda.columna] = -1;
            } 
            filaToEliminar.Clear();
        }
        /*
        * Método que elimina los valores asignados a las celdas que se han registrado
        * previamente que componen una terna, cuarteta u otra posibilidad en una columna.
        * Se asigna un valor de -1 a la posición en la matriz para referencia posterior
        */
        private void eliminarCol()
        {
            foreach (PosicionMatriz celda in colToEliminar)
            {
                valores[celda.fila, celda.columna] = -1;
            }
            colToEliminar.Clear();
        }
        /*
         * Método que baja los valores de las celdas desde la primera fila hasta el 
         * valor de la fila dada por el parametro fila de acuerdo al valor de la columna
         * col. Asigna el valor a la celda en la primera fila a un nuevo valor aleatorio
         */
        public void bajarCelda(int fila, int col)
        {
            Random random = new Random();
            if(fila == 0)
            {
                valores[fila, col] = random.Next(cantCaramelos);
                return;
            }
            for(int i=fila; i > 0; i--)
            {
                valores[i, col] = valores[i-1, col];
            }
            valores[0, col] = random.Next(cantCaramelos);
        }
        /*
         * Método que llena las celdas que han quedado vacías una vez se han bajado las
         * celdas correspondientes a la posicion (i, j)
         */
        public void llenarVacios(int fila, int col)
        {
            Random random = new Random();
            for(int i =fila; i >= 0; i--)
            {
                if (valores[i, col] == -1)
                {
                    valores[i, col] = random.Next(cantCaramelos);
                }
            }
        }

        /*
         * Genera una distribución inicial del juego de manera que exista posibilidad
         * de encontrar por lo menos una terna consecutiva del mismo tipo de dulces
         * pero que no aparezcan ya juntas dichas ternas, pues en este caso, el juego
         * iniciaría eliminando las ternas (o conjuntos mayores) encontrados inicialmente
         */
        private void asignarValores(int cantCaramelos)
        {
            // creacion de instancia de la clase random para generar los enteros aleatorios
            Random rand = new Random();
            for (int i = 0; i < valores.GetLength(0); i++)
            {
                for (int j = 0; j < valores.GetLength(1); j++)
                {
                    valores[i, j] = rand.Next(cantCaramelos);
                    // Se comprueba que no se de el caso en que se asignen tres valores
                    // consecutivos iguales en una fila o en una columna
                    // en una fila:
                    if (j > 1)
                    {
                        int valorComparacion = valores[i, j - 1];
                        if (valores[i, j - 2] == valorComparacion)
                        {
                            while (valores[i, j] == valorComparacion)
                            {
                                valores[i, j] = rand.Next(cantCaramelos);
                            }
                        }
                    }
                    // en una columna:
                    if (i > 1)
                    {
                        int valorComparacion = valores[i - 1, j];
                        if (valores[i - 2, j] == valorComparacion)
                        {
                            while (valores[i, j] == valorComparacion)
                            {
                                valores[i, j] = rand.Next(cantCaramelos);
                            }
                        }
                    }

                }
            }
        }
        /*
         * Método que recibe como parametros las posiciones (i = filaSal,j = colSal) de una
         * celda o posición en la matriz que se intercambiaría con la celda (i = filaLLeg,
         * j = colLleg), evalua si el par ordenado que representa el intercambio no ha
         * sido asignado a la lista de jugadas posibles y en dicho caso añade dicho
         * par a la lista de jugadas. La relación es simetrica por lo que se añade el
         * par simetrico celdaLleg --> celdaSal
         */
        private void agregarJugada(int filaSal, int colSal, int filaLleg, int colLleg)
        {
            // creacion de los objetos tipo posicion en matriz
            PosicionMatriz celdaSalida =
                                      new PosicionMatriz(filaSal, colSal);
            PosicionMatriz celdaLlegada =
                      new PosicionMatriz(filaLleg, colLleg);
            // Creacion del par ordenado
            ParOrdenado jugada = new ParOrdenado(celdaSalida, celdaLlegada);
            // Solo se añade si la jugada no se ha encontrado antes
            if (!jugadaEnLista(jugada))
            {
                jugadasPosibles.Add(jugada);
            }
            // El par ordenado CeldaLLegada --> CeldaSalida es equivalente, la relación
            // es simetrica
            ParOrdenado jugadaSim = new ParOrdenado(celdaLlegada, celdaSalida);
            if (!jugadaEnLista(jugadaSim))
            {
                jugadasPosibles.Add(jugadaSim);
            }
        }
        /*
         * Método que evalua, dado un par ordenado que representa un movimiento o jugada en
         * el tablero, si este movimiento es una jugada real posible, es decir, si esta en
         * la lista de jugadas que formarían alguna terna al intercambiar las celdas
         * dadas por el par ordenado
         */
        public bool jugadaEnLista(ParOrdenado jugComparacion)
        {
            if(jugadasPosibles == null)
            {
                return false;
            }
            foreach(ParOrdenado jugada in jugadasPosibles)
            {
                if (jugada.esIgual(jugComparacion))
                {
                    return true;
                }
            }
            return false;
        }
        /*
         * Método para comprobar si la celda dada por el parametro pos a se ha añadido en
         * la lista de celdas de la fila a eliminar
         */
        public bool celdaEnListaFila(PosicionMatriz pos)
        {
            foreach(PosicionMatriz celda in filaToEliminar)
            {
                if (celda.celdasIguales(pos))
                {
                    return true;
                }
            }
            return false;
        }
        /*
         * Método para comprobar si la celda dada por el parametro pos a se ha añadido en
         * la lista de celdas de la columna a eliminar
         */
        public bool celdaEnListaColumna(PosicionMatriz pos)
        {
            foreach (PosicionMatriz celda in colToEliminar)
            {
                if (celda.celdasIguales(pos))
                {
                    return true;
                }
            }
            return false;
        }
        /*
         * Método que agrega una posición a la lista de fila o columna a eliminar
         * Se identifica con un entero enumerador que lista se debe usar: el valor 0 para
         * la lista de fila, mientras que el valor 1 corresponde a la lista columna
         */
        private void agregarPosicion(int fila, int columna, int filaOrCol)
        {
            PosicionMatriz celdaAux = new PosicionMatriz(fila, columna);
            switch (filaOrCol)
            {
                case 0:
                    
                    if (!celdaEnListaFila(celdaAux))
                    {
                        filaToEliminar.Add(celdaAux);
                    }
                    break;
                case 1:
                    if (!celdaEnListaColumna(celdaAux))
                    {
                        colToEliminar.Add(celdaAux);
                    }
                    break;
            }
        }
        /*
         * Método para evaluar si al realizar una jugada de la lista se forma, una terna
         * dos ternas, una cuarteta, o cualquier otra combinación posible. Esto de acuerdo
         * a la posición de llegada
         */
        public void ternasFormadas(ParOrdenado jugada)
        {
            PosicionMatriz celdaSal = jugada.x;
            int i = celdaSal.fila;
            int j = celdaSal.columna;
            PosicionMatriz celdaLleg = jugada.y;
            int fila = celdaLleg.fila;
            int col = celdaLleg.columna;
            // evalua terna o cuarteta formada en la fila
            bool bandera = true;
            int fase = 1;
            while (bandera && (col - fase >= 0))
            {
                int cont = 0;
                if (col - fase >= 0 && col - fase != j)
                {
                    if (valores[fila, col - fase] == valores[i, j])
                    {
                        agregarPosicion(fila, col - fase, 0);
                        cont++;
                    }
                }
                if (cont == 0)
                {
                    bandera = false;
                }
                fase++;
            }
            bandera = true;
            fase = 1;
            while(bandera && col + fase < cantidadCol)
            {
                int cont = 0;   
                if (col + fase < cantidadCol && col + fase != j)
                {
                    if (valores[fila, col + fase] == valores[i, j])
                    {
                        agregarPosicion(fila, col + fase, 0);
                        cont++;
                    }
                }
                if (cont == 0)
                {
                    bandera = false;
                }
                fase++;
            }
            // Si la lista no es vacia se debe adicionar la celda que recibe el cambio    
            if(filaToEliminar.Count > 0)
            {
                agregarPosicion(fila, col, 0);
            }
            // evalua terna o cuarteta formada en la columna
            fase = 1;
            bandera = true;
            while (bandera && (fila - fase) >= 0)
            {
                int cont = 0;
                if (fila - fase >= 0 && fila - fase != i)
                {
                    if (valores[fila - fase, col] == valores[i, j])
                    {
                        agregarPosicion(fila -fase, col, 1);
                        cont++;
                    }
                }
                
                if (cont == 0)
                {
                    bandera = false;
                }
                fase++;
            }
            bandera = true;
            fase = 1;
            while(bandera && (fila +fase) < cantidadFil)
            {
                int cont = 0;
                if (fila + fase < cantidadFil && fila + fase != i)
                {
                    if (valores[fila + fase, col] == valores[i, j])
                    {
                        agregarPosicion(fila + fase, col, 1);
                        cont++;
                    }
                }
                if (cont == 0)
                {
                    bandera = false;
                }
                fase++;
            }
            // Si la lista no es vacia se debe de adicionar la celda que recibe el cambio
            if (colToEliminar.Count > 0)
            {
                agregarPosicion(fila, col, 1);
            }

        }

        /*
         * Método que evalua el número de jugadas posibles que se tiene en el tablero actual
         * de acuerdo a la asignación de valores actuales de la matriz que representa la
         * distriucción de caramelos en la pantalla
         */
        private void encontrarJugadas()
        {
            // se evaluan si existen jugadas posicion a posicion
            for (int i = 0; i < valores.GetLength(0); i++)
            {
                for (int j = 0; j < valores.GetLength(1); j++)
                {
                    // se evaluan si existen jugadas a partir de la posicion i,j evaluando
                    // formacion de ternas al intercambiar hacia una fila superior o inferior
                    if (i > 0)
                    {
                        evaluarJugadaEnFila(i, j, i - 1);
                    }
                    if(i< valores.GetLength(0) - 1)
                    {
                        evaluarJugadaEnFila(i, j, i + 1);
                    }
                    // se evaluan si existen jugadas a partir de la posicion i,j evaluando
                    // formacion de ternas al intercambiar hacia una columna anterior o 
                    // posterior
                    if(j > 0)
                    {
                        evaluarJugadaEnColumna(i, j, j - 1);
                    }
                    if(j< valores.GetLength(1) - 1)
                    {
                        evaluarJugadaEnColumna(i, j, j + 1);
                    }
                    
                }
            }
        }
        /*
         * Método que evalua si hay una posible jugada con respecto en una fila superior o
         * en una fila inferior al intercambiar el valor dado por el valor de los parametros
         * (celda posFila, posCol) hacia una fila superior o una fila inferior, siendo el 
         * paramétro filaCambio, la fila hacia la cual se efectúa el cambio
         */
        private void evaluarJugadaEnFila(int posFila, int posCol, int filaCambio)
        {
            // Se evalua si se genera una terna al intercambiar la celda hacia una fila
            // superior o inferior dada por el parametro de filaCambio
            if (posCol > 0)
            {
                // Se evalua la posibilidad de formar una terna hacia la izquierda
                if (valores[filaCambio, posCol - 1] == valores[posFila, posCol])
                {
                    //Se forma terna siendo la celda intercambiada el tercer elemento
                    // a la derecha
                    if (posCol > 1)
                    {
                        if (valores[filaCambio, posCol - 2] == valores[posFila, posCol])
                        {
                            agregarJugada(posFila, posCol, filaCambio, posCol);
                        }
                    }
                    // Se forma terna con la celda intercambiada como el elemento del
                    // medio
                    if (posCol < cantidadCol - 1)
                    {
                        if (valores[filaCambio, posCol + 1] == valores[posFila, posCol])
                        {
                            agregarJugada(posFila, posCol, filaCambio, posCol);
                        }
                    }
                }
            }
            // Se evalua la posibilidad de formar una terna hacia la derecha siendo la celda
            // intercambiada la posición a la izquierda de la terna
            if (posCol < cantidadCol - 2)
            {
                if (valores[filaCambio, posCol + 1] == valores[posFila, posCol])
                {
                    if (valores[filaCambio, posCol + 2] == valores[posFila, posCol])
                    {
                        agregarJugada(posFila, posCol, filaCambio, posCol);
                    }
                }
            }
            // Se evalua la posibilidad de formar una terna en la columna del cambio si
            // la celda se mueve hacia arriba: la terna se forma con las tres filas anteriores
            // a la posicion i, j
            if (posFila >2 && filaCambio < posFila)
            {
                if (valores[filaCambio-1, posCol] == valores[posFila, posCol])
                {
                    if (valores[filaCambio-2, posCol] == valores[posFila, posCol])
                    {
                        agregarJugada(posFila, posCol, filaCambio, posCol);
                    }
                }
            }
            // Se evalua la posibilidad de formar una terna en la columna del cambio si
            // la celda se mueve hacia abajo: la terna se forma con las tres filas siguientes
            // a la posición i, j
            if(posFila < cantidadFil -3 && filaCambio > posFila)
            {
                if (valores[filaCambio +1, posCol] == valores[posFila, posCol])
                {
                    if (valores[filaCambio +2, posCol] == valores[posFila, posCol])
                    {
                        agregarJugada(posFila, posCol, filaCambio, posCol);
                    }
                }
            }
        }
        /*
         * Método que evalua si hay una posible jugada con respecto a una columna posterior o
         * en una columna anterior al intercambiar el valor dado por el valor de los 
         * parametros (celda posFila, posCol) hacia una columna posterior o una columna 
         * anterior, siendo el paramétro colCambio, la columna hacia la cual se efectúa 
         * el cambio
        */
        private void evaluarJugadaEnColumna(int posFila, int posCol, int colCambio)
        {
            // Se evalua si se genera una terna al intercambiar la celda hacia una columna
            // superior o inferior dada por el parametro de colCambio
            if (posFila > 0)
            {
                // Se evalua la posibilidad de formar una terna hacia arriba
                if (valores[posFila - 1, colCambio] == valores[posFila, posCol])
                {
                    //Se forma terna siendo la celda intercambiada el tercer elemento
                    // abajo
                    if (posFila > 1)
                    {
                        if (valores[posFila -2, colCambio] == valores[posFila, posCol])
                        {
                            agregarJugada(posFila, posCol, posFila, colCambio);
                        }
                    }
                    // Se forma terna con la celda intercambiada como el elemento del
                    // medio
                    if (posFila < cantidadFil - 1)
                    {
                        if (valores[posFila + 1, colCambio] == valores[posFila, posCol])
                        {
                            agregarJugada(posFila, posCol, posFila, colCambio);
                        }
                    }
                }
            }
            // Se evalua la posibilidad de formar una terna hacia abajo siendo la celda
            // intercambiada la correspondiente a la fila superior de la terna
            if (posFila < cantidadFil - 2)
            {
                if (valores[posFila +1, colCambio] == valores[posFila, posCol])
                {
                    if (valores[posFila + 2, colCambio] == valores[posFila, posCol])
                    {
                        agregarJugada(posFila, posCol, posFila, colCambio);
                    }
                }
            }
            // Se evalua la posibilidad de formar terna en la fila del cambio hacia la 
            // izquierda, es decir, con las posiciones anteriores a la posicion i, j
            if(posCol > 2 && colCambio < posCol)
            {
                if (valores[posFila, colCambio-1] == valores[posFila, posCol])
                {
                    if (valores[posFila, colCambio -2] == valores[posFila, posCol])
                    {
                        agregarJugada(posFila, posCol, posFila, colCambio);
                    }
                }
            }
            // Se evalua la posibilidad de formar terna en la fila del cambio hacia la 
            // derecha, es decir con las posiciones posteriores a la posición i, j
            if(posCol < cantidadCol -3 && colCambio > posCol)
            {
                if (valores[posFila, colCambio +1] == valores[posFila, posCol])
                {
                    if (valores[posFila, colCambio +2] == valores[posFila, posCol])
                    {
                        agregarJugada(posFila, posCol, posFila, colCambio);
                    }
                }
            }
        }

    }

}
