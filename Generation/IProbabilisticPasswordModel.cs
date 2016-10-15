namespace pw.Generation
{
    public interface IProbabilisticPasswordModel : Evaluation.IProbabilisticPasswordModel
    {
        string GenerateSamplePassword();
    }
}
