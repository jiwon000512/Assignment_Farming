using UnityEngine;

/// <summary>
/// ����ʿ� ��ġ�Ǵ� ������Ʈ
/// </summary>
public class FieldObject : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected UI_FieldObject ui;

    public virtual void Init(FieldObjectData data)
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        ui = gameObject.GetComponent<UI_FieldObject>();

        transform.position = data.position.ToVector3();
        LoadSprite(data.info.spritePath);
        gameObject.SetActive(true);

        ui.Init();
    }

    public void LoadSprite(string spriteName)
    {
        AddressableManager.Instance.LoadAsset<Sprite>(Define.spritePath + spriteName + ".png", LoadComplete);
    }

    protected virtual void LoadComplete(Sprite loaded)
    {
        spriteRenderer.sprite = loaded;
    }

    public virtual void Select(bool bSelect)
    {
        ui.Select(bSelect);
    }
}
