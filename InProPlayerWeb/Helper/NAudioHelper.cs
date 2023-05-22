using NAudio.Wave;

namespace InProPlayerWeb.Helper
{
    public class NAudioHelper 
    {
        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        public string audioFilePath = "";
        public TimeSpan startTime = TimeSpan.Zero;
        public float Volume { get; set; } = 0.5f; // 默认音量

        public double Play()
        {
            Stop(); // 先停止正在播放的音頻（如果有）

            audioFile = new AudioFileReader(audioFilePath);
            audioFile.CurrentTime = startTime; // 设置播放的起始时间
            outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Volume = Volume;
            outputDevice.Play();
            return GetTotalDuration();
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

        public double GetCurrentTime()
        {
            if (audioFile != null)
            {
                return audioFile.CurrentTime.TotalSeconds;
            }
            else
            {
                return TimeSpan.Zero.TotalSeconds;
            }
        }
        public double GetTotalDuration()
        {
            if(audioFile != null)
            {
                return audioFile.TotalTime.TotalSeconds;
            }
            else
            {
                return TimeSpan.Zero.TotalSeconds;
            }
        }
        public void SetVolume(float volume)
        {
            outputDevice.Volume = volume;
        }
    }
}
