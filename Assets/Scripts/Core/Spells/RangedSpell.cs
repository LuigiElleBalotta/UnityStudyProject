using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedSpell : MonoBehaviour
{
    public GameObject Target;
    public event Action OnDestroyEvent;

    void Start()
    {
        if (Target == null)
        {
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if( Target != null )
        {
            var targetStats = Target.GetComponent<UnitStats>();
            if (targetStats != null)
            {
                if (!targetStats.IsAlive())
                {
                    Debug.Log("Target is dead, cannot cast any spell");
                    return;
                }
            }

            //Probably the target should be a special box inside the target model.
            Vector3 targetPosition = new Vector3(Target.transform.position.x,
                                                 Target.transform.position.y + 1.0f,
                                                 Target.transform.position.z);

            this.transform.LookAt(targetPosition);

            float distance2 = Vector3.Distance(Target.transform.position, this.transform.position);

            if( distance2 > 2.0f )
            {
                transform.Translate(Vector3.forward * 30.0f * Time.deltaTime);
            }
            else
            {
                HitTarget();
            }
        }
    }

    void HitTarget()
    {
        OnDestroyEvent?.Invoke();

        //Send Message like "deal damage"
        Destroy(this.gameObject);
    }
}
