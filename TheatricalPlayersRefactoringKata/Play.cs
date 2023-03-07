namespace TheatricalPlayersRefactoringKata
{
    public class Play
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public Play(string name, string type) {
            Name = name;
            Type = type;
        }
    }
}
