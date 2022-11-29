using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using SimplePerceptron.Data;
using SimplePerceptron.Input;
using SimplePerceptron.Model;

using SFML.Graphics;
using SFML.Window;
using SFML.System;

namespace SimplePerceptron
{
    public class Program
    {
        private const double TRAIN_RATIO = 0.8f;

        private const double STUDY_SPEED = 0.1f;
        private const int COUNT_OF_TYPES = 3;

        private static Random _rand = new Random();

        private static Network Network = new Network(2, 5, COUNT_OF_TYPES);
        private static (ICollection<TrainDataItem> train, ICollection<TrainDataItem> test) Data { get; set; }

        private static bool Control { get; set; }
        private static RenderWindow Window { get; set; }

        public static void Main(string[] args)
        {
            var rawData1 = GenerateDataNormal(10, 5, 20, 8, 150, 0);
            var rawData2 = GenerateDataNormal(50, 10, 30, 11, 150, 1);
            var rawData3 = GenerateDataNormal(30, 7, 10, 7, 50, 2);

            var rawData = rawData1.Concat(rawData2).Concat(rawData3).ToArray();
            rawData.RandomSort();

            var dataSet = rawData.ToTrainDataItems(COUNT_OF_TYPES);
            dataSet.Normalize();

            //using (StreamWriter str = new StreamWriter(Environment.CurrentDirectory + "/input.txt", false))
                //str.WriteLine(JsonConvert.SerializeObject(dt));

            Network.OnTrainEpoche += Network_OnTrainEpoche;
            Network.OnTestEpoche += Network_OnTestEpoche;

            Network.InitRandomWeights(-0.5f, 0.5f);

            Network.Activation = x => 1.0 / (1.0 + Math.Pow(Math.E, -x));
            Network.ActivationDerivative = x => x * (1 - x);

            //string input = "";
            //using (StreamReader str = new StreamReader(Environment.CurrentDirectory + "/input.txt"))
                //input = str.ReadToEnd();

            //IEnumerable<Input2D> rawData = JsonConvert.DeserializeObject<IEnumerable<Input2D>>(input);

            Data = dataSet.TrainTestSplit(TRAIN_RATIO);
            Console.WriteLine($"Training on {Data.train.Count()} cases (speed = {STUDY_SPEED})");

            Window = new RenderWindow(VideoMode.FullscreenModes[0], "graphics");
            Window.MouseButtonPressed += Window_MouseButtonPressed;
            Window.KeyPressed += Window_KeyPressed;
            Window.KeyReleased += Window_KeyReleased;

            double delta = 0.02;
            Vector2f topLeft = new Vector2f(0, 1);
            Vector2f size = new Vector2f(1, 1);
            Color[] colors = new Color[4] { Color.Red, Color.Green, Color.Blue, Color.Yellow };

            Vector2f counts = size / (float)delta;
            Vector2f rectSize = new Vector2f(size.X / counts.X, size.Y / counts.Y);

            float minScreenSize = Math.Min(Window.Size.X, Window.Size.Y);
            Vector2f projSize = new Vector2f(minScreenSize / counts.X, minScreenSize / counts.Y);

            for (int i = 0; !Network.TrainStop; ++i)
            {
                Window.Clear();
                Window.DispatchEvents();

                if (Data.train.Count() != 0)
                {
                    Console.Write($"{i}) ");
                    Network.Train(Data.train.RandomSort(), STUDY_SPEED);
                    Network.Test(Data.test.RandomSort());

                    for (int j = 0; j < counts.Y; ++j)
                    {
                        for (int k = 0; k < counts.X; ++k)
                        {
                            DataItem data = new DataItem(new double[] {
                                k * rectSize.X + topLeft.X,
                                topLeft.Y - j * rectSize.Y
                            });

                            int type = Network.Decide(data);

                            if (type == -1)
                                continue;

                            Shape shape = new RectangleShape(projSize);
                            shape.Position = new Vector2f(k * projSize.X, j * projSize.Y);
                            shape.FillColor = colors[type];

                            Window.Draw(shape);
                        }
                    }

                    foreach (TrainDataItem data in Data.train)
                    {
                        Shape point = new CircleShape(5);

                        point.Position = new Vector2f(
                            (float)data.Input[0] * minScreenSize,
                            (1 - (float)data.Input[1]) * minScreenSize
                        );

                        point.FillColor = colors[data.Output.GetResultIndex()];
                        point.OutlineColor = Color.Black;
                        point.OutlineThickness = 2;

                        Window.Draw(point);
                    }
                }

                Window.Display();
            }

            int counter = 0;
            Console.WriteLine($"***\n\nTesting started\n\n***");
            foreach (TrainDataItem data in Data.test)
            {
                int result = Network.Decide(data);

                if (result == data.Output.GetResultIndex())
                    ++counter;
            }

            Console.WriteLine($"{counter} of {Data.test.Count()} test cases were successful");

            Console.WriteLine("Do you want to save network? (Y/N) ");
            if(Console.ReadLine().StartsWith("Y"))
            {
                Console.WriteLine("Input network name: ");
                string name = Console.ReadLine();

                string config = JsonConvert.SerializeObject(Network);
                using (StreamWriter strw = File.CreateText(Environment.CurrentDirectory + $"/Networks/{name}.json"))
                    strw.WriteLine(config);

                string errors = JsonConvert.SerializeObject(Network.EpocheErrors);
                using (StreamWriter strw = File.CreateText(Environment.CurrentDirectory + $"/Networks/{name}-errors.json"))
                    strw.WriteLine(errors);
            }
        }

        private static void Window_KeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.LControl)
                Control = false;
        }

        private static void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.LControl)
                Control = true;
        }

        private static void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button != Mouse.Button.Left && e.Button != Mouse.Button.Right)
                return;

            int output = e.Button == Mouse.Button.Left ? 0 : 1;

            if (Control)
                output += 2;

            double minScreenSize = Math.Min(Window.Size.X, Window.Size.Y);

            double[] point = new double[] {
                e.X / minScreenSize,
                1 - e.Y / minScreenSize
            };

            Data.train.Add(new TrainDataItem(point, output.GetResultArray(COUNT_OF_TYPES)));
        }

        private static void Network_OnTrainEpoche(object sender, EventArgs e)
        {
            double error = Network.EpocheErrors.Last();
            Console.WriteLine($"EPOCHE TRAIN ERROR = {error}");
        }

        private static void Network_OnTestEpoche(object sender, EventArgs e)
        {
            double error = Network.TestErrors.Last();
            Console.WriteLine($"TEST ERROR = {error}");
        }

        private static IList<Input2D> GenerateDataNormal(double xMu, double xSigma, double yMu, double ySigma, int count, int type)
        {
            var dt = new List<Input2D>();            
            for (int i = 0; i < count / 2; ++i)
            {
                (var x1, var x2) = GetNormallyDistributedVariables(xMu, xSigma);
                (var y1, var y2) = GetNormallyDistributedVariables(yMu, ySigma);

                dt.Add(new Input2D(x1, y1, type));
                dt.Add(new Input2D(x2, y2, type));
            }
            return dt;
        }

        private static (double x1, double x2) GetNormallyDistributedVariables(double mu, double sigma)
        {
            double x1 = _rand.NextDouble();
            double x2 = _rand.NextDouble();

            var log = Math.Sqrt(-2 * Math.Log(x1));
            var cos = Math.Cos(2 * Math.PI * x2);

            var y1 = log * cos;
            var y2 = log * Math.Sqrt(1 - cos * cos);

            return (y1 * sigma + mu, y2 * sigma + mu);
        }
    }
}
