using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Timers;
using SpaceRaceG.Annotations;
using Timer = System.Timers.Timer;

namespace SpaceRaceG.DataProvider
{
    [Serializable]
    public class FileSystemDataProvider : IDataProvider, ISerializable
    {
        public FileSystemDataProviderSettings Settings { get; }
        public string BoardFile { get; }
        public string Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartTime { get; private set; }

        public uint FrameNumber { get; private set; }
        public int FrameCount => _boards?.Count ?? 0;
        public int FrameMaximumKey => _boards?.Count - 1 ?? 0;

        private Dictionary<uint, DataFrame> _boards;
        private Dictionary<uint, string> _responses;
        private static readonly Regex Pattern = new Regex(@"^\[([\.\d\s]*)\]\s\((\d*)\):\s(.*)$");
        private readonly Timer _timer = new Timer(800);
        private string _name;

        public FileSystemDataProvider(FileSystemDataProviderSettings settings)
        {
            Settings = settings;

            BoardFile = FindBoardFile(Settings.Path);

            _timer.AutoReset = true;
            _timer.Elapsed += TimerOnElapsed;

            if (!string.IsNullOrEmpty(BoardFile))
                Name = GetNameFromDir(BoardFile);
        }

        private string FindBoardFile(string path)
        {
            const string boardFileName = "Board.txt";

            try
            {
                if (Path.GetFileName(path) == boardFileName && File.Exists(path)) return path;

                var boardFile = Path.Combine(path, boardFileName);
                if (File.Exists(boardFile)) return boardFile;

                var lastStartTime = Directory.GetDirectories(path)
                    .Select(dir => DateTime.ParseExact(dir, Settings.DataFormat, CultureInfo.InvariantCulture))
                    .Max();

                boardFile = Path.Combine(path, lastStartTime.ToString(Settings.DataFormat), boardFileName);
                if (File.Exists(boardFile)) return boardFile;
            }
            catch (Exception e)
            {
                throw new Exception($"Can't find {boardFileName}", e);
            }

            throw new Exception($"Can't find {boardFileName}");
        }

        public FileSystemDataProvider(SerializationInfo info, StreamingContext context) : 
            this(info.GetValue("Settings", typeof(FileSystemDataProviderSettings)) as FileSystemDataProviderSettings)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Settings", Settings);
        }

        private string GetNameFromDir(string file)
        {
            var dir = Path.GetDirectoryName(file);
            var startDir = Path.GetFileName(dir);

            var subDir = Path.GetDirectoryName(dir);
            var instanceDir = Path.GetFileName(subDir);

            return $"{instanceDir} {startDir}";
        }

        private DataFrame ProcessMessage(string message)
        {
            var match = Pattern.Match(message);
            if (!match.Success)
            {
                throw new ApplicationException($"Cannot match message: '{message}'");
            }

            var time = DateTime.ParseExact(match.Groups[1].Value, Settings.DataFormat, CultureInfo.InvariantCulture);

            return new DataFrame(time, match.Groups[3].Value, uint.TryParse(match.Groups[2].Value, out uint frameNumber) ? frameNumber : 0);
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            if (FrameNumber < FrameCount)
            {
                OnDataReceived(_boards[FrameNumber]);
                OnTimeChanged(FrameNumber);
                FrameNumber++;
            }
            else _timer.Stop();
        }

        public void Start()
        {
            if (!File.Exists(BoardFile))
                return; //throw new Exception();

            StartTime = DateTime.Now;
            _boards = File.ReadAllLines(BoardFile).Select(ProcessMessage).ToDictionary(frame => frame.FrameNumber, frame => frame);

            var responseFilePath = Path.Combine(Path.GetDirectoryName(BoardFile), "Response.txt");
            if (File.Exists(responseFilePath))
                _responses = File.ReadAllLines(responseFilePath).Select(ProcessMessage).ToDictionary(frame => frame.FrameNumber, frame => frame.Board);

            MoveToFrame(0);

            OnPropertyChanged(nameof(FrameCount));
            OnPropertyChanged(nameof(FrameMaximumKey));

            OnStarted();
        }

        public void Stop()
        {
            _timer.Stop();

            OnStopped();
        }

        public void RecordPlay() => _timer?.Start();

        public void RecordStop() => _timer?.Stop();

        public void MoveToFrame(uint frameNumber)
        {
            FrameNumber = frameNumber;
            OnTimeChanged(FrameNumber);
            OnDataReceived(_boards[FrameNumber]);

            if (_responses != null)
            {
                var response = _responses.ContainsKey(FrameNumber) ? _responses[FrameNumber] : "NOT RESPONSE";
                //                OnLogDataReceived(new LogRecord(_boards[FrameNumber], $"Response {response}"));
            }
        }

        public void SendResponse(string response)
        {
            //throw new NotImplementedException();
        }

        public event EventHandler<uint> TimeChanged;

        public event EventHandler<DataFrame> DataReceived;
        protected virtual void OnDataReceived(DataFrame frame) => DataReceived?.Invoke(this, frame);

        public event EventHandler Started;
        public event EventHandler Stopped;

        protected virtual void OnStarted() => Started?.Invoke(this, EventArgs.Empty);


        //Никогда не останавливается? ...
        protected virtual void OnStopped() => Stopped?.Invoke(this, EventArgs.Empty);

        protected virtual void OnTimeChanged(uint e) => TimeChanged?.Invoke(this, e);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}