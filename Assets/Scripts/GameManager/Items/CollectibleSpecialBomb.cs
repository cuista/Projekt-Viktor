using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectibleSpecialBomb : MonoBehaviour
{
    [SerializeField] private string itemName;

    //add special bomb liquid
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<CharacterController>()){
            if(Managers.Inventory.GetItemCount(itemName)<Managers.Inventory.GetSpecialBombsCapacity())
            {
                Managers.Inventory.AddSpecialBomb(itemName);
            }
            Destroy(this.gameObject);
        }
    }
}
