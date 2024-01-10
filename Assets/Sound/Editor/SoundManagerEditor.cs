using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable CheckNamespace

namespace SoundSystem 
{
    public static class SoundManagerEditorItem
    {
        [MenuItem("GameObject/Sun Tools/Managers/Sounds")]
        public static void AddSoundManagerToProject()
        {
            if (!Object.FindObjectOfType<SoundManager>())
            {
                var soundManager = new GameObject("Sound Manager");
                soundManager.AddComponent<SoundManager>();
            }
            else
            {
                Debug.Log("Scene already have Sound Manager.");
            }
        }
    }

    [CustomEditor(typeof(SoundManager))]
    public class SoundManagerEditor : Editor
    {
        private SoundManager soundManager;
        private SerializedObject sManager;
        
        public void OnEnable()
        {
            soundManager = (SoundManager)target;
            sManager = new SerializedObject(target);
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
        
            var droppedObject = DropAreaGUI();
            if (droppedObject.Length != 0)
            {
                foreach (var droppedObj in droppedObject)
                {
                    var splitObj = droppedObj.ToString().Split('/');
                    var nameObj = splitObj[^1];
                    var splitSoundName = nameObj.Split('-');
                    if (splitSoundName.Length is 0 or > 2)
                    {
                        Debug.LogWarning("Check name of AudioClip.");
                        continue;
                    }
        
                    var soundName = splitSoundName[0];
                    var soundClip = droppedObj as AudioClip;
                    var soundGroup = SoundTrackHelper.Background;
                    var soundVolume = 1f;
                    var soundPitch = 1f;
                    var soundLoop = false;
                    var soundFadeIn = false;
                    var soundFadeInTime = 0f;
                    var soundFadeOut = false;
                    var soundFadeOutTime = 0f;
        
                    if (splitSoundName[1].ToUpper().StartsWith("BGM"))
                    {
                        soundGroup = SoundTrackHelper.Background;
                        soundLoop = true;
                        soundFadeIn = true;
                        soundFadeInTime = .25f;
                        soundFadeOut = true;
                        soundFadeOutTime = .25f;
                    }
                    else if (splitSoundName[1].ToUpper().StartsWith("EFT"))
                    {
                        soundGroup = SoundTrackHelper.Effect;
                    }
                    else if (splitSoundName[1].ToUpper().StartsWith("UI"))
                    {
                        soundGroup = SoundTrackHelper.UInterface;
                    }
                    else
                    {
                        Debug.LogWarning("Check name of AudioClip, set to background group.");
                    }
        
                    var myNewSound = new ConfigSound(soundName, soundClip, soundGroup, soundVolume, soundPitch,
                        soundLoop, soundFadeIn, soundFadeInTime, soundFadeOut, soundFadeOutTime);
                    soundManager.ConfigSounds.Add(myNewSound);
                }
            }
        
            sManager.ApplyModifiedProperties();
        
            EditorUtility.SetDirty(target);
        }
        
        public object[] DropAreaGUI()
        {
            var toReturn = Array.Empty<object>();
        
            var evt = Event.current;
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            var drop_area = GUILayoutUtility.GetRect(350.0f, 70f, GUILayout.ExpandWidth(true));
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUI.Box(drop_area, "Drag multiple AudioClips here." +
                               "\nBackground clips end with -Bgm." +
                               "\nEffect clips end with -Eft." +
                               "\nUInterface clips end with -Ui.");
        
            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (drop_area.Contains(evt.mousePosition))
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                        if (evt.type == EventType.DragPerform)
                        {
                            var canGo = false;
                            var quantityToGo = 0;
                            foreach (var obj in DragAndDrop.objectReferences)
                            {
                                if (obj is AudioClip)
                                {
                                    canGo = true;
                                    quantityToGo++;
                                }
                            }
        
                            if (canGo)
                            {
                                DragAndDrop.AcceptDrag();
                                toReturn = new object[quantityToGo];
                                var counter = 0;
                                for (var i = 0; i < DragAndDrop.objectReferences.Length; i++)
                                {
                                    if (DragAndDrop.objectReferences[i] is AudioClip)
                                    {
                                        DragAndDrop.objectReferences[i].name = DragAndDrop.paths[i];
                                        toReturn[counter] = DragAndDrop.objectReferences[i];
                                        counter++;
                                    }
                                }
                            }
                        }
                    }
        
                    break;
            }
        
            return toReturn;
        }
    }
}