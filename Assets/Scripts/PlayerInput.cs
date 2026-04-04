using System;
//using System.Numerics;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string XAxisMove = "Horizontal";
    public static readonly string YAxisMove = "Vertical";
    public static readonly string FireButton = "Fire1";

    public float MoveX {get; private set;} // X축 움직임
    public float MoveY {get; private set;} // Y축 움직임
    public Vector2 MousePosition {get; private set;}
    public bool Fire {get; private set;}
    // public bool Dash {get; private set;} // SpaceBar 로 대시
    // public bool Explode {get; private set;} // Q로 스킬 사용

    // Update is called once per frame
    void Update()
    {
        MoveX = Input.GetAxisRaw(XAxisMove);
        MoveY = Input.GetAxisRaw(YAxisMove);
        MousePosition = Input.mousePosition;
        Fire = Input.GetButton(FireButton);
        // Dash = Input.GetKeyDown(KeyCode.Space);
        // Explode = Input.GetKeyDown(KeyCode.Q);
    }
}
