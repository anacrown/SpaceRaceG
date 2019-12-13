using System;
using System.Runtime.Serialization;

namespace SpaceRaceG.DataProvider
{
    public interface IDataLogger : ISerializable
    {
        void Log(string name, DateTime startTime, DataFrame frame, Exception e);

        void Log(string name, DateTime startTime, DataFrame frame);

        void Log(string name, DateTime startTime, DateTime time, int frameNumber, string response);

        void LogDead(string name, DateTime startTime, DataFrame frame);
    }
}