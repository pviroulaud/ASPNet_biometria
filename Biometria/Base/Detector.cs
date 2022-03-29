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
using Emgu.CV.Structure;

namespace Base
{
    public partial class Detector : Form
    {
        private Capture _capture = null;
        private Mat _frame;
        private bool _captureInProgress;
        private Identificacion.Detector det;

        public Detector()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            

            det= new Identificacion.Detector(Application.StartupPath + @"\HaarCascade\haarcascade_frontalface_default.xml");
            _frame = new Mat();
        }

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

        private void _capture_ImageGrabbed(object sender, EventArgs e)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                _capture.Retrieve(_frame, 0);
                pb_CAM.Image = _frame.Bitmap;

                Mat _frameFaceDetect = _frame.Clone();

                List<Mat> f= det.ObtenerRostros(_frame, _frameFaceDetect);

                /* Solo para visualizacion */
                lbl_cantidad.Text = f.Count.ToString();
                int n = 0;
                imageList1.Images.Clear();
                listView1.Items.Clear();
                foreach (Mat im in f)
                {
                    imageList1.Images.Add(im.Bitmap);
                    listView1.Items.Add("face_" + n.ToString(), n);
                    n++;
                }
                pb_FaceDetect.Image = _frameFaceDetect.Bitmap;

            }
        }
    }
}
