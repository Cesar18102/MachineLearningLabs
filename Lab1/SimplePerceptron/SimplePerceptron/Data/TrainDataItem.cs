namespace SimplePerceptron.Data
{
    public class TrainDataItem : DataItem
    {
        public double[] Output { get; private set; }

        public TrainDataItem(double[] input, double[] output) : base(input)
        {
            Output = output;
        }
    }
}
