using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject PillarPrefab;

    public void GeneratePillars()
    {
        //if (Physics.Raycast(this.transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        //{
        //    int PillarAmount = Mathf.CeilToInt(hit.distance / PillarHeight);
        //    for (int i = 1; i < PillarAmount; i++)
        //    {
        //        Instantiate(PillarPrefab, new Vector3(PillarCornerOne.x, PillarCornerOne.y - i * PillarHeight, PillarCornerOne.z), Quaternion.identity, NewPlatform.transform);
        //    }
        //}
        //Debug.DrawRay(this.transform.position, Vector3.down * 5, Color.green, 400);
        StartCoroutine(Waity());

    }

    public IEnumerator Waity()
    {
        yield return new WaitForSeconds(10);
        Debug.Log(this.transform.position);
        Debug.DrawRay(this.transform.position, Vector3.down * 5, Color.green, 400);
    }
}
