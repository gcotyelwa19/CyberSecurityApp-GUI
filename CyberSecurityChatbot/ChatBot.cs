using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot
{
    internal class ChatBot
    {
        private KeywordResponder _responder;
        private SentimentDetector _sentiment;
        private MemoryStore _memory;
        private TaskManager _taskManager;

        public ChatBot()
        {
            _responder = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory = new MemoryStore();
            _taskManager = new TaskManager();
        }

        public string GetGreeting()
        {
            return "Hello! I'm JARVIS your cybersecurity assistant. What’s your name?";
        }

        public string? GetUserName()
        {
            return _memory?.UserName;
        }

        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please enter a question.";

            string lower = input.Trim().ToLowerInvariant();

            // 1. Handle name input
            if (lower.StartsWith("my name is"))
            {
                const string pattern = "my name is";
                string name = input.Substring(pattern.Length).Trim();
                _memory.UserName = name;
                return $"👋 Nice to meet you, {name}! How can I help with cybersecurity today?";
            }

            // 1.0 Handling favourite topic input (accept with or without apostrophe)
            if (lower.StartsWith("i'm interested in") || lower.StartsWith("im interested in"))
            {
                int offset = lower.StartsWith("i'm interested in") ? "i'm interested in".Length : "im interested in".Length;
                string topic = input.Substring(offset).Trim();
                _memory.FavouriteTopic = topic;
                return $"⭐ Got it! I’ll remember that you’re interested in {topic}.";
            }

            // 2. Handle follow-up phrases
            if ((lower.Contains("tell me more") || lower.Contains("explain more")) && _memory.HasLastTopic())
            {
                return $"🔎 Continuing on {_memory.LastTopic}: {_responder.GetResponse(_memory.LastTopic)}";
            }

            // 3. Sentiment detection
            Sentiment sentiment = _sentiment.Detect(input);
            string sentimentResponse = _sentiment.GetSentimentResponse(sentiment);

            // 4. Keyword response
            string keywordResponse = _responder.GetResponse(input);

            // Save last topic for follow-ups
            _memory.LastTopic = input;

            // 5. Special phrases
            if (lower.Contains("how are you"))
                return "🙂 I’m just code, but I’m running smoothly! Thanks for asking.";
            if (lower.Contains("what can you do"))
                return "🛡️ I can explain cybersecurity topics like phishing, firewalls, VPNs, and more.";
            if (lower.Contains("purpose"))
                return "🎯 My purpose is to help you learn about cybersecurity best practices.";
            string personalisedOpener = _memory.GetPersonalisedOpener();

            // 6. Default fallback
            return $"{sentimentResponse}{personalisedOpener}{keywordResponse}";
        }

        public List<string> GetAllKeywordsList()
        {
            return _responder.GetKeywordsList();
        }

        public string GetResponse(string userMessage)
        {
            // Normalize input
            string lowerMsg = userMessage.ToLower();

            // --- TASK TRIGGER ---
            if (lowerMsg.Contains("task"))
            {
                // Get current tasks
                var tasks = _taskManager.GetAllTasks();

                // Format task list nicely
                string taskList = tasks.Count == 0
                    ? "You currently have no tasks."
                    : string.Join("\n", tasks.Select(t =>
                        $"{t.Id}. {t.Title} - {(t.IsComplete ? "✅ Complete" : "❌ Incomplete")}"));

                // Respond with prompt + list
                return $"What task would you like to add, {_memory.UserName}? \nHere are your current tasks:\n{taskList}";
            }

            // --- Other chatbot logic here ---
            // e.g. keyword detection for greetings, quiz, etc.
            return _responder.GetResponse(userMessage);
        }

    }
}
