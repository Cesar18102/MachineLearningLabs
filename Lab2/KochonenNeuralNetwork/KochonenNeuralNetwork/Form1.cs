using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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

                PlotResults(results, XAxisParameter.SelectedIndex, YAxisParameter.SelectedIndex);
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

        private void PlotResults(IDictionary<int, IList<double[]>> results, int axisX, int axisY)
        {
            if(axisX == -1 || axisY == -1)
            {
                return;
            }

            Plot.Plot.Clear();

            Plot.Plot.SetAxisLimitsX(0, 1);
            Plot.Plot.SetAxisLimitsY(0, 1);

            foreach (var result in results)
            {
                var xs = result.Value.Select(dataItem => dataItem[axisX]).ToArray();
                var ys = result.Value.Select(dataItem => dataItem[axisY]).ToArray();

                Plot.Plot.AddScatterPoints(xs, ys, _colorsCache[result.Key]);
            }

            Plot.Refresh();
        }

        private void PlotData(IList<double[]> data, int axisX, int axisY)
        {
            if (axisX == -1 || axisY == -1)
            {
                return;
            }

            var xs = data.Select(dataItem => dataItem[axisX]).ToArray();
            var ys = data.Select(dataItem => dataItem[axisY]).ToArray();

            Plot.Plot.Clear();

            Plot.Plot.SetAxisLimitsX(0, 1);
            Plot.Plot.SetAxisLimitsY(0, 1);

            Plot.Plot.AddScatterPoints(xs, ys, Color.Red);
            Plot.Refresh();
        }

        private void ResetNetworkButton_Click(object sender, EventArgs e)
        {
            ResetNetwork();
        }

        private void ResetNetwork()
        {
            _kochonenNN = new KochonenNN(_data[0].Length, Convert.ToInt32(CountOfClusters.Value), Convert.ToDouble(InitStudySpeed.Value), WinnerTakesAllCheckBox.Checked);
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

        private void GenerateDataButton_Click(object sender, EventArgs e)
        {
            _data = GenerateData();

            ResetAxisSelectors(new string[] { "X", "Y" });
            PlotData(_data, XAxisParameter.SelectedIndex, YAxisParameter.SelectedIndex);
        }

        private void LoadPhonesData_Click(object sender, EventArgs e)
        {
            var phones = GetPhonesData();

            var universalData = phones.ToUniversalData(
                phone => phone.Price,
                phone => phone.RAM,
                phone => phone.ROM,
                phone => phone.ProcessorsCount,
                phone => phone.AverageProcessorFrequency,
                phone => phone.ScreenDiagonal,
                phone => phone.TotalFrontCameraResolution,
                phone => phone.TotalMainCameraResolution,
                phone => phone.AccumulatorCapacity,
                phone => phone.Weight
            );

            universalData.Normalize();
            _data = universalData.RandomSort();

            var dataNames = new string[] 
            { 
                "Price", "RAM", "ROM", "ProcessorsCount", "AverageProcessorFrequency", 
                "ScreenDiagonal", "TotalFrontCameraResolution", "TotalMainCameraResolution", 
                "AccumulatorCapacity", "Weight"
            };
            ResetAxisSelectors(dataNames);
        } 

        private void ResetAxisSelectors(object[] axes)
        {
            XAxisParameter.Items.Clear();
            XAxisParameter.Items.AddRange(axes);

            YAxisParameter.Items.Clear();
            YAxisParameter.Items.AddRange(axes);

            XAxisParameter.SelectedIndex = 0;
            YAxisParameter.SelectedIndex = 1;
        }

        private Phone[] GetPhonesData()
        {
            using (var phonesStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KochonenNeuralNetwork.res.phones.json"))
            using (var str = new StreamReader(phonesStream))
            {
                var phonesData = str.ReadToEnd();
                return JsonConvert.DeserializeObject<Phone[]>(phonesData);
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

        private void YAxisParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlotData(_data, XAxisParameter.SelectedIndex, YAxisParameter.SelectedIndex);
        }

        private void XAxisParameter_SelectedIndexChanged(object sender, EventArgs e)
        {
            PlotData(_data, XAxisParameter.SelectedIndex, YAxisParameter.SelectedIndex);
        }
    }
}
