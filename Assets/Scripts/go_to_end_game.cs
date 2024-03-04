using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class go_to_end_game : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger");
            //other.transform.position = Vector3.zero;
            SceneManager.LoadScene(sceneName);
        }
    }
}
