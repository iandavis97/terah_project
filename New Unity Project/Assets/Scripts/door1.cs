using UnityEngine;
using UnityEngine.SceneManagement;

public class door1 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void GotoMainScene()
    {
        SceneManager.LoadScene("flatearth");
    }

}
