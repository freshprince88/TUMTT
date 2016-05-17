using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TT.Lib.Managers;
using MediaToolkit;
using NReco.VideoConverter;
using TT.Lib.Util;
using TT.Models;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace TT.Lib.Results
{
    public class ExportPlaylistSaveResult : IResult
    {

        private IMatchManager Manager;
        private IDialogCoordinator Dialogs;
        public string Location { get; set; }
        public bool singleRallies { get; set; }
        public bool rallyCollection { get; set; }
        

        public ExportPlaylistSaveResult(IMatchManager manager, IDialogCoordinator dialogs, string location, bool sr, bool rc)
        {
            Manager = manager;
            Location = location;
            Dialogs = dialogs;
            singleRallies = sr;
            rallyCollection = rc;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed = delegate { };

        public async void Execute(CoroutineExecutionContext context)
        {
            var shell = (IoC.Get<IShell>() as Screen);
            var dialog = await Dialogs.ShowProgressAsync(shell, "Bitte warten...", "Ballwechsel werden exportiert", false);
            if (singleRallies != false || rallyCollection != false) { 
            await Task.Factory.StartNew(() => ExportVideo(dialog));
            }
            await dialog.CloseAsync();
        }
        

        private void ExportVideo(ProgressDialogController progress)
        {   
            var inputFile = new MediaToolkit.Model.MediaFile { Filename = @Manager.Match.VideoFile };
            string videoName = Manager.Match.VideoFile.Split('\\').Last();
            videoName = videoName.Split('.').First();
            Directory.CreateDirectory(@Location);
            Directory.CreateDirectory(@Location + @"\" + Manager.ActivePlaylist.Name);
            int rallyCount = Manager.ActivePlaylist.Rallies.Count();
            string[] RallyCollection = new string[rallyCount];
            progress.Minimum = 0;
            if (rallyCollection) { 
            progress.Maximum = rallyCount + 1;
            }
            else
            {
                progress.Maximum = rallyCount;
            }

            progress.SetProgress(0);

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                var options = new MediaToolkit.Options.ConversionOptions();
                options.VideoBitRate = inputFile.Metadata.VideoData.BitRateKbs;
                options.VideoSize = MediaToolkit.Options.VideoSize.Hd1080;

                for (int i = 0; i < rallyCount; i++)
                {
                    Rally curRally = Manager.ActivePlaylist.Rallies[i];
                    TimeSpan startRally = TimeSpan.FromMilliseconds(curRally.Anfang);
                    TimeSpan endRally = TimeSpan.FromMilliseconds(curRally.Ende);
                    TimeSpan duration = TimeSpan.FromMilliseconds(curRally.Ende - curRally.Anfang);
                    string RallyScore = curRally.CurrentRallyScore.ToString();
                    RallyScore = RallyScore.Replace(":", "-");
                    string SetScore = curRally.CurrentSetScore.ToString();
                    SetScore = SetScore.Replace(":", "-");


                    string fileName = Location + @"\" + Manager.ActivePlaylist.Name + @"\" + RallyScore + " (" + SetScore + ").mp4";
                    var outputFile = new MediaToolkit.Model.MediaFile { Filename = fileName };
                    RallyCollection[i] = fileName;


                    options.CutMedia(startRally, duration);
                    engine.Convert(inputFile, outputFile, options);

                    progress.SetProgress(i+1);
                }

            }

            if (rallyCollection) { 
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            NReco.VideoConverter.ConcatSettings settings = new NReco.VideoConverter.ConcatSettings();
            ffMpeg.ConcatMedia(RallyCollection, @Location + @"\" + Manager.ActivePlaylist.Name + @"\" + Manager.ActivePlaylist.Name + "_collection.mp4", NReco.VideoConverter.Format.mp4, settings);
            progress.SetProgress(rallyCount + 1);
            }

            if (!singleRallies) { 
            for (int i = 0; i < rallyCount; i++)
            {
                File.Delete(RallyCollection[i]);
            }
            }
            
        }
    }
}
