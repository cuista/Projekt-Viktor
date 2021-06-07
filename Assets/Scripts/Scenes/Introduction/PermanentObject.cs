using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PermanentObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoadManager.PermanentObject(this.gameObject);
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("InitialMenu");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
