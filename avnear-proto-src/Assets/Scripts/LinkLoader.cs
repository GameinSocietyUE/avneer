using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LinkLoader : TextParser {
    public static List<LinkData> datas = new List<LinkData>();
    public struct LinkData {
        public string buttonName;
        public string title;
        public string description;
        public string url;
    }
    public override void ParseCSV() {
        datas.Clear();
        base.ParseCSV();
    }

    public override void GetCell(int row_index, List<string> cells) {
        base.GetCell(row_index, cells);
        if (row_index == 0 || cells.Count < 1) return;
        LinkData data = new LinkData();
        data.buttonName = cells[0];
        data.title = cells[1];
        data.description = cells[2];
        Debug.Log($"{cells[0]} : {cells.Count}");
        data.url = cells[3];
        datas.Add(data);
    }
}
