using System;
using UnityEngine;

public enum FarmLandState
{
    Idle,       //농사 가능
    Farming,    //농사중
    Finish,     //농사 완료
}

/// <summary>
/// 추가 데이터가 필요한 논밭의 데이터 클래스입니다.
/// </summary>
[Serializable]
public class FieldObjectData_FarmLand : FieldObjectData
{
    public DateTime farmStartTime;
    public CropsInfo crop;
    public FarmLandState state = FarmLandState.Idle;

    public FieldObjectData_FarmLand(string idx, FieldObjectInfo info, Vector3 position) : base(idx, info, position)
    {
    }
}
