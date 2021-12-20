using System.Collections;
using TMPro;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    public GameObject points;
    public GameObject ball;
    public Timer runnin;
    private bool scored = false;
    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ball") && !scored)
        {
            scored = true;
            TextMeshProUGUI tmp = points.GetComponent<TextMeshProUGUI>();
            int score = int.Parse(tmp.text) + 1;
            if (score < 10)
            {
                tmp.text = "0" + score;
            }
            else
            {
                tmp.text = "" + score;
            }
            StartCoroutine(waitBeforeReset());
        }
    }

    private IEnumerator waitBeforeReset()
    {
        string time = runnin.stop();
        yield return new WaitForSeconds(5);
        Reset.ResetNow();
        scored = false;
        runnin.resume(int.Parse(time.Substring(0,2)), int.Parse(time.Substring(3)));

    }
}

