using Emgu.CV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Base
{
    public partial class Camara : Form
    {
        private Capture _capture = null;
        private Mat _frame;
        private bool _captureInProgress;
        private int nImagen = 0;

        private Identificacion.IMetodoReconocimiento reconocimiento, reconocimientoLBPH;

      
        public Camara()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
          
            reconocimientoLBPH=new Identificacion.LBPH(Application.StartupPath + @"\HaarCascade\haarcascade_frontalface_default.xml");

            reconocimiento = new Identificacion.Eigen(Application.StartupPath + @"\HaarCascade\haarcascade_frontalface_default.xml");

            _frame = new Mat();
            CheckForIllegalCrossThreadCalls = false;
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
                try
                {
                    if (checkBox3.Checked)
                    {
                        bool res = false;


                        double distanciaObtenida = 0;
                        pictureBox3.Image = reconocimiento.Identificar(Convert.ToDouble(numericUpDown5.Value), _frame.Bitmap, (int)numericUpDown6.Value, ref res, ref distanciaObtenida);
                        label10.Text = distanciaObtenida.ToString();
                        checkBox2.Checked = res;
                    }
                }
                catch (Exception)
                {

                    
                }
                

            }
        }



        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult r = folderBrowserDialog1.ShowDialog();
            if (r== DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (timer1.Enabled)
            {
                button1.Text = "Detener Captura";
                
                timer1.Interval = (int)numericUpDown1.Value;
                timer1.Enabled = false;
            }
            else
            {
                nImagen = 0;
                button1.Text = "Capturar Imagenes";
                timer1.Interval = (int)numericUpDown1.Value;
                timer1.Enabled = true;
            }
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_capture != null && _capture.Ptr != IntPtr.Zero)
            {
                //_capture.Retrieve(_frame, 0);

                
                pb_CAM.Image.Save(folderBrowserDialog1.SelectedPath + "\\IM_" + nImagen.ToString()+".jpg");

                imageList1.Images.Add(pb_CAM.Image);
                listView1.Items.Add("ID=" + nImagen.ToString(), nImagen);

                nImagen++;

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            DialogResult r = folderBrowserDialog1.ShowDialog();
            if (r== DialogResult.OK)
            {
                textBox2.Text = folderBrowserDialog1.SelectedPath;
            }
            else
            {
                textBox2.Text = "";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            reconocimiento.Entrenar((int)numericUpDown2.Value, textBox2.Text,10, textBox3.Text);

            reconocimientoLBPH.Entrenar((int)numericUpDown2.Value, textBox2.Text, textBox3.Text);


        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult r = folderBrowserDialog1.ShowDialog();
            if (r == DialogResult.OK)
            {
                textBox3.Text = folderBrowserDialog1.SelectedPath;
            }
            else
            {
                textBox3.Text = "";
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "Archivos de entrenamiento (*.trn)|*.trn";
            openFileDialog1.FileName = "";
            if (textBox3.Text!="")
            {
                openFileDialog1.InitialDirectory = textBox3.Text;
            }
            openFileDialog1.ShowDialog();

        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (openFileDialog1.FileName != "")
            {
                if (openFileDialog1.FileName.ToLower().EndsWith(".trn"))
                {
                    textBox4.Text = openFileDialog1.FileName;
                }
                if (openFileDialog1.FileName.ToLower().EndsWith(".jpg"))
                {
                    pictureBox1.Load(openFileDialog1.FileName);
                    
                }
                if (openFileDialog1.FileName.ToLower().EndsWith(".txt"))
                {
                    StreamReader rd = new StreamReader(openFileDialog1.FileName);
                    string im = rd.ReadToEnd();
                    rd.Close();
                    rd = null;
                    processImg64(im);
                    
                }
            }
        }
        private void processImg64(string basae64)
        {





            Identificacion.Detector detector = new Identificacion.Detector();
            List<string> rostros = detector.ObtenerRostros_base64IM(basae64,false);


        }
        private void button10_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "Archivos de imagen (*.jpg)|*.jpg|Archivos de texto con imagen base64 (*.txt)|*.txt";
            openFileDialog1.FileName = "";
            if (textBox3.Text != "")
            {
                openFileDialog1.InitialDirectory = textBox3.Text;
            }
            openFileDialog1.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            reconocimiento.SetArchivoDeEntrenamiento(textBox4.Text);
            reconocimientoLBPH.SetArchivoDeEntrenamiento(textBox4.Text.Replace("EIGEN","LBPH"));

            bool res = false;
            Bitmap IM = new Bitmap(pictureBox1.Image);

            double distanciaObtenida=0;
            pictureBox2.Image = reconocimiento.Identificar(Convert.ToDouble(numericUpDown4.Value), IM, (int)numericUpDown3.Value, ref res,ref distanciaObtenida);

            label7.Text = distanciaObtenida.ToString();
            checkBox1.Checked = res;


            pictureBox2.Image = reconocimientoLBPH.Identificar(100, IM, (int)numericUpDown3.Value, ref res, ref distanciaObtenida);

            label7.Text = distanciaObtenida.ToString();
            checkBox1.Checked = res;



            //Identificacion.Identificador identificador = new Identificacion.Identificador(Application.StartupPath + @"\HaarCascade\haarcascade_frontalface_default.xml",
            //                                                                textBox4.Text);

            //bool res=false;
            //Bitmap IM = new Bitmap(pictureBox1.Image);

            //pictureBox2.Image= identificador.Buscar(double.PositiveInfinity, IM, (int)numericUpDown3.Value, ref res);
            //checkBox1.Checked = res;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                button12.Enabled = false;
                button7.Enabled = true;
                numericUpDown2.Enabled = true;
            }
            else
            {
                button12.Enabled = true;
                button7.Enabled = false;
                numericUpDown2.Enabled = false;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            
            reconocimiento.EntrenarMultiple(textBox2.Text, textBox3.Text);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            reconocimiento.SetArchivoDeEntrenamiento(textBox4.Text);
            Dictionary<int, Bitmap> Encontrados= new Dictionary<int, Bitmap>();

            Bitmap IM = new Bitmap(pictureBox1.Image);

            pictureBox2.Image = reconocimiento.IdentificarMultiple(double.PositiveInfinity, IM,ref Encontrados);

            label6.Text = "";
            foreach (int id in Encontrados.Keys)
            {
                label6.Text += id.ToString() + ", ";
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\Users\pviroulaud\Desktop\Fotos\StackFotos\383");
            reconocimiento.SetArchivoDeEntrenamiento(textBox4.Text);

            label6.Text = "";
            foreach (FileInfo fi in di.EnumerateFiles())
            {
                Bitmap IM = new Bitmap(fi.FullName);
                bool res = false;
                double d = 0;
                reconocimiento.Identificar(double.PositiveInfinity, IM, (int)numericUpDown3.Value, ref res,ref d);
                if (res==true)
                {
                    label6.Text += fi.Name;
                }
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                reconocimiento.SetArchivoDeEntrenamiento(textBox4.Text);
            }
        }
    }
}
