using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicPickup : MonoBehaviour
{
    Vector3 startPosition;
    bool goingUp;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y >= startPosition.y + .5f)
        {
            goingUp = false;
        }
        if (gameObject.transform.position.y <= startPosition.y - .5f)
        {
            goingUp = true;
        }

        if (goingUp)
        {
            transform.Translate(0, Time.deltaTime * 1.5f, 0);
        }
        else
        {
            transform.Translate(0, -Time.deltaTime * 1.5f, 0);
        }

        transform.Rotate(0, .25f, 0);
    }
}
