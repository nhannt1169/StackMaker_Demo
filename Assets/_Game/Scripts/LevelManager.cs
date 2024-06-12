using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    private int currLevel;

    [SerializeField] Level[] levels;
    Level currentLevel;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        currLevel = 0;
        ChangeLevel(0);
    }

    public void ToNextLevel()
    {
        UIManager.instance.CloseAll();
        DeactiveLevel();
        ChangeLevel(currLevel + 1);
    }

    public void RestartLevel()
    {
        UIManager.instance.CloseAll();
        DeactiveLevel();
        ChangeLevel(currLevel);
    }

    private void ChangeLevel(int levelIndex)
    {
        if (levelIndex >= levels.Length)
        {
            levelIndex = Random.Range(0, levels.Length);
            currLevel = levelIndex;
        }
        currentLevel = Instantiate(levels[levelIndex]);
        Player.Instance.ToLevel(currentLevel.transform.position);
        currentLevel.gameObject.SetActive(true);
        currLevel = levelIndex;
    }

    private void DeactiveLevel()
    {
        if (currentLevel != null)
        {
            Destroy(currentLevel.gameObject);
            Player.Instance.ClearBrick();
        }
    }
}
