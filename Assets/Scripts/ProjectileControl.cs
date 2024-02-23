using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

public class ProjectileControl : MonoBehaviour
{
    public float speed = 20f;
    public AmmoType ammoType; // for the damage

    public CapsuleCollider refCollider;
    Vector3 point1Offset;
    Vector3 point2Offset;
    Vector3 forwardVector = new Vector3(0,0,1);
    public LayerMask hitMask;

    [Header("Explosion")]
    public bool explodes = true;
    public GameObject explosionPrefab;

    void Start()
    {
        switch (refCollider.direction)
        {
            case 0: //x
                point1Offset = new Vector3(0 + refCollider.bounds.extents.x, 0f, 0f);
                point2Offset = new Vector3(0 - refCollider.bounds.extents.x, 0f, 0f);
                break;

            case 1: //y
                point1Offset = new Vector3(0f, 0 + refCollider.bounds.extents.y, 0f);
                point2Offset = new Vector3(0f, 0 - refCollider.bounds.extents.y, 0f);
                break;

            case 2: //z
                point1Offset = new Vector3(0f, 0f, 0 + refCollider.bounds.extents.z);
                point2Offset = new Vector3(0f, 0f, 0 - refCollider.bounds.extents.z);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Vector3 nextMove = forwardVector * speed * Time.deltaTime;
        if (Physics.CapsuleCast(transform.position + point1Offset, 
            transform.position + point2Offset, //create a capsule collider between these 2 points
            refCollider.radius, // and with this radius
            transform.forward, //move it forward
            out hit, //store it in hit
            nextMove.magnitude, hitMask)) //move it this much forward , ignoring these layers
        {
            Explode(hit.point);
        }
        
        transform.Translate(nextMove); // move us forward
    }

    void Explode(Vector3 hitPos)
    {
        Instantiate(explosionPrefab, hitPos, Quaternion.identity);  //spawn explosion
        Destroy(gameObject); 
    }
}
