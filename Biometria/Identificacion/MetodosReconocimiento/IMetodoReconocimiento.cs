using Emgu.CV.Face;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identificacion
{
    public interface IMetodoReconocimiento
    {
        /// <summary>
        /// Numero de componentes a utlizar por el predictor
        /// </summary>
        /// <param name="nComponentes"></param>
        void SetNumeroComponentes(int nComponentes);

        /// <summary>
        /// Numero de componentes a utlizar por el predictor
        /// </summary>
        /// <param name="nComponentes"></param>
        int getNumeroComponentes();

        /// <summary>
        /// Establece el archivo de entrenamiento para el predictor
        /// </summary>
        /// <param name="ArchivoEntrenamiento"></param>
        void SetArchivoDeEntrenamiento(string ArchivoEntrenamiento);

        /// <summary>
        /// Identifica a la persona de la imagen especificada utilizando los datos de entrenamiento cargados previamente.
        /// </summary>
        /// <param name="Distancia">Distancia</param>
        /// <param name="IM">Imagen de la persona a identificar.</param>
        /// <param name="ID_Buscar">ID de la persona que se le asignó al momento de realizar el entrenamiento.</param>
        /// <param name="Encontrado">Resultado de la identificacion. Verdadero si la identificacion fue exitosa.</param>
        /// <param name="DistanciaObtenida">Distancia obtenida por el reconocedor.</param>
        /// <returns>Imagen de la persona identificada con un recuadro en el rostro</returns>
        Bitmap Identificar(double Distancia, Bitmap IM, int ID_Buscar, ref bool Encontrado,ref double DistanciaObtenida);

        /// <summary>
        /// Genera una carpeta con los datos de entrenamiento.
        /// </summary>
        /// <param name="ID_Persona">ID de la persona a entrenar.</param>
        /// <param name="rutaFotosEntrenamiento">Ruta completa de las fotos de la persona a entrenar.</param>
        /// <param name="rutaResultados">Ruta completa donde se guardaran los datos de entrenamiento, en la carpeta se guardaran las imagenes de los rostros con las que se entrenó, el archivo
        /// con los datos de entrenamiento (.trn) y un archivo con informacion adicional (.txt)</param>
        void Entrenar(int ID_Persona, string rutaFotosEntrenamiento, string rutaResultados);

        /// <summary>
        /// Genera una carpeta con los datos de entrenamiento.
        /// </summary>
        /// <param name="ID_Persona">ID de la persona a entrenar.</param>
        /// <param name="rutaFotosEntrenamiento">Ruta completa de las fotos de la persona a entrenar.</param>
        /// <param name="cantidadDeFotosUtilizar">Cantidad de fotos que se utilizaran para el entrenamiento.</param>
        /// <param name="rutaResultados">Ruta completa donde se guardaran los datos de entrenamiento, en la carpeta se guardaran las imagenes de los rostros con las que se entrenó, el archivo
        /// con los datos de entrenamiento (.trn) y un archivo con informacion adicional (.txt)</param>
        void Entrenar(int ID_Persona, string rutaFotosEntrenamiento,int cantidadDeFotosUtilizar, string rutaResultados);


        /// <summary>
        /// Genera una carpeta con los datos de entrenamiento para la identificacion de multiples personas.
        /// </summary>
        /// <param name="rutaFotosEntrenamiento">Ruta completa de las fotos de las personas a entrenar. Dentro de esta carpeta debe haber una subcarpeta por cada persona y el nombre de la misma
        /// debe ser el numero de ID que se le asigna a la persona.</param>
        /// <param name="rutaResultados">Ruta completa donde se guardaran los datos de entrenamiento, en la carpeta se guardaran las imagenes de los rostros con las que se entrenó, el archivo
        /// con los datos de entrenamiento (.trn) y un archivo con informacion adicional (.txt)</param>
        void EntrenarMultiple(string rutaFotosEntrenamiento, string rutaResultados);

        /// <summary>
        /// Identifica a las personas de la imagen especificada utilizando los datos de entrenamiento (EntrenarMultiple)
        /// cargados previamente.
        /// </summary>
        /// <param name="Distancia">Distancia</param>
        /// <param name="IM">Imagen de las personas a identificar.</param>
        /// <param name="Encontrados">Diccionario con pares Clave/Valor de las personas identificadas. Las claves corresponeden a los IDs y los valores a la 
        /// imagen del rostro.</param>
        /// /// <returns>Imagen de las personas identificadas con un recuadro en los rostros</returns>
        Bitmap IdentificarMultiple(double Distancia, Bitmap IM,ref Dictionary<int,Bitmap> Encontrados);
    }
}
