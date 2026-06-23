using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityChatbot
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex;
        private int _score;

        public QuizManager()
        {
            _questions = new List<QuizQuestion>
        {
            // Easy Q's
            new QuizQuestion("What does 2FA stand for?", "Two-Factor Authentication"),
            new QuizQuestion("Which of these is a strong password?", "A mix of letters, numbers, and symbols"),
            new QuizQuestion("What does VPN stand for?", "Virtual Private Network"),
            new QuizQuestion("Which of these is a common phishing indicator?", "Suspicious links or urgent requests"),
            new QuizQuestion("What is the purpose of a firewall?", "To block unauthorized access to a network"),

            // Medium (with clues)
            new QuizQuestion("What does HTTPS signify compared to HTTP?", "Encrypted communication using SSL/TLS", "🔑 Think about the padlock icon in your browser."),
            new QuizQuestion("Which type of malware demands payment to restore files?", "Ransomware", "💰 It holds your files hostage."),
            new QuizQuestion("What is the main risk of using public Wi-Fi without a VPN?", "Data interception by attackers", "📡 Someone could be 'listening in'."),
            new QuizQuestion("What does social engineering rely on?", "Manipulating human trust rather than technical flaws", "🧑 It tricks people, not machines."),
            new QuizQuestion("Which cybersecurity principle means giving users only the access they need?", "Least privilege", "🔒 Think minimal permissions."),

            // Challenging 
            new QuizQuestion("What is the difference between symmetric and asymmetric encryption?", "Symmetric uses one key, asymmetric uses public/private keys"),
            new QuizQuestion("What does multi-factor authentication (MFA) add beyond passwords?", "Additional verification factors like SMS codes or biometrics"),
            new QuizQuestion("Which attack floods a system with traffic to make it unavailable?", "Denial of Service (DoS)"),
            new QuizQuestion("What is the purpose of data backup in cybersecurity?", "To restore data after loss, corruption, or attack"),
            new QuizQuestion("Which hashing algorithm is commonly used to verify file integrity?", "SHA-256")
        };

            _currentIndex = 0;
            _score = 0;
        }

        public QuizQuestion GetNextQuestion()
        {
            if (_currentIndex < _questions.Count)
                return _questions[_currentIndex++];
            return null;
        }

        public void CheckAnswer(string answer)
        {
            var current = _questions[_currentIndex - 1];
            if (current.CorrectAnswer.Equals(answer, StringComparison.OrdinalIgnoreCase))
                _score++;
        }

        public int GetScore() => _score;
        public int GetTotalQuestions() => _questions.Count;

        public QuizQuestion GetCurrentQuestion()
        {
            if (_currentIndex == 0) return null;
            return _questions[_currentIndex - 1];
        }

        public void RestartQuiz()
        {
            _currentIndex = 0;
            _score = 0;
        }
    }
}