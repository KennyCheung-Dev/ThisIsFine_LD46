using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public List<GameObject> inventory;
    FireManagerScript fireManager;
    public GameObject relative;
    public float sizeReduction = .75f;
    void Start()
    {
        fireManager = GameObject.Find("ItemManagers").GetComponent<FireManagerScript>();
		//TAMIKO: changed fireManager.allItemsOnMap to TimeManager.absolutelyAllItems
		foreach (GameObject m in TimeManagerScript.instance.absolutelyAllItems)
        {
            inventory.Add(m);
            StartCoroutine(CheckInventory(m));
            StartCoroutine(checkSide(m));
        }

    }
    IEnumerator CheckInventory(GameObject g)
    {
        GridItem item = g.GetComponent<GridItem>();
        while (true)
        {

            yield return new WaitUntil(() => item.isPicked());
            inventory.Remove(g);
            yield return new WaitUntil(() => !item.isPicked());
            yield return new WaitForSeconds(.5f);
            yield return new WaitUntil(() => item.SingleGridOccupied[0] == -1);
            inventory.Add(g);
        }
    }
    IEnumerator checkSide(GameObject g)
    {
        Vector3 scale = g.transform.localScale;
        Vector3 reducedScale = new Vector3(g.transform.localScale.x * sizeReduction, g.transform.localScale.y * sizeReduction, g.transform.localScale.z * sizeReduction);
        while (true) {
            g.transform.localScale = reducedScale;
            yield return new WaitUntil(() => g.transform.position.x < relative.transform.position.x);
            g.transform.localScale = scale;
            yield return new WaitUntil(() => g.transform.position.x > relative.transform.position.x);

        }
    }
    


   
}
