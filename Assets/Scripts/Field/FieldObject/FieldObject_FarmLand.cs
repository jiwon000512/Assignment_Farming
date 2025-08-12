using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// ����ʿ� ��ġ�Ǵ� ��
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
    /// ��� ����
    /// </summary>
    /// <param name="cropInfo">����� �۹� ���̺� ����</param>
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
    /// ���� ���º� ó��
    /// </summary>
    /// <param name="bSelect"></param>
    public override void Select(bool bSelect)
    {
        base.Select(bSelect);

        switch(data.state)
        {
            case FarmLandState.Idle:
                {
                    ui.SetText("��� ����");
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
    /// ���� ���º� sprite�� �ε��մϴ�.
    /// </summary>
    /// <param name="state">���� ����</param>
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
    /// ������� ���, 0.5�� �������� ���� ���¸� üũ�մϴ�.
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
                //�����
                data.state = FarmLandState.Farming;
                ui.SetText($"{data.crop.name} ��� �Ϸ���� : {(int)(data.crop.farmingTime - remainTime)}");
            }
            else
            {
                //��糡
                data.state = FarmLandState.Finish;
                ui.SetText("��� �Ϸ�");
            }
            LoadStateSprite(data.state);
            yield return delay;
        }
    }
}
