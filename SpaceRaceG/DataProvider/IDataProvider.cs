using System;
using System.ComponentModel;

namespace SpaceRaceG.DataProvider
{
    public interface IDataProvider : INotifyPropertyChanged
    { 
        string Name { get; }
        DateTime StartTime { get; }

        void Start();
        void Stop();

        event EventHandler Started;
        event EventHandler Stopped;

        void SendResponse(string response);
        event EventHandler<DataFrame> DataReceived;
    }
}