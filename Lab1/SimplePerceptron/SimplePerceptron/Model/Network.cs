using System;
using System.Linq;
using System.Collections.Generic;

using Newtonsoft.Json;

using SimplePerceptron.Data;

namespace SimplePerceptron.Model
{
    public class Network
    {
        public event EventHandler<EventArgs> OnTrainEpoche;
        public event EventHandler<EventArgs> OnTestEpoche;

        [JsonIgnore]
        private List<double> IterationErrors { get; set; } = new List<double>();

        public List<double> EpocheErrors { get; private set; } = new List<double>();
        public List<double> TestErrors { get; private set; } = new List<double>();

        [JsonIgnore]
        public double[][] VertexErrors { get; private set; }

        [JsonIgnore]
        public double[][,] WeightErrors { get; private set; }

        /// <summary>
        /// Neuron input values
        /// </summary>
        [JsonIgnore]
        public double[][] Values { get; private set; }

        /// <summary>
        /// Neuron output values
        /// </summary>
        [JsonIgnore]
        public double[][] ActivatedValues { get; private set; }

        [JsonProperty]
        private int[] Layers { get; set; }

        [JsonProperty]
        public double[][,] Weights { get; private set; }

        [JsonIgnore]
        public double[][] BiasErrors { get; private set; }

        [JsonProperty]
        public double[][] Biases { get; private set; }

        [JsonIgnore]
        public bool TrainStop { get; set; }

        [JsonIgnore]
        public double TotalError { get; set; }

        public Func<double, double> Activation { get; set; }
        public Func<double, double> ActivationDerivative { get; set; }

        [JsonConstructor]
        public Network() { }

        public Network(params int[] layers)
        {
            Layers = layers;

            Values = new double[layers.Length][];
            ActivatedValues = new double[layers.Length][];
            VertexErrors = new double[layers.Length][];

            for (int i = 0; i < layers.Length; ++i)
            {
                Values[i] = new double[layers[i]];
                ActivatedValues[i] = new double[layers[i]];
                VertexErrors[i] = new double[layers[i]];
            }

            Biases = new double[layers.Length - 1][];
            BiasErrors = new double[layers.Length - 1][];
            WeightErrors = new double[layers.Length - 1][,];
            Weights = new double[layers.Length - 1][,];

            for (int i = 0; i < layers.Length - 1; ++i)
            {
                Biases[i] = Enumerable.Range(0, layers[i + 1]).Select(j => 1.0).ToArray();
                BiasErrors[i] = new double[layers[i + 1]];

                Weights[i] = new double[layers[i], layers[i + 1]];
                WeightErrors[i] = new double[layers[i], layers[i + 1]];
            }
        }

        public void Train(IEnumerable<TrainDataItem> trainDataSet, double speed)
        {
            foreach(TrainDataItem data in trainDataSet)
            {
                double[] output = Process(data);

                FindError(data, output);
                CorrectWeights(speed);

                IterationErrors.Add(TotalError);

                ResetValues();
                ResetErrors();
            }

            EpocheErrors.Add(IterationErrors.Sum());
            IterationErrors.Clear();

            OnTrainEpoche?.Invoke(this, new EventArgs());
        }

        public void Test(IEnumerable<TrainDataItem> dataItems)
        {
            foreach (TrainDataItem data in dataItems)
            {
                double[] output = Process(data);
                FindError(data, output);
                IterationErrors.Add(TotalError);
                ResetValues();
            }

            TestErrors.Add(IterationErrors.Average());
            IterationErrors.Clear();

            OnTestEpoche?.Invoke(this, new EventArgs());
        }

        public int Decide(DataItem input)
        {
            double[] output = Process(input);
            ResetValues();

            int maxi = 0;
            for (int i = 0; i < output.Length; ++i)
                if (output[i] > output[maxi])
                    maxi = i;

            return maxi;
        }

        private void FindError(TrainDataItem input, double[] output)
        {
            TotalError = 0;

            for (int i = 0; i < Layers.Last(); ++i)
            {
                TotalError += Math.Pow(input.Output[i] - output[i], 2.0) / 2.0;
                VertexErrors[VertexErrors.Length - 1][i] = -(input.Output[i] - output[i]) * ActivationDerivative(output[i]);

                for (int j = 0; j < Layers[Layers.Length - 2]; ++j)
                {
                    WeightErrors.Last()[j, i] = VertexErrors[VertexErrors.Length - 1][i] * ActivatedValues[Layers.Length - 2][j];
                }

                BiasErrors[Layers.Length - 2][i] += VertexErrors[Layers.Length - 1][i] * Biases[Layers.Length - 2][i];
            }

            for (int i = Layers.Length - 2; i > 0; --i)
            {
                for (int j = 0; j < Layers[i]; ++j) //current layer counter
                {
                    VertexErrors[i][j] = 0;
                    for (int k = 0; k < Layers[i + 1]; ++k)
                    {
                        VertexErrors[i][j] += VertexErrors[i + 1][k] * Weights[i][j, k];
                    }
                    VertexErrors[i][j] *= ActivationDerivative(ActivatedValues[i][j]);
                    BiasErrors[i - 1][j] += VertexErrors[i][j] * Biases[i - 1][j];

                    for (int k = 0; k < Layers[i - 1]; ++k)
                    {
                        WeightErrors[i - 1][k, j] = VertexErrors[i][j] * ActivatedValues[i - 1][k];
                    }
                }
            }
        }

        private void CorrectWeights(double speed)
        {
            for (int i = 0; i < Layers.Length - 1; ++i)
            {
                for (int j = 0; j < Layers[i]; ++j)
                {
                    for (int k = 0; k < Layers[i + 1]; ++k)
                    {
                        Weights[i][j, k] -= speed * WeightErrors[i][j, k];
                        Biases[i][k] -= speed * BiasErrors[i][k];
                    }
                }
            }
        }

        private double[] Process(DataItem data)
        {
            if (Layers[0] != data.Input.Length)
                throw new ArgumentException("Invalid input size");

            Array.Copy(data.Input, Values[0], Layers[0]);
            Array.Copy(data.Input, ActivatedValues[0], Layers[0]);

            for (int i = 0; i < Layers.Length - 1; ++i)
            {
                for (int j = 0; j < Layers[i + 1]; ++j) //next layer counter
                {
                    for (int k = 0; k < Layers[i]; ++k) //current layer counter
                    {
                        Values[i + 1][j] += ActivatedValues[i][k] * Weights[i][k, j];
                    }

                    Values[i + 1][j] += Biases[i][j];
                    ActivatedValues[i + 1][j] = Activation(Values[i + 1][j]);
                }
            }

            double[] result = new double[Layers.Last()];
            Array.Copy(ActivatedValues.Last(), result, result.Length);

            return result;
        }

        private void ResetValues()
        {
            for (int i = 0; i < Values.Length && i < ActivatedValues.Length; ++i)
            {
                Array.Clear(Values[i], 0, Values[i].Length);
                Array.Clear(ActivatedValues[i], 0, ActivatedValues[i].Length);
            }
        }

        private void ResetErrors()
        {
            for (int i = 0; i < WeightErrors.Length; ++i)
                Array.Clear(WeightErrors[i], 0, WeightErrors[i].Length);

            for (int i = 0; i < BiasErrors.Length; ++i)
                Array.Clear(BiasErrors[i], 0, BiasErrors[i].Length);
        }

        public override string ToString()
        {
            return string.Join("\n", Weights.Select(weight => string.Join(" ", weight)));
        }
    }
}
