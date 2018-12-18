using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

using System.IO.Ports;

using AForge;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;
using AForge.Imaging;
using AForge.Vision;
using AForge.Math.Geometry;

using Point = System.Drawing.Point;
using AForge.Vision.Motion;

namespace thresholdd
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCapTureDevices;
        private VideoCaptureDevice Finalvideo;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Finalvideo = new VideoCaptureDevice(VideoCapTureDevices[comboBox1.SelectedIndex].MonikerString);
            Finalvideo.NewFrame += new NewFrameEventHandler(Finalvideo_NewFrame);
            Finalvideo.DesiredFrameRate = 15;
            Finalvideo.DesiredFrameSize = new Size(320, 240);
            Finalvideo.Start();
        }

        void Finalvideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();

            pictureBox1.Image = image;

            
            if (radioButton1.Checked)
            {
                Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();

                Grayscale filter = new Grayscale(0.2125, 0.7154, 0.0721);
                Bitmap grayImage = filter.Apply(image1);
                Threshold filter2 = new Threshold(100);
                filter2.ApplyInPlace(grayImage);
                Invert filter3 = new Invert();
                filter3.ApplyInPlace(grayImage);

                pictureBox2.Image = grayImage;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox2.DataSource = SerialPort.GetPortNames();
            int sayi = comboBox2.Items.Count;
            if (sayi == 0)
            {
                toolStripLabel1.Text = "Port Bulunamadı.Kontrol et!!";
                comboBox2.Enabled = false;
                button2.Enabled = false;

            }

            else
            {
                toolStripLabel1.Text = sayi + "Tane Port var";
            }

            VideoCapTureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo VideoCaptureDevice in VideoCapTureDevices)
            {

                comboBox1.Items.Add(VideoCaptureDevice.Name);

            }

            comboBox1.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox2.SelectedItem.ToString();
            serialPort1.BaudRate = 9600;
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                toolStripLabel1.Text = comboBox2.SelectedItem.ToString() + "portuna bağlandı";

            }
        }
    }
    }
    

