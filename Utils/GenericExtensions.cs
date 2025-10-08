using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public static class GenericExtensions
{
    public static void SetAsInteractable(this Button btn, bool value)
    {
        btn.interactable = value;
        var img = btn.GetComponentsInChildren<Image>();
        var txt = btn.GetComponentsInChildren<TextMeshProUGUI>();

        foreach (var item in img)
        {
            var c = item.color;
            item.color = value ? new Color(c.r, c.g, c.b, 1) : new Color(c.r, c.g, c.b, .1f);
        }

        foreach (var item in txt)
        {
            var c = item.color;
            item.color = value ? new Color(c.r, c.g, c.b, 1) : new Color(c.r, c.g, c.b, .1f);
        }
    }

    public static void AddWrappedCallback(this Button button, UnityAction action)
    {
        if (action == null || button == null)
            return;

        //IGNORAR SUGESTAO, TEM QUE USAR LAMBDA PRA SER LOCAL
        UnityAction wrappedAction = null;

        wrappedAction = () =>
        {
            action?.Invoke();
            button.onClick.RemoveListener(wrappedAction);
        };

        button.onClick.AddListener(wrappedAction);
    }

    public static void RebuildRect(this RectTransform rect)
    {
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
    }

    public static void DestroyAllChildren(this Transform transform)
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            UnityEngine.Object.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public static string PaintText(this string text, string colorName)
    {
        return $"<color=\"{colorName}\">{text}</color>";
    }

    public static bool GetComponentInAnyParent<T>(this GameObject gameObject, out T component)
    {
        var parents = gameObject.transform.GetAllParentsRecursive();

        foreach (var parent in parents)
        {
            if(parent.TryGetComponent(out component))
            {
                return true;
            }
        }

        component = default;
        return false;
    }

    public static List<Transform> GetAllParentsRecursive(this Transform transform)
    {
        List<Transform> parents = new List<Transform>();
        Transform current = transform.parent;

        while (current != null)
        {
            parents.Add(current);
            current = current.parent;
        }

        parents.Reverse();
        return parents;
    }
}