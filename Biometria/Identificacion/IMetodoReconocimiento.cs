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
        void CargarArchivoEntrenamiento(string ArchivoEntrenamiento);

        FaceRecognizer.PredictionResult Reconocer(Bitmap Imagen);


    }
}
