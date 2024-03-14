using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitManager : MonoBehaviour
{
    // インプットを管理：インプットがtouch area内で行われたかを確認し、コライダーとエフェクターを管理

    [SerializeField] private GameObject prefab_hitCollider;
    [SerializeField] private ParticleSystem prefab_hitEffect;

    [SerializeField] private StageManager stageManager;

    [SerializeField] private HitCollider[] hitColliders;
    [SerializeField] private ParticleSystem[] hitEffects;

    [SerializeField] private JudgeLine judgeLine;
    [SerializeField] private TouchArea touchArea;

    [SerializeField] private int hitCnt; // インプットの最大数。プレイの際、ユーザは2~4個の指を使うため、4で設定。

    private void Start()
    {
        hitColliders = new HitCollider[hitCnt];
        hitEffects = new ParticleSystem[hitCnt];

        MakeHitCollidersAndEffects();
    }

    private void Update()
    {
        if (!stageManager.isPause)
        {
            // PC. 
            if (Input.GetMouseButtonDown(0))
            {
                // マウスクリックがあれば、そのインプットがtouch area内で行われたか確認
                Vector3 point = isinTouchArea(Input.mousePosition);

                if (point != new Vector3(999, 999, 999))
                {
                    // touch area内ならば、その位置を判定線上の位置に変換
                    point = DecideHitPos(point);
                }

                // マウスクリックは同時に一つの位置でだけ行われるため、一つのhitを発生。
                ActivateHit(0, point);
            }

            if (Input.GetMouseButtonUp(0))
            {
                // マウスクリックが終わればコライダーを非活性化
                hitColliders[0].gameObject.SetActive(false);
            }

            //　モバイル
            if (Input.touchCount > 0)
            {
                // モバイルではインプットが同時に一つ以上の位置で行われる。
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch tmpTouch = Input.GetTouch(i);
                    Vector3 point = isinTouchArea(tmpTouch);

                    if (point != new Vector3(999, 999, 999))
                    {
                        point = DecideHitPos(point);
                    }

                    //　タッチが始めればhitを発生。
                    if (tmpTouch.phase == TouchPhase.Began)
                    {
                        ActivateHit(i, point);
                    }
                    // タッチ中、hitを移動させる
                    else if (tmpTouch.phase == TouchPhase.Moved || tmpTouch.phase == TouchPhase.Stationary)
                    {
                        hitColliders[i].transform.position = point;
                    }
                    // タッチが終わればコライダーを非活性化
                    else
                    {
                        hitColliders[i].gameObject.SetActive(false);
                    }
                }
            }
        }
        
    }

    private void MakeHitCollidersAndEffects()
    {
        for (int i = 0; i < hitCnt; i++)
        {
            HitCollider hitCollider = Instantiate(prefab_hitCollider).GetComponent<HitCollider>();
            ParticleSystem hitEffect = Instantiate(prefab_hitEffect).GetComponent<ParticleSystem>();

            hitCollider.stageManager = stageManager;

            hitCollider.gameObject.SetActive(false);
            hitEffect.gameObject.SetActive(false);

            hitCollider.transform.SetParent(transform);
            hitEffect.transform.SetParent(transform);

            hitColliders[i] = hitCollider;
            hitEffects[i] = hitEffect;
        }
    }

    private Vector3 isinTouchArea(Vector3 mousepos)
    {
        // インプットがtouch area内で行われたかを確認

        Ray ray = Camera.main.ScreenPointToRay(mousepos);
        RaycastHit rayHit;
        int layerMask = (1 << 9); // touch areaのレイヤー
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, layerMask))
        {
            // touch area内なら、その位置を返還
            return rayHit.point;
        }
        else 
        { 
            // じゃなければ意味のないベクトルを返還
            return new Vector3(999, 999, 999); 
        }
    }

    private Vector3 isinTouchArea(Touch touch) // モバイル
    {
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
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

    private Vector3 DecideHitPos(Vector3 point)
    {
        // touch areaは判定線より広いため、インプットの位置を判定線上の位置に変換

        Vector3 dirVec = judgeLine.lineCenterPos - point;
        dirVec = Vector3.Normalize(dirVec);

        RaycastHit rayHit;
        int layerMask = (1 << 7); // 判定線のレイヤー

        if (Physics.Raycast(point, dirVec, out rayHit, Mathf.Infinity, layerMask) 
            || Physics.Raycast(point, -dirVec, out rayHit, Mathf.Infinity, layerMask))
        {
            // インプットの位置から判定線に向かってrayをし、rayに打たれた位置を返還
            return rayHit.point;
        }
        else
        {
            // じゃなければ意味のないベクトルを返還
            return new Vector3(999, 999, 999);
        }
    }

    private void ActivateHit(int index, Vector3 pos)
    {
        //　インプットがあればコライダーとエフェクターを活性化
        hitColliders[index].gameObject.SetActive(true);
        hitEffects[index].gameObject.SetActive(true);
        hitColliders[index].transform.position = pos;
        hitEffects[index].transform.position = pos;
        hitEffects[index].Play();
    }
}
