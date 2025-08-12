using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ������ ���۹� ���̺� Ŭ����
/// </summary>
[System.Serializable]
public class CropsInfo
{
    public int no;
    public int fieldObjectNo;
    public string name;
    public string growingSprite;
    public string finishSprite;
    public float farmingTime; //��

    public CropsInfo(int no, int fieldObjectNo, string name, string growingSprite, string finishSprite, float farmingTime)
    {
        this.no = no;
        this.fieldObjectNo = fieldObjectNo;
        this.name = name;
        this.growingSprite = growingSprite;
        this.finishSprite = finishSprite;
        this.farmingTime = farmingTime;
    }   
}

/// <summary>
/// ���̺� �ε� ��� ���� �����͸� ���� �ʱ�ȭ �߽��ϴ�.
/// </summary>
public class CropsTable
{
    Dictionary<int, CropsInfo> infoDic = new Dictionary<int, CropsInfo>()
    {
        {1, new CropsInfo(1, 2, "���", "carrot_growing", "carrot_complete", 5) },
        {2, new CropsInfo(2, 3, "��", "wheat_growing", "wheat_complete", 3)  }
    };

    public CropsInfo GetData(int no)
    {
        infoDic.TryGetValue(no, out CropsInfo info);
        return info;
    }

    public CropsInfo GetDataByFieldObjectNo(int objectNo)
    {
        foreach (var v in infoDic.Values)
        {
            if(v.fieldObjectNo == objectNo)
                return v;
        }

        return null;
    }
}
