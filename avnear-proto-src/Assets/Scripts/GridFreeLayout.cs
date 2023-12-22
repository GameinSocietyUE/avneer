using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridFreeLayout : MonoBehaviour
{
    float width = 1f;
    public DisplayFormationInfo prefab;
    private List<DisplayFormationInfo> displayFormationInfos = new List<DisplayFormationInfo>();
    public float spacing_X;
    public float spacing_Y;
    public float height = 0f;
    public RectTransform rectTransform;

    // Update is called once per frame
    public void DisplayInfos(List<string> infos, out float newHeight) {
        foreach (var dis in displayFormationInfos)
            dis.Hide();

        for (int i = 0; i < infos.Count; i++) {
            if ( i <= displayFormationInfos.Count)
                displayFormationInfos.Add(Instantiate(prefab, rectTransform));

            displayFormationInfos[i].Show();
            displayFormationInfos[i].GetComponentInChildren<TextMeshProUGUI>().text = infos[i];
        }

        Canvas.ForceUpdateCanvases();

        width = rectTransform.rect.width;
        float x = 0f;
        float y = 0f;
        int lCount = 1;
        float h = displayFormationInfos[0].GetComponent<RectTransform>().rect.height;
        for (int i = 0; i < displayFormationInfos.Count; i++) {
            var rT = displayFormationInfos[i].GetComponent<RectTransform>();
            if (x + rT.rect.width>=width) {
                x = 0f;
                y -= h + spacing_Y;
                lCount ++;
            }
            rT.anchoredPosition = new Vector2(x, y);
            x += rT.rect.width + spacing_X;
        }

        height = (h+spacing_Y)*lCount;
        newHeight= (h+spacing_Y) * (lCount-1);
        rectTransform.sizeDelta = new Vector2(width, height);
    }
}
