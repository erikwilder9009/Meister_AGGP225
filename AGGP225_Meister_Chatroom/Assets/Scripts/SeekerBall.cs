using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerBall : Ball
{
    public GameObject Target;
    public override void Start()
    {
        RaycastHit hit;
        Ray ray = Camera.current.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            Target = hit.transform.gameObject;
            Debug.Log(Target.name);
        }
    }
    void Update() 
    { 
        if(Target!=null)
        {
            transform.Translate(Vector3.MoveTowards(gameObject.transform.position, Target.transform.position, 1));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Target = null;
    }
}
