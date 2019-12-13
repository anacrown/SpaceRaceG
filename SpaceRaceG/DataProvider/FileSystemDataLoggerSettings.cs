using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SpaceRaceG.Annotations;

namespace SpaceRaceG.DataProvider
{
    [Serializable]
    public class FileSystemDataLoggerSettings : ISerializable, INotifyPropertyChanged
    {
        public string MainLogDir { get; set; } = FileSystemConfigurator.MainLogDir;
        public string DataFormat { get; set; } = "yyyy.MM.dd hh_mm_ss.FFF";

        public FileSystemDataLoggerSettings() : base()
        {
        }

        protected FileSystemDataLoggerSettings(SerializationInfo info, StreamingContext context)
        {
            MainLogDir = info.GetString("MainLogDir");
            DataFormat = info.GetString("DataFormat");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("MainLogDir", MainLogDir);
            info.AddValue("DataFormat", DataFormat);
        }

        public IDataLogger CreateDataLogger() => new FileSystemDataLogger(this);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}