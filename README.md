# CyberSecurityApp GUI (Part 2)

An interactive WPF chatbot application that teaches cybersecurity concepts through keyword recognition, random responses, sentiment detection, and memory. This project extends the Part 1 console app into a fully functional GUI with voice greeting and ASCII art.

---

 Author
**Gcotyelwa Sivuyile Mbuti**  
Student Number: [10468229]  
East London, South Africa  

---

## Features Implemented
- **GUI Design**: Clean WPF layout with header, ASCII art, chat history, input box, and send button.  
- **Voice Greeting**: Plays `greeting(1).wav` on startup.  
- **Keyword Recognition**: Recognises at least 5 cybersecurity topics (password, phishing, privacy, scam, malware).  
- **Random Responses**: Each keyword has multiple responses chosen randomly.  
- **Conversation Flow**: Handles follow‑ups like “tell me more” without resetting the chat.  
- **Memory**: Remembers user’s name and favourite topic, personalises responses.  
- **Sentiment Detection**: Detects worried, curious, frustrated, happy, and responds empathetically.  
- **Code Optimisation**: Logic split across `ChatBot.cs`, `KeywordResponder.cs`, `SentimentDetector.cs`, `MemoryStore.cs`.  
- **GitHub CI**: Automated build workflow with green tick.  

---
** Project Structure
CyberSecurityApp/
├── MainWindow.xaml

├── MainWindow.xaml.cs

├── ChatBot.cs

├── KeywordResponder.cs

├── SentimentDetector.cs

├── MemoryStore.cs

├── AudioPlayer.cs

├── AssemblyInfo.cs

├── greeting(1).wav

├── README.md

└── .github/workflows/build.yml
