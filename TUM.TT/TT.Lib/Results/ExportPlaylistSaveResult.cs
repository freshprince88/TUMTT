﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Managers;

using NReco.VideoConverter;
using TT.Lib.Util;
using TT.Models;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Diagnostics;

namespace TT.Lib.Results
{
    public class ExportPlaylistSaveResult : IResult
    {

        private IMatchManager Manager;
        private IDialogCoordinator Dialogs;
        public string Location { get; set; }
        public bool singleRallies { get; set; }
        public bool rallyCollection { get; set; }
        public IEventAggregator Events { get; private set; }
        public double ConvertProgress { get; set; }
        public double ConvertDuration { get; set; }
        public ProgressDialogController dialog { get; set; }
        double currentProgress { get; set; }
        double progressBar { get; set; }


        public ExportPlaylistSaveResult(IMatchManager manager, IDialogCoordinator dialogs, string location, bool sr, bool rc)
        {
            Manager = manager;
            Location = location;
            Dialogs = dialogs;
            singleRallies = sr;
            rallyCollection = rc;
            Events = IoC.Get<IEventAggregator>();
            Events.Subscribe(this);
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public async void Execute(CoroutineExecutionContext context)
        {
            var shell = (IoC.Get<IShell>() as Screen);
            dialog = await Dialogs.ShowProgressAsync(shell, "Please wait...", "Export is in Progress...", false);
            if (singleRallies != false || rallyCollection != false)
            {
                   await Task.Factory.StartNew(() => ExportVideo(dialog));
            }
            await dialog.CloseAsync();
        }


        public void ExportVideo(ProgressDialogController progress)
        {
            

            string inputFile = @Manager.Match.VideoFile;
            string videoName = Manager.Match.VideoFile.Split('\\').Last();
            videoName = videoName.Split('.').First();
            Directory.CreateDirectory(@Location);
            string collectionName = @Location + @"\" + Manager.ActivePlaylist.Name + "_collection.mp4";
            int rallyCount = Manager.ActivePlaylist.Rallies.Count();
            int sum = 0;
            for (int s = 1; s <= rallyCount; s++)
            {
                sum = sum + s;
            }
            if (rallyCollection)
            {
                progressBar = sum * 2;
            }
            else
            {
                progressBar = sum;
            }
            
            currentProgress = 0;

            string[] RallyCollection = new string[rallyCount];
            string[] ConcatRally = new string[2];
            progress.Minimum = 0;
            progress.Maximum = progressBar;
            progress.SetProgress(0);

            for (int i = 0; i < rallyCount; i++)
            {   


                progress.SetMessage("Export Playlist '" + Manager.ActivePlaylist.Name + "': \n\nRally " + (i + 1) + " is being created...");
                Rally curRally = Manager.ActivePlaylist.Rallies[i];
                string RallyNumber = curRally.Number.ToString();
                string RallyScore = curRally.CurrentRallyScore.ToString();
                RallyScore = RallyScore.Replace(":", "-");
                string SetScore = curRally.CurrentSetScore.ToString();
                SetScore = SetScore.Replace(":", "-");
                string fileName = @Location + @"\#" + RallyNumber + "_" + RallyScore + " (" + SetScore + ").mp4";
                RallyCollection[i] = fileName;

                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

                NReco.VideoConverter.ConvertSettings settings = new NReco.VideoConverter.ConvertSettings()
                {
                    Seek = Convert.ToSingle(curRally.Start / 1000),
                    MaxDuration = Convert.ToSingle((curRally.End - curRally.Start) / 1000),
                    //VideoFrameSize = NReco.VideoConverter.FrameSize.hd720,
                    AudioCodec = "copy", VideoCodec="copy"
        

                };
                ffMpeg.ConvertMedia(@Manager.Match.VideoFile, null, fileName, null, settings);


                currentProgress = currentProgress + (i + 1);
                progress.SetProgress(currentProgress);
            }

            if (rallyCollection)
            {
                progress.SetMessage("\n Collection is currently being created! \n\nIt may take a while...");
                var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
                ffMpeg.ConvertProgress += UpdateProgress;                

                NReco.VideoConverter.ConcatSettings settings = new NReco.VideoConverter.ConcatSettings();
                ffMpeg.ConcatMedia(RallyCollection, @Location + @"\" + Manager.ActivePlaylist.Name + "_collection("+rallyCount+").mp4", NReco.VideoConverter.Format.mp4, settings);
                progress.SetProgress(progressBar);
            }

            if (!singleRallies)
            {
                for (int i = 0; i < rallyCount; i++)
                {
                    File.Delete(RallyCollection[i]);
                }
            }
        }

        private void UpdateProgress (object sender, ConvertProgressEventArgs e)
        {   double rT = e.TotalDuration.TotalSeconds-e.Processed.TotalSeconds;
            if (rT < 0) rT = 0;
            TimeSpan remainingTime = TimeSpan.FromSeconds(rT);
            ConvertProgress = (double) e.Processed.TotalMilliseconds;
            ConvertDuration = (double) e.TotalDuration.TotalMilliseconds;
            currentProgress = (progressBar / 2) + (progressBar / 2) * (ConvertProgress / ConvertDuration);
            if (currentProgress > progressBar)
            {
                currentProgress = progressBar;
            }
            dialog.SetMessage("\nCollection is currently being created! \n\nIt may take a while...(" + remainingTime.ToString(@"mm\:ss") + ")");
            dialog.SetProgress(currentProgress);
            
        }
        private static string Execute(string exePath, string parameters)
        {
            string result = String.Empty;

            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.FileName = exePath;
                p.StartInfo.Arguments = parameters;
                p.Start();
                p.WaitForExit();

                result = p.StandardOutput.ReadToEnd();
            }

            return result;
        }
    }
}
