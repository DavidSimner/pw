namespace pw.Evaluation
{
    public interface IProbabilisticPasswordModel
    {
        double CalculateProbability(string password);
    }
}
