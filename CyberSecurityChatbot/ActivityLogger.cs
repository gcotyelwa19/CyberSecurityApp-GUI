using System;
using System.Collections.Generic;
using System.Linq;

public class ActivityLogger
{
    private List<string> _log = new List<string>();

    // Log an action with timestamp
    public void Log(string action)
    {
        string entry = DateTime.Now.ToString("[HH:mm] ") + action;
        _log.Add(entry);
    }

    // Get recent log entries (default 10)
    public string GetRecentLog(int count = 10)
    {
        var recent = _log.TakeLast(count).ToList();
        return FormatLog(recent);
    }

    // Get full log
    public string GetFullLog()
    {
        return FormatLog(_log);
    }

    // Get total count
    public int GetCount()
    {
        return _log.Count;
    }

    // Format entries as numbered list
    private string FormatLog(List<string> entries)
    {
        if (entries.Count == 0) return "No actions logged yet.";

        string result = "Here's a summary of recent actions:\n";
        for (int i = 0; i < entries.Count; i++)
        {
            result += $"{i + 1}. {entries[i]}\n";
        }
        return result.Trim();
    }
}

