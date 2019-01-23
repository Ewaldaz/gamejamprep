using System;
using System.Collections;
using UnityEngine;

public class MapLoader : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Tile") || other.CompareTag("Collectible")) && !other.GetComponent<MeshRenderer>().enabled)
        {
            other.GetComponent<MeshRenderer>().enabled = true;
            if (other.CompareTag("Tile"))
            {
                StartCoroutine(AnimateTileCo(other.transform));
            }
        }
    }

    private IEnumerator AnimateTileCo(Transform transform)
    {
        float moveSpeed = 0.5f;

        float elapsedTime = 0f;
        var startPos = transform.position;
        Vector3 position = new Vector3(transform.position.x, 0, transform.position.z);

        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector3.Lerp(startPos, position, elapsedTime / moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = position;
    }
}