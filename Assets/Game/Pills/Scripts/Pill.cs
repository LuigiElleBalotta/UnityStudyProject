using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour
{
    [SerializeField]
    AudioSource audioSource;

    public bool goUp;
    public float speed = 1;
    public float rotationSpeed = 100;

    private void Start()
    {
        StartCoroutine(SwitchDirection());
    }

    // Update is called once per frame
    void Update()
    {
        if (goUp)
        {
            transform.position = transform.position + new Vector3(0, speed * Time.deltaTime, 0);
        }
        else
        {
            transform.position = transform.position - new Vector3(0, speed * Time.deltaTime, 0);
        }

        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }

    IEnumerator SwitchDirection()
    {
        while (gameObject.activeSelf)
        {
            yield return new WaitForSeconds(0.5f);
            goUp = !goUp;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnPicked(other);
        }
    }


    protected virtual void OnPicked(Collider other)
    {
        audioSource.Play();
        HidePill();
        GetComponent<Collider>().enabled = false;
        Debug.Log("Hai preso: " + gameObject.name);
    }

    void HidePill()
    {
        GetComponent<Renderer>().enabled = false;
    }

    void OnValidate()
    {
        if( audioSource ) { return; }
        audioSource = GetComponent<AudioSource>();
    }
}
