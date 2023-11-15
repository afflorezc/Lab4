using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Candy
{
    // Se define un enumerador de dificultad para asignar el tamaño del tablero de juego
    enum Dificultad
    {
        Facil,
        Medio,
        Dificil,
        Supremo
    }

    /*
     * Formulario principal del juego de Candy crush. Se utilizan las variables
     * globales de tipo tablero y Operaciones Basicas que permiten aplicar las diferentes
     * funcionalidades de cada una. La clase operaciones básicas incluye una serie de
     * métodos para el manejo de archivos y formularios emergentes para registrar
     * datos del jugador
     */
    public partial class JuegoPpal : Form
    {
        Tablero tab;
        // Matriz de imágenes
        PictureBox[,] matPictureBox;
        // Vector que indica las dimensiones de ancho y alto de cada picture box
        int[] sizePictureBox;
        int filasJuego;
        int colJuego;
        int cantCaramelos;
        int numMinJugadas;
        int puntaje;
        PosicionMatriz celdaSeleccionada;
        Dificultad dificultad;
        OperacionesBasicas operBas;
        const String nombreArchivo = "Puntajes.txt";
        public JuegoPpal()
        {
            //Método generado automaticamente
            InitializeComponent();
            // se identifica si existen puntajes registrados y se cargan
            operBas = new OperacionesBasicas();
            if (operBas.existenArchivos())
            {
                cargarPuntajes();
            }
            //Se inicializan las dificultad estandar como facil y se asignan los valores 
            // apropiadas
            dificultad = Dificultad.Supremo;
            asignarVariablesJuego();            

        }

        /*
         * Método que abre y lee el archivo de texto que almacena la información de los mejores
         * puntajes registrados
         */
        void cargarPuntajes()
        {
            
            String[] lineasArchivo = operBas.leerArchivo(nombreArchivo);
            if(lineasArchivo != null)
            {
                int n = lineasArchivo.Length;
                for(int i = 0; i < n; i++)
                {
                    String linea = lineasArchivo[i];
                    int inicio = linea.IndexOf(':');
                    int longitud = inicio - 1;
                    String subCadena = linea.Substring(inicio + 2);
                    String identificador = linea.Substring(0, longitud);
                    puntajesGridView.Rows.Add(identificador, subCadena);
                }
            }
            
        }

        /*
         * Método que asigna las variables principales del juego de acuerdo a la
         * dificultad seleccionada mediante el boton opciones. Si no se ha seleccionado
         * la dificultad de inicio es facil 
         */
        void asignarVariablesJuego()
        {
            switch (dificultad)
            {
                case Dificultad.Facil:
                    this.filasJuego = 12;
                    this.colJuego = 12;
                    this.cantCaramelos = 6;
                    this.numMinJugadas = 10;
                    break;
                case Dificultad.Medio:
                    this.filasJuego = 10;
                    this.colJuego = 10;
                    this.cantCaramelos = 6;
                    this.numMinJugadas = 8;
                    break;
                case Dificultad.Dificil:
                    this.filasJuego= 8;
                    this.colJuego = 8;
                    this.cantCaramelos= 6;
                    this.numMinJugadas = 4;
                    break;
                case Dificultad.Supremo:
                    this.filasJuego = 8;
                    this.colJuego = 8;
                    this.cantCaramelos= 6;
                    this.numMinJugadas = 2;
                    break;
                default:
                    this.filasJuego = 12;
                    this.colJuego = 12;
                    this.cantCaramelos = 6;
                    this.numMinJugadas = 10;
                    break;
            }
        }

        /*
         * Metodo que ajusta el TableLayoutPanel que representara el area de juego.
         * Asigna el número de filas y columnas de acuerdo al tamaño del tablero
         * seleccionado (por nivel de dificultad) y ajusta sus tamaños de igual tamaño
         * y se establecen el ancho y alto de las imagenes a usar en el TableLayoutPanel
         */
        public void organizarTableroDeJuego()
        {
            //Ajusta el TableLayoutPanel del juego principal a la cantidad de filas
            // y columnas requeridas
            tableroPpal.ColumnCount = tab.cantidadCol;
            tableroPpal.RowCount = tab.cantidadFil;

            // Ajusta el tamaño de filas y columnas a tamaños iguales
            // evalua primero el porcentaje del largo o ancho total requerido
            float porcentajeFil = 100f/tab.cantidadFil;
            float porcentajeCol = 100f/tab.cantidadCol;

            // Se asigna el tamaño de cada fila y cada columna en pixeles de acuerdo
            // al tamaño del TableLaoyoutPanel
            sizePictureBox = new int[2];
            sizePictureBox[0] = (int)((porcentajeFil / 100) * tableroPpal.Height);
            sizePictureBox[1] = (int)((porcentajeCol / 100) * tableroPpal.Width);

            // se asigna el número de filas y columnas del TableLayout 
            TableLayoutRowStyleCollection estilosFil = tableroPpal.RowStyles;
            TableLayoutColumnStyleCollection estilosCol = tableroPpal.ColumnStyles;

            // Ajuste del tamaño de cada fila y columna del TableLayout
            foreach(RowStyle estiloFil in estilosFil) 
            {
                estiloFil.SizeType = SizeType.Absolute;
                estiloFil.Height = sizePictureBox[0];
            }

            foreach (ColumnStyle estiloCol in estilosCol)
            {
                estiloCol.SizeType = SizeType.Absolute;
                estiloCol.Width = sizePictureBox[1];
            }

        }

        /*
         * Método que inicializa el tablero de juego de acuerdo a la matriz del objeto
         * tipo tablero al asignar valores a una matriz de objetos tipo PictureBox
         * que dibujaran cada caramelo
         */
        public void pintarTablero()
        {

            //se inicializa la matriz de imágenes
            matPictureBox = new PictureBox[tab.cantidadFil, tab.cantidadCol];
            // se obtienen las dimensiones de cada imagen y a su vez se organiza el
            //tablero de juego
            organizarTableroDeJuego();
            
            int alto = sizePictureBox[0];
            int ancho = sizePictureBox[1];
            
            // Creación y ubicación de cada una de las imagenes en cada celda (i, j)
            for (int i = 0; i < tab.cantidadFil; i++)
            {
                for (int j = 0; j < tab.cantidadCol; j++)
                {
                    Image recurso = seleccionarRecursoCaramelo(tab.valores[i, j]);
                    asignarPicture(i, j, recurso);
                }
            }

        }
        /*
         * Método que hace una asignación de una PictureBox a la matriz de imagenes en la
         * posición establecida
         */
        private void asignarPicture(int i, int j, Image imagenRecurso)
        {
            int alto = sizePictureBox[0];
            int ancho = sizePictureBox[1];
            PictureBox pictureAux = new PictureBox();
            pictureAux.Image = imagenRecurso;
            pictureAux.Location = new System.Drawing.Point(0, 0);
            pictureAux.Name = $"pictureBox{i},{j}";
            pictureAux.Size = new System.Drawing.Size(ancho, alto);
            // cada imagen se ubica extendida a todo el tamaño de cada celda
            // sin margenes
            pictureAux.Margin = new Padding(0);
            pictureAux.Anchor = (AnchorStyles.Left | AnchorStyles.Right |
                                  AnchorStyles.Top | AnchorStyles.Bottom);
            pictureAux.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // si ya existe una imagen en el TableLayoutPanel se elimina para
            // luego reemplazar
            if (tableroPpal.Controls.Contains(matPictureBox[i, j]))
            {
                tableroPpal.Controls.Remove(matPictureBox[i, j]);
            }
            matPictureBox[i, j] = pictureAux;
            // Se adiciona la nueva imagen
            tableroPpal.Controls.Add(pictureAux, j, i);
        }
        /*
         * Método que asigna el puntaje adecuado de acuerdo a la formación de una terna,
         * una cuarteta, etc
         */
        private void asignarPuntaje(int celdas)
        {
            switch (celdas)
            {
                case 3:
                    puntaje += 6;
                    break;
                case 4:
                    puntaje += 10;
                    break;
                case 5:
                    puntaje += 15;
                    break;
                default:
                    if(celdas > 5)
                    {
                        puntaje += 3 * celdas;
                    }
                    break;
            }
        }

        /*
         * Método que hace un intercambio entre dos posiciones de la matriz del tablero y
         * reasigna las imagenes respectivas, el cambio se efectúa solo si el intercambio
         * corresponde a una jugada posible en el tablero de juego
         */
        private void intercambiarPosiciones(ParOrdenado jugada)
        {
            // Se hace el intercambio de imagenes
            PosicionMatriz celdaUno = jugada.x;
            PosicionMatriz celdaDos = jugada.y;
            Image pictureCeldaUno =
                     matPictureBox[celdaUno.fila, celdaUno.columna].Image;
            Image pictureCeldaDos =
                     matPictureBox[celdaDos.fila, celdaDos.columna].Image;
            asignarPicture(celdaUno.fila, celdaUno.columna, pictureCeldaDos);
            asignarPicture(celdaDos.fila, celdaDos.columna, pictureCeldaUno);
            // Se aplica una pequeña pausa para simular animacion
            Thread.Sleep(10);
            // El cambio solamente se mantiene si es una jugada válida
            if (tab.jugadaEnLista(jugada))
            {
                eliminarCeldas(jugada);
                // Se evaluan si los movimientos de bajar celdas generan ternas

                tab.jugadasPosibles.Remove(jugada);
                if (tab.jugadaEnLista(jugada))
                {
                    Console.WriteLine("No se pudo eliminar la jugada de la lista");
                }

            }
            else
            {
                // Se reasigna el cambio
                asignarPicture(celdaUno.fila, celdaUno.columna, pictureCeldaUno);
                asignarPicture(celdaDos.fila, celdaDos.columna, pictureCeldaDos);
                // Se hace penalización del puntaje. Nunca se decrementa por debajo de cero
                if (puntaje > 0)
                {
                    if(puntaje - 5 >= 0)
                    {
                        puntaje -= 5;
                    }
                    else
                    {
                        puntaje = 0;
                    }
                }
            }
        }
        /*
         * Método que identifica las celdas que componen una terna, cuerteta, quinteta o
         * cualquier otra combinación y ejecuta el proceso de eliminarlas del tablero
         * bajar cada una de las filas superiores y genera nuevos valores cuando sea 
         * necesario
         */
        private void eliminarCeldas(ParOrdenado jugada)
        {
            // Se obtienen primero las ternas formadas en el tablero de juego
            tab.ternasFormadas(jugada);
            // Se hace el intercambio
            tab.intercambiarCeldas(jugada);
            // se usan las listas encontradas para actualizar la imagen de cada celda que
            // se va a eliminar y se realiza el conteo para sumar el puntaje adecuado
            int cont = 0;
            int filaInferior = 0;
            int filaSuperior = tab.cantidadFil;
            Image picture = seleccionarRecursoCaramelo(-1);
            foreach (PosicionMatriz celda in tab.filaToEliminar)
            {
                if (celda.fila > filaInferior)
                {
                    filaInferior = celda.fila;
                }
                if (celda.fila < filaSuperior)
                {
                    filaSuperior = celda.fila;
                }
                tab.valores[celda.fila, celda.columna] = -1;
                asignarPicture(celda.fila, celda.columna, picture);
                cont++;
            }
            foreach (PosicionMatriz celda in tab.colToEliminar)
            {
                if (celda.fila > filaInferior)
                {
                    filaInferior = celda.fila;
                }
                if (celda.fila < filaSuperior)
                {
                    filaSuperior = celda.fila;
                }
                tab.valores[celda.fila, celda.columna] = -1;
                asignarPicture(celda.fila, celda.columna, picture);
                cont++;
            }
            // se asigna el puntaje obtenido
            asignarPuntaje(cont);
            // Se hace una pausa para simular animación
            Thread.Sleep(500);
            // Se bajan las celdas superiores a las celdas eliminadas
            for (int i = filaInferior; i >= filaSuperior; i--)
            {
                for (int j = 0; j < tab.cantidadCol; j++)
                {
                    PosicionMatriz celda = new PosicionMatriz(i, j);
                    if (tab.celdaEnListaFila(celda) || tab.celdaEnListaColumna(celda))
                    {
                        tab.bajarCelda(i, j);
                        for (int k = i; k >= 0; k--)
                        {
                            picture = seleccionarRecursoCaramelo(tab.valores[k, j]);
                            asignarPicture(k, j, picture);
                        }
                    }
                }
            }
            for (int i = 0; i < tab.cantidadCol; i++)
            {
                int fila = filaInferior;
                while (fila >= filaSuperior)
                {
                    PosicionMatriz celda = new PosicionMatriz(fila, i);
                    if (tab.celdaEnListaColumna(celda))
                    {
                        tab.llenarVacios(fila, i);
                        for (int k = fila; k >= 0; k--)
                        {
                            picture = seleccionarRecursoCaramelo(tab.valores[k, i]);
                            asignarPicture(k, i, picture);
                        }
                        i = tab.cantidadCol;
                    }
                    fila--;
                }
            }
            // Se limpian las listas de celdas a eliminar
            tab.filaToEliminar.Clear();
            tab.colToEliminar.Clear();
        }

        /*
         * método que da inicio a un juego nuevo. Inicialmente se muestra un cuadro de
         * dialogo tipo InputBox (con una entrada de texto) para que el usuario ingrese
         * su nombre que aparecerá en la pantalla de Juego. El tamaño de juego depende de la
         * dificultad seleccionada mediante el boton de opciones
         */
        private void nuevoJuegoBtn_Click(object sender, EventArgs e)
        {
            // Se inicializa la posicion en matriz con un valor equivalente al vacio
            celdaSeleccionada = new PosicionMatriz(-1, -1);
            // Se inicializa el puntaje en cero
            puntaje = 0;
            tab = new Tablero(filasJuego, colJuego, cantCaramelos, numMinJugadas);
            pintarTablero();
            celdaPuntero();
                        
        }
        /*
         * Método que reasigna una imagen para la posición i,j de la matriz del tablero
         * para indicar que se ha señalado una celda
         */
        private void imagenSeleccionada(int fila, int col)
        {
            int recurso = tab.valores[fila, col];
            Image picture;
            switch(recurso)
            {
                case 0:
                   picture = global::Candy.Properties.Resources._0a;
                    break;
                case 1:
                    picture = global::Candy.Properties.Resources._1a;
                    break;
                case 2:
                    picture = global::Candy.Properties.Resources._2b;
                    break;
                case 3:
                    picture = global::Candy.Properties.Resources._3b;
                    break;
                case 4:
                    picture = global::Candy.Properties.Resources._4b;
                    break;
                default:
                    picture = global::Candy.Properties.Resources._5b;
                    break;
            }
            asignarPicture(fila, col, picture);
        }


        /*
         * Método que selecciona la imagen del caramelo a asignar de acuerdo al valor
         * númerico existente en la posición correspondiente en la matriz del objeto
         * tablero
         */
        private Image seleccionarRecursoCaramelo(int recurso)
        {
            switch (recurso)
            {
                case -1:
                    return global::Candy.Properties.Resources.elimina;
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

        private void opcionesBtn_Click(object sender, EventArgs e)
        {

        }
        /*
         * Método que evalua cuando se hace click sobre un objeto del tipo PictureBox
         * del tablero de juego, es decir, cuando se selecciona una celda del juego con
         * el objetivo de aplicar una jugada a partir de intercambiar esta celda a sus
         * posiciones vecinas
         */
        private void celdaPuntero()
        {
            foreach(Control control in tableroPpal.Controls)
            {
                control.MouseClick += (s, e) =>
                {
                    String nombre = control.Name;
                    // establecer la celda
                    String posicion = nombre.Substring(10);
                    int posComa = posicion.IndexOf(",");
                    int fila = int.Parse(posicion.Substring(0, posComa));
                    int col = int.Parse(posicion.Substring(posComa + 1));
                    if (celdaSeleccionada.fila >-1 && celdaSeleccionada.columna > -1)
                    {
                        int recurso
                          = tab.valores[celdaSeleccionada.fila, celdaSeleccionada.columna];
                        Image picture = seleccionarRecursoCaramelo(recurso);
                        asignarPicture(celdaSeleccionada.fila, celdaSeleccionada.columna,
                                       picture);
                        // Si la celda clickeada es vecina da la celda previamente seleccionada
                        // se evalua la posibilidad de intercambiar las celdas como jugada valida
                        if (((fila == celdaSeleccionada.fila -1 ||
                             fila == celdaSeleccionada.fila +1) 
                             && col == celdaSeleccionada.columna) ||
                             ((col == celdaSeleccionada.columna - 1 ||
                             col == celdaSeleccionada.columna + 1)
                             && fila == celdaSeleccionada.fila))
                        {
                            PosicionMatriz celda = new PosicionMatriz(fila, col);
                            ParOrdenado jugada = new ParOrdenado(celdaSeleccionada, celda);
                            intercambiarPosiciones(jugada);
                            // Se reinicia el estado de la celda seleccionada al valor indicador
                            // de no celdas en seleccion
                            celdaSeleccionada.fila = -1;
                            celdaSeleccionada.columna = -1;
                        }
                        // en caso contrario se señala la nueva celda
                        else
                        {
                            imagenSeleccionada(fila, col);
                            celdaSeleccionada.fila = fila;
                            celdaSeleccionada.columna = col;
                        }
                        return;
                    }
                    else
                    {
                        // Se selecciona la celda clickeada
                        celdaSeleccionada.fila = fila;
                        celdaSeleccionada.columna = col;
                        imagenSeleccionada(fila, col);
                        Console.WriteLine(posicion);
                        return;
                    }
                   
                };                
            }
        }
    }
}
