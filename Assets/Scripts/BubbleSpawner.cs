using System;
using System.Collections;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour {
    [SerializeField] float spawnCheckDelay = 10f;
    GameObject currentBubble;

    void Start() {
        StartCoroutine(BubbleSpawnCoroutine());
    }

    private IEnumerator BubbleSpawnCoroutine() {
        while (true) {
            yield return new WaitForSeconds(spawnCheckDelay);
            if (currentBubble == null) {
                // Spawn bubble
                currentBubble = Instantiate(Resources.Load<GameObject>("Prefabs/Bubble"), transform.position + Vector3.up * 1f, Quaternion.identity);
            }
        }
        yield return null;
    }

}
