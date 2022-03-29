using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;

namespace Identificacion
{
    public class Eigen : Detector, IMetodoReconocimiento
    {
        private string archivoEntrenamiento = "";
        private EigenFaceRecognizer faceRecognizer;
        private int componentes = 80;

        #region Constructores
        public Eigen() : base()
        {

        }
        public Eigen(string rutaCascadeClasifier) : base(rutaCascadeClasifier)
        {
            
        }

        public Eigen(string rutaCascadeClasifier, int NumeroDeComponentes) : base(rutaCascadeClasifier)
        {
            componentes = NumeroDeComponentes;
        }

        public Eigen(string rutaCascadeClasifier, string ArchivoDeEntrenamiento) : base(rutaCascadeClasifier)
        {
            archivoEntrenamiento = ArchivoDeEntrenamiento;
        }
        public Eigen(string rutaCascadeClasifier, string ArchivoDeEntrenamiento,int NumeroDeComponentes) : base(rutaCascadeClasifier)
        {
            archivoEntrenamiento = ArchivoDeEntrenamiento;
            componentes = NumeroDeComponentes;
        }
        #endregion

        #region Interfaz IMetodoReconocimiento
        public void SetArchivoDeEntrenamiento(string ArchivoDeEntrenamiento)
        {
            archivoEntrenamiento = ArchivoDeEntrenamiento;

        }

        
        public void Entrenar(int ID_Persona,string rutaFotosEntrenamiento, string rutaResultados)
        {
            ProcesoDeEntrenamiento(ID_Persona, rutaFotosEntrenamiento, 0, rutaResultados);

        }


        public void Entrenar(int ID_Persona, string rutaFotosEntrenamiento, int cantidadDeFotosUtilizar, string rutaResultados)
        {
            ProcesoDeEntrenamiento(ID_Persona, rutaFotosEntrenamiento, cantidadDeFotosUtilizar, rutaResultados);
        }

        public Bitmap Identificar(double Distancia, Bitmap IM, int ID_Buscar, ref bool Encontrado, ref double DistanciaObtenida)
        {
            if (archivoEntrenamiento != "")
            {
                if (System.IO.File.Exists(archivoEntrenamiento))
                {
                    faceRecognizer = new EigenFaceRecognizer(componentes, Distancia);
                    faceRecognizer.Load(archivoEntrenamiento);

                    Bitmap Imagen = new Bitmap(IM);
                    Mat ImagenMAT = Bitmap_to_Mat(Imagen);

                    Mat ImagenRES = ImagenMAT.Clone();

                    Mat rost = ObtenerRostros(ImagenMAT, ImagenRES).FirstOrDefault();
                    if (rost != null)
                    {
                        Image<Gray, byte> userFace = new Image<Gray, byte>(rost.Bitmap);
                        FaceRecognizer.PredictionResult res = faceRecognizer.Predict(userFace.Resize(64, 64, Inter.Cubic));
                        DistanciaObtenida = res.Distance;

                        Encontrado = res.Label == ID_Buscar;
                    }

                    return ImagenRES.Bitmap;
                }
                else
                {
                    throw new Exception("No existe el archivo de entrenamiento especificado.");
                }
            }
            else
            {
                throw new Exception("Debe especificar un archivo de entrenamiento.");
            }

        }

        public void SetNumeroComponentes(int nComponentes)
        {
            componentes = nComponentes;
        }

        public int getNumeroComponentes()
        {
            return componentes;
        }

