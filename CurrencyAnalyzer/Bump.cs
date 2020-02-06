namespace CurrencyAnalyzer
{
    internal class Bump
    {
        public readonly decimal StartRate;
        public uint Days;

        public Bump(decimal startRate)
        {
            StartRate = startRate;
            Days = 1;
        }
    }
}
