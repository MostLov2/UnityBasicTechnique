using UnityEngine;

public class Util
{

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transforms =  FindChild<Transform>(go, name, recursive);
        if (transforms == null)
            return null;

        return transforms.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (!recursive)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name)||transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if(component != null)
                        return component;
                }

            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name)||component.name == name)
                    return component;
            }
        }

        return null;
    }
}