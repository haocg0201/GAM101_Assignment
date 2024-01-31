using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillianBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D Rigidbody2D;
    private Animator Animator;
    public GameObject John;
    void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TargetJohn();
    }

    public void TargetJohn()
    {
        Vector3 direction = John.transform.position - transform.position;
        if(direction.x >= 0.0f)
        {
            transform.localScale = new Vector3(1.0f,1.0f,1.0f);
        }
        else{
            transform.localScale = new Vector3(-1.0f,1.0f,1.0f);
        }

        // float distance = direction.x - transform.localPosition.x;
    }
}
