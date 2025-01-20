using UnityEngine;

public class EndingTriggerArea : MonoBehaviour
{
    public GameObject endGamePanel;

    void OnTriggerEnter(Collider other)
    {
        Rigidbody rb = other.GetComponentInParent<Rigidbody>();

        if ( rb != null && other.CompareTag("Player"))
        {
            EndGame();
            Debug.Log("player made it to the finish");
        }

        else
        {
            Debug.Log("Rigidbody missing");
        }
    }


    void EndGame()
    {
        Cursor.lockState = CursorLockMode.None;
        endGamePanel.SetActive(true);
    }

}
