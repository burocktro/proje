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
using AForge.Vision.Motion;
using AForge.Math.Geometry;

using Point = System.Drawing.Point;

namespace step_motor
{
    public partial class Form1 : Form
    {

        private FilterInfoCollection VideoCapTureDevices;
        private VideoCaptureDevice Finalvideo;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = SerialPort.GetPortNames();
            int sayi = comboBox1.Items.Count;
            if (sayi == 0)
            {
                toolStripLabel1.Text = "Port Bulunamadı.Kontrol et!!";
                comboBox1.Enabled = false;
                button1.Enabled = false;

            }

            else
            {
                toolStripLabel1.Text = sayi + "Tane Port var";
            }

            VideoCapTureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo VideoCaptureDevice in VideoCapTureDevices)
            {

                comboBox2.Items.Add(VideoCaptureDevice.Name);

            }

            comboBox2.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox1.SelectedItem.ToString();
            serialPort1.BaudRate = 9600;
            serialPort1.Open();
            if (serialPort1.IsOpen)
            {
                toolStripLabel1.Text = comboBox1.SelectedItem.ToString() + "portuna bağlandı";

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Finalvideo = new VideoCaptureDevice(VideoCapTureDevices[comboBox2.SelectedIndex].MonikerString);
            Finalvideo.NewFrame += new NewFrameEventHandler(Finalvideo_NewFrame);
            Finalvideo.DesiredFrameRate = 20;
            Finalvideo.DesiredFrameSize = new Size(350, 240);
            Finalvideo.Start();
        }
        void Finalvideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap image = (Bitmap)eventArgs.Frame.Clone();
            Bitmap image1 = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = image;

            if (radioButton1.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(215, 0, 0));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }

            if (radioButton2.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(0, 255, 0));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }

            if (radioButton3.Checked)
            {
                EuclideanColorFiltering filter = new EuclideanColorFiltering();
                filter.CenterColor = new RGB(Color.FromArgb(30, 144, 255));
                filter.Radius = 100;
                filter.ApplyInPlace(image1);
                nesnebul(image1);
            }
        }

        public void nesnebul(Bitmap image)
        {

            BlobCounter blobCounter = new BlobCounter();
            blobCounter.MinWidth = 5;
            blobCounter.MinHeight = 5;
            blobCounter.FilterBlobs = true;
            blobCounter.ObjectsOrder = ObjectsOrder.Size;

            BitmapData objectsData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, image.PixelFormat);
            Grayscale grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            UnmanagedImage grayImage = grayscaleFilter.Apply(new UnmanagedImage(objectsData));
            image.UnlockBits(objectsData);
            blobCounter.ProcessImage(image);
            Rectangle[] rects = blobCounter.GetObjectsRectangles();
            Blob[] blobs = blobCounter.GetObjectsInformation();
            pictureBox2.Image = image;


            foreach (Rectangle recs in rects)
            {

                if (rects.Length > 0)
                {
                    Rectangle objectRect = rects[0];

                    Graphics g = pictureBox1.CreateGraphics();
                    using (Pen pen = new Pen(Color.FromArgb(250, 0, 0), 2))
                    {
                        g.DrawRectangle(pen, objectRect);
                    }

                    int objectX = objectRect.X + (objectRect.Width / 2);
                    int objectY = objectRect.Y + (objectRect.Height / 2);

                    String area = "";

                    if (objectX < 120)
                    {
                        area = "1.Bölge";
                        serialPort1.Write("1");


                    }

                    else if ((objectX > 212 && objectX < 360))
                    {
                        area = "3.Bölge";

                        serialPort1.Write("3");

                    }

                    g.DrawString(area, new Font("Arial", 12), Brushes.Red, new System.Drawing.Point(1, 1));
                    g.Dispose();


                }
            }
        }

    }
}
