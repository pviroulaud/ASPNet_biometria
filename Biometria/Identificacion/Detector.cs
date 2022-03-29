using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Identificacion
{
    public class Detector
    {
        private string clasificador;
        private CascadeClassifier faceDetector;
        private bool mascaraElipse =true;
        private string carpetaTemporal;
        public bool MascaraElipticaDeRostro
        { get
            {
                return mascaraElipse;
            }
            set
            {
                mascaraElipse = value;
            }

        }

        public Detector()
        {
            
            clasificador = AppDomain.CurrentDomain.BaseDirectory+ "Clasificadores\\haarcascade_frontalface_default.xml";

            if (!System.IO.File.Exists(clasificador))
            {
                clasificador = "";
            }
            else
            {
                faceDetector = new CascadeClassifier(clasificador);// Application.StartupPath + @"\HaarCascade\haarcascade_frontalface_default.xml");
            }
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "IMGtemp"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "IMGtemp");                
            }
            carpetaTemporal = AppDomain.CurrentDomain.BaseDirectory + "IMGtemp";
            //var a = ObtenerRostros_base64IM("hola");
        }
        public Detector(string rutaCascadeClasifier)
        {
            clasificador = rutaCascadeClasifier;
            faceDetector = new CascadeClassifier(clasificador);// Application.StartupPath + @"\HaarCascade\haarcascade_frontalface_default.xml");
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "IMGtemp"))
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "IMGtemp");
            }
            carpetaTemporal = AppDomain.CurrentDomain.BaseDirectory + "IMGtemp";
        }

        public List<Mat> ObtenerRostros(Mat ImagenMatrix, Mat FrameConRecuadrosDeRostros)
        {
            List<Mat> rostros = new List<Mat>();
            Mat _frame = new Mat();
            bool recuadrar = FrameConRecuadrosDeRostros != null;
            

            _frame=AcondicionarImagen(ImagenMatrix);
            Rectangle[] CarasEncontradas = faceDetector.DetectMultiScale(_frame, 1.2, 10, new Size(20, 20), Size.Empty);// faceDetector.DetectMultiScale encuentra los rostros
            if (CarasEncontradas.Count() > 0)
            {
                foreach (Rectangle roi in CarasEncontradas)
                {
                    if (recuadrar)
                    {
                        EncuadrarRostro(FrameConRecuadrosDeRostros, roi);
                        //CvInvoke.Rectangle(FrameConRecuadrosDeRostros, roi, new MCvScalar(255, 255, 255));// Se dibuja el rectacngulo decada rostro
                    }
                    if (mascaraElipse)
                    {
                        rostros.Add(EnmarcarRostro(_frame, roi));
                    }
                    else
                    {
                        rostros.Add(recortarRostro(_frame, roi));
                    }
                    
                }
                
            }
            return rostros;
        }

        protected void EncuadrarRostro(Mat ImagenMatrix,Rectangle roi)
        {
            CvInvoke.Rectangle(ImagenMatrix, roi, new MCvScalar(0, 255, 0),4);// Se dibuja el rectacngulo decada rostro
        }

        private Mat recortarRostro(Mat ImagenMatrix,Rectangle ROI)
        {
            return new Mat(ImagenMatrix, ROI);
        }

        private Mat EnmarcarRostro(Mat ImagenMatrix,Rectangle ROI)
        {
            // Creacion de una mascar eliptica con fondo negro (como un marco circular para la cara)
            Image<Gray, byte> mask = new Image<Gray, byte>(ImagenMatrix.Width, ImagenMatrix.Height);
            CvInvoke.Ellipse(mask, new Point(ROI.X + (ROI.Width / 2),
                                             ROI.Y + (ROI.Height / 2)),
                                             new Size((int)Math.Floor(ROI.Width / 2.3), ROI.Height / 2), 0, 0, 360,
                                             new MCvScalar(255, 255, 255), -1, Emgu.CV.CvEnum.LineType.AntiAlias, 0); // 
            Mat MaskedFace = new Mat();
            CvInvoke.BitwiseAnd(ImagenMatrix, mask, MaskedFace);

            return new Mat(MaskedFace, ROI);
        }

        public List<Mat> ObtenerRostros(Bitmap IM)
        {
            return ObtenerRostros(Bitmap_to_Mat(IM),null);
        }
        public List<String> ObtenerRostros_base64IM(String base64Image,bool agregarEncabezadoSRC_html)
        {
            List<String> ret = new List<string>();

            string nombreTMP = carpetaTemporal+ new Guid().ToString()+".png";


            Helpers.ImageHelper.Base64String_2_ImageFile(base64Image, nombreTMP); // Convierte de base64 a un archivo temporal
            Mat imagenM = new Mat(nombreTMP, LoadImageType.AnyColor);// Se convierte el archivo temporal a Mat
           
            List<Mat> rostros= ObtenerRostros(imagenM, null); // Se buscan los rostros y devuelve una lista de Mat con los rostros

            File.Delete(nombreTMP);

            foreach (var r in rostros)
            {
                Image<Gray, byte> img = r.ToImage<Gray, byte>();
                string b64 = Convert.ToBase64String(img.ToJpegData());
                if (agregarEncabezadoSRC_html)
                {
                    b64 = "data:image/png;base64,"+b64;
                }
                ret.Add(b64);
            }
            return ret;
        }

        protected Mat Bitmap_to_Mat(Bitmap IM)
        {
           
            Image<Bgr, byte> imageCV = new Image<Bgr, byte>(IM); //Image Class from Emgu.CV
            return imageCV.Mat; //This is your Image converted to Mat
             
        }
        protected Bitmap Mat_To_Bitmap(Mat matrix)
        {
            return matrix.Bitmap;
        }

        private Mat AcondicionarImagen(Mat ImagenMatrix)
        {
            Mat _frameAcond = new Mat();
            Mat _frameAcond2 = new Mat();
            CvInvoke.CvtColor(ImagenMatrix, _frameAcond, ColorConversion.Bgr2Gray); // se convierte a escala de grises
            CvInvoke.EqualizeHist(_frameAcond, _frameAcond2); // equalizacion de histograma (mejora las condiciones de iluminacion ajustando el contraste)

            return _frameAcond2;
        }

        private Bitmap GetFace_GrayScale(Bitmap IM)
        {
            Bitmap ret = null;
            Mat _frame, _frameGS, _frameFaceDetect, _frameGS_Hequ;
            CascadeClassifier faceDetector;
            _frame = new Mat();
            _frameGS = new Mat();
            _frameGS_Hequ = new Mat();

            faceDetector = new CascadeClassifier(clasificador);// Application.StartupPath + @"\HaarCascade\haarcascade_frontalface_default.xml");

            Image<Bgr, byte> imageCV = new Image<Bgr, byte>(IM); //Image Class from Emgu.CV
            _frame = imageCV.Mat; //This is your Image converted to Mat

            CvInvoke.CvtColor(_frame, _frameGS, ColorConversion.Bgr2Gray); // se convierte a escala de grises
            CvInvoke.EqualizeHist(_frameGS, _frameGS_Hequ); // equalizacion de histograma (mejora las condiciones de iluminacion ajustando el contraste)

            Rectangle[] CarasEncontradas = faceDetector.DetectMultiScale(_frameGS_Hequ, 1.2, 10, new Size(20, 20), Size.Empty);// faceDetector.DetectMultiScale encuentra los rostros
            if (CarasEncontradas.Count() > 0)
            {
                // Creacion de una mascar eliptica con fondo negro (como un marco circular para la cara)
                Image<Gray, byte> mask = new Image<Gray, byte>(_frame.Width, _frame.Height);
                CvInvoke.Ellipse(mask, new Point(CarasEncontradas[0].X + (CarasEncontradas[0].Width / 2),
                                                 CarasEncontradas[0].Y + (CarasEncontradas[0].Height / 2)),
                                                 new Size((int)Math.Floor(CarasEncontradas[0].Width / 2.3), CarasEncontradas[0].Height / 2), 0, 0, 360,
                                                 new MCvScalar(255, 255, 255), -1, Emgu.CV.CvEnum.LineType.AntiAlias, 0); // 
                Mat MaskedFace = new Mat();
                CvInvoke.BitwiseAnd(_frameGS_Hequ, mask, MaskedFace);

                _frameFaceDetect = new Mat(MaskedFace, CarasEncontradas[0]);
                ret = _frameFaceDetect.Bitmap;
            }
            return ret;
        }
    }
}
