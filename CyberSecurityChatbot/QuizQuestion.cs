using System;
using System.Collections.Generic;
using System.Text;

namespace CyberSecurityChatbot
{
    public class  QuizQuestion
    {
        public string Question {  get; set; }
        public string CorrectAnswer { get; set; }
        public string Clue { get; set; }

        public QuizQuestion(string question, string correctAnswer, string clue = "")
        {
            Question = question;
            CorrectAnswer = correctAnswer;
            Clue = clue;
        }
    }
}
