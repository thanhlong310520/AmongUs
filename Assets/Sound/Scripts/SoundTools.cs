using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

// ReSharper disable CheckNamespace

namespace SoundSystem
{
    public static class SoundTools
    {
        public static T GetOrAddComponent<T>(this GameObject child) where T : Component
        {
            var result = child.GetComponent<T>();
            if (result == null) result = child.AddComponent<T>();
            return result;
        }
        
        public static Tweener AddDOTween(float from, float to, float duration, Action onStart = null, Action<float> onUpdate = null, Action onComplete = null, Ease ease = Ease.Linear)
        {
            var currentValue = from;
            return DOTween.To(() => currentValue, setter => currentValue = setter, to, duration)
                .SetEase(ease)
                .OnStart(() => onStart?.Invoke())
                .OnUpdate(() => onUpdate?.Invoke(currentValue))
                .OnComplete(() => onComplete?.Invoke());
        }

        public static Tweener AddDOTween(float duration, Action onStart = null, Action<float> onUpdate = null, Action onComplete = null, Ease ease = Ease.Linear)
        {
            return AddDOTween(0f, 1f, duration, onStart, onUpdate, onComplete, ease);
        }
        
        public static IEnumerator DelayCoroutine(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }
    }
}