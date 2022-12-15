using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace HopfieldNN
{
    public partial class Form1 : Form
    {
        private Random _rand = new Random();
        private HopfieldNetwork _hopfieldNetwork;
        private int[][,] _data;

        public Form1()
        {
            InitializeComponent();
        }

        private void SelectTrainDataFolderButton_Click(object sender, EventArgs e)
        {
            string path = null;

            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = Environment.CurrentDirectory;
                if(dialog.ShowDialog() == DialogResult.OK)
                {
                    path = dialog.SelectedPath;
                }
            }

            if(path == null)
            {
                return;
            }

            _data = Directory.GetFiles(path).Select(file => GetDataFromBitmap(new Bitmap(file))).ToArray();

            var height = _data[0].GetLength(0);
            var width = _data[0].GetLength(1);

            if (_hopfieldNetwork != null)
            {
                _hopfieldNetwork.OnTrainItem -= _hopfieldNetwork_Changed;
                _hopfieldNetwork.OnTestItem -= _hopfieldNetwork_Changed;
            }

            _hopfieldNetwork = new HopfieldNetwork(height, width, x => x >= 0 ? 1 : -1);

            _hopfieldNetwork.OnTrainItem += _hopfieldNetwork_Changed;
            _hopfieldNetwork.OnTestItem += _hopfieldNetwork_Changed;
        }

        private void _hopfieldNetwork_Changed(object sender, int[,] e)
        {
            OutputPictureBox.Image = GetBitmapFromData(e);
        }

        private async void TrainButton_Click(object sender, EventArgs e)
        {
            await _hopfieldNetwork.InitWeights(_data);
            MessageBox.Show("Train finished");
        }

        private async void TestButton_Click(object sender, EventArgs e)
        {
            string path = null;
            using(var dialog = new OpenFileDialog())
            {
                dialog.FileName = Environment.CurrentDirectory;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    path = dialog.FileName;
                }
            }

            if(path == null)
            {
                return;
            }

            var data = GetDataFromBitmap(new Bitmap(path));
            var result = await _hopfieldNetwork.Decide(data);

            OutputPictureBox.Image = GetBitmapFromData(result);
        }

        private int[,] GetDataFromBitmap(Bitmap bitmap)
        {
            var bitmapData = new int[bitmap.Height, bitmap.Width];
            for (int i = 0; i < bitmap.Height; ++i)
            {
                for (int j = 0; j < bitmap.Width; ++j)
                {
                    var pixel = bitmap.GetPixel(j, i);
                    bitmapData[i, j] = (pixel.R + pixel.G + pixel.B) / 3 > 128 ? 1 : -1;
                }
            }
            return bitmapData;
        }

        private Bitmap GetBitmapFromData(int[,] data)
        {
            var height = data.GetLength(0);
            var width = data.GetLength(1);

            var bitmap = new Bitmap(width, height);
            for (int i = 0; i < height; ++i)
            {
                for (int j = 0; j < width; ++j)
                {
                    var pixel = data[i, j] == 1 ? Color.White : Color.Black;
                    bitmap.SetPixel(j, i, pixel);
                }
            }
            return bitmap;
        }
    }
}
