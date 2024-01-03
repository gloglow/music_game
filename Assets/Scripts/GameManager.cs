using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public AudioManager audioManager;
    public JudgeLine judgeLine;
    public Hit hit;

    private void Start()
    {
        StartUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit))
            {
                if (rayHit.collider.name == "TouchArea")
                {
                    Vector3 dirVec = ((judgeLine.startPos + judgeLine.endPos) * 0.5f) - rayHit.point;
                    dirVec = Vector3.Normalize(dirVec);

                    RaycastHit ray2Hit;
                    int layerMask = (1 << 7);

                    if (Physics.Raycast(rayHit.point, dirVec, out ray2Hit, 100f, layerMask) || Physics.Raycast(rayHit.point, -dirVec, out ray2Hit, 100f, layerMask))
                    {
                        hit.hitting(ray2Hit.point);
                    }
                }
            }
        }
    }

    private void StartUI()
    {
        Invoke("StartUIFinish", 3f);
    }

    private void StartUIFinish()
    {
        //audioManager.MusicPlay();
    }
}
