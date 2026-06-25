namespace CyberSecurityChatbot
{
    public class MemoryStore
    {
        // Stores the user's name once they introduce themselves
        public string UserName { get;private set; }

        public void SaveName(string name)
        {
            UserName = name;
        }

        // Stores the last topic the user asked about
        public string LastTopic { get; set; }

        // Stores the user's favourite topic
        public string FavouriteTopic { get; set; }

        public MemoryStore()
        {
            UserName = string.Empty;
            LastTopic = string.Empty;
            FavouriteTopic = string.Empty;
        }

        // check if we know the user's name
        public bool HasUserName()
        {
            return !string.IsNullOrWhiteSpace(UserName);
        }

        // check if we have a last topic
        public bool HasLastTopic()
        {
            return !string.IsNullOrWhiteSpace(LastTopic);
        }

        public bool HasFavouriteTopic()
        {
            return !string.IsNullOrWhiteSpace(FavouriteTopic);
        }

        // Build a personalised opener using stored info
        public string GetPersonalisedOpener()
        {
            string opener = "";

            if (HasUserName())
                opener += $"👋 Hi {UserName}! ";

            if (HasFavouriteTopic())
                opener += $"As someone interested in {FavouriteTopic}, here’s a tip: ";

            return opener;
        }
    }
}
