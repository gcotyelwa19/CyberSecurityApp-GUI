using System;

namespace CyberSecurityChatbot
{
    public static class ActivityLogger
    {
        public static void Log(string message)
        {
            // Minimal logger to avoid breaking references. Writes to console.
            Console.WriteLine(message);
        }
    }
}
