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
        private ActivityLogger _logger;

        public ChatBot()
        {
            _responder = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory = new MemoryStore();
            _logger = new ActivityLogger();
            _taskManager = new TaskManager(_logger);
        }

        public string GetGreeting()
        {
            return "Hello! I'm JARVIS your cybersecurity assistant. What’s your name?";
        }

        public string? GetUserName()
        {
            return _memory?.UserName;
        }

        public string ProcessInput(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "Please enter a question.";

            string input = userInput.ToLower();

            // --- Step 1: Add Task intent ---
            if (input.Contains("add task") || input.Contains("add a task") ||
                input.Contains("create task") || input.Contains("enable") || input.Contains("set up") || input.Contains("i need to"))
            {
                string taskName = ExtractTaskName(input);
                try { _taskManager.AddTask(taskName, "Created from chat", ""); } catch { }
                try { _logger.Log($"Task added: '{taskName}'"); } catch { }
                return $"Task added: '{taskName}'. Would you like to set a reminder for this task?";
            }

            // --- Step 2: Reminder intent ---
            if (input.Contains("remind me") || input.Contains("reminder") ||
                input.Contains("set a reminder") || input.Contains("remind me to") || input.Contains("don't forget"))
            {
                string reminderText = ExtractReminderText(input);
                try { ReminderManager.SetReminder(reminderText, DateTime.Now.AddDays(1)); } catch { }
                try { _logger.Log($"Reminder set for '{reminderText}' tomorrow."); } catch { }
                return $"Reminder set for '{reminderText}' on tomorrow's date.";
            }

            // --- Step 3: Quiz intent ---
            if (input.Contains("start quiz") || input.Contains("take quiz") ||
                input.Contains("test my knowledge") || input.Contains("quiz me") || input.Contains("play the game"))
            {
                return "Starting the quiz now! 🎯";
            }

            // --- Step 4: Show log intent ---
            if (input.Contains("show activity log") || input.Contains("what have you done") ||
                input.Contains("what did you do") || input.Contains("show log") || input.Contains("recent actions"))
            {
                try { return _logger.GetRecentLog(); } catch { return "No activity log available."; }
            }

            // --- Step 5: Cybersecurity topics (existing Part 2 logic) ---
            if (input.Contains("password") || input.Contains("phishing") || input.Contains("privacy") ||
                input.Contains("scam") || input.Contains("malware") || input.Contains("2fa"))
            {
                return _responder.GetResponse(input);
            }

            // --- Step 6: Fallback ---
            return "I did not quite understand that. Could you rephrase?";
        }

        // Helper: crude extraction of task name from input
        private string ExtractTaskName(string lowerInput)
        {
            string[] triggers = { "add task", "add a task", "create task", "i need to", "set up", "enable" };
            foreach (var t in triggers)
            {
                int idx = lowerInput.IndexOf(t);
                if (idx >= 0)
                {
                    int start = idx + t.Length;
                    var rest = lowerInput.Substring(start).Trim(new char[] { ' ', ':', '-', '\'' , '"' });
                    if (!string.IsNullOrEmpty(rest))
                        return rest;
                }
            }
            return "New Task";
        }

        // Helper: crude extraction of reminder text
        private string ExtractReminderText(string lowerInput)
        {
            string[] triggers = { "remind me to", "remind me", "set a reminder for", "set a reminder" };
            foreach (var t in triggers)
            {
                int idx = lowerInput.IndexOf(t);
                if (idx >= 0)
                {
                    int start = idx + t.Length;
                    var rest = lowerInput.Substring(start).Trim(new char[] { ' ', ':', '-' , '\'' , '"' });
                    if (!string.IsNullOrEmpty(rest))
                        return rest;
                }
            }
            return "your task";
        }

        public List<string> GetAllKeywordsList()
        {
            return _responder.GetKeywordsList();
        }

        public string GetResponse(string userMessage)
        {
            string lowerMsg = userMessage.ToLower();

            // --- TASK TRIGGER ---
            if (lowerMsg.Contains("task"))
            {
                // Case 1: Add task directly
                if (lowerMsg.StartsWith("add task"))
                {
                    string[] parts = userMessage.Split('-', 2);
                    string title = parts.Length > 1 ? parts[1].Trim() : "New Task";

                    _taskManager.AddTask(title, "Created from chat", "");
                    return $"✅ Task added: '{title}'. Would you like to set a reminder, {_memory.UserName}?";
                }

                // Case 2: Complete task by ID
                if (lowerMsg.StartsWith("complete task"))
                {
                    string[] words = lowerMsg.Split(' ');
                    if (words.Length >= 3 && int.TryParse(words[2], out int id))
                    {
                        _taskManager.MarkAsComplete(id);
                        return $"✔️ Task {id} marked as complete.";
                    }
                }

                // Case 3: Delete task by ID
                if (lowerMsg.StartsWith("delete task"))
                {
                    string[] words = lowerMsg.Split(' ');
                    if (words.Length >= 3 && int.TryParse(words[2], out int id))
                    {
                        _taskManager.DeleteTask(id);
                        return $"🗑️ Task {id} deleted.";
                    }
                }

                // Case 4: General mention of "task" → show list
                var tasks = _taskManager.GetAllTasks();
                string taskList = tasks.Count == 0
                    ? "You currently have no tasks."
                    : string.Join("\n", tasks.Select(t =>
                        $"{t.Id}. {t.Title} - {(t.IsComplete ? "✅ Complete" : "❌ Incomplete")}"));

                return $"What task would you like to add, {_memory.UserName}? \nHere are your current tasks:\n{taskList}";
            }

            // --- Other chatbot logic ---
            return _responder.GetResponse(userMessage);
        }
    }

    internal class ReminderManager
    {
        private record Reminder(string Text, DateTime When);

        private static readonly List<Reminder> _reminders = new List<Reminder>();
        private static readonly object _lock = new object();

        public static void SetReminder(string text, DateTime when)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Reminder text must be provided.", nameof(text));

            // Normalize past times to now
            if (when < DateTime.Now)
                when = DateTime.Now;

            var reminder = new Reminder(text, when);
            lock (_lock)
            {
                _reminders.Add(reminder);
            }

            try
            {
                // Note: Static context, cannot access instance logger
                // Logging is handled by ChatBot.ProcessInput instead
            }
            catch
            {
                // Swallow logging errors to avoid breaking callers
            }
        }

        public static IReadOnlyList<(string Text, DateTime When)> GetReminders()
        {
            lock (_lock)
            {
                return _reminders.Select(r => (r.Text, r.When)).ToList().AsReadOnly();
            }
        }
    }
}
