using System;
using System.Linq;
using System.Threading.Tasks;

namespace HopfieldNN
{
    public class HopfieldNetwork
    {
        private readonly int _height;
        private readonly int _width;
        private readonly int _totalSize;

        private readonly int[][] _weights;
        private readonly Func<int, int> _activation;

        public event EventHandler<int[,]> OnTrainItem;
        public event EventHandler<int[,]> OnTestItem;

        public HopfieldNetwork(int height, int width, Func<int, int> activation)
        {
            _height = height;
            _width = width;
            _totalSize = height * width;
            _activation = activation;

            _weights = new int[_totalSize][];

            for(int i = 0; i < _totalSize; ++i)
            {
                _weights[i] = new int[_totalSize];
            }
        }

        public async Task InitWeights(int[][,] trainData)
        {
            foreach(var trainDataItem in trainData)
            {
                if(trainDataItem.GetLength(0) != _height || trainDataItem.GetLength(1) != _width)
                {
                    throw new InvalidOperationException("Data size doesn't fit network size");
                }

                OnTrainItem?.Invoke(this, trainDataItem);

                for(int i = 0; i < _totalSize; ++i)
                {
                    for(int j = 0; j < _totalSize; ++j)
                    {
                        if (i != j)
                        {
                            _weights[i][j] += trainDataItem[i / _width, i % _width] * trainDataItem[j / _width, j % _width];
                        }
                    }
                }

                for (int i = 0; i < _totalSize; ++i)
                {
                    for (int j = 0; j < _totalSize; ++j)
                    {
                        _weights[i][j] = Math.Max(Math.Min(_weights[i][j], 1), -1);
                    }
                }

                await Task.Delay(100);
            }
        }

        public async Task<int[,]> Decide(int[,] testData)
        {
            int[] input = Enumerable.Range(0, _totalSize).Select(i => testData[i / _width, i % _width]).ToArray();

            while (true)
            {
                int[] newOutput = new int[_totalSize];

                for (int i = 0; i < _totalSize; ++i)
                {
                    for (int j = 0; j < _totalSize; ++j)
                    {
                        newOutput[i] += input[j] * _weights[j][i];
                    }
                }

                for(int i = 0; i < _totalSize; ++i)
                {
                    newOutput[i] = _activation(newOutput[i]);
                }

                OnTestItem?.Invoke(this, GetOutput(input));

                if (Enumerable.SequenceEqual(input, newOutput))
                {
                    break;
                }

                input = newOutput;

                await Task.Delay(1000);
            }

            return GetOutput(input);
        }

        private int[,] GetOutput(int[] data)
        {
            int[,] result = new int[_height, _width];
            for (int i = 0; i < _totalSize; ++i)
            {
                result[i / _width, i % _width] = data[i];
            }
            return result;
        }
    }
}
