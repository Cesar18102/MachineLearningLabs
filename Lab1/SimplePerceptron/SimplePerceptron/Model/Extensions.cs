using System;
using System.Linq;
using System.Collections.Generic;
using SimplePerceptron.Data;
using SimplePerceptron.Input;

namespace SimplePerceptron.Model
{
    public static class Extensions
    {
        private static Random Random = new Random(DateTime.Now.Millisecond);

        public static double GetRandomNumber(double min, double max)
        {
            double rand = (double)Random.NextDouble();
            return rand * (max - min) + min;
        }

        public static void InitRandomWeights(this Network network, double min, double max)
        {
            for(int i = 0; i < network.Weights.Length; ++i)
            {
                int currentLayerNeuronsCount = network.Weights[i].GetLength(0);
                int nextLayerNeuronsCount = network.Weights[i].GetLength(1);

                for (int j = 0; j < currentLayerNeuronsCount; ++j)
                    for(int k = 0; k < nextLayerNeuronsCount; ++k)
                        network.Weights[i][j, k] = GetRandomNumber(min, max);
            }
        }

        public static IEnumerable<T> RandomSort<T>(this IEnumerable<T> data)
        {
            return data.OrderBy(item => Random.NextDouble()).ToList();
        }

        public static (ICollection<T> train, ICollection<T> test) TrainTestSplit<T>(this IEnumerable<T> data, double trainRatio)
        {
            int testCount = (int)((1.0 - trainRatio) * data.Count());

            List<T> train = new List<T>(data);
            List<T> test = new List<T>();

            for(int i = 0; i < testCount; ++i)
            {
                int index = Random.Next(0, train.Count);

                test.Add(train.ElementAt(index));
                train.RemoveAt(index);
            }

            return (train, test);
        }

        public static IList<TrainDataItem> ToTrainDataItems(this IList<Input2D> inputs, int outputTypesCount)
        {
            return inputs.Select(input => new TrainDataItem(new double[] { input.X, input.Y }, input.T.GetResultArray(outputTypesCount)))
                .ToArray();
        }

        public static void Normalize(this IList<TrainDataItem> dataItems)
        {
            var parametersCount = dataItems[0].Input.Length;

            (double min, double max)[] minsAndMaxes = Enumerable.Range(0, parametersCount)
                .Select(i => (dataItems.Min(dataItem => dataItem.Input[i]), dataItems.Max(dataItem => dataItem.Input[i])))
                .ToArray();

            for(int i = 0; i < parametersCount; ++i)
            {
                foreach (var dataItem in dataItems)
                {
                    dataItem.Input[i] = (dataItem.Input[i] - minsAndMaxes[i].min) / (minsAndMaxes[i].max - minsAndMaxes[i].min);
                }
            }
        }

        public static double[] GetResultArray(this int correctIndex, int count)
        {
            var resultArray = new double[count];
            resultArray[correctIndex] = 1.0;
            return resultArray;
        }

        public static int GetResultIndex(this double[] values)
        {
            var maxValue = values.Max();
            for (int i = 0; i < values.Length; ++i)
            {
                if (values[i] == maxValue)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
