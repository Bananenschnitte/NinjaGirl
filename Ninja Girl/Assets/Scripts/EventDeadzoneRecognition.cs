using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDeadzoneRec : MonoBehaviour {

    [SerializeField]    private LayerMask groundLayer;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == groundLayer.value)
        {
            HealthController hc =
                other.gameObject.GetComponent<HealthController>();
            hc.kill();
        }
    }
}
