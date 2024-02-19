using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mastermind : MonoBehaviour
{
    [SerializeField] tempManager left1;
    [SerializeField] tempManager left2;
    [SerializeField] tempManager right1;
    [SerializeField] tempManager right2;

    [SerializeField] VisualTag visualTag;

    private float time;

    // Start is called before the first frame update
    void Start()
    {
        time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - time > 5) {
            time = Time.time;
            left2.item.SetVisualTag(visualTag);
            left2.UpdateItem();
            Debug.Log("Changed Visual Tag");
        }
    }
}
