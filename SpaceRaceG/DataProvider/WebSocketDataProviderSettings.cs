using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SpaceRaceG.Annotations;

namespace SpaceRaceG.DataProvider
{
    [Serializable]
    public class WebSocketDataProviderSettings : ISerializable, INotifyPropertyChanged
    {

        public IdentityUser IdentityUser { get; set; } = new IdentityUser();

        public IDataProvider CreateDataProvider() => new WebSocketDataProvider(this);

        public WebSocketDataProviderSettings() { }

        public WebSocketDataProviderSettings(IdentityUser identityUser) : this()
        {
            IdentityUser = identityUser;
        }

        public WebSocketDataProviderSettings(SerializationInfo info, StreamingContext context)
        {
            IdentityUser = info.GetValue("IdentityUser", typeof(IdentityUser)) as IdentityUser ?? new IdentityUser();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("IdentityUser", IdentityUser);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}