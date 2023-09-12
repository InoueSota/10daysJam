using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpParticle : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject Parent = null;

    void Update()
    {
        if(Parent != null)
        {
            this.transform.position = Parent.transform.position;
        }
    }

    public void SetParent(GameObject parent)
    {
        Parent = parent;
    }

    public void DoDestroy()
    {
        Destroy(this.gameObject);
    }
}
