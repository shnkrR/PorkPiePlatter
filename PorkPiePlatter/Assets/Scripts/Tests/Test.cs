using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	void Start ()
    {
        WordTree helper = new WordTree();
        float time = Time.realtimeSinceStartup;
        string word = helper.FindWord("zealousnesses");
        Debug.Log("End: " + word + ": " + (Time.realtimeSinceStartup - time));
    }
}
