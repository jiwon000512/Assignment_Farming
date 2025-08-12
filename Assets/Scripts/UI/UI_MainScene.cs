using UnityEngine;

public class UI_MainScene : MonoBehaviour
{
    [SerializeField] GameObject PlaceEndButton;

    private void Start()
    {
        PlaceEndButton.SetActive(false);
    }

    public void OnPlace(int fieldObjectTableNo)
    {
        FieldManager.Instance.StartPlacing(TableData.fieldObjectTable.GetData(fieldObjectTableNo));
        PlaceEndButton.SetActive(true);
    }

    public void OnPlaceEnd()
    {
        FieldManager.Instance.EndPlacing();
        PlaceEndButton.SetActive(false);
    }
}
