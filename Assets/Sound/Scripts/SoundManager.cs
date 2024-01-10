using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

// ReSharper disable CheckNamespace

namespace SoundSystem
{
    //------------------------------------//
    public enum SoundTrackHelper { Background = 0, Effect = 1, UInterface = 2 }

    public enum TrackHelper { Background = 0, Effect = 1, UInterface = 2, All = 3 }

    //------------------------------------//
    public class SoundManager : MonoBehaviour
    {
        #region Singleton

        public bool DontDestroy;

        private static SoundManager instance;
        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SoundManager>();

                    if (instance == null)
                    {
                        var singletonObject = new GameObject($"Singleton - {nameof(SoundManager)}");
                        instance = singletonObject.AddComponent<SoundManager>();
                    }
                }

                return instance;
            }
        }

        #endregion

        #region Variables

        public List<ConfigSound> ConfigSounds = new();

        public static ObjectPool<SoundItem> PoolSound { get; set; }
        public static List<SoundItem> AllSoundPlaying { get; set; }
        public static List<SoundItem> AllSoundPause { get; set; }
        public static Action<SoundItem> CompleteSound { get; set; }
        
        public static SoundPref<float> BackgroundPref { get; set; }
        public static SoundPref<float> EffectPref { get; set; }
        public static SoundPref<float> UInterfacePref { get; set; }
        public static SoundPref<float> AllPref { get; set; }

        public const string BackgroundPrefKey = "SoundManager_Background";
        public const string EffectPrefKey = "SoundManager_Effect";
        public const string UInterfacePrefKey = "SoundManager_UInterface";
        public const string AllPrefKey = "SoundManager_All";

        #endregion

        #region Unity callback functions

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                if (DontDestroy) DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning($"Another instance of {nameof(SoundManager)} is already exist! Destroying self...");
                Destroy(gameObject);
            }

            CheckStorage();
            CheckPrefs();
            CheckAction();
        }

        #endregion

        #region First checks

        private void CheckStorage()
        {
            PoolSound = new ObjectPool<SoundItem>(OnCreatePool, OnActionGet, OnActionRelease);
            AllSoundPlaying = new List<SoundItem>();
            AllSoundPause = new List<SoundItem>();
        }
        
        private void CheckPrefs()
        {
            if (BackgroundPref == null) BackgroundPref = new SoundPref<float>(BackgroundPrefKey, 1f);
            if (EffectPref == null) EffectPref = new SoundPref<float>(EffectPrefKey, 1f);
            if (UInterfacePref == null) UInterfacePref = new SoundPref<float>(UInterfacePrefKey, 1f);
            if (AllPref == null) AllPref = new SoundPref<float>(AllPrefKey, 1f);
        }

        private void CheckAction()
        {
            CompleteSound += item => PoolSound.Release(item);
        }

        #endregion

        #region Volume functions

        public static void Volume(float volume, TrackHelper trackCompare)
        {
            volume = Mathf.Clamp01(volume);
            switch (trackCompare)
            {
                case TrackHelper.Background:
                    BackgroundPref?.Set(volume);
                    break;
                case TrackHelper.Effect:
                    EffectPref?.Set(volume);
                    break;
                case TrackHelper.UInterface:
                    UInterfacePref?.Set(volume);
                    break;
                case TrackHelper.All:
                    AllPref?.Set(volume);
                    break;
            }
            InternalVolume(volume, trackCompare);
        }

        public static void Mute(bool mute, TrackHelper trackCompare)
        {
            switch (trackCompare)
            {
                case TrackHelper.Background:
                    BackgroundPref?.Set(mute ? 0f : 1f);
                    break;
                case TrackHelper.Effect:
                    EffectPref?.Set(mute ? 0f : 1f);
                    break;
                case TrackHelper.UInterface:
                    UInterfacePref?.Set(mute ? 0f : 1f);
                    break;
                case TrackHelper.All:
                    AllPref?.Set(mute ? 0f : 1f);
                    break;
            }

            InternalVolume(mute ? 0f : 1f, trackCompare);
        }

        public static float GetVolume(TrackHelper track)
        {
            var currentValue = 0f;
            switch (track)
            {
                case TrackHelper.Background:
                    currentValue = BackgroundPref.Get();
                    break;
                case TrackHelper.Effect:
                    currentValue = EffectPref.Get();
                    break;
                case TrackHelper.UInterface:
                    currentValue = UInterfacePref.Get();
                    break;
                case TrackHelper.All:
                    currentValue = AllPref.Get();
                    break;
            }

            return currentValue;
        }

        private static float GetVolume(SoundTrackHelper track)
        {
            var currentValue = 0f;
            switch (track)
            {
                case SoundTrackHelper.Background:
                    currentValue = BackgroundPref.Get();
                    break;
                case SoundTrackHelper.Effect:
                    currentValue = EffectPref.Get();
                    break;
                case SoundTrackHelper.UInterface:
                    currentValue = UInterfacePref.Get();
                    break;
            }

            return currentValue;
        }

        private static void InternalVolume(float volume, TrackHelper trackCompare)
        {
            if (trackCompare == TrackHelper.All)
            {
                volume = Mathf.Clamp01(volume);
                AllSoundPlaying?.ForEach(sound =>
                {
                    switch (sound.ConfigSound.Track)
                    {
                        case SoundTrackHelper.Background:
                            sound.CurrentSystemVolume = Mathf.Lerp(0f, BackgroundPref.Get(), volume);
                            sound.SetVolume(volume);
                            break;
                        case SoundTrackHelper.Effect:
                            sound.CurrentSystemVolume = Mathf.Lerp(0f, EffectPref.Get(), volume);
                            sound.SetVolume(volume);
                            break;
                        case SoundTrackHelper.UInterface:
                            sound.CurrentSystemVolume = Mathf.Lerp(0f, UInterfacePref.Get(), volume);
                            sound.SetVolume(volume);
                            break;
                    }
                });
                AllSoundPause?.ForEach(sound =>
                {
                    switch (sound.ConfigSound.Track)
                    {
                        case SoundTrackHelper.Background:
                            sound.CurrentSystemVolume = Mathf.Lerp(0f, BackgroundPref.Get(), volume);
                            sound.SetVolume(volume);
                            break;
                        case SoundTrackHelper.Effect:
                            sound.CurrentSystemVolume = Mathf.Lerp(0f, EffectPref.Get(), volume);
                            sound.SetVolume(volume);
                            break;
                        case SoundTrackHelper.UInterface:
                            sound.CurrentSystemVolume = Mathf.Lerp(0f, UInterfacePref.Get(), volume);
                            sound.SetVolume(volume);
                            break;
                    }
                });
            }
            else
            {
                volume = Mathf.Clamp01(volume);
                AllSoundPlaying?.ForEach(sound =>
                {
                    if (string.Equals(sound.ConfigSound.Track.ToString(), trackCompare.ToString()))
                    {
                        sound.CurrentSystemVolume = volume;
                        sound.SetVolume(volume);
                    }
                });
                AllSoundPause?.ForEach(sound =>
                {
                    if (string.Equals(sound.ConfigSound.Track.ToString(), trackCompare.ToString()))
                    {
                        sound.CurrentSystemVolume = volume;
                        sound.SetVolume(volume);
                    }
                });
            }
        }

        #endregion

        #region Manager callback functions

        public static void Play(string name)
        {
            var playingConfigSound = GetConfigSound(name);
            if (playingConfigSound == null)
            {
                Debug.LogWarning($"Wrong name, there's none: {name} Sound.");
                return;
            }

            var currentVolume = Mathf.Lerp(0f, GetVolume(playingConfigSound.Track), GetVolume(TrackHelper.All));
            var sunSound = PoolSound.Get();
            sunSound.Initialize(playingConfigSound, currentVolume);
            sunSound.Play();
            AllSoundPlaying.Add(sunSound);
        }

        public static void PlayContinue(string name)
        {
            var playingSound = AllSoundPause.Find(o => o.Name == name);
            if (playingSound != null)
            {
                playingSound.Play();
                AllSoundPause.Remove(playingSound);
                AllSoundPlaying.Add(playingSound);
            }
            else
            {
                Play(name);
            }
        }

        public static void Pause(string name)
        {
            var playingSound = AllSoundPlaying.Find(o => o.Name == name);
            if (playingSound == null)
            {
                Debug.LogWarning($"Wrong name, there's none: {name} Sound.");
                return;
            }

            playingSound.Pause();
            AllSoundPause.Add(playingSound);
            AllSoundPlaying.Remove(playingSound);
        }

        public static void Stop(string name)
        {
            var playingSound = AllSoundPlaying.Find(o => o.Name == name);
            if (playingSound == null)
            {
                Debug.LogWarning($"Wrong name, there's none: {name} Sound.");
                return;
            }

            playingSound.Stop();
            AllSoundPlaying.Remove(playingSound);
        }

        private static ConfigSound GetConfigSound(string name)
        {
            var soundsFound = new List<ConfigSound>();
            foreach (var configSound in Instance.ConfigSounds)
            {
                if (string.Equals(configSound.Name, name)) soundsFound.Add(configSound);
            }

            if (soundsFound.Count == 1) return soundsFound[0];
            if (soundsFound.Count > 1) return soundsFound[Random.Range(0, soundsFound.Count)];
            return null;
        }

        #endregion

        #region Pooling audio source

        private SoundItem OnCreatePool()
        {
            var newSunSound = new GameObject("SunSound_Pool").AddComponent<SoundItem>();
            newSunSound.transform.SetParent(transform);
            return newSunSound;
        }

        private void OnActionGet(SoundItem soundItem)
        {
            soundItem.gameObject.SetActive(true);
        }

        private void OnActionRelease(SoundItem soundItem)
        {
            soundItem.Name = string.Empty;
            soundItem.gameObject.SetActive(false);
        }

        #endregion
    }
}