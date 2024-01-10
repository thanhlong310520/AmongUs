using System;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace SoundSystem
{
    public class SoundPref<T>
    {
        #region Variables

        public string Key { get; }
        public T DefaultValue { get; }

        #endregion

        #region Constructor

        public SoundPref(string key, T defaultValue)
        {
            Key = key;
            DefaultValue = defaultValue;
            CheckPref();
        }

        private void CheckPref()
        {
            if (!PlayerPrefs.HasKey(Key))
            {
                var value = DefaultValue.ToString();
                PlayerPrefs.SetString(Key, value);
            }
        }

        #endregion

        #region Functions

        public void Set(T value)
        {
            var setValue = value.ToString();
            PlayerPrefs.SetString(Key, setValue);
        }

        public T Get()
        {
            var getValue = PlayerPrefs.GetString(Key);
            return (T)Convert.ChangeType(getValue, typeof(T));
        }

        #endregion
    }
}