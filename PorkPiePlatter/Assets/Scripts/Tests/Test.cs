using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
	void Start ()
    {
        WordTreeHelper helper = new WordTreeHelper();
        helper.DebugPrintTree("western");
	}
}
