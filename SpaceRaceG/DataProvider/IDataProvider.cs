using System;
using System.ComponentModel;

namespace SpaceRaceG.DataProvider
{
    public interface IDataProvider : INotifyPropertyChanged
    { 
        string Name { get; }
        int FrameCount { get; }
        int FrameNumber { get; }
        DateTime StartTime { get; }

        void Start();
        void Stop();

        void RecordPlay();
        void RecordStop();
        void MoveToFrame(int frameNumber);

        event EventHandler Started;
        event EventHandler Stopped;

        void SendResponse(string response);
        event EventHandler<int> TimeChanged;
        event EventHandler<DataFrame> DataReceived;
    }
}