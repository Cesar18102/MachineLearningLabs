using System;
using System.Collections.Generic;
using System.Linq;

namespace KochonenNeuralNetwork
{
    public static class Extensions
    {
        private static Random _rand = new Random();

        public static IList<T> RandomSort<T>(this IEnumerable<T> data)
        {
            return data.OrderBy(item => _rand.NextDouble()).ToList();
        }

        public static IList<double[]> ToUniversalData<T>(this IList<T> inputData, params Func<T, double>[] dataSelectors)
        {
            return inputData.Select(dataItem => dataSelectors.Select(selector => selector(dataItem)).ToArray()).ToArray();
        }

        public static void Normalize(this IList<double[]> dataItems)
        {
            var parametersCount = dataItems[0].Length;

            (double min, double max)[] minsAndMaxes = Enumerable.Range(0, parametersCount)
                .Select(i => (dataItems.Min(dataItem => dataItem[i]), dataItems.Max(dataItem => dataItem[i])))
                .ToArray();

            for (int i = 0; i < parametersCount; ++i)
            {
                foreach (var dataItem in dataItems)
                {
                    dataItem[i] = (dataItem[i] - minsAndMaxes[i].min) / (minsAndMaxes[i].max - minsAndMaxes[i].min);
                }
            }
        }
    }
}
