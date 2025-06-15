namespace apbd16_ex2.Utils;

public class OverfillException : Exception
{
    public OverfillException(string message) : base(message) { }
}