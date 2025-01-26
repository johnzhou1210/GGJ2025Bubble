using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class EndTrigger : MonoBehaviour {
    [SerializeField] Image image;
    
    bool debounce = false;
    void OnTriggerEnter2D(Collider2D other) {
        if (debounce) return;
        if (other.CompareTag("Player")) {
            debounce = true;
            StartCoroutine(FlashToWhite());
        }
    }

    private IEnumerator FlashToWhite() {
        for (float i = 0f ; i < 50f; i++) {
            image.color = new Color(i / 50f, i / 50f, i / 50f, i / 50f);
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(2);
    }
    
}
