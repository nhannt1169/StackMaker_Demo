using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum CanvasName { Victory = 0, Restart = 1 }

    [SerializeField] List<Canvas> canvasList = new();
    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void ShowCanvas(CanvasName canvas)
    {
        canvasList[(int)canvas].gameObject.SetActive(true);
    }

    public void CloseAll()
    {
        foreach (Canvas c in canvasList)
        {
            c.gameObject.SetActive(false);
        }
    }
}
