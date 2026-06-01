using System;
using System.Media;

namespace CyberSecurityChatbot
{
    public static class AudioPlayer
    {
        public static void Play(string filePath)
        {
            try
            {
                SoundPlayer player = new SoundPlayer(filePath);
                player.Play();
            }
            catch (Exception ex)
            {
                // Optional: log or show error
                Console.WriteLine($"Audio playback failed: {ex.Message}");
            }
        }

        public static void PlaySync(string filePath)
        {
            try
            {
                SoundPlayer player = new SoundPlayer(filePath);
                player.PlaySync(); // blocks until finished
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audio playback failed: {ex.Message}");
            }
        }
    }
}
