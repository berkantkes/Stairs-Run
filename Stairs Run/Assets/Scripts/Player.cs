using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject stackParent;
    [SerializeField] private GameObject spawnPos;
    [SerializeField] private GameObject stair;
    [SerializeField] private GameObject GameOverCanvas;
    [SerializeField] private GameObject LevelCompleteCanvas;
    [SerializeField] private Text pointText;
    

    private int lineDiffrence = 0;
    private int stacksIndex = 0;
    private Rigidbody rb;
    private GameObject lastStair;
    private GameObject[] stacks = new GameObject[999];
    private bool isMouseDown = false;
    private bool velo = false;
    public bool gameOver = false;
    private bool isFinish = false;
    private float yDistance = 0.03f;
    private float maxSpeed = 4f;
    private float levelTime = 0;
    private float gameTime = 0;
    private float levelTimeText;
    private float gameTimeText;
    
    void Start()
    {
        Debug.Log("Game Started");
        StartCoroutine(CreateStair());
        rb = GetComponent<Rigidbody>();
        Time.timeScale = 1;
        isFinish = false;
        levelTime = 0;
    }

    void Update()
    {
        levelTime += Time.deltaTime;
        levelTimeText = Mathf.Round(levelTime);
        gameTime += Time.deltaTime;
        gameTimeText = Mathf.Round(gameTime);

        Run();

        for (int i = 0; i < stacksIndex; i++)
        {
            stacks[i].transform.position = spawnPos.transform.position + new Vector3(0, yDistance * i, 0);
        }

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
        }

        if (Input.GetMouseButton(0) || isFinish)
        {
            if (stacksIndex <= 0)
            {
                rb.useGravity = true;
            }
            else
            {
                rb.useGravity = false;
            }

            ZeroVelocity();

            if (stacksIndex > 0)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, 1.5f, 0), Time.deltaTime * 2);
            }

            isMouseDown = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if(stacksIndex > 0)
            {
                transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(-4f, 4, 0), Time.deltaTime * 10);
                isMouseDown = false;
                lastStair = null;
                rb.useGravity = true;
                velo = true;
            }
        }
    }

    private void ZeroVelocity()
    {
        if (velo)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
        velo = false;
    }

    private void Run()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(-1.5f, 0, 0), Time.deltaTime * 2);
    }

    IEnumerator CreateStair()
    {
        while (true)
        {
            if (isMouseDown)
            {
                
                if (lastStair == null)
                {
                    if(stacksIndex > 0)
                    {
                        Debug.Log("Stairs created");
                        lastStair = Instantiate(stair, transform.position + new Vector3(-0.514f, -0.3f, 0), Quaternion.identity);
                        stacksIndex--;
                        Destroy(stacks[stacksIndex]);
                    }
   
                }
                else
                {
                    
                    if (stacksIndex > 0)
                    {
                        Debug.Log("Stairs created");
                        lastStair = Instantiate(stair, lastStair.transform.position + new Vector3(-0.2f, 0.2f, 0), Quaternion.identity);
                        stacksIndex--;
                        Destroy(stacks[stacksIndex]);
                    }
                    else
                    {
                        transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(-4f, 4, 0), Time.deltaTime * 10);
                        isMouseDown = false;
                        lastStair = null;
                        rb.useGravity = true;
                        velo = true;
                    }
                    
                }
            }
            yield return new WaitForSeconds(0.0645f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Stack")
        {
            Debug.Log("Collect");
            if(stacksIndex < 0)
            {
                stacksIndex = 0;
            }
            other.transform.parent = stackParent.transform;
            AssignLocation(other);
            stacks[stacksIndex] = other.gameObject;
            stacksIndex++;
        }

        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("Hit the Barrier");

            gameOver = true;
            Time.timeScale = 0;
            GameOverCanvas.SetActive(true);
        }

        if(other.gameObject.tag == "Point")
        {
            Debug.Log("Game has finished  in " + gameTimeText + "seconds");
            Debug.Log("Level has finished  in " + levelTimeText + "seconds");
            gameOver = true;
            Time.timeScale = 0;
            LevelCompleteCanvas.SetActive(true);
            float point = other.GetComponent<Point>().point;
            pointText.text = "Point : " + point;
        }

        if(other.gameObject.tag == "FinishLine")
        {
            isFinish = true;
        }
    }

    private void AssignLocation(Collider stack)
    {
        stack.transform.localPosition = spawnPos.transform.localPosition + new Vector3 (0, yDistance * lineDiffrence, 0);
        stack.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
        stack.transform.Rotate(0, 90f, 0, Space.World);
        lineDiffrence++;
    }

    public void Restart()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene("Scene_01");
    }
}
