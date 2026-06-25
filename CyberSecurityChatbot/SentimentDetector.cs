using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public enum Sentiment
    {
        Neutral,
        Worried,
        Curious,
        Frustrated,
        Happy
    }

    public class SentimentDetector
    {
        private Dictionary<Sentiment, List<string>> _triggers;

        public SentimentDetector()
        {
            _triggers = new Dictionary<Sentiment, List<string>>
            {
                { Sentiment.Worried, new List<string> { "worried", "scared", "afraid", "anxious", "nervous", "unsafe" } },
                { Sentiment.Curious, new List<string> { "curious", "wondering", "interested", "want to know", "how does" } },
                { Sentiment.Frustrated, new List<string> { "frustrated", "annoyed", "confused", "don't understand" } },
                { Sentiment.Happy, new List<string> { "great", "thanks", "helpful", "awesome", "love it" } }
            };
        }

        public Sentiment Detect(string input)
        {
            string lower = input.ToLower();
            foreach (var pair in _triggers)
            {
                foreach (var word in pair.Value)
                {
                    if (lower.Contains(word))
                        return pair.Key;
                }
            }
            return Sentiment.Neutral;
        }

        public string GetSentimentResponse(Sentiment sentiment)
        {
            switch (sentiment)
            {
                case Sentiment.Worried:
                    return "😟 I understand your concern. Let’s look at some ways to stay safe. ";
                case Sentiment.Curious:
                    return "🤔 Great question! Let’s explore that together. ";
                case Sentiment.Frustrated:
                    return "😤 I know this can be confusing. Let me break it down. ";
                case Sentiment.Happy:
                    return "😊 I’m glad you’re feeling positive! Here’s more info: ";
                default:
                    return ""; // Neutral → no opener
            }
        }
    }
}
