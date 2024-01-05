using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class Hit : MonoBehaviour
{
    public GameObject prefab_hitCollider;
    public ParticleSystem prefab_hitEffect;
    public HitCollider[] hitColliders;
    public ParticleSystem[] hitEffects;

    public TouchArea touchArea;

    public int hitCnt;



    private void Start()
    {
        hitColliders = new HitCollider[hitCnt];
        hitEffects = new ParticleSystem[hitCnt];

        for (int i = 0; i < hitCnt; i++)
        {
            HitCollider hitCollider = Instantiate(prefab_hitCollider).GetComponent<HitCollider>();
            ParticleSystem hitEffect = Instantiate(prefab_hitEffect).GetComponent<ParticleSystem>();
            hitCollider.gameObject.SetActive(false);
            hitEffect.gameObject.SetActive(false);
            hitCollider.transform.SetParent(transform);
            hitEffect.transform.SetParent(transform);
            hitColliders[i] = hitCollider;
            hitEffects[i] = hitEffect;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 point = isinTouchArea(Input.mousePosition);
            if (point != new Vector3(999, 999, 999))
            {
                point = DecideColliderPos(point);
            }
            int index = 0;
            hitEffects[index].gameObject.SetActive(true);
            hitEffects[index].transform.position = point;
            hitEffects[index].Play();
            hitColliders[index].gameObject.SetActive(true);
            hitColliders[index].transform.position = point;
        }
        if(Input.GetMouseButtonUp(0))
        {
            hitColliders[0].gameObject.SetActive(false);
        }
        /*
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch tmpTouch = Input.GetTouch(i);
                Vector3 point  = isinTouchArea(tmpTouch);
                if (point != new Vector3(999,999,999))
                {
                    point = DecideColliderPos(point);
                }
                if (tmpTouch.phase == TouchPhase.Began)
                {
                    ActivateHit(i, point);
                }
                else if (tmpTouch.phase == TouchPhase.Moved || tmpTouch.phase == TouchPhase.Stationary)
                {
                    hitColliders[i].transform.position = point;
                }
                else
                {
                    hitColliders[i].gameObject.SetActive(false);
                }
            }
        }*/
    }

    private Vector3 isinTouchArea(Vector3 mousepos)//(Touch touch)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousepos);//touch.position);
        RaycastHit rayHit;
        int layerMask = (1 << 9);
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask))
        {
            return rayHit.point;
        }
        else 
        { 
            return new Vector3(999, 999, 999); 
        }
    }

    private Vector3 DecideColliderPos(Vector3 point)
    {
        Vector3 dirVec = ((GameManager.Instance.lineStartPos + GameManager.Instance.lineEndPos) * 0.5f) - point;
        dirVec = Vector3.Normalize(dirVec);

        RaycastHit rayHit;
        int layerMask = (1 << 7);

        if (Physics.Raycast(point, dirVec, out rayHit, Mathf.Infinity, layerMask) 
            || Physics.Raycast(point, -dirVec, out rayHit, Mathf.Infinity, layerMask))
        {
            return rayHit.point;
        }
        else
        {
            return new Vector3(999, 999, 999);
        }
    }

    private void ActivateHit(int index, Vector3 pos)
    {
        hitEffects[index].gameObject.SetActive(true);
        hitEffects[index].transform.position = pos;
        hitEffects[index].Play();
        hitColliders[index].gameObject.SetActive(true);
        hitColliders[index].transform.position = pos;
    }
}
