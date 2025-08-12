using System;
using UnityEngine;

/// <summary>
/// ����ʿ� ��ġ�Ǵ� ������Ʈ�� ������ Ŭ����
/// �ش� �����͸� ����/�ε�
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
