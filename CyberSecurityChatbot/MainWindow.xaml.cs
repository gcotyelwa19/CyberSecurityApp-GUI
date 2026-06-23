using System.Media; // for greeting sound
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Linq;

namespace CyberSecurityChatbot
{
    public partial class MainWindow : Window
    {
        private ChatBot _chatBot;

        public MainWindow()
        {
            InitializeComponent();
            _chatBot = new ChatBot();
            QuestionsList.ItemsSource = _chatBot.GetAllKeywordsList(); 
            TestTaskStorage(); // temporary test


            // Wire events programmatically to avoid generated partial class issues
            if (SendButton != null)
                SendButton.Click += SendButton_Click;

            if (UserInput != null)
                UserInput.KeyDown += UserInput_KeyDown;

            PlayVoiceGreeting();
            LoadAsciiArt();

            AppendBotMessage(_chatBot.GetGreeting());

            BackButton.Click += BackButton_Click;
            ExitButton.Click += ExitButton_Click;

            // Populate questions list with available keywords
            QuestionsList.ItemsSource = _chatBot.GetAllKeywordsList();
            // Load saved tasks into the TaskListView (if present)
            RefreshTasksDisplay();
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;

                // Preferred filenames
                string exact = Path.Combine(baseDir, "greeting(1).wav");
                string alt = Path.Combine(baseDir, "greeting.wav");

                // Find a matching file
                string found = null;
                if (File.Exists(exact))
                    found = exact;
                else if (File.Exists(alt))
                    found = alt;
                else
                {
                    var files = Directory.GetFiles(baseDir, "greeting*.wav");
                    if (files != null && files.Length > 0)
                        found = files.First();
                }

                if (string.IsNullOrEmpty(found))
                {
                    AppendBotMessage("[Voice greeting file not found in output folder]");
                    return;
                }

                var player = new SoundPlayer(found);
                player.Play();
            }
            catch (Exception ex)
            {
                AppendBotMessage($"[Voice greeting failed: {ex.Message}]");
            }
        }

        private void TestTaskStorage()
        {
            TaskStorageHelper helper = new TaskStorageHelper();

            // Add two tasks
            helper.AddTask("Enable two-factor authentication",
                           "Set up 2FA on all important accounts",
                           "Remind me in 5 days");

            helper.AddTask("Review privacy settings",
                           "Review account privacy settings",
                           "");

            // Mark the first task as complete
            helper.MarkAsComplete(1);

            // Delete the second task
            helper.DeleteTask(2);

            // Load tasks back and print them
            var tasks = helper.LoadTasks();
            foreach (var task in tasks)
            {
                Console.WriteLine($"{task.Id}: {task.Title} - Complete? {task.IsComplete}");
            }
        }



        private void LoadAsciiArt()
        {
            string art = @"
   oooo       .o.       ooooooooo.   oooooo     oooo ooooo  .oooooo..o 
   `888      .888.      `888   `Y88.  `888.     .8'  `888' d8P'    `Y8 
    888     .8""888.      888   .d88'   `888.   .8'    888  Y88bo.       
    888    .8' `888.     888ooo88P'     `888. .8'     888   `""Y8888o.   
    888   .88ooo8888.    888`88b.        `888.8'      888       `""Y88b 
    888  .8'     `888.   888  `88b.       `888'       888  oo     .d8P 
.o. 88P o88o     o8888o o888o  o888o       `8'       o888o 8""""88888P' 
`Y888P                                                                 
";
            // Display the ASCII art in the dedicated ASCII art control if available
            if (AsciiArtDisplay != null)
            {
                AsciiArtDisplay.Text = art;
            }
            else if (ChatDisplay != null)
            {
                ChatDisplay.Text += art + "\n";
            }
        }


        private void SendButton_Click(object sender, RoutedEventArgs e) => SendMessage();

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SendMessage();
        }

        private void SendMessage()
        {
            string userMessage = UserInput?.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(userMessage)) return;

            AppendUserMessage(userMessage);
            string botReply = _chatBot.ProcessInput(userMessage);
            AppendBotMessage(botReply);
            UserInput.Clear();
        }

        private void AppendUserMessage(string message)
        {
            ChatDisplay.Text += $"👤 You: {message}\n";
            ScrollToBottom();
        }

        private void AppendBotMessage(string message)
        {
            if (_chatBot != null)
            {
                var userName = _chatBot.GetUserName();
                if (!string.IsNullOrEmpty(userName))
                {
                    ChatDisplay.Text += $"🤖 {userName}'s Assistant: {message}\n";
                }
                else
                {
                    ChatDisplay.Text += $"🤖 Bot: {message}\n";
                }
            }
            else
            {
                ChatDisplay.Text += $"🤖 Bot: {message}\n";
            }
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer?.ScrollToEnd();
        }

        // Handler for adding a new task from the UI. If you have UI inputs for
        // title/description/due date replace the placeholder values below.
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Placeholder values; replace with actual input control values when available
                string title = "New Task";
                string description = "Created from UI";
                string due = string.Empty;

                var helper = new TaskStorageHelper();
                helper.AddTask(title, description, due);

                AppendBotMessage("✅ Task added.");
            }
            catch (System.Exception ex)
            {
                AppendBotMessage($"❌ Failed to add task: {ex.Message}");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AppendBotMessage("↩️ Going back to the previous step...");
            // Defines what "Back" means for JARVIS (e.g., clear input, reload greeting, etc.)
            UserInput.Clear();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit?",
                                         "Confirm Exit",
                                         MessageBoxButton.YesNo,
                                         MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                AppendBotMessage("👋 Bye for now! Stay safe online.");
                Application.Current.Shutdown();
            }
            else
            {
                AppendBotMessage("👍 Okay, let’s keep chatting!");
            }
        }

        // Refresh the tasks shown in the UI from storage
        private void RefreshTasksDisplay()
        {
            try
            {
                var helper = new TaskStorageHelper();
                var tasks = helper.LoadTasks();
                if (TaskListView != null)
                {
                    TaskListView.ItemsSource = null;
                    TaskListView.ItemsSource = tasks;
                }
            }
            catch (System.Exception ex)
            {
                AppendBotMessage($"❌ Failed to load tasks: {ex.Message}");
            }
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskListView?.SelectedItem is CyberTask selected)
                {
                    var helper = new TaskStorageHelper();
                    helper.MarkAsComplete(selected.Id);
                    AppendBotMessage($"✅ Marked task #{selected.Id} as complete.");
                    RefreshTasksDisplay();
                }
                else
                {
                    AppendBotMessage("⚠️ Please select a task to complete.");
                }
            }
            catch (System.Exception ex)
            {
                AppendBotMessage($"❌ Failed to complete task: {ex.Message}");
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TaskListView?.SelectedItem is CyberTask selected)
                {
                    var helper = new TaskStorageHelper();
                    helper.DeleteTask(selected.Id);
                    AppendBotMessage($"🗑️ Deleted task #{selected.Id}.");
                    RefreshTasksDisplay();
                }
                else
                {
                    AppendBotMessage("⚠️ Please select a task to delete.");
                }
            }
            catch (System.Exception ex)
            {
                AppendBotMessage($"❌ Failed to delete task: {ex.Message}");
            }
        }

    private QuizManager quizManager = new QuizManager();
    private QuizQuestion currentQuestion;

        private void StartQuiz()
        {
            currentQuestion = quizManager.GetNextQuestion();

            if (currentQuestion != null)
            {
                QuizQuestionBlock.Text = currentQuestion.Question;
                FeedbackBlock.Text = "";
                NextQuestionButton.Visibility = Visibility.Collapsed;

                // Reset options
                OptionA.Visibility = Visibility.Visible;
                OptionB.Visibility = Visibility.Visible;
                OptionC.Visibility = Visibility.Visible;
                OptionD.Visibility = Visibility.Visible;

                OptionA.IsChecked = false;
                OptionB.IsChecked = false;
                OptionC.IsChecked = false;
                OptionD.IsChecked = false;

                QuizScoreBlock.Text = $"Score: {quizManager.GetScore()} / {quizManager.GetTotalQuestions()}";
            }
            else
            {
                FinalResultsBlock.Visibility = Visibility.Visible;
                FinalResultsBlock.Text = $"🎉 Quiz finished!\n" +
                                         $"Final Score: {quizManager.GetScore()} / {quizManager.GetTotalQuestions()}\n" +
                                         $"Completed on: {DateTime.Now:dddd, dd MMMM yyyy HH:mm}";

                QuizQuestionBlock.Text = "";
                OptionA.Visibility = Visibility.Collapsed;
                OptionB.Visibility = Visibility.Collapsed;
                OptionC.Visibility = Visibility.Collapsed;
                OptionD.Visibility = Visibility.Collapsed;
                SubmitAnswerButton.Visibility = Visibility.Collapsed;
                NextQuestionButton.Visibility = Visibility.Collapsed;
                HintButton.Visibility = Visibility.Collapsed;
            }
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            string selectedAnswer = null;

            if (OptionA.IsChecked == true) selectedAnswer = OptionA.Content.ToString();
            else if (OptionB.IsChecked == true) selectedAnswer = OptionB.Content.ToString();
            else if (OptionC.IsChecked == true) selectedAnswer = OptionC.Content.ToString();
            else if (OptionD.IsChecked == true) selectedAnswer = OptionD.Content.ToString();

            if (selectedAnswer == null)
            {
                FeedbackBlock.Text = "⚠️ Please select an answer.";
                return;
            }

            quizManager.CheckAnswer(selectedAnswer);

            if (selectedAnswer.Equals(currentQuestion.CorrectAnswer, StringComparison.OrdinalIgnoreCase))
                FeedbackBlock.Text = $"✅ Correct! {currentQuestion.CorrectAnswer}";
            else
                FeedbackBlock.Text = $"❌ Incorrect. The correct answer is: {currentQuestion.CorrectAnswer}";

            NextQuestionButton.Visibility = Visibility.Visible;
            QuizScoreBlock.Text = $"Score: {quizManager.GetScore()} / {quizManager.GetTotalQuestions()}";
        }

        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            StartQuiz();
        }

        private void HintButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentQuestion != null && !string.IsNullOrEmpty(currentQuestion.Clue))
                FeedbackBlock.Text = $"💡 Hint: {currentQuestion.Clue}";
            else
                FeedbackBlock.Text = "No hint available for this question.";
        }

        private void RestartQuiz_Click(object sender, RoutedEventArgs e)
        {
            quizManager.RestartQuiz();
            FinalResultsBlock.Visibility = Visibility.Collapsed;
            SubmitAnswerButton.Visibility = Visibility.Visible;
        }

    }

}
