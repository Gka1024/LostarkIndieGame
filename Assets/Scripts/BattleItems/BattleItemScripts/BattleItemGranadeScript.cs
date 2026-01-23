using System.Collections;
using UnityEngine;

public class BattleItemGranadeScript : MonoBehaviour
{

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("HexTile"))
        {
            StartCoroutine(DestroyOnTime());
        }
    }

    private IEnumerator DestroyOnTime()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(this.gameObject);
    }
}
