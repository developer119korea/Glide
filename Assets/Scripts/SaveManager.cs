using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    public SaveState state;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        Instance = this;
        Load();

        if (state.usingAccelerometer && !SystemInfo.supportsAccelerometer)
        {
            state.usingAccelerometer = false;
            Save();
        }
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveState>(state));
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveState>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveState();
            Save();
            Debug.Log("No Save File Found, Creating a new one!");
        }
    }

    public bool IsColorOwned(int index)
    {
        return (state.colorOwned & (1 << index)) != 0;
    }

    public bool IsTrailOwned(int index)
    {
        return (state.trailOwned & (1 << index)) != 0;
    }

    public bool BuyColor(int index, int cost)
    {
        if(state.gold >= cost)
        {
            state.gold -= cost;
            UnLockColor(index);
            Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool BuyTrail(int index, int cost)
    {
        if (state.gold >= cost)
        {
            state.gold -= cost;
            UnLockTrail(index);
            Save();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UnLockColor(int index)
    {
        state.colorOwned |= 1 << index;
    }

    public void UnLockTrail(int index)
    {
        state.trailOwned |= 1 << index;
    }

    public void CompleteLevel(int index)
    {
        if (state.completedLevel == index)
        {
            state.completedLevel++;
            Save();
        }
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}
