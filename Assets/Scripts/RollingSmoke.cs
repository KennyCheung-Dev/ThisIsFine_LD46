using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingSmoke : MonoBehaviour
{
    public float rollSpeed = 1.0f;
    public enum rollingDirection
    {
        Left = 0,
        Right = 1
    };
    public rollingDirection dir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (dir == rollingDirection.Left)
        {
            transform.localPosition -= new Vector3(rollSpeed * Time.deltaTime, 0, 0);
            if (transform.localPosition.x < -1920)
            {
                Debug.Log("It's over 1920");
                transform.localPosition = new Vector3(0, transform.position.y, transform.position.z);
            }
        }
        else if (dir == rollingDirection.Right)
        {
            transform.localPosition += new Vector3(rollSpeed * Time.deltaTime, 0, 0);
            if (transform.localPosition.x > 1920)
            {
                transform.localPosition = new Vector3(0, transform.position.y, transform.position.z);
            }
        }
        
    }
}
