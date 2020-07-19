using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCastDamage : MonoBehaviour
{
    public int Damage = 10;
    [SerializeField] LayerMask _mask;
    [SerializeField] float _radius = 0.2f;
    [SerializeField] float _distance;
    [SerializeField] Transform _owner;

    [SerializeField] bool _debug = true;

    void Update()
    {
        RaycastHit hit;
        Vector3 position = transform.position;
        bool isHit = Physics.SphereCast(position, _radius, transform.forward, out hit, _distance, _mask);

        if (isHit)
        {
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            if (playerHealth)
            {
                if (playerHealth.CanGetDamage)
                {
                    playerHealth.AddDamage(-Damage, _owner.position);
                    gameObject.SetActive(false);
                }
            }

        
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!_debug)
        {
            return;
        }

        RaycastHit hit;
        Vector3 position = transform.position;
        bool isHit = Physics.SphereCast(position, _radius, transform.forward, out hit, _distance, _mask);

        if (isHit)
        {
            Gizmos.color = Color.red;
        }
        else
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawRay(position, transform.forward * _distance);
        Gizmos.DrawWireSphere(position + transform.forward * _distance, _radius);

    }
#endif
}