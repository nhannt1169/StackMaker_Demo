using UnityEngine;

public class Camera : MonoBehaviour
{
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.Instance.transform.position + offset;
    }
}
