using Newtonsoft.Json;

namespace SimplePerceptron.Input
{
    public class Input2D
    {
        [JsonProperty]
        public double X { get; private set; }

        [JsonProperty]
        public double Y { get; private set; }

        [JsonProperty]
        public int T { get; private set; }

        [JsonConstructor]
        public Input2D() { }

        public Input2D(double x, double y, int t)
        {
            X = x;
            Y = y;
            T = t;
        }
    }
}
