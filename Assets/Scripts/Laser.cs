using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    //Speed Variable of 8
    private float _speed = 8.0f;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if(transform.position.y > 8f)
        {
            Debug.Log("Laser Hit");
            if (transform.parent != null)
                    Destroy(transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }
}
