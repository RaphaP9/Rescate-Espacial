using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMusicPoolSO", menuName = "ScriptableObjects/Audio/MusicPool")]
public class MusicPoolSO : ScriptableObject
{
    public List<SceneNameMusic> sceneNameMusicList;
}

[System.Serializable]
public class SceneNameMusic
{
    public string sceneName;
    public AudioClip music;
}
