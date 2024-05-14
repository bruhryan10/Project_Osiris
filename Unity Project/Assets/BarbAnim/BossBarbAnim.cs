using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBarbAnim : MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator.Play("boss barbarian|boss idle");
    }

    // Update is called once per frame
    void Update()
    {

    }

}
