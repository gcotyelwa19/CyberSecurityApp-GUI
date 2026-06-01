using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    internal class KeywordResponder
    {
        private Dictionary<string, List<string>> _responses;
        private static readonly Random _rand = new Random();

        public KeywordResponder()
        {
            _responses = new Dictionary<string, List<string>>
            {
                { "password", new List<string> {
                    "Use unique, strong passwords for each account.",
                    "Consider a password manager to keep track safely."
                }},
                { "phishing", new List<string> {
                    "Phishing emails often look real but contain malicious links.",
                    "Always check the sender’s address carefully before clicking."
                }},
                { "firewall", new List<string> {
                    "A firewall blocks unauthorized access to your network.",
                    "Think of it as a security guard for your computer."
                }},
                { "vpn", new List<string> {
                    "A VPN encrypts your internet traffic and hides your IP address.",
                    "Use a VPN when on public Wi‑Fi for extra safety."
                }},
                { "ransomware", new List<string> {
                    "Ransomware locks your files until you pay a ransom.",
                    "Back up your data regularly to protect against ransomware."
                }},
                { "two factor", new List<string> {
                    "Two‑factor authentication adds an extra layer of security.",
                    "Even if your password is stolen, 2FA keeps your account safe."
                }},
                { "update", new List<string> {
                    "Keep your systems and software up to date to patch vulnerabilities.",
                    "Enable automatic updates to stay protected."
                }},
                { "encryption", new List<string> {
                    "Encryption scrambles data so only authorized parties can read it.",
                    "It's essential for protecting sensitive information."
                }},
                { "privacy", new List<string> {
                    "🕵️ Review app permissions carefully to protect your privacy.",
                    "🔍 Limit what personal data you share online."
                }},
                    { "social media", new List<string> {
                        "Be cautious about what you share on social media.",
                        "Use strong privacy settings to control who sees your posts."
                    }},
                    { "public wifi", new List<string> {
                        "Avoid accessing sensitive accounts on public Wi‑Fi.",
                        "Use a VPN if you need to connect to public Wi‑Fi."
                    }},
                    { "malware", new List<string> {
        "🐛 Malware is malicious software designed to harm your system.",
        "🧹 Use antivirus tools to detect and remove malware."
    }},
    { "social engineering", new List<string> {
        "🎭 Social engineering tricks people into revealing sensitive info.",
        "☎️ Always verify requests before sharing personal data."
    }},
    { "backup", new List<string> {
        "💾 Regular backups protect your data from loss.",
        "☁️ Cloud backups add extra resilience."
    }},
    { "antivirus", new List<string> {
        "🛡️ Antivirus software scans and removes threats.",
        "⚡ Keep your antivirus definitions updated."
    }},
    { "cybersecurity", new List<string> {
        "🌍 Cybersecurity protects systems, networks, and data from attacks.",
        "🔐 It’s about people, processes, and technology working together."
    }}
            };
        }

        public string GetResponse(string input)
        {
            foreach (var keyword in _responses.Keys)
            {
                if (input.ToLower().Contains(keyword))
                {
                    var options = _responses[keyword];
                    return options[_rand.Next(options.Count)];
                }
            }
            return "Hmmm 🤔 I’m not sure about that. Try asking about passwords, phishing, firewalls, VPNs, or ransomware.";
        }

        // List all keywords
        public string GetAllKeywords()
        {
            return "📚 You can ask me about: " + string.Join(", ", _responses.Keys);
        }

        public List<string> GetKeywordsList()
        {
            return new List<string>(_responses.Keys);
        }

    }
}
