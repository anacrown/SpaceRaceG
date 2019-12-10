using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using SpaceRaceG.Annotations;

namespace SpaceRaceG
{
    public enum DataSource { WEB, FILE }

    [Serializable]
    public class SpaceRaceBotSettings : ISerializable
    {
        public DataSource DataSource { get; set; }
        public string Uri { get; set; }
        public string Path { get; set; }

        public SpaceRaceBotSettings()
        {
        }

        protected SpaceRaceBotSettings(SerializationInfo info, StreamingContext context)
        {
            
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            
        }

        public SpaceRaceBot CreateBot() => new SpaceRaceBot(this);
    }
}