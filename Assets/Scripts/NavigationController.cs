using UnityEngine;

public class NavigationController : MonoBehaviour
{
    public GameObject[] views;

    public void ShowView(int index)
    {
        if (views == null || views.Length == 0) return;

        for (int i = 0; i < views.Length; i++)
        {
            if (views[i] != null)
            {
                views[i].SetActive(i == index);
            }
        }
    }
}