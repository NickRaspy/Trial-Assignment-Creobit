using System;
using UnityEngine;

namespace CB_TA.Adventure
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] private GameObject endScreen;
        public event Action OnFinish;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerBehavior>().CanMove = false;
                endScreen.SetActive(true);
                OnFinish();
            }
        }
    }
}
