using UnityEngine;
using Assets.Scripts.Input;
using System.Collections;

public class PlayerController : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis(InputConstants.HORIZONTAL);

        if (x < 0.0f)
        {
            x = -1.0f;
        }
        else if (x > 0.0f)
        {
            x = 1.0f;
        }

        x *= Time.deltaTime;
        transform.Translate(x, 0.0f, 0.0f);
    }
}
