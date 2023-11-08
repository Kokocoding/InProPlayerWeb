using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace InProPlayerWeb.Helper
{
    public class NAudioHelper 
    {
        private WaveOutEvent? outputDevice;
        private AudioFileReader? audioFile;

        public string audioFilePath = "";
        public TimeSpan startTime = TimeSpan.Zero;
        public float Volume { get; set; } = 0.5f; // 預設音量

        public double Play()
        {
            audioFile = new AudioFileReader(audioFilePath);
            audioFile.CurrentTime = startTime; // 播放的開始時間
            outputDevice = new WaveOutEvent();
            outputDevice.Volume = Volume;
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
                    //if (audioFile != null) keyValuePairs["CurrentTime"] = audioFile.TotalTime.TotalSeconds;
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
            Volume = volume;
            if(outputDevice != null) outputDevice.Volume = Volume;
        }

        public void ConvertTextToSpeech(string text)
        {
            using (var synthesizer = new SpeechSynthesizer())
            {
                // 设置语音合成的参数
                synthesizer.Volume = 100; // 设置音量（0-100）
                synthesizer.Rate = 0; // 设置语速（-10到10）

                // 将文字转换为音频流
                synthesizer.Speak(text);
            }
        }

        public void ConvertSpeechToText()
        {
            WaveInEvent waveInEvent = new WaveInEvent();
            waveInEvent.DeviceNumber = 0; // 音频输入设备索引
            waveInEvent.WaveFormat = new WaveFormat(16000, 16, 1); // 采样率、位深度、声道数

            SpeechRecognitionEngine recognitionEngine = new SpeechRecognitionEngine();
            recognitionEngine.SpeechRecognized += RecognizedSpeechHandler;

            waveInEvent.StartRecording();
            recognitionEngine.SetInputToDefaultAudioDevice(); // 设置语音输入源为默认音频设备
            recognitionEngine.RecognizeAsync(RecognizeMode.Multiple); // 开始语音识别



            waveInEvent.StopRecording();
            recognitionEngine.RecognizeAsyncStop();
        }

        private void RecognizedSpeechHandler(object sender, SpeechRecognizedEventArgs e)
        {
            string recognizedText = e.Result.Text;
            // 处理识别到的文字结果
        }
    }
}
