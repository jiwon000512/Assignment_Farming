using System;
using UnityEngine;

public enum FarmLandState
{
    Idle,       //��� ����
    Farming,    //�����
    Finish,     //��� �Ϸ�
}

/// <summary>
/// �߰� �����Ͱ� �ʿ��� ����� ������ Ŭ�����Դϴ�.
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
