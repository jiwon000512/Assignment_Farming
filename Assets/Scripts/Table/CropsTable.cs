using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 농사 가능한 농작물 테이블 클래스
/// </summary>
[System.Serializable]
public class CropsInfo
{
    public int no;
    public int fieldObjectNo;
    public string name;
    public string growingSprite;
    public string finishSprite;
    public float farmingTime; //초

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
/// 테이블 로드 대신 더미 데이터를 만들어서 초기화 했습니다.
/// </summary>
public class CropsTable
{
    Dictionary<int, CropsInfo> infoDic = new Dictionary<int, CropsInfo>()
    {
        {1, new CropsInfo(1, 2, "당근", "carrot_growing", "carrot_complete", 5) },
        {2, new CropsInfo(2, 3, "밀", "wheat_growing", "wheat_complete", 3)  }
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
