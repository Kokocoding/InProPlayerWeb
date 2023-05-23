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
            outputDevice.Play();
            return (double)GetInit()["Duration"];
        }

        public void Pause()
        {
            if(outputDevice != null && outputDevice.PlaybackState == PlaybackState.Playing)
            {
                outputDevice.Pause();
            }
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
        public Dictionary<string, object> GetInit()
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>(); 
            if (audioFile != null)
            {
                string[] fileNameArr = audioFile.FileName.Split("\\");
                string fileName = fileNameArr[fileNameArr.Length - 1];
                keyValuePairs.Add("Duration", audioFile.TotalTime.TotalSeconds);
                keyValuePairs.Add("CurrentTime", audioFile.CurrentTime.TotalSeconds);
                keyValuePairs.Add("FileName", fileName);
            }
            else
            {
                keyValuePairs.Add("Duration", TimeSpan.Zero.TotalSeconds);
                keyValuePairs.Add("CurrentTime", TimeSpan.Zero.TotalSeconds);
                keyValuePairs.Add("FileName", "");
            }

            

            if (outputDevice != null)
            {
                if (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    keyValuePairs.Add("isPlay", true);
                }
                else
                {
                    keyValuePairs.Add("isPlay", false);
                }
                keyValuePairs.Add("Volume", outputDevice.Volume);
            }
            else
            {
                keyValuePairs.Add("Volume", 0.5f);
                keyValuePairs.Add("isPlay", false);
            }
            return keyValuePairs;
        }
        public void SetVolume(float volume)
        {
            outputDevice.Volume = volume;
        }
    }
}
