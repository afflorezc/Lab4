using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq.Expressions;
using System.Windows.Forms;

namespace Candy
{
    /*
     * Clase que define funcionalidades básicas para el manejo de archivos, estos para
     * poder registrar y cargar la información sobre los mayores puntajes y los respectivos
     * jugadores que los han alcanzado
     */
    internal class OperacionesBasicas
    {
        /*
         * Método que crea un archivo con el nombre indicado en su paramétro
         * y lo guarda en el directorio actual. Si el procedimiento es exitoso devuelve 
         * la variale booleana true, en caso contrario retorna el valor false
         */
        public bool crearArchivo(String nombre)
        {
            // Obtención de ruta actual
            String ruta = System.IO.Directory.GetCurrentDirectory();
            Console.WriteLine("Directorio actual: " + ruta);
            // creacion de ruta con nombre de archivo
            ruta = ruta + @"\" + nombre + ".txt";

            //intento de creacción
            try
            {
                using (FileStream fs = File.Create(ruta))
                {
                    fs.Close();
                }
                // el procedimiento fue exitoso
                return true;
            }
            catch (Exception e)
            {
                // se encuentra una excepción y el procedimiento no termina de manera
                // exitosa
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        /*
         * Método para escribir una linea adicional en un archivo de texto dado, recibe
         * como parametros el nombre de archivo que se desea modificar (o crear sino existe)
         * y la cadena de caracteres que representa la linea a escribir. Retorna un valor 
         * booleano que indica si la operación tuvo exito
         */
        public bool escribirLinea(String nombreArchivo, String linea)
        {
            // establecimiento del directorio y ruta para el archivo que se
            // desea modificar
            String ruta = System.IO.Directory.GetCurrentDirectory();
            ruta = ruta + @"\" + nombreArchivo + ".txt";

            // Intento de modificación
            try
            {
                using (StreamWriter sw = File.AppendText(ruta))
                {
                    sw.WriteLine(linea);
                }
                // resultado exitoso
                return true;

            }
            catch (Exception e)
            {
                // Intento fallido que arroja una excepción
                Console.WriteLine(e.ToString());
                return false;
            }
        }
        /*
         * Procedimiento que recibe como paramétros el nombre de un archivo de texto
         * que se desea modificar, la cadena de caracteres que representa la nueva información
         * a escribir y un entero pos que indica el número de la posición que se desea 
         * modificar
         */
        public bool sobreEscribirLinea(String nombreArchivo, String lineaNUeva, int pos)
        {
            // se define una ruta para el archivo a modificar y un archivo copia del
            // original
            String nombreCopia = nombreArchivo + "Copia";
            String ruta = System.IO.Directory.GetCurrentDirectory();
            ruta = ruta + @"\" + nombreArchivo + ".txt";
            // se crea el archivo copia que va a contener toda la información original
            // y en su linea dada por pos será cambiada por la nueva entrada
            if (crearArchivo(nombreCopia))
            {
                try
                {
                    // Lectura linea por linea del archivo original para copia linea por
                    //linea al archivo copia
                    using (StreamReader sr = File.OpenText(ruta))
                    {
                        String linea = "";
                        int i = 0;
                        while ((linea = sr.ReadLine()) != null)
                        {
                            if (i == pos)
                            {
                                // cuando el número de linea coincide con la que se desea
                                // cambiar escribimos la nueva entrada
                                escribirLinea(nombreArchivo, lineaNUeva);
                            }
                            else
                            { 
                                // Las demas lineas se mantienen
                                escribirLinea(nombreArchivo, linea);
                            }
                        }
                        // se cierra el archivo original
                        sr.Close();
                    }
                    // se realiza una copia al archivo de texto original y se elimina
                    // la copia utilizada en el proceso
                    String rutaCopia = System.IO.Directory.GetCurrentDirectory();
                    rutaCopia = rutaCopia + @"\" + nombreCopia + ".txt";
                    File.Copy(rutaCopia, ruta);
                    File.Delete(rutaCopia);
                    // Proceso finaliza de manera exitosa
                    return true;
                }
                catch
                {
                    // El proceso no se pudo desarrollar de manera correcta,
                    // se encuentra alguna excepcion
                    return false;
                }
            }
            else
            {
                // Proceso fallido, no se pudo crear el archivo copia
                return false;
            }
        }
        /*
         * Método que lee la primera linea de un archivo de texto.
         * Ingresa como paramétros el nombre de archivo 
         * y devuelve la linea que se desea leer
         */
        public String leerLinea(String nombreArchivo)
        {
            // Se define la ruta mediante el nombre de archivo y directorio actual
            String ruta = System.IO.Directory.GetCurrentDirectory();
            ruta = ruta + @"\" + nombreArchivo + ".txt";
            // intento de lectura
            try
            {
                using (StreamReader sr = File.OpenText(ruta))
                {
                    // Se hace lectura de la primera linea del archivo
                    String linea = "";
                    linea = sr.ReadLine();
                    return linea;
                }

            }
            catch (Exception e)
            {
                // Proceso fallido se retorna la cadena de texto vacia
                return null;
            }

        }
        /*
         * Método que lee todo el archivo de texto completo cuyo nombre se indica como
         * parámetro de la función. Retorna un vector de cadena de texto String que 
         * contiene cada una de las lineas del archivo leído
         */
        public String[] leerArchivo(String nombreArchivo)
        {
            // definición de la ruta del archivo mediante el directorio actual
            String ruta = System.IO.Directory.GetCurrentDirectory();
            ruta = ruta + @"\" + nombreArchivo + ".txt";
            // Intento de lectura
            try
            {
                // Se hace la lectura de todas las lineas del documento
                String[] result = File.ReadAllLines(ruta);
                return result;
            }
            catch
            {
                // Si el intento es fallido se retorna el vector vacío
                return null;
            }

        }
        /*
         * Método que establece si dado un nombre de un archivo, este es de tipo texto
         * (extensión .txt). Retorna variable booleana de valor true cuando coincide
         * la extensión o false en caso contrario
         */
        public bool esArchivoTexto(String archivo)
        {
            // se identifica el tamaño de la cadena de texto que define el nombre
            // del archivo (en realidad corresponde a la ruta completa)
            int n = archivo.Length - 4;
            // Se obtiene una subcadena de longitud igual a 4 que corresponde a la extensión
            // de un archivo
            String formato = archivo.Substring(n);
            if (formato == ".txt")
            {
                // si el formato o extension coincide con el tipo texto retorna true
                return true;
            }
            else
            {
                // no es archivo de texto
                return false;
            }
        }
        /*
         * Método que evalua si existen archivos de textos en el actual directorio
         * de trabajo. Retorna variable booleana de valor true cuando identifica al
         * menos un archivo de texto en el directorio
         */
        public bool existenArchivos()
        {
            // Se obtinenen todos los nombres (rutas) de los archivos existentes
            // en el actual directorio
            String ruta = System.IO.Directory.GetCurrentDirectory();
            String[] archivos = Directory.GetFiles(ruta);
            foreach (String archivo in archivos)
            {
                if (esArchivoTexto(archivo))
                {
                    // si se identifica al menos uno, se retorna true
                    return true;
                }
            }
            // Si ninguno de los archivos que se identificaron dentro del directorio
            // es un archivo de texto se retorna false
            return false;
        }
        /*
         * Método que tiene como párametros un par de cadenas de texto para generar un cuadro
         * de dialogo para la toma de una desicion a partir de una pregunta. Ejemplo: Desea
         * terminar el Juego?. Se retorna el objeto de tipo DialogResult que almacena el valor
         * seleccionado para posterior procesamiento
         */
        public DialogResult mensajeDesicion(String titulo, String mensaje)
        {
            // Se establecen los botones Si o No para la toma de la desición
            MessageBoxButtons buttons;
            buttons = MessageBoxButtons.YesNo;
            // titulo: Mensaje sobre la ventana (nombre), mensaje: pregunta de desición
            DialogResult result = MessageBox.Show(mensaje, titulo, buttons);
            return result;
        }

        /*
         * Método que muestra un mensaje emergente para informar un detalle de algún proceso.
         * Por ejemplo: "El archivo se ha creado y guardado con exito"
         */
        public void mensajeEmergente(String titulo, String mensaje)
        {
            // Se establece el boton del tipo OK pues no se debe tomar una desición
            MessageBoxButtons buttons;
            buttons = MessageBoxButtons.OK;
            // titulo: Mensaje sobre la ventana (nombre), mensaje: información
            DialogResult result = MessageBox.Show(mensaje, titulo, buttons);
        }
    }
}
