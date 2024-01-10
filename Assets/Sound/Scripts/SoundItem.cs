using DG.Tweening;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace SoundSystem
{
    public class SoundItem : MonoBehaviour
    {
        #region Variables

        public string Name { get; set; }
        public AudioSource AudioSource { get; set; }
        public ConfigSound ConfigSound { get; set; }
        public float CurrentSystemVolume { get; set; }
        public bool OnFadeVolume { get; set; }

        private Coroutine coroutineEndClip;
        private Coroutine coroutineFadeoutEndClip;
        private Tweener tweenFadeVolume;

        #endregion

        #region Initialize

        public void Initialize(ConfigSound configSound, float currentSystemVolume)
        {
            Name = configSound.Name;
            ConfigSound = configSound;
            AudioSource = gameObject.GetOrAddComponent<AudioSource>();
            AudioSource.playOnAwake = false;
            AudioSource.clip = configSound.Clip;
            AudioSource.volume = currentSystemVolume;
            AudioSource.pitch = configSound.Pitch;
            AudioSource.loop = configSound.Loop;
            CurrentSystemVolume = currentSystemVolume;
            OnFadeVolume = false;
        }

        #endregion

        #region Control functions
        
        public void Play()
        {
            if (IsPlaying() || OnFadeVolume) return;
        
            // Play audio
            if (ConfigSound.FadeIn)
            {
                tweenFadeVolume?.Kill();
                tweenFadeVolume = SoundTools.AddDOTween(ConfigSound.FadeInTime,
                    () =>
                    {
                        SetVolume(0f);
                        AudioSource.Play();
                        OnFadeVolume = true;
                    },
                    process => { SetVolume(Mathf.Lerp(0f, CurrentSystemVolume, process)); },
                    () => { OnFadeVolume = false; });
            }
            else
            {
                AudioSource.Play();
                OnFadeVolume = false;
            }
        
            // Check loop
            if (!ConfigSound.Loop)
            {
                // Event complete playing sound
                if (coroutineEndClip != null) StopCoroutine(coroutineEndClip);
                coroutineEndClip = StartCoroutine(SoundTools.DelayCoroutine(
                    () => SoundManager.CompleteSound.Invoke(this),
                    Duration()));
        
                // Check fade out
                if (ConfigSound.FadeOut)
                {
                    if (coroutineFadeoutEndClip != null) StopCoroutine(coroutineFadeoutEndClip);
                    coroutineFadeoutEndClip = StartCoroutine(SoundTools.DelayCoroutine(() =>
                    {
                        var currentVolume = AudioSource.volume;
                        tweenFadeVolume?.Kill();
                        tweenFadeVolume = SoundTools.AddDOTween(ConfigSound.FadeOutTime,
                            () => { OnFadeVolume = true; },
                            process => { SetVolume(Mathf.Lerp(currentVolume, 0f, process)); },
                            () =>
                            {
                                AudioSource.Pause();
                                OnFadeVolume = false;
                            });
                    }, Duration() - ConfigSound.FadeOutTime));
                }
            }
        }
        
        public void Pause()
        {
            if (!IsPlaying()) return;
            if (ConfigSound.FadeOut)
            {
                var currentVolume = AudioSource.volume;
                tweenFadeVolume?.Kill();
                tweenFadeVolume = SoundTools.AddDOTween(ConfigSound.FadeOutTime,
                    () => { OnFadeVolume = true; },
                    process => { SetVolume(Mathf.Lerp(currentVolume, 0f, process)); },
                    () =>
                    {
                        AudioSource.Pause();
                        OnFadeVolume = false;
                    });
            }
            else
            {
                AudioSource.Pause();
                OnFadeVolume = false;
            }
        
            if (!ConfigSound.Loop && coroutineEndClip != null)
            {
                StopCoroutine(coroutineEndClip);
            }
        }
        
        public void Stop()
        {
            if (!IsPlaying()) return;
            if (ConfigSound.FadeOut)
            {
                var currentVolume = AudioSource.volume;
                tweenFadeVolume?.Kill();
                tweenFadeVolume = SoundTools.AddDOTween(ConfigSound.FadeOutTime,
                    () => { OnFadeVolume = true; },
                    process => { SetVolume(Mathf.Lerp(currentVolume, 0f, process)); },
                    () =>
                    {
                        AudioSource.Pause();
                        OnFadeVolume = false;
                        SoundManager.CompleteSound.Invoke(this);
                    });
            }
            else
            {
                AudioSource.Stop();
                OnFadeVolume = false;
                SoundManager.CompleteSound.Invoke(this);
            }
        
            if (!ConfigSound.Loop && coroutineEndClip != null)
            {
                StopCoroutine(coroutineEndClip);
            }
        }
        
        #endregion

        #region Audiosource functions

        public void SetVolume(float volume)
        {
            AudioSource.volume = volume <= ConfigSound.Volume ? volume : ConfigSound.Volume;
        }

        public void SetVolume(bool volume)
        {
            AudioSource.volume = volume ? ConfigSound.Volume : 0f;
        }

        public void SetPitch(float pitch)
        {
            AudioSource.pitch = pitch;
        }

        public float Duration()
        {
            return (AudioSource.clip != null ? AudioSource.clip.length * (1 / AudioSource.pitch) : 0f) -
                   AudioSource.time;
        }

        public bool IsPlaying()
        {
            return AudioSource.isPlaying;
        }

        #endregion
    }
}