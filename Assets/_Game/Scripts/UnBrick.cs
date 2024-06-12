using UnityEngine;

public class UnBrick : MonoBehaviour
{
    bool claimed = false;
    [SerializeField] GameObject brickPrefab;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (!claimed)
        {
            if (other.CompareTag("Player"))
            {
                claimed = true;
                Player.Instance.RemoveBrick();
                var newBrick = Instantiate(brickPrefab, new Vector3(transform.position.x, transform.position.y - 1, transform.position.z), transform.rotation);
                newBrick.transform.parent = transform;
            }
        }
    }
}
