using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Update()
    {
        if (!FindObjectOfType<Player>().gameOver)
        {
            transform.position = player.transform.position + new Vector3(5f, 3f, 1.77f);
            transform.rotation = Quaternion.Euler(14.5f, 252, 0);
        }
        
    }
}
