using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SpaceRaceG.Annotations;

namespace SpaceRaceG.DataProvider
{
    [Serializable]
    public class FileSystemDataProviderSettings : ISerializable, INotifyPropertyChanged
    {
        public string Path { get; }
        public string DataFormat { get; set; } = "yyyy.MM.dd hh_mm_ss.FFF";

        public FileSystemDataProviderSettings(string path)
        {
            Path = path;
        }

        protected FileSystemDataProviderSettings(SerializationInfo info, StreamingContext context)
        {
            Path = info.GetString("Path");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Path", Path);
        }

        public IDataProvider CreateDataProvider() => new FileSystemDataProvider(this);

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}