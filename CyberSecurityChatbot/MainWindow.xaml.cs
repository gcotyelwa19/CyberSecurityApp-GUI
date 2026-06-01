using System.Media; // for greeting sound
using System.Windows;
using System.Windows.Input;

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
        }
        private void PlayVoiceGreeting()
        {
            try
            {
                string path = System.IO.Path.Combine(
                    System.AppDomain.CurrentDomain.BaseDirectory,
                    "greeting(1).wav"
                );
                var player = new SoundPlayer(path);
                player.Play();
            }
            catch
            {
                AppendBotMessage("[Voice greeting missing or failed to play]");
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

    }
}
