namespace SimplePerceptron.Data
{
    public class DataItem
    {
        public double[] Input { get; private set; }
        
        public DataItem(double[] input)
        {
            Input = input;
        }
    }
}
