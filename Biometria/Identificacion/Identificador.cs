using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identificacion
{
    public class Identificador : Detector
    {
        private string archivoEntrenamiento="";
        private EigenFaceRecognizer faceRecognizer;
        public Identificador(string rutaCascadeClasifier) : base(rutaCascadeClasifier)
        {
        }
        public Identificador(string rutaCascadeClasifier,string ArchivoDeEntrenamiento) : base(rutaCascadeClasifier)
        {
            archivoEntrenamiento = ArchivoDeEntrenamiento;
        }
        public void SetArchivoDeEntrenamiento(string ArchivoDeEntrenamiento)
        {
            archivoEntrenamiento = ArchivoDeEntrenamiento;

        }


        public Bitmap Buscar(double Distancia,Bitmap IM,int ID_Buscar,ref bool Encontrado)
        {
            if (archivoEntrenamiento!="")
            {
                if (System.IO.File.Exists(archivoEntrenamiento))
                {
                    faceRecognizer = new EigenFaceRecognizer(80, Distancia);
                    faceRecognizer.Load(archivoEntrenamiento);

                    Bitmap Imagen = new Bitmap(IM);
                    Mat ImagenMAT = Bitmap_to_Mat(Imagen);

                    Mat ImagenRES = ImagenMAT.Clone();
                    

                    Image<Gray, byte> userFace = new Image<Gray, byte>(ObtenerRostros(ImagenMAT,ImagenRES).FirstOrDefault().Bitmap);
                    FaceRecognizer.PredictionResult res = faceRecognizer.Predict(userFace.Resize(64, 64, Inter.Cubic));                                       

                    Encontrado =res.Label == ID_Buscar;
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
    }
}
