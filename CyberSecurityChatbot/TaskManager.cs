using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityChatbot
{
    public class TaskManager
    {
        private TaskStorageHelper _storage;
        private ActivityLogger _logger;

        public TaskManager(ActivityLogger logger)
        {
            _storage = new TaskStorageHelper();
            _logger = logger;
        }

        public string AddTask(string title, string description, string reminder)
        {
            _storage.AddTask(title, description, reminder);
            _logger.Log($"Task added: '{title}' {(string.IsNullOrEmpty(reminder) ? "(no reminder set)" : $"(Reminder: {reminder})")}");
            return $"Task added with the description '{description}'. Would you like a reminder?";
        }

        public List<CyberTask> GetAllTasks()
        {
            return _storage.LoadTasks();
        }

        public void MarkAsComplete(int id)
        {
            _storage.MarkAsComplete(id);
            var task = _storage.LoadTasks().Find(t => t.Id == id);
            if (task != null)
            {
                _logger.Log($"Task marked complete: '{task.Title}'");
            }
        }

        public void DeleteTask(int id)
        {
            var task = _storage.LoadTasks().Find(t => t.Id == id);
            if (task != null)
            {
                _logger.Log($"Task deleted: '{task.Title}'");
            }
            _storage.DeleteTask(id);
        }
    }
}