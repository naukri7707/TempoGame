using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBehaviour : MonoBehaviour
{
    const string NOTETAG = "Note";

    void Start()
    {

    }

    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Stay");
        if (collision.gameObject.CompareTag(NOTETAG))
        {
            if (Input.GetKeyDown(collision.gameObject.GetComponent<NoteBehaviour>().CorrespondButton))
            {
                Destroy(collision.gameObject);
            }
        }
    }
}
