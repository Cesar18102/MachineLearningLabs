using System;
using System.Collections.Generic;
using System.Linq;

namespace KochonenNeuralNetwork
{
    public class KochonenNN
    {
        private Random _rand = new Random();

        private int _inputSize;
        private int _clustersCount;

        private double _studySpeed;
        private int _currentROIWidth;
        private int _currentROIHeight;

        public double[,][] Weights { get; private set; }

        public KochonenNN(int inputSize, int clustersCount, double startStudySpeed)
        {
            _inputSize = inputSize;
            _clustersCount = clustersCount;
            _studySpeed = startStudySpeed;
            _currentROIWidth = clustersCount % 2 == 0 ? clustersCount - 1 : clustersCount;
            _currentROIHeight = inputSize % 2 == 0 ? inputSize - 1 : inputSize;

            Weights = new double[inputSize, clustersCount][];

            for (int i = 0; i < inputSize; ++i)
            {
                for (int j = 0; j < clustersCount; ++j)
                {
                    Weights[i, j] = GetRandomWeights(0, 1, inputSize);
                }
            }
        }

        public int ProcessInput(double[] data)
        {
            var closestNeuron = FindClosestNeuron(data);
            var neighbours = GetNeighbours(closestNeuron.i, closestNeuron.j);

            foreach (var neighbour in neighbours)
            {
                ModifyNeuronWeights(neighbour.i, neighbour.j, data);
            }*

            return closestNeuron.j;
        }

        public void ReduceStudySpeed()
        {
            _studySpeed *= 0.99;
        }

        public void ReduceRoi()
        {
            _currentROIWidth = Math.Max(_currentROIWidth - 2, 1);
            _currentROIHeight = Math.Max(_currentROIHeight - 2, 1);
        }

        public int Decide(double[] data)
        {
            return FindClosestNeuron(data).j;
        }

        private IList<(int i, int j)> GetNeighbours(int i, int j)
        {
            var neighbours = new List<(int i, int j)>() { };

            for(int k = -_currentROIHeight / 2; k <= _currentROIHeight / 2; ++k)
            {
                for(int q = -_currentROIWidth / 2; q <= _currentROIWidth / 2; ++q)
                {
                    var y = (i + k + _inputSize) % _inputSize;
                    var x = (j + q + _clustersCount) % _clustersCount;

                    neighbours.Add((y, x));
                }
            }

            return neighbours;
        }

        private void ModifyNeuronWeights(int i, int j, double[] data)
        {
            var weights = Weights[i, j];
            for(int k = 0; k < weights.Length; ++k)
            {
                weights[k] += _studySpeed * (data[k] - weights[k]);
            }
        }

        private (int i, int j) FindClosestNeuron(double[] data)
        {
            (int bestI, int bestJ) result = (0, 0);
            var minError = GetError(Weights[0, 0], data);

            for (int i = 0; i < _inputSize; ++i)
            {
                for (int j = 0; j < _clustersCount; ++j)
                {
                    var error = GetError(Weights[i, j], data);

                    if (error < minError)
                    {
                        minError = error;
                        result.bestI = i;
                        result.bestJ = j;
                    }
                }
            }

            return result;
        }

        private double GetError(double[] weights, double[] target)
        {
            double error = 0;
            for(int i = 0; i < weights.Length; ++i)
            {
                error += Math.Pow(weights[i] - target[i], 2.0);
            }
            return error;
        }

        private double[] GetRandomWeights(double min, double max, int count)
        {
            var range = max - min;
            return Enumerable.Range(0, count).Select(i => _rand.NextDouble() * range - min).ToArray();
        }
    }
}
