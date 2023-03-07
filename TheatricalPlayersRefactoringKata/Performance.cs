namespace TheatricalPlayersRefactoringKata
{
    public class Performance
    {
        public string PlayID { get; }

        public int Audience { get; }

        public Performance(string playID, int audience)
        {
            PlayID = playID;
            Audience = audience;
        }

    }
}
