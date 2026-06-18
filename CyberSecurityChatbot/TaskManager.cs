using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityChatbot
{
    public class TaskManager
    {
        private TaskStorageHelper storage = new TaskStorageHelper();

        public string AddTask(string title, string description, string reminder)
        {
            storage.AddTask(title, description, reminder);
            ActivityLogger.Log($"Task added: {title}");
            return $"Task added with the description '{description}'. Would you like a reminder?";
        }

        public List<CyberTask> GetAllTasks()
        {
            return storage.LoadTasks();
        }

        public void MarkAsComplete(int id)
        {
            storage.MarkAsComplete(id);
            ActivityLogger.Log($"Task {id} marked complete.");
        }

        public void DeleteTask(int id)
        {
            storage.DeleteTask(id);
            ActivityLogger.Log($"Task {id} deleted.");
        }
    }

}
