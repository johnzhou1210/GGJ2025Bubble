using UnityEngine;

public class BubbleTrigger : MonoBehaviour {
    bool debounce = false;
    
    private void OnTriggerEnter2D(Collider2D collision) {
        if (debounce) return;
        debounce = true;
        if (collision.CompareTag("Player")) {
            PlayerMovement.Instance.inBubble = true;
            PlayerMovement.Instance.ActivateBubble();
        }
        else {
            PlayerMovement.Instance.inBubble = false;
        }
        Destroy(gameObject);
    }
}
