using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public enum Direction { Forward = 0, Backward = 1, Left = 2, Right = 3 }
    [SerializeField] GameObject brickPrefab;
    [SerializeField] GameObject character;
    [SerializeField] float speed;
    [SerializeField] private LayerMask walllLayer;

    List<GameObject> bricks = new();
    Vector3? initMousePos = null;

    public static Player Instance;
    private bool isMoving = false;
    private const int offsetCheck = 100;
    private Vector3 targetPos;

    private void Awake()
    {
        Instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        //Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                initMousePos = Input.mousePosition;
            }
            if (Input.GetMouseButtonUp(0))
            {
                Control();
            }
        }
    }

    void Control()
    {
        if (initMousePos == null)
        {
            return;
        }
        StopCoroutine(Move());
        Vector2 c = Input.mousePosition - (Vector3)initMousePos;
        if (c.y > offsetCheck + 200)
        {
            CheckWall(Direction.Forward);
        }
        else if (c.y < -offsetCheck - 200)
        {
            CheckWall(Direction.Backward);
        }
        else if (c.x < -offsetCheck)
        {
            CheckWall(Direction.Left);
        }
        else if (c.x > offsetCheck)
        {
            CheckWall(Direction.Right);
        }
        if (isMoving)
            StartCoroutine(Move());

        initMousePos = null;
    }

    private IEnumerator Move()
    {
        while (isMoving)
        {
            if (Vector3.Distance(transform.position, targetPos) < 0.0001f)
            {
                isMoving = false;
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            yield return null;
        }
    }

    private bool CheckWall(Direction dir)
    {
        var currPos = transform.position;
        Vector3 movePos = Vector3.zero;
        switch (dir)
        {
            case Direction.Left:
                movePos = new Vector3(currPos.x, currPos.y, currPos.z + 1);
                break;
            case Direction.Right:
                movePos = new Vector3(currPos.x, currPos.y, currPos.z - 1);
                break;
            case Direction.Forward:
                movePos = new Vector3(currPos.x + 1, currPos.y, currPos.z);
                break;
            case Direction.Backward:
                movePos = new Vector3(currPos.x - 1, currPos.y, currPos.z);
                break;
        }

        bool hit = Physics.Raycast(transform.position, movePos - transform.position, out RaycastHit hitInfo, Int32.MaxValue, walllLayer);

        if (hit)
        {
            isMoving = true;
            float targetX;
            float targetZ;
            if (transform.position.x < hitInfo.point.x)
            {
                targetX = Mathf.Floor(hitInfo.point.x);
            }
            else
            {
                targetX = Mathf.Ceil(hitInfo.point.x);
            }

            if (transform.position.z < hitInfo.point.z)
            {
                targetZ = Mathf.Floor(hitInfo.point.z);
            }
            else
            {
                targetZ = Mathf.Ceil(hitInfo.point.z);
            }
            targetPos = new Vector3(targetX, hitInfo.point.y, targetZ);
            StartCoroutine(Move());
        }
        return hit;
    }

    public void AddBrick()
    {
        //add brick and increase player height
        var currPos = character.transform.position;
        float height = character.transform.position.y - 1;
        if (bricks.Count > 0)
        {
            height = bricks[bricks.Count - 1].transform.position.y + 0.2f;
        }
        GameObject newBrick = Instantiate(brickPrefab, new Vector3(currPos.x, height, currPos.z), transform.rotation);
        bricks.Add(newBrick);
        character.transform.position = new Vector3(currPos.x, currPos.y + 0.2f, currPos.z);
        newBrick.transform.parent = transform;
    }
    public void RemoveBrick()
    {
        //remove brick and decrease player height

        GameObject lastBrick = bricks[bricks.Count - 1];
        if (lastBrick != null && bricks.Count > 1)
        {
            bricks.Remove(lastBrick);
            character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y - 0.2f, character.transform.position.z);
            Destroy(lastBrick);
        }
        else
        {
            isMoving = false;
            StopCoroutine(Move());
            StopPlaying();
            UIManager.instance.ShowCanvas(UIManager.CanvasName.Restart);
        }
    }
    public void ClearBrick()
    {
        //remove all

        for (int i = bricks.Count - 1; i >= 0; i--)
        {
            Destroy(bricks[i]);
        }

        bricks.Clear();

        character.transform.position = new Vector3(character.transform.position.x, 0, character.transform.position.z);
    }

    public void ToLevel(Vector3 startPos)
    {
        isMoving = false;
        transform.position = startPos;
        initMousePos = null;
        enabled = true;
    }

    public void StopPlaying()
    {
        enabled = false;
    }
}
