using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

public class MapEditor : MonoBehaviour
{
    enum EditorMode
    {
        NavigationAI,
        GameOn
    }

    [Header("Editor Mode")]
    [SerializeField] EditorMode _editorMode;

    [Space]
    [SerializeField] List<GameObject> _NavigationAI;
    [SerializeField] List<GameObject> _AIOnly;

    [Space]
    [SerializeField] GameObject _planeGround;

    private void OnValidate()
    {
        if (_editorMode == EditorMode.NavigationAI)
        {
            foreach (GameObject go in _NavigationAI)
            {
                go.GetComponent<MeshRenderer>().enabled = true;
            }
            foreach (GameObject go in _AIOnly)
            {
                go.SetActive(true);
            }
        }
        else if (_editorMode == EditorMode.NavigationAI)
        {
            foreach (GameObject go in _NavigationAI)
            {
                go.GetComponent<MeshRenderer>().enabled = false;
            }
            foreach (GameObject go in _AIOnly)
            {
                go.SetActive(false);
            }
        }
    }

}

#endif
