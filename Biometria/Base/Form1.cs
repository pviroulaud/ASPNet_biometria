﻿using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;

namespace Base
{
    public partial class Form1 : Form
    {
        private Capture _capture = null;
        private Mat _frame, _frameGS, _frameFaceDetect;
        private bool _captureInProgress;

        private CascadeClassifier faceDetector;
        private List<Image> faces = new List<Image>();


        private FaceRecognizer faceRecognizer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            _frame = new Mat();
            _frameGS = new Mat();
            _frameFaceDetect = new Mat();

            faceDetector = new CascadeClassifier(Application.StartupPath + @"\HaarCascade\haarcascade_frontalface_default.xml");

            faceRecognizer = new EigenFaceRecognizer(80, double.PositiveInfinity);
            //faceRecognizer.Load(Application.StartupPath + @"\DatosEntrenamiento\TrainData_0.trn");
            faceRecognizer.Load(Application.StartupPath + @"\DatosEntrenamiento\Yo_123.trn");
        }


        #region BASE
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                _capture = new Capture();
                _capture.ImageGrabbed += _capture_ImageGrabbed;
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            if (_capture != null)
            {
                if (_captureInProgress)
                {  //stop the capture

                    _capture.Pause();
                }
                else
                {
                    //start the capture
                    _capture.Start();
                }

                _captureInProgress = !_captureInProgress;
            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if (_captureInProgress)
            {
                _capture.Stop();
                _captureInProgress = !_captureInProgress;
            }
            if (_capture != null)
                _capture.Dispose();
        }
        #endregion


        #region FACERECON






        private void button6_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Teach the system with samples to predict the face
        /// </summary>
        /// <returns></returns>
        public bool learn(Dictionary<string, List<System.IO.FileInfo>> TrainData)
        {

            var faceLabels = new int[TrainData.Count];
            var faceImages = new Image<Gray, byte>[TrainData.Count];
            int n = 0;
            foreach (var data in TrainData)
            {
                if (data.Value.Count > 0)
                {

                    for (int i = 0; i < data.Value.Count; i++)
                    {
                        var faceImage = new Image<Gray, byte>(new Bitmap(data.Value[i].FullName));

                        faceImages[n] = faceImage;
                        faceLabels[n] = Convert.ToInt32(data.Key);
                        n++;
                    }
                }
            }

            faceRecognizer.Train(faceImages, faceLabels);


            return true;
        }
        /// <summary>
        /// Convert image to byte to store it on the local sql db
        /// </summary>
        /// <param name="myImage"></param>
        /// <returns></returns>
        public static byte[] ConvertImageToByte(Image myImage)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            new Bitmap(myImage).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] header = new byte[] { 255, 216 };
            header = ms.ToArray();
            return (header);
        }


        #endregion






        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);
                pb_CAM.Image = _frame.Bitmap;

                faces.Clear();
                _frameFaceDetect = _frame.Clone();
                CvInvoke.CvtColor(_frame, _frameGS, ColorConversion.Bgr2Gray);
                MCvScalar ColorRecuadro = new MCvScalar(255, 255, 255);
                Rectangle[] CarasEncontradas = faceDetector.DetectMultiScale(_frameGS, 1.1, 10, new Size(20, 20), Size.Empty);// faceDetector.DetectMultiScale encuentra los rostros
                /* Solo para visualizacion */
                int n = 0;
                imageList1.Images.Clear();
                listView1.Items.Clear();
                /***************************/
                foreach (Rectangle Cara in CarasEncontradas)
                {

                    CvInvoke.Rectangle(_frameFaceDetect, Cara, ColorRecuadro);// Se dibuja el rectacngulo decada rostro


                    Image<Gray, byte> userFace = new Image<Gray, byte>(new Mat(_frameGS, Cara).Bitmap);
                    FaceRecognizer.PredictionResult res = faceRecognizer.Predict(userFace.Resize(64, 64, Inter.Cubic));

                    
                    faces.Add(new Mat(_frameFaceDetect, Cara).Bitmap);// se guarda cada rostro en una lista
                    /* Solo para visualizacion */
                    imageList1.Images.Add(new Mat(_frameFaceDetect, Cara).Bitmap);
                    listView1.Items.Add("ID=" + res.Label.ToString(), n);
                    n++;
                    /***************************/



                }
                lbl_cantidad.Text = CarasEncontradas.Count().ToString();

                pb_FaceDetect.Image = _frameFaceDetect.Bitmap;
            }
        }
    }
}