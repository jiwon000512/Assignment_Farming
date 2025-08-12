using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 오브젝트 관리
/// </summary>
public class FieldManager : Singleton<FieldManager>
{
    public Dictionary<string, FieldObjectData> fieldObjectDatas = new Dictionary<string, FieldObjectData>();
    public Dictionary<string, FieldObject> objects = new Dictionary<string, FieldObject>();
    GameObject field;
    FieldObject selectedObject = null;
    ObjectPlacer objectPlacer = new ObjectPlacer();

    /// <summary>
    /// 데이터 로드, 저장됐던 오브젝트 배치
    /// </summary>
    /// <param name="field"></param>
    public void Init(GameObject field)
    {
        this.field = field;
        var saveDataSet = SaveManager.Load<FieldObjectDataSet>();
        fieldObjectDatas.Clear();
        if (saveDataSet != null)
        {
            saveDataSet.datas.ForEach(x => fieldObjectDatas.Add(x.idx, x));
        }

        foreach (var v in fieldObjectDatas)
        {
            CreateObject(v.Value);
        }

        objectPlacer.Init(PlaceCheck, PlaceObject);
    }

    private void Update()
    {
        objectPlacer?.Update();

        //배치된 오브젝트 선택 기능
        if(objectPlacer?.isPlacing == false && Input.GetMouseButtonUp(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, LayerMask.GetMask("FieldObject"));

            if (hit.collider != null)
            {
                FieldObject ob = hit.collider.GetComponent<FieldObject>();
                if (ob != null && ob != selectedObject)
                {
                    selectedObject?.Select(false);
                    selectedObject = ob;
                    selectedObject.Select(true);
                    return;
                }
            }
            selectedObject?.Select(false);
            selectedObject = null;
        }
    }

    private void OnDestroy()
    {
        SaveFieldData();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) SaveFieldData();
    }

    void SaveFieldData()
    {
        FieldObjectDataSet dataSet = new FieldObjectDataSet();
        dataSet.datas = fieldObjectDatas.Values.ToList();
        SaveManager.Save<FieldObjectDataSet>(dataSet);
    }

    public void StartPlacing(FieldObjectInfo info)
    {
        objectPlacer.StartPlacing(info);
    }

    public void EndPlacing()
    {
        objectPlacer.EndPlacing();
    }

    /// <summary>
    /// 배치 가능한지 체크
    /// </summary>
    /// <param name="info">배치할 오브젝트 테이블 정보</param>
    /// <param name="checkPos">배치할 위치</param>
    /// <returns></returns>
    private bool PlaceCheck(FieldObjectInfo info, Vector3 checkPos)
    {
        switch (info.type)
        {
            case FieldObjectType.FarmLand:
                {
                    foreach (var v in fieldObjectDatas)
                    {
                        if (v.Value.position.ToVector3() == checkPos)
                            return false;
                    }
                    return true;
                }
            case FieldObjectType.Crop:
                {
                    foreach (var v in fieldObjectDatas)
                    {
                        if (v.Value.position.x == checkPos.x && v.Value.position.y == checkPos.y && v.Value.info.type == FieldObjectType.FarmLand)
                        {
                            if ((objects[v.Key] as FieldObject_FarmLand).isFarming == false)
                                return true;
                        }


                    }
                    return false;
                }
        }

        return false;
    }

    /// <summary>
    /// 오브젝트 배치 콜백
    /// </summary>
    /// <param name="info">배치할 오브젝트 테이블 정보</param>
    /// <param name="pos">배치할 위치</param>
    private void PlaceObject(FieldObjectInfo info, Vector3 pos)
    {
        switch (info.type)
        {
            case FieldObjectType.FarmLand:
                {
                    FieldObjectData_FarmLand data = new FieldObjectData_FarmLand(GetIdx().ToString(), info, pos);
                    CreateObject(data);
                }
                break;
            case FieldObjectType.Crop:
                {
                    string targetFarmLandIdx = "";
                    foreach (var v in fieldObjectDatas)
                    {
                        if (v.Value.position.ToVector3() == pos)
                        {
                            targetFarmLandIdx = v.Key;
                            break;
                        }
                    }

                    FieldObject_FarmLand targetFarmLand = objects[targetFarmLandIdx] as FieldObject_FarmLand;
                    targetFarmLand.StartFarming(TableData.cropsTable.GetDataByFieldObjectNo(info.no));
                }
                break;
        }
    }

    /// <summary>
    /// 오브젝트 생성
    /// </summary>
    /// <param name="data">생성할 오브젝트의 데이터</param>
    private async void CreateObject(FieldObjectData data)
    {
        var ob = await AddressableManager.Instance.InstantiateAsync(Define.fieldObjectPath);
        ob.name = data.idx;

        switch (data.info.type)
        {
            case FieldObjectType.FarmLand:
                {
                    ob.AddComponent<FieldObject_FarmLand>();
                }
                break;
            default:
                {
                    ob.AddComponent<FieldObject>();
                }
                break;
        }

        FieldObject fieldObject = ob.GetComponent<FieldObject>();
        fieldObject.transform.SetParent(field.transform);
        fieldObject.Init(data);

        if (fieldObjectDatas.ContainsKey(data.idx) == false)
        {
            fieldObjectDatas.Add(data.idx, data);
        }
        if(objects.ContainsKey(data.idx) == false)
        {
            objects.Add(data.idx, fieldObject);
        }
    }

    int GetIdx()
    {
        return fieldObjectDatas.Count + 1;
    }
}
