using System;
using UnityEngine;

public enum LevelPresetType
{
    Type1,
    Type2,
    Type3,
    Type4,
    Type5,
}

public class LevelPreset : MonoBehaviour
{
    public LevelPresetType Type;

    public Action OnLoadNextLevelPreset;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnLoadNextLevelPreset?.Invoke();
        }
    }
}
