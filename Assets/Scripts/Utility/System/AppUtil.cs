using GodTouches;
using UnityEngine;
using System.Collections;

public static class AppUtil
{
    private static Vector3 TouchPosition = Vector3.zero;

    /// <summary>
    /// タッチ情報を取得(エディタと実機を考慮)
    /// </summary>
    /// <returns>タッチ情報。タッチされていない場合は null</returns>
    public static TouchInfo GetTouch()
    {
        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                return TouchInfo.Began;
            }
            if (Input.GetMouseButton(0))
            {
                return TouchInfo.Moved;
            }
            if (Input.GetMouseButtonUp(0))
            {
                return TouchInfo.Ended;
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                return (TouchInfo)((int)Input.GetTouch(0).phase);
            }
        }
        return TouchInfo.None;
    }

    /// <summary>
    /// 複数のタッチ情報を取得(エディタと実機を考慮)
    /// エディタはマルチ非対応
    /// </summary>
    /// <returns>タッチ情報。タッチされていない場合は null</returns>
    public static TouchInfo[] GetTouches()
    {
        TouchInfo[] touchInfoArray = new TouchInfo[1]{TouchInfo.None};

        if (Application.isEditor)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchInfoArray[0] = TouchInfo.Began;
                    return touchInfoArray;
            }
            if (Input.GetMouseButton(0))
            {

                touchInfoArray[0] = TouchInfo.Moved;
                return touchInfoArray;
            }
            if (Input.GetMouseButtonUp(0))
            {
                touchInfoArray[0] = TouchInfo.Ended;
                return touchInfoArray;
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                touchInfoArray = new TouchInfo[Input.touchCount];
                for (var loopValue = 0; loopValue < Input.touchCount; loopValue++)
                {
                    touchInfoArray[loopValue] = (TouchInfo)((int)Input.GetTouch(loopValue).phase);
                }
                return touchInfoArray;
            }
        }
        return touchInfoArray;
    }

    /// <summary>
    /// タッチポジションを取得(エディタと実機を考慮)
    /// </summary>
    /// <returns>タッチポジション。タッチされていない場合は (0, 0, 0)</returns>
    public static Vector3 GetTouchPosition(int id = 0)
    {
        if (Application.isEditor)
        {
            TouchInfo touch = AppUtil.GetTouch();
            if (touch != TouchInfo.None)
            {
                return Input.mousePosition;
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(id);
                TouchPosition.x = touch.position.x;
                TouchPosition.y = touch.position.y;
                return TouchPosition;
            }
        }
        return Vector3.zero;
    }

    /// <summary>
    /// タッチワールドポジションを取得(エディタと実機を考慮)
    /// </summary>
    /// <param name='camera'>カメラ</param>
    /// <returns>タッチワールドポジション。タッチされていない場合は (0, 0, 0)</returns>
    public static Vector3 GetTouchWorldPosition(Camera camera)
    {
        return camera.ScreenToWorldPoint(GetTouchPosition());
    }
}

/// <summary>
/// タッチ情報。UnityEngine.TouchPhase に None の情報を追加拡張。
/// </summary>
public enum TouchInfo
{
    /// <summary>
    /// タッチなし
    /// </summary>
    None = 99,

    // 以下は UnityEngine.TouchPhase の値に対応
    /// <summary>
    /// タッチ開始
    /// </summary>
    Began = 0,
    /// <summary>
    /// タッチ移動
    /// </summary>
    Moved = 1,
    /// <summary>
    /// タッチ静止
    /// </summary>
    Stationary = 2,
    /// <summary>
    /// タッチ終了
    /// </summary>
    Ended = 3,
    /// <summary>
    /// タッチキャンセル
    /// </summary>
    Canceled = 4,
}