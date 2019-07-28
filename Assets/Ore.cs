using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OreType
{
    Regular,
    Bomb
}
public class Ore : MonoBehaviour
{
    float m_health;
    public PlayerController m_playerController;
    public GameObject rockParticlePrefab;
    public GameObject bombParticlePrefab;
    GameObject audioManager;
    public OreType oreType = OreType.Regular;

    public float explosionForce;
    public float explosionRadius;
    public float upwardsModifier;

    void Start()
    {
        if(m_playerController == null)
        m_playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        audioManager = GameObject.FindGameObjectWithTag("Sound");

        m_health = 3;
    }
    void Update()
    {
        if (m_health <= 0)
        {
           DestroyOre();
        }
    }

    public void StartMineOre()
    {
        StartCoroutine(MineOre());
    }

    IEnumerator MineOre()
    {
        yield return new WaitForSeconds(1);
        HitRock();
        StartCoroutine(MineOre());
    }

    void HitRock()
    {
        GameObject clone = Instantiate(rockParticlePrefab, transform.position, transform.rotation);
        m_health -= 1;
    }
    void DestroyOre()
    {
        switch (oreType)
        {
            case OreType.Regular:
                break;
            case OreType.Bomb:
                Explode();
                //m_playerController.gameObject.GetComponent<PlayerController>().KillPuppet();
                //GameObject clone = Instantiate(bombParticlePrefab, transform.position, transform.rotation);
                break;
        }
        m_playerController.m_canMove = true;
        m_playerController.StopMining();
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if( other.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
    }

    void Explode()
    {
        Instantiate(bombParticlePrefab, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardsModifier);
                audioManager.GetComponent<AudioManager>().PlaySFX(0);
                m_playerController.gameObject.GetComponent<PlayerController>().KillPuppet();
            }
        }
    }
}
