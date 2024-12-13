using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Sector : MonoBehaviour
{
    private PrizeData data;
    public Image icon;
    public TMP_Text quantity;

    public void SetSector(PrizeData data, Sprite icon, int quantity)
    {
        this.data = data;
        this.icon.sprite = icon;
        this.quantity.text = (data.Type == PrizeType.Life) ? quantity.ToString() + "mins" : quantity.ToString() + "X";
    }
}
