using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    float m_health;
    public PlayerController m_playerController;
    public GameObject rockParticlePrefab;

    void Awake()
    {
        m_playerController = GameObject.Find("Player").GetComponent<PlayerController>();
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
        GameObject clone;
        clone = Instantiate(rockParticlePrefab, transform.position, transform.rotation);
        m_health -= 1;
    }
    void DestroyOre()
    {
        m_playerController.m_canMove = true;
        m_playerController.StopMining();
        Destroy(this.gameObject);
    }
}
