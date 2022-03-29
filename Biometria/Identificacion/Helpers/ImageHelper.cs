using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Identificacion.Helpers
{
    public class ImageHelper
    {
        /// <summary>
        /// Convierte una cadena con el formato xxxx,yyyy a un tipo Size.
        /// en el caso de que el formato no sea valido retorna Size(256,256)
        /// </summary>
        /// <param name="tamaño">tamaño</param>
        /// <returns></returns>
        public static Size String2Size(string tamaño)
        {
            Size ret = new Size(256, 256);
            if (tamaño.Contains(","))
            {
                string[] XY = tamaño.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (XY.Length == 2)
                {
                    try
                    {
                        int X = Convert.ToInt32(XY[0]);
                        int Y = Convert.ToInt32(XY[1]);
                        if ((X > 0) && (Y > 0))
                        {
                            ret = new Size(X, Y);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            return ret;
        }
        public static string AjustarTamañoImagen(Size tamaño, Bitmap imagen)
        {
            try
            {
                Bitmap IM = new Bitmap(imagen, tamaño);
                return Image_2_Base64(IM);
            }
            catch (Exception ex)
            {

                throw new Exception("Ha ocurrido un error al ajustar el tamaño de la imagen." + ex.Message);
            }

        }
        public static string Image_2_Base64(Bitmap Imagen)
        {
            System.IO.MemoryStream ms = new MemoryStream();
            Imagen.Save(ms, ImageFormat.Jpeg);
            byte[] byteImage = ms.ToArray();
            return Convert.ToBase64String(byteImage);
        }

        public static void Base64String_2_ImageFile(string base64Image,string nombreDeArchivo)
        {
            byte[] dataBuffer = Convert.FromBase64String(base64Image.Replace("data:image/png;base64,", ""));
            using (FileStream fileStream = new FileStream(nombreDeArchivo, FileMode.Create, FileAccess.Write))
            {
                if (dataBuffer.Length > 0)
                {
                    fileStream.Write(dataBuffer, 0, dataBuffer.Length);
                }
            }
            
        }

        public static Bitmap Base64string_2_Bitmap(string Imagen_Base64)
        {
            Bitmap bmpReturn = null;


            byte[] byteBuffer = Convert.FromBase64String(Imagen_Base64);
            MemoryStream memoryStream = new MemoryStream(byteBuffer);


            memoryStream.Position = 0;


            bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);


            memoryStream.Close();
            memoryStream = null;
            byteBuffer = null;


            return bmpReturn;
        }
    }
}
