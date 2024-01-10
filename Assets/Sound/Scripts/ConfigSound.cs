using System;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace SoundSystem
{
    [Serializable] public class ConfigSound
    {
        public string Name;
        public AudioClip Clip;
        public SoundTrackHelper Track;
        public float Volume;
        public float Pitch;
        public bool Loop;
        public bool FadeIn;
        public float FadeInTime;
        public bool FadeOut;
        public float FadeOutTime;

        public ConfigSound(string sName, AudioClip sClip, SoundTrackHelper sTrack, float sVolume, float sPitch, bool sLoop, bool sFadeIn, float sFadeInTime, bool sFadeOut, float sFadeOutTime)
        {
            Name = sName;
            Clip = sClip;
            Track = sTrack;
            Volume = sVolume;
            Pitch = sPitch;
            Loop = sLoop;
            FadeIn = sFadeIn;
            FadeInTime = sFadeInTime;
            FadeOut = sFadeOut;
            FadeOutTime = sFadeOutTime;
        }
    }
}