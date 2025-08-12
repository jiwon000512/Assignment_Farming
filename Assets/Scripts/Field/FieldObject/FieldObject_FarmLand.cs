using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 월드맵에 배치되는 논
/// </summary>
[Serializable]
public class FieldObject_FarmLand : FieldObject
{
    public bool isFarming => (data.state == FarmLandState.Farming || data.state == FarmLandState.Finish);
    FieldObjectData_FarmLand data;
    FarmLandState currentLoadedState = FarmLandState.Idle;

    public override void Init(FieldObjectData data)
    {
        base.Init(data);
        this.data = data as FieldObjectData_FarmLand;

        StartCoroutine(CheckState());
    }

    protected override void LoadComplete(Sprite loaded)
    {
        if (GetSpriteName(currentLoadedState).Contains(loaded.name) == false) return;

        base.LoadComplete(loaded);
    }
    
    /// <summary>
    /// 농사 시작
    /// </summary>
    /// <param name="cropInfo">농사할 작물 테이블 정보</param>
    public void StartFarming(CropsInfo cropInfo)
    {
        if (cropInfo == null) return;

        data.state = FarmLandState.Farming;
        data.crop = cropInfo;
        data.farmStartTime = DateTime.Now;
        StopAllCoroutines();
        StartCoroutine(CheckState());
    }

    /// <summary>
    /// 논의 상태별 처리
    /// </summary>
    /// <param name="bSelect"></param>
    public override void Select(bool bSelect)
    {
        base.Select(bSelect);

        switch(data.state)
        {
            case FarmLandState.Idle:
                {
                    ui.SetText("농사 가능");
                }
                break;
            case FarmLandState.Farming:
                {

                }
                break;
            case FarmLandState.Finish:
                {
                    data.state = FarmLandState.Idle;
                    LoadStateSprite(FarmLandState.Idle);
                }
                break;
        }
    }

    /// <summary>
    /// 논의 상태별 sprite를 로드합니다.
    /// </summary>
    /// <param name="state">논의 상태</param>
    private void LoadStateSprite(FarmLandState state)
    {
        if(currentLoadedState == state) return;

        string spriteName = GetSpriteName(state);
        LoadSprite(spriteName);

        currentLoadedState = state;
    }

    public string GetSpriteName(FarmLandState state)
    {
        switch (state)
        {
            case FarmLandState.Idle:
                {
                    return data.info.spritePath;
                }
            case FarmLandState.Farming:
                {
                    return data.crop.growingSprite;
                }
            case FarmLandState.Finish:
                {
                    return data.crop.finishSprite;
                }
        }

        return "";
    }

    /// <summary>
    /// 농사중인 경우, 0.5초 간격으로 논의 상태를 체크합니다.
    /// </summary>
    /// <returns></returns>
    IEnumerator CheckState()
    {
        if (data.state == FarmLandState.Idle) yield break;

        double remainTime = 0;
        WaitForSeconds delay = new WaitForSeconds(0.5f);
        while (isFarming)
        {
            remainTime = (DateTime.Now - data.farmStartTime).TotalSeconds;

            if(remainTime < data.crop.farmingTime)
            {
                //농사중
                data.state = FarmLandState.Farming;
                ui.SetText($"{data.crop.name} 농사 완료까지 : {(int)(data.crop.farmingTime - remainTime)}");
            }
            else
            {
                //농사끝
                data.state = FarmLandState.Finish;
                ui.SetText("농사 완료");
            }
            LoadStateSprite(data.state);
            yield return delay;
        }
    }
}