        public void EntrenarMultiple(string rutaFotosEntrenamiento, string rutaResultados)
        {
            FileInfo[] fotos;
            DirectoryInfo df = new DirectoryInfo(rutaFotosEntrenamiento);
            fotos = df.GetFiles("*.*", SearchOption.AllDirectories);
            int n = 0;
            DirectoryInfo re = Directory.CreateDirectory(rutaResultados + "\\ImagenesEIGEN"); // Se crea la carpeta de resultados

            List<int> IDs = new List<int>();
            // Se obtiene el rostro de cada foto de entrenamiento (1)
            foreach (FileInfo imagen in fotos)
            {
                Bitmap IM = new Bitmap(imagen.FullName);
                if (!IDs.Contains(Convert.ToInt32(imagen.Directory.Name)))
                {
                    IDs.Add(Convert.ToInt32(imagen.Directory.Name));
                }
                if (!Directory.Exists(re.FullName + "\\" + imagen.Directory.Name))
                {
                    Directory.CreateDirectory(re.FullName + "\\" + imagen.Directory.Name);
                }
                ObtenerRostros(IM).FirstOrDefault().Bitmap.Save(rutaResultados + "\\ImagenesEIGEN\\" + imagen.Directory.Name + "\\IM_" + n.ToString() + ".jpg");
                n++;
            }

            // se define el nombre y ubicacion del archivo de entrenamiento (2)
            string archivoEntrenamiento = rutaResultados + "\\DatosEntrenamientoEIGEN_Multiple.trn";

            // Se entrena con las imagenes de los rostros (1) y se guarda el resultado en (2)
            GenerateTrainDataMultiple(rutaResultados + "\\ImagenesEIGEN\\", archivoEntrenamiento);

            // Se obtiene la distancia de entrenamiento
            double distancia = ObtenerDistancia(rutaFotosEntrenamiento, archivoEntrenamiento);

            // se crea el archivo con info adicional.
            StreamWriter wr = new StreamWriter(rutaResultados + "\\DatosEntrenaminento.txt");
            wr.WriteLine("ID_Personas: ");
            foreach (int id in IDs)
            {
                wr.Write(id.ToString() + ",");
            }
            wr.WriteLine();

            wr.WriteLine("Archivo de datos de entrenamiento: DatosEntrenamiento_Multiple.trn");
            wr.WriteLine("Distancia: " + distancia.ToString());
            wr.WriteLine("Metodo de reconocimineto: EIGEN");
            wr.WriteLine("Cantidad de imagenes por ID: ");
            foreach (DirectoryInfo carpeta in new DirectoryInfo(rutaResultados + "\\ImagenesEIGEN").EnumerateDirectories())
            {
                wr.WriteLine("\t" + carpeta.Name + ": " + carpeta.EnumerateFiles("*.*").Count().ToString());
            }
            wr.WriteLine("Cantidad total de imagenes: " + fotos.Length.ToString());
            wr.Close();
        }

        public Bitmap IdentificarMultiple(double Distancia, Bitmap IM, ref Dictionary<int, Bitmap> Encontrados)
        {
            Encontrados = new Dictionary<int, Bitmap>();

            if (archivoEntrenamiento != "")
            {
                if (System.IO.File.Exists(archivoEntrenamiento))
                {
                    faceRecognizer = new EigenFaceRecognizer(componentes, Distancia);
                    faceRecognizer.Load(archivoEntrenamiento);

                    Bitmap Imagen = new Bitmap(IM);
                    Mat ImagenMAT = Bitmap_to_Mat(Imagen);

                    Mat ImagenRES = ImagenMAT.Clone();

                    foreach (Mat rostro in ObtenerRostros(ImagenMAT, ImagenRES))
                    {
                        Image<Gray, byte> userFace = new Image<Gray, byte>(rostro.Bitmap);
                        FaceRecognizer.PredictionResult res = faceRecognizer.Predict(userFace.Resize(64, 64, Inter.Cubic));

                        if (res.Label != 0)
                        {
                            Encontrados.Add(res.Label, rostro.Bitmap);
                        }


                    }





                    return ImagenRES.Bitmap;
                }
                else
                {
                    throw new Exception("No existe el archivo de entrenamiento especificado.");
                }
            }
            else
            {
                throw new Exception("Debe especificar un archivo de entrenamiento.");
            }
        }

        #endregion

        #region Metodos privados
        private void ProcesoDeEntrenamiento(int ID_Persona, string rutaFotosEntrenamiento, int cantidadDeFotosUtilizar, string rutaResultados)
        {
            FileInfo[] fotos;
            DirectoryInfo df = new DirectoryInfo(rutaFotosEntrenamiento);
            fotos = df.GetFiles("*.*", SearchOption.AllDirectories);
            int n = 0;
            Directory.CreateDirectory(rutaResultados + "\\ImagenesEIGEN_" + ID_Persona.ToString()); // Se crea la carpeta de resultados

            
                // Se obtiene el rostro de cada foto de entrenamiento (1)
                foreach (FileInfo imagen in fotos)
                {
                    Bitmap IM = new Bitmap(imagen.FullName);
                    Mat rost = ObtenerRostros(IM).FirstOrDefault();
                    if (rost != null)
                    {
                        rost.Bitmap.Save(rutaResultados + "\\ImagenesEIGEN_" + ID_Persona.ToString() + "\\IM_" + n.ToString() + ".jpg");
                        n++;
                        if (cantidadDeFotosUtilizar > 0) // si el parametro es 0 se utilizan todas las fotos
                        {
                            if (n >= cantidadDeFotosUtilizar)
                            {
                                break;
                            }
                        }
                    }

                }
            


            // se define el nombre y ubicacion del archivo de entrenamiento (2)
            string archivoEntrenamiento = rutaResultados + "\\DatosEntrenamientoEIGEN_" + ID_Persona.ToString() + ".trn";

            // Se entrena con las imagenes de los rostros (1) y se guarda el resultado en (2)
            GenerateTrainData(rutaResultados + "\\ImagenesEIGEN_" + ID_Persona.ToString() + "\\", archivoEntrenamiento, ID_Persona);

            // Se obtiene la distancia de entrenamiento
            double distancia = ObtenerDistancia(rutaFotosEntrenamiento, archivoEntrenamiento, ID_Persona);

            // se crea el archivo con info adicional.
            StreamWriter wr = new StreamWriter(rutaResultados + "\\DatosEntrenaminentoEIGEN.txt");
            wr.WriteLine("ID_Persona:" + ID_Persona.ToString());

            wr.WriteLine("Archivo de datos de entrenamiento: DatosEntrenamiento_" + ID_Persona.ToString() + ".trn");
            wr.WriteLine("Distancia: " + distancia.ToString());
            wr.WriteLine("Metodo de reconocimineto: EIGEN");
            wr.WriteLine("Canridad de imagenes: " + fotos.Length.ToString());
            wr.Close();
        }

