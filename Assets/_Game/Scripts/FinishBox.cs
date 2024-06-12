using UnityEngine;
using static UIManager;

public class FinishBox : MonoBehaviour
{
    //[SerializeField] GameObject brick;
    bool reached = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!reached)
        {
            if (other.CompareTag("Player"))
            {
                reached = true;
                //LevelManager.Instance.ToNextLevel();
                UIManager.instance.ShowCanvas(CanvasName.Victory);
                Player.Instance.StopPlaying();
                //brick.SetActive(false);
            }
        }
    }
}
