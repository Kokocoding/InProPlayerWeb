using NAudio.Wave;

namespace InProPlayerWeb.Helper
{
    public class NAudioHelper 
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        public string audioFilePath = "";

        public void Play()
        {
            Stop(); // 先停止正在播放的音頻（如果有）

            audioFile = new AudioFileReader(audioFilePath);
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();
        }

        public void Stop()
        {
            if (outputDevice != null && outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Stop();
            }

            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }

            if (outputDevice != null)
            {
                outputDevice.Dispose();
                outputDevice = null;
            }
        }

        public TimeSpan GetCurrentTime()
        {
            if (audioFile != null)
            {
                return audioFile.CurrentTime;
            }
            else
            {
                return TimeSpan.Zero;
            }
        }
    }
}
