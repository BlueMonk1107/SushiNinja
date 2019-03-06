using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Window_ChooseHeroList : MonoBehaviour {
    public int _count_Items;
    RectTransform _this_Rect;
    GridLayoutGroup _layout;
    // Use this for initialization
    void Start()
    {
        _layout = transform.GetComponent<GridLayoutGroup>();
        _this_Rect = this.transform.GetComponent<RectTransform>();
        _this_Rect.sizeDelta = new Vector2(_layout.cellSize.x * _count_Items + _layout.spacing.x * (_count_Items - 1)+140,_this_Rect.sizeDelta.y);
    }

    void OnEnable()
    {
        gameObject.transform.parent.GetComponentInChildren<Scrollbar>().value = 1;
    }
}
