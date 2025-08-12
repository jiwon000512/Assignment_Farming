using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 월드맵에 배치되는 오브젝트의 ui
/// </summary>
public class UI_FieldObject : MonoBehaviour
{
    [SerializeField] Text stateText;

    public void Init()
    {
        if (stateText == null || Camera.main == null) return;

        Vector3 worldPosition = transform.position;
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);

        stateText.rectTransform.position = screenPosition;
        stateText.gameObject.SetActive(false);
    }

    public virtual void Select(bool bSelect)
    {
        stateText.gameObject.SetActive(bSelect);
    }

    public virtual void SetText(string str)
    {
        stateText.text = str;
    }
}
