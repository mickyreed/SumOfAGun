using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    public float destroyTime = 1f;
    public bool singleFrame = false;

    
    // Start is called before the first frame update
    void Start()
    {
        //print("created");
        StartCoroutine(DestroyRoutine());
    }

    IEnumerator DestroyRoutine()
    {
        if (!singleFrame)
        {
            yield return new WaitForSeconds(destroyTime);
        }
        yield return new WaitForEndOfFrame();
        Destroy(gameObject);
    }

}
