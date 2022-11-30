using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KochonenNeuralNetwork
{
    public partial class Form1 : Form
    {
        private Random _rand = new Random();
        private KochonenNN _kochonenNN;
        private IList<double[]> _data;
        private bool _studyInProcess;
        private IList<Color> _colorsCache = new Color[] { Color.Red, Color.Green, Color.Blue, Color.Yellow, Color.Black, Color.Cyan, Color.Magenta };

        public Form1()
        {
            InitializeComponent();
        }

        private async void Study()
        {
            var epoche = 1;

            var epochesToDecreaseStudySpeed = Convert.ToInt32(EpochesToDecreaseStudySpeed.Value);
            var epochesToReduceROI = Convert.ToInt32(EpochesToReduceROI.Value);

            var shuffledData = _data.RandomSort();

            while (_studyInProcess)
            {
                var results = new Dictionary<int, IList<double[]>>();
                
                for (int i = 0; i < shuffledData.Count; ++i)
                {
                    var result = _kochonenNN.ProcessInput(shuffledData[i]);

                    if(!results.ContainsKey(result))
                    {
                        results.Add(result, new List<double[]>());
                    }

                    results[result].Add(shuffledData[i]);
                }

                PlotResults(results);
                ++epoche;

                if (epoche % epochesToDecreaseStudySpeed == 0)
                {
                    _kochonenNN.ReduceStudySpeed();
                }

                if (epoche % epochesToReduceROI == 0)
                {
                    _kochonenNN.ReduceRoi();
                }

                shuffledData = _data.RandomSort();

                await Task.Delay(1000);
            }
        }

        private IList<double[]> GenerateData()
        {
            var rawData1 = GenerateDataNormal(10, 2, 20, 3, 150);
            var rawData2 = GenerateDataNormal(50, 3, 30, 5, 150);
            var rawData3 = GenerateDataNormal(30, 3, 10, 1, 50);

            var rawData = rawData1.Concat(rawData2).Concat(rawData3).ToArray();
            var universalData = rawData.ToUniversalData(dataItem => dataItem.x, dataItem => dataItem.y);

            universalData.Normalize();
            var shuffledData = universalData.RandomSort();

            return shuffledData;
        }

        private void PlotResults(IDictionary<int, IList<double[]>> results)
        {
            Plot.Plot.Clear();

            Plot.Plot.SetAxisLimitsX(0, 1);
            Plot.Plot.SetAxisLimitsY(0, 1);

            foreach (var result in results)
            {
                var xs = result.Value.Select(dataItem => dataItem[0]).ToArray();
                var ys = result.Value.Select(dataItem => dataItem[1]).ToArray();

                Plot.Plot.AddScatterPoints(xs, ys, _colorsCache[result.Key]);
            }

            Plot.Refresh();
        }

        private void PlotData(IList<double[]> data)
        {
            var xs = data.Select(dataItem => dataItem[0]).ToArray();
            var ys = data.Select(dataItem => dataItem[1]).ToArray();

            Plot.Plot.Clear();

            Plot.Plot.SetAxisLimitsX(0, 1);
            Plot.Plot.SetAxisLimitsY(0, 1);

            Plot.Plot.AddScatterPoints(xs, ys, Color.Red);
            Plot.Refresh();
        }

        private IList<(double x, double y)> GenerateDataNormal(double xMu, double xSigma, double yMu, double ySigma, int count)
        {
            var dt = new List<(double x, double y)>();
            for (int i = 0; i < count / 2; ++i)
            {
                (var x1, var x2) = GetNormallyDistributedVariables(xMu, xSigma);
                (var y1, var y2) = GetNormallyDistributedVariables(yMu, ySigma);

                dt.Add((x1, y1));
                dt.Add((x2, y2));
            }
            return dt;
        }

        private (double x1, double x2) GetNormallyDistributedVariables(double mu, double sigma)
        {
            double x1 = _rand.NextDouble();
            double x2 = _rand.NextDouble();

            var log = Math.Sqrt(-2 * Math.Log(x1));
            var cos = Math.Cos(2 * Math.PI * x2);

            var y1 = log * cos;
            var y2 = log * Math.Sqrt(1 - cos * cos);

            return (y1 * sigma + mu, y2 * sigma + mu);
        }

        private void GenerateDataButton_Click(object sender, EventArgs e)
        {
            _data = GenerateData();
            PlotData(_data);
        }

        private void ResetNetworkButton_Click(object sender, EventArgs e)
        {
            ResetNetwork();
        }

        private void ResetNetwork()
        {
            _kochonenNN = new KochonenNN(2, 3, Convert.ToDouble(InitStudySpeed.Value), WinnerTakesAllCheckBox.Checked);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if(_kochonenNN == null)
            {
                ResetNetwork();
            }

            StartButton.Click -= StartButton_Click;
            StartButton.Click += StopButton_Click;
            StartButton.Text = "Stop Study";

            GenerateDataButton.Enabled = false;
            ResetNetworkButton.Enabled = false;

            _studyInProcess = true;
            Study();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            StartButton.Click -= StopButton_Click;
            StartButton.Click += StartButton_Click;
            StartButton.Text = "Start Study";

            GenerateDataButton.Enabled = true;
            ResetNetworkButton.Enabled = true;

            _studyInProcess = false;
        }
    }
}
