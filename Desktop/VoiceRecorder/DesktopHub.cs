using System;
using Microsoft.AspNetCore.SignalR.Client;
using NAudio.Wave;

namespace VoiceRecorder
{
    public class DesktopHub
    {
        private readonly string _connection;
        public string IP;
        public HubConnection connection;
        public string RecordingStatus { get; set; }
        public readonly DateTime date =  DateTime.Now;  

        public WaveInEvent waveSource = null;
        public WaveFileWriter waveFile = null;

        public DesktopHub(string connection)
        {
            _connection = connection;
        }

        public async void Connect()
        {

            connection =
                new HubConnectionBuilder().WithUrl(_connection).Build();
            await connection.StartAsync();

            await connection.InvokeAsync("StartApplication", IP);

            connection
                .On("startRecord",
                (string ip) =>
                {
                    Console.WriteLine(IP);
                    if (ip.ToString().Trim().ToLower() == IP.ToString().Trim().ToLower())
                    {

                        waveSource = new WaveInEvent();
                        waveSource.WaveFormat = new WaveFormat(44100, 1);

                        waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                        waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

                        waveFile = new WaveFileWriter(@"C:\Users\User\Desktop\recordings\" + date.ToString("yyyy-MM-dd-HH-mm-ss")+"-"+IP + ".mp3", waveSource.WaveFormat);

                        waveSource.StartRecording();

                    }
                });

            connection
                .On("stopRecord",
                (string ip) =>
                {
                    if (ip.ToString().Trim().ToLower() == IP.ToString().Trim().ToLower())
                    {
                        waveSource.StopRecording();
                    }
                });

        }

        public async void Disconnect () 
        {
            await connection.InvokeAsync("StopApplication", IP);

            await connection.StopAsync();
        }

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }
    }

}
