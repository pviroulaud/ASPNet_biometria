namespace Base
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pb_CAM = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.lbl_cantidad = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pb_FaceDetect = new System.Windows.Forms.PictureBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pb_CAM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_FaceDetect)).BeginInit();
            this.SuspendLayout();
            // 
            // pb_CAM
            // 
            this.pb_CAM.Location = new System.Drawing.Point(32, 63);
            this.pb_CAM.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pb_CAM.Name = "pb_CAM";
            this.pb_CAM.Size = new System.Drawing.Size(346, 268);
            this.pb_CAM.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_CAM.TabIndex = 7;
            this.pb_CAM.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(140, 18);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(112, 35);
            this.button3.TabIndex = 6;
            this.button3.Text = "CAPTURE";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(280, 18);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(112, 35);
            this.button2.TabIndex = 5;
            this.button2.Text = "CLOSE CAM";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 18);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 35);
            this.button1.TabIndex = 4;
            this.button1.Text = "INIT CAM";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.LargeImageList = this.imageList1;
            this.listView1.Location = new System.Drawing.Point(830, 68);
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(590, 259);
            this.listView1.SmallImageList = this.imageList1;
            this.listView1.TabIndex = 19;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // lbl_cantidad
            // 
            this.lbl_cantidad.AutoSize = true;
            this.lbl_cantidad.Location = new System.Drawing.Point(609, 335);
            this.lbl_cantidad.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_cantidad.Name = "lbl_cantidad";
            this.lbl_cantidad.Size = new System.Drawing.Size(18, 20);
            this.lbl_cantidad.TabIndex = 18;
            this.lbl_cantidad.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(448, 335);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 20);
            this.label1.TabIndex = 17;
            this.label1.Text = "Cantidad de rostros:";
            // 
            // pb_FaceDetect
            // 
            this.pb_FaceDetect.Location = new System.Drawing.Point(453, 63);
            this.pb_FaceDetect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pb_FaceDetect.Name = "pb_FaceDetect";
            this.pb_FaceDetect.Size = new System.Drawing.Size(346, 268);
            this.pb_FaceDetect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pb_FaceDetect.TabIndex = 16;
            this.pb_FaceDetect.TabStop = false;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(64, 64);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 356);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.lbl_cantidad);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pb_FaceDetect);
            this.Controls.Add(this.pb_CAM);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pb_CAM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_FaceDetect)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pb_CAM;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label lbl_cantidad;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pb_FaceDetect;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.ImageList imageList1;
    }
}

