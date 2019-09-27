﻿using Android.Util;
using Xamarin.Forms;
using System;
using System.IO;
using System.Threading.Tasks;
using Rollbar;
using SetBoxTV.VideoPlayer.Helpers;
using ILogger = SetBoxTV.VideoPlayer.Interface.ILogger;
using Java.Lang;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;

[assembly: Dependency(typeof(SetBoxTV.VideoPlayer.Droid.Controls.LoggerService))]
namespace SetBoxTV.VideoPlayer.Droid.Controls
{
    public class LoggerService : ILogger
    {

        private static readonly object lockSync = new object();

        private static LoggerService _instance;
        public static LoggerService Instance => _instance ?? (_instance = new LoggerService());

        public string DeviceIdentifier { get; set; } = "ABCD";
        public string Platform { get; set; } = "Android";
        public string Version { get; set; } = "1.0";

         public void Debug(string text)
        {
            Log.Debug("SetBoxTV", $"{text}");
            RollbarLocator.RollbarInstance.Debug(text);
            SaveFile("DEBUG ", text, null);
        }

        public void Error(string text, System.Exception ex)
        {
            Log.Error("SetBoxTV", Throwable.FromException(ex), $"{text} - {ex.Message}");
            Crashes.TrackError(ex);
            RollbarLocator.RollbarInstance.Error(ex);
            SaveFile("ERRO  ", text, ex);
        }

        public void Error(System.Exception ex)
        {
            Log.Error("SetBoxTV", Throwable.FromException(ex), $"{ex.Message}");
            Crashes.TrackError(ex);
            RollbarLocator.RollbarInstance.Error(ex);
            SaveFile("ERRO  ", null, ex);
        }

        public void Info(string text)
        {
            Log.Info("SetBoxTV", $"{text}");
            Analytics.TrackEvent(text);
            RollbarLocator.RollbarInstance.Info(text);
            SaveFile("INFO  ", text, null);
        }


        private void SaveFile(string tipo,  string text, System.Exception ex)
        {
            Task.Run(() =>
            {
                try
                {
                    lock (lockSync)
                    {
                        string directory = Path.Combine(PlayerSettings.PathFiles, "LOGS");

                        if (!Directory.Exists(directory))
                            Directory.CreateDirectory(directory);

                        string fileName = Path.Combine(directory, $"LOG-{DateTime.Now:yyyy-MM-dd}.txt");


                        using (var streamWriter = !File.Exists(fileName)
                            ? File.CreateText(fileName)
                            : new StreamWriter(fileName, true))
                        {
                            if (!string.IsNullOrEmpty(text))
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | {tipo} | {text}");

                            if (ex != null)
                            {
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | ERROR  | {ex.Message}");
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | STACK  | {ex.StackTrace}");
                                streamWriter.WriteLine($"{DateTime.Now:HH:mm} | SOURCE | {ex.Source}");

                                if (ex.InnerException != null)
                                {
                                    streamWriter.WriteLine($"{DateTime.Now:HH:mm} | ERROR  | {ex.InnerException.Message}");
                                    streamWriter.WriteLine($"{DateTime.Now:HH:mm} | STACK  | {ex.InnerException.StackTrace}");
                                    streamWriter.WriteLine($"{DateTime.Now:HH:mm} | SOURCE | {ex.InnerException.Source}");
                                }
                            }
                        }
                    }
                }
                catch (System.Exception)
                {
                    //Ignore
                }
            });
        }
    }
}