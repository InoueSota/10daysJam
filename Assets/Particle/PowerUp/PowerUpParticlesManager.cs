using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpParticlesManager : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private PowerUpParticle powerUp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetParticle()
    {
        PowerUpParticle p = Instantiate(powerUp, this.transform.position, Quaternion.identity);
        p.SetParent(this.gameObject);
    }
}
