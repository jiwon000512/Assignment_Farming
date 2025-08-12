using System.Collections.Generic;
using UnityEngine;

public enum FieldObjectType
{
    None,
    FarmLand,
    Crop,
}

/// <summary>
/// ����ʿ� ��ġ�Ǵ� ������Ʈ ���̺� Ŭ����
/// </summary>
[System.Serializable]
public class FieldObjectInfo
{
    public int no;
    public FieldObjectType type;
    public string spritePath;

    public FieldObjectInfo(int no, int type, string spritePath)
    {
        this.no = no;
        this.type = (FieldObjectType)type;
        this.spritePath = spritePath;
    }
}


/// <summary>
/// ���̺� �ε� ��� ���� �����͸� ���� �ʱ�ȭ �߽��ϴ�.
/// </summary>
public class FieldObjectTable
{
    Dictionary<int, FieldObjectInfo> infoDic = new Dictionary<int, FieldObjectInfo>()
    {
        {1, new FieldObjectInfo(1, 1, "field_empty") },
        {2, new FieldObjectInfo(2, 2, "carrot")},
        {3, new FieldObjectInfo(3, 2, "wheat")}
    };

    public FieldObjectInfo GetData(int no)
    {
        infoDic.TryGetValue(no, out var result);
        return result;
    }
}
