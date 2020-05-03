using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputHandler : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {
    MoveCommand moveCommand;
    AttackCommand attackCommand;
    
    public void Init(MoveCommand move, AttackCommand attack) {
        moveCommand = move;
        attackCommand = attack;
    }
 
    [SerializeField] RectTransform joystickBg;
    [SerializeField] RectTransform joystick;
    
    float m_fRadius;
 
    Vector2 m_vecMove;
    bool m_bTouch = false;
 
    void Start() {
        m_fRadius = joystickBg.rect.width * 0.5f;
    }
 
    void OnTouch(Vector2 vecTouch) {
        Vector2 vec = new Vector2(vecTouch.x - joystickBg.position.x, vecTouch.y - joystickBg.position.y);

        // vec값을 m_fRadius 이상이 되지 않도록 합니다.
        vec = Vector2.ClampMagnitude(vec, m_fRadius);
        joystick.localPosition = vec;
 
        // 조이스틱 배경과 조이스틱과의 거리 비율로 이동합니다.
        float fSqr = (joystickBg.position - joystick.position).sqrMagnitude / (m_fRadius * m_fRadius);
 
        // 터치위치 정규화
        Vector2 vecNormal = vec.normalized;
 
        m_vecMove = new Vector2(vecNormal.x * 3 * Time.deltaTime * fSqr, vecNormal.y * 3 * Time.deltaTime * fSqr);
        // player.transform.Translate(m_vecMove * Time.deltaTime);
    }
 
    public void OnDrag(PointerEventData eventData) {
        moveCommand.Execute(eventData.delta);
        // OnTouch(eventData.position);
    }
 
    public void OnPointerDown(PointerEventData eventData) {
        joystickBg.position = eventData.position;
        // OnTouch(eventData.position);
    }
 
    public void OnPointerUp(PointerEventData eventData) {
        joystick.localPosition = Vector2.zero;
    }
}