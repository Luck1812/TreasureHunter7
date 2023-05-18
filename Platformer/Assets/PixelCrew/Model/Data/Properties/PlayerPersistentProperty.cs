using System;
using System.IO;
using PixelCrew.Utils;
using UnityEngine;

namespace PixelCrew.Model.Data.Properties
{
    [Serializable]
    public class PlayerPersistentProperty : PrefsPersistentProperty<GameDataItems>
    {
        private readonly string _saveFileName = "playerdata.dat";
        
        public PlayerPersistentProperty(GameDataItems defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }

        protected override void Write(GameDataItems value)
        {
            string path = Path.Combine(Application.dataPath, _saveFileName);
            BinarySerializer.Serialize(path, value);
        }

        public void Writes(GameDataItems value)
        {
            Write(value);
        }
        public void Reads(GameDataItems value)
        {
            Read(value);
        }
        protected override GameDataItems Read(GameDataItems defaultValue)
        {
            string path = Path.Combine(Application.dataPath, _saveFileName);

            if (File.Exists(path))
            {
                var data = BinarySerializer.Deserialize<GameDataItems>(path);
                return data;
            }
            else
            {
                Write(defaultValue);
                return defaultValue;
            }
            /*throw new FileNotFoundException();*/
        }
    }
}