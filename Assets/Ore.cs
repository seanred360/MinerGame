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
    public OreType oreType = OreType.Regular;

    void Awake()
    {
        if(m_playerController == null)
        m_playerController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerController>();

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
                m_playerController.gameObject.GetComponent<PlayerController>().KillPuppet();
                GameObject clone = Instantiate(bombParticlePrefab, transform.position, transform.rotation);
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
}
