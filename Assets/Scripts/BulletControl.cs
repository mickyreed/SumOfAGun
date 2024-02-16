using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    public float speed = 50f;
    public LineRenderer line;
    Vector3 forwardVector = new Vector3 (0f, 0f, 1f);
    Vector3 impactSite;
    bool hit = false;

    public void Initialise(bool hit,Vector3 impactSite)
    {
        this.impactSite = impactSite;

        this.hit = hit;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(forwardVector * speed * Time.deltaTime);

        if (hit)
        {
            SortLine();
        }
    }

    void SortLine()
    {
        for (int i = line.positionCount-1; i >= 0; i--)
        {
            Vector3 linePos = line.GetPosition(i);
            if(Vector3.Distance(linePos, transform.position) > Vector3.Distance(impactSite, transform.position))
            {
                line.SetPosition(i, transform.InverseTransformPoint(impactSite)); // line will crunch down on impact instead of going thru

                if(i == 0) // destroy self if the entire line is now at the impact site
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
