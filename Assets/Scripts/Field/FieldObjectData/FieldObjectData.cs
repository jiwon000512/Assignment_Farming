using System;
using UnityEngine;

/// <summary>
/// 월드맵에 배치되는 오브젝트의 데이터 클래스
/// 해당 데이터를 저장/로드
/// </summary>
[Serializable]
public class FieldObjectData
{
    public string idx;
    public SerializableVector3 position;
    public FieldObjectInfo info;
   
    public FieldObjectData(string idx, FieldObjectInfo info, Vector3 position)
    {
        this.idx = idx; 
        this.position = new SerializableVector3(position);
        this.info = info;
    }
}
