using UnityEditor;
using UnityEngine;

public class LetterDataCollectionEditor : EditorWindow
{
    private LetterDataCollection mLetterDataCollection;

    private Vector2 mScrollVector;


    [MenuItem("Game Tools/Letter Data Collection Editor")]
    private static void ShowWindow()
    {
        GetWindow(typeof(LetterDataCollectionEditor));
    }

    private void OnEnable()
    {
        Init();
    }

    private void OnFocus()
    {
        Init();
    }

    private void OnLostFocus()
    {
        Init();
    }

    private void OnSelectionChange()
    {
        Init();
    }

    private void Init()
    {
        if (Selection.activeObject is LetterDataCollection)
        {
            mLetterDataCollection = Selection.activeObject as LetterDataCollection;
        }
        else
        {
            mLetterDataCollection = null;
        }
    }

    private void OnGUI()
    {
        if (mLetterDataCollection == null)
        {
            EditorGUILayout.LabelField("Select a Word Data Collection file");

            if (GUILayout.Button("Go to folder"))
            {
                Object obj = AssetDatabase.LoadAssetAtPath("Assets/Resources/" + StringIdentifier.RESOURCE_PATH_LETTER_COLLECTION_FOLDER, typeof(Object));
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }

            if (GUILayout.Button("Create new collection"))
            {
                LetterDataCollection letterDataCollection = ScriptableObject.CreateInstance<LetterDataCollection>();
                letterDataCollection.Initialize();
                string path = "Assets/Resources/" + StringIdentifier.RESOURCE_PATH_LETTER_COLLECTION + ".asset";
                AssetDatabase.CreateAsset(letterDataCollection, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = letterDataCollection;
                EditorGUIUtility.PingObject(letterDataCollection);

                Init();
            }
        }
        else
        {
            if (GUILayout.Button("Added Letter Data"))
            {
                mLetterDataCollection._Letters.Add(new LetterDataCollection.LetterData());
            }

            mScrollVector = EditorGUILayout.BeginScrollView(mScrollVector);
            for (int i = 0; i < mLetterDataCollection._Letters.Count; i++)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUILayout.BeginHorizontal();

                mLetterDataCollection._Letters[i]._Letter = EditorGUILayout.TextField("Letter: ", mLetterDataCollection._Letters[i]._Letter);
                mLetterDataCollection._Letters[i]._Count = EditorGUILayout.IntField("Count: ", mLetterDataCollection._Letters[i]._Count);

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndScrollView();
        }
    }
}
