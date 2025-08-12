using UnityEngine;

public class MainScene : MonoBehaviour
{
    [SerializeField] GameObject field;
    

    private void Start()
    {
        FieldManager.Instance.Init(field);
    }
}
