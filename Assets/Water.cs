using UnityEngine;
using UnityEngine.SceneManagement;

public class Water : MonoBehaviour
{
    void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "Frog") 
        {
            Debug.Log("Sink");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
