using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPlayer : MonoBehaviour
{
    [SerializeField] private Transform spawnpoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Return(other.gameObject);
        }
    }
    public void Return(GameObject player)
    {
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.transform.position = spawnpoint.position;
    }
}