        private double ObtenerDistancia(string RutaConImagenes, string ArchivoEntrenamiento, int ID_PersonaBuscada)
        {
            faceRecognizer = new EigenFaceRecognizer(componentes, double.PositiveInfinity);

            faceRecognizer.Load(ArchivoEntrenamiento);

            FileInfo[] fotos;
            DirectoryInfo df = new DirectoryInfo(RutaConImagenes);
            fotos = df.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            double sum = 0;
            int n = 0;
            foreach (FileInfo imagen in fotos)
            {
                Bitmap IM = new Bitmap(imagen.FullName);

                Mat rostro = ObtenerRostros(IM).FirstOrDefault();
                if (rostro!=null)
                {
                    Image<Gray, byte> userFace = new Image<Gray, byte>(rostro.Bitmap);
                    FaceRecognizer.PredictionResult res = faceRecognizer.Predict(userFace.Resize(64, 64, Inter.Cubic));
                    if (res.Label == ID_PersonaBuscada)
                    {
                        n++;
                        sum += res.Distance;
                    }
                }
                

            }
            if (n > 0)
            {
                return sum / n;
            }
            else
            {
                return 0;
            }

        }
        private double ObtenerDistancia(string RutaConImagenes, string ArchivoEntrenamiento)
        {
            faceRecognizer = new EigenFaceRecognizer(componentes, double.PositiveInfinity);

            faceRecognizer.Load(ArchivoEntrenamiento);

            FileInfo[] fotos;
            DirectoryInfo df = new DirectoryInfo(RutaConImagenes);
            fotos = df.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            double sum = 0;
            int n = 0;
            foreach (FileInfo imagen in fotos)
            {
                Bitmap IM = new Bitmap(imagen.FullName);

                Image<Gray, byte> userFace = new Image<Gray, byte>(ObtenerRostros(IM).FirstOrDefault().Bitmap);
                FaceRecognizer.PredictionResult res = faceRecognizer.Predict(userFace.Resize(64, 64, Inter.Cubic));

                n++;
                sum += res.Distance;


            }
            if (n > 0)
            {
                return sum / n;
            }
            else
            {
                return 0;
            }

        }

        private void GenerateTrainData(string CarpetaConRostrosDeEntrenamiento, string ArchivoEntrenamiento, int ID_Persona)
        {
            faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);

            FileInfo[] ret;
            DirectoryInfo df = new DirectoryInfo(CarpetaConRostrosDeEntrenamiento);
            ret = df.GetFiles("*.*", SearchOption.TopDirectoryOnly);
            var faceLabels = new int[ret.Length];
            var faceImages = new Image<Gray, byte>[ret.Length];
            int n = 0;
            foreach (FileInfo f in ret)
            {
                faceLabels[n] = ID_Persona;
                faceImages[n] = new Image<Gray, byte>(new Bitmap(f.FullName)).Resize(64, 64, Inter.Cubic);
                n++;
            }
            faceRecognizer.Train(faceImages, faceLabels);

            faceRecognizer.Save(ArchivoEntrenamiento);

        }
        private void GenerateTrainDataMultiple(string CarpetaConRostrosDeEntrenamiento, string ArchivoEntrenamiento)
        {
            faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);

            FileInfo[] ret;
            DirectoryInfo df = new DirectoryInfo(CarpetaConRostrosDeEntrenamiento);
            ret = df.GetFiles("*.*", SearchOption.AllDirectories);
            var faceLabels = new int[ret.Length];
            var faceImages = new Image<Gray, byte>[ret.Length];
            int n = 0;
            foreach (FileInfo f in ret)
            {
                faceLabels[n] = Convert.ToInt32(f.Directory.Name);
                faceImages[n] = new Image<Gray, byte>(new Bitmap(f.FullName)).Resize(64, 64, Inter.Cubic);
                n++;
            }
            faceRecognizer.Train(faceImages, faceLabels);

            faceRecognizer.Save(ArchivoEntrenamiento);

        }

        #endregion


    }
}
