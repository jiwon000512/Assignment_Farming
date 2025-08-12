using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;

/// <summary>
/// FieldObjectTable 값을 이용해 오브젝트를 배치
/// </summary>
public class ObjectPlacer
{
    public bool isPlacing = false;

    GameObject placeObject;             //배치할 더미 오브젝트
    FieldObjectInfo placeObjectInfo;    //배치할 오브젝트 정보    
    Func<FieldObjectInfo, Vector3, bool> checkFunc;     //오브젝트 배치가 가능한지 체크하는 함수
    Action<FieldObjectInfo, Vector3> placeCallback;     //오브젝트 배치 콜백

    public void Update()
    {
        if (!isPlacing) return;

        //ui입력 시 무효
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, 1 << LayerMask.NameToLayer("Ground")))
        {
            Vector3 snappedPos = SnapToGrid(hit.point);
            placeObject.transform.position = snappedPos;

            if (Input.GetMouseButtonUp(0))
            {
                if(isPlacing && checkFunc(placeObjectInfo, placeObject.transform.position))
                {
                    placeCallback(placeObjectInfo, new Vector3(snappedPos.x, snappedPos.y, -1));
                }
            }
        }
    }
    
    public void Init(Func<FieldObjectInfo, Vector3, bool> checkFunc, Action<FieldObjectInfo, Vector3> placeCallback)
    {
        this.checkFunc = checkFunc;
        this.placeCallback = placeCallback;
        isPlacing = false;
    }

    public async void StartPlacing(FieldObjectInfo placeObjectInfo)
    {
        this.placeObjectInfo = placeObjectInfo;
        isPlacing = true;
        if (placeObject == null)
        {
            placeObject = new GameObject("dummy");
            placeObject.AddComponent<SpriteRenderer>();
        }
        placeObject.SetActive(true);
        var sprite = placeObject.GetComponent<SpriteRenderer>();
        sprite.sprite = await AddressableManager.Instance.LoadAssetAsync<Sprite>(Define.spritePath + placeObjectInfo.spritePath + ".png");
        sprite.color = new Color(1, 1, 1, 0.5f);
    }

    public void EndPlacing()
    {
        isPlacing = false;
        placeObject.gameObject.SetActive(false);
    }

    /// <summary>
    /// 보정 좌표 리턴
    /// </summary>
    /// <param name="pos">보정 전 월드 좌표</param>
    /// <returns></returns>
    private Vector3 SnapToGrid(Vector3 pos)
    {
        float cellWidth = Define.testSize.x;
        float cellHeight = Define.testSize.y;

        float isoX = pos.x / (cellWidth / 2f);
        float isoY = pos.y / (cellHeight / 2f);

        float gridX = (isoY + isoX) / 2f;
        float gridY = (isoY - isoX) / 2f;

        gridX = Mathf.Round(gridX);
        gridY = Mathf.Round(gridY);

        float worldX = (gridX - gridY) * (cellWidth / 2f);
        float worldY = (gridX + gridY) * (cellHeight / 2f);

        return new Vector3(worldX, worldY, -5);
    }
}

