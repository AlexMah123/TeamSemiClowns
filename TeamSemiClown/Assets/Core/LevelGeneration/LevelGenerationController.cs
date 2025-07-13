using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelGenerationController : MonoBehaviour
{
    [Header("Level Generation Settings")]
    [SerializeField] private List<LevelPreset> LevelPresetTypeList = new List<LevelPreset>();
    [SerializeField] private float levelPresetSpawnOffset = 30f;
    [SerializeField] private int copiesOfPresetPerType = 2;

    [Space(10)]
    [SerializeField] GameObject presetPoolContainer;

    private List<LevelPreset> levelPresetPool = new List<LevelPreset>();

    private LevelPreset currentLevelPreset;
    private LevelPreset nextLevelPreset;
    private LevelPreset bufferedNextLevelPreset;

    private void OnDisable()
    {
        //UnbindLevelPresetListeners();
    }

    private void Awake()
    {
        if(presetPoolContainer == null)
        {
            Debug.LogError("Preset Pool Container is not assigned.");
            return;
        }

        if(LevelPresetTypeList.Count == 0)
        {
            Debug.LogError("LevelPresetList is empty.");
            return;
        }

        InitializePool();
        SetupInitialLevels();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            AdvanceLevel();
        }
#endif
    }

    private void Start()
    {
        if(SFXController.Instance != null)
        {
            SFXController.Instance.PlayBGM();

        }
    }

    void InitializePool()
    {
        foreach (LevelPresetType type in System.Enum.GetValues(typeof(LevelPresetType)))
        {
            LevelPreset prefabToSpawn = LevelPresetTypeList[(int)type];

            // spawn X copies of it
            for (int i = 0; i < copiesOfPresetPerType; i++)
            {
                LevelPreset spawnedInstance = Instantiate(prefabToSpawn, presetPoolContainer.transform);
                spawnedInstance.gameObject.SetActive(false);

                spawnedInstance.OnLoadNextLevelPreset += AdvanceLevel;

                levelPresetPool.Add(spawnedInstance);
            }
        }
    }

    LevelPreset GetFromPool()
    {
        if (levelPresetPool.Count == 0)
        {
            Debug.LogWarning("Level preset pool is empty, cannot get a new preset.");
            return null;
        }

        int randomIndex = Random.Range(0, levelPresetPool.Count);

        if (levelPresetPool.Count > 0)
        {
            LevelPreset preset = levelPresetPool[randomIndex];
            levelPresetPool.RemoveAt(randomIndex);
            return preset;
        }

        // fallback if pool somehow empty
        LevelPreset fallback = Instantiate(LevelPresetTypeList[randomIndex], presetPoolContainer.transform);
        fallback.OnLoadNextLevelPreset += AdvanceLevel;
        fallback.gameObject.SetActive(false);

        return fallback;
    }

    void ReturnToPool(LevelPreset preset)
    {
        preset.gameObject.SetActive(false);
        levelPresetPool.Add(preset);
    }

    void SetupInitialLevels()
    {
        nextLevelPreset = GetFromPool();
        nextLevelPreset.transform.position = new Vector3(0, 0, levelPresetSpawnOffset);
        nextLevelPreset.gameObject.SetActive(true);

        LevelPreset nextNext = GetFromPool();
        nextNext.transform.position = new Vector3(0, 0, levelPresetSpawnOffset * 2);
        nextNext.gameObject.SetActive(true);

        bufferedNextLevelPreset = nextNext;
    }

    void AdvanceLevel()
    {
        if (currentLevelPreset != null)
        {
            ReturnToPool(currentLevelPreset);
        }

        currentLevelPreset = nextLevelPreset;
        nextLevelPreset = bufferedNextLevelPreset;

        bufferedNextLevelPreset = GetFromPool();
        bufferedNextLevelPreset.transform.position = currentLevelPreset.transform.position + new Vector3(0, 0, levelPresetSpawnOffset * 2);
        bufferedNextLevelPreset.gameObject.SetActive(true);
    }

    void UnbindLevelPresetListeners()
    {
        foreach (LevelPreset preset in levelPresetPool)
        {
            if (preset != null)
            {
                preset.OnLoadNextLevelPreset -= AdvanceLevel;
            }
        }

        // Also unsubscribe the active instances
        if (currentLevelPreset != null)
        {
            currentLevelPreset.OnLoadNextLevelPreset -= AdvanceLevel;
        }

        if (nextLevelPreset != null)
        {
            nextLevelPreset.OnLoadNextLevelPreset -= AdvanceLevel;
        }
    }
}
