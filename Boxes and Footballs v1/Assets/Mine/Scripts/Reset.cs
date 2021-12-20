using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reset : MonoBehaviour
{
    private static bool should_reset = false;
    private Vector3 pos;
    private Quaternion rot;
    private static int count = 0;
    private static int count2;
    public static void ResetNow()
    {
        count2 = count;
        should_reset = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(count);
        count++;
        pos = transform.position;
        rot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Revert();
    }
    private void Revert ()
    {
        
        if (should_reset)
        {
            count2--;
            Debug.Log(count2);
            this.transform.position = pos;
            this.transform.rotation = rot;
            if (count2 == 0) 
            {
                should_reset = false;
            }
        }
    }
}
