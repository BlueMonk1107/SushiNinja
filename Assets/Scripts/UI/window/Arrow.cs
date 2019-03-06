using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Arrow : MonoBehaviour
{
    public Transform _grid;//图标排序组件
    private Transform _left_Arrow;
    private Transform _right_Arrow;
	// Use this for initialization
	void Start ()
	{
	    _left_Arrow = transform.GetChild(0);
	    _right_Arrow = transform.GetChild(1);

	    _left_Arrow.DOLocalMoveX(_left_Arrow.localPosition.x - 2f, 0.5f).SetLoops(-1, LoopType.Restart);
	    _left_Arrow.DOPlay();

        _right_Arrow.DOLocalMoveX(_right_Arrow.localPosition.x + 2f, 0.5f).SetLoops(-1, LoopType.Restart);
        _right_Arrow.DOPlay();

        JudgeGridPositionToSetArrowActive(_grid, _left_Arrow, _right_Arrow);
    }
	
	// Update is called once per frame
	void Update () {
	    if (Input.GetMouseButtonUp(0))
	    {
            JudgeGridPositionToSetArrowActive(_grid,_left_Arrow,_right_Arrow);
        }
	}

    void JudgeGridPositionToSetArrowActive(Transform grid,Transform left,Transform right)
    {
        float x = grid.GetComponent<RectTransform>().anchoredPosition.x;
        if (x > -325)
        {
            left.gameObject.SetActive(false);
            right.gameObject.SetActive(true);
        }
        else
        {
            left.gameObject.SetActive(true);
            right.gameObject.SetActive(false);
        }
    }
    public void Click(int mark)
    {
        switch (mark)
        {
            case 0:
                _grid.Translate(100,0,0);
                break;
            case 1:
                _grid.Translate(-100, 0, 0);
                break;
        }
    }
}
