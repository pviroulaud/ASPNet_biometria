using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identificacion
{
    public class Entrenador:Detector
    {
        private string rutaFotos ;
        private string rutaresultados;
        public Entrenador(string rutaCascadeClasifier,string CarpetaConImagenesDeEntrenamiento,string CarpetaResultadosEntrenamiento) : base(rutaCascadeClasifier)
        {
            rutaFotos = CarpetaConImagenesDeEntrenamiento;
            rutaresultados = CarpetaResultadosEntrenamiento;
        }

        public void Entrenar(int ID_Persona)
        {
            FileInfo[] fotos;
            DirectoryInfo df = new DirectoryInfo(rutaFotos);
            fotos = df.GetFiles("*.*", SearchOption.AllDirectories);
            int n = 0;
            Directory.CreateDirectory(rutaresultados + "\\Imagenes_" + ID_Persona.ToString());

            foreach (FileInfo imagen in fotos)
            {
                Bitmap IM = new Bitmap(imagen.FullName);
                ObtenerRostros(IM).FirstOrDefault().Bitmap.Save(rutaresultados + "\\Imagenes_"+ ID_Persona.ToString() + "\\IM_"+n.ToString()+".jpg");
                n++;
            }
            string archivoEntrenamiento = rutaresultados + "\\DatosEntrenamiento_" + ID_Persona.ToString() + ".trn";

            GenerateTrainData(rutaresultados + "\\Imagenes_" + ID_Persona.ToString() + "\\", archivoEntrenamiento,ID_Persona);

            double distancia = ObtenerDistancia(rutaFotos,archivoEntrenamiento, ID_Persona);

            StreamWriter wr = new StreamWriter(rutaresultados +  "\\DatoDistancia.txt");
            wr.WriteLine("ID_Persona:" + ID_Persona.ToString());
            wr.WriteLine("Archivo de datos de entrenamiento: DatosEntrenamiento_" + ID_Persona.ToString() + ".trn");
            wr.WriteLine("Distancia: " + distancia.ToString());
            wr.Close();

        }

        private double ObtenerDistancia(string RutaConImagenes,string ArchivoEntrenamiento,int ID_PersonaBuscada)
        {
            EigenFaceRecognizer faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
       
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
                if (res.Label==ID_PersonaBuscada)
                {
                    n++;
                    sum += res.Distance;
                }
             
            }
            if (n>0)
            {
                return sum / n;
            }
            else
            {
                return 0;
            }
            
        }

        private void GenerateTrainData(string CarpetaConRostrosDeEntrenamiento,string ArchivoEntrenamiento,int ID_Persona)
        {
            FaceRecognizer faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);

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
    }
}
