using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingNameHandler : MonoBehaviour
{

    public GameObject TextName;
    TextMesh textMesh;

    // Start is called before the first frame update
    void Start()
    {
        if (TextName != null)
            textMesh = TextName.GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        if( TextName != null )
        {
            TextName.transform.LookAt(Camera.main.transform.position);
            TextName.transform.Rotate(0, 180, 0);
            textMesh = TextName.GetComponent<TextMesh>();

            var ai = TextName.GetComponentInParent<CreatureAI>();

            if( ai )
            {
                textMesh.color = !ai.isSelectedByPlayer ? Color.blue : Color.green;
                textMesh.text = ai.creatureTemplate.Name;
            }
                
        }
    }
}
