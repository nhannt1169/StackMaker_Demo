using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] GameObject brick;
    bool claimed = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!claimed)
        {
            if (other.CompareTag("Player"))
            {
                claimed = true;
                Player.Instance.AddBrick();
                brick.SetActive(false);
            }
        }
    }
}
