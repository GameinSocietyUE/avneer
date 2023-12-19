using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CanvasManager : MonoBehaviour {
    public enum Page {
        Login,
        Chat,
        JobInfo,
        None,
    }
    [SerializeField] private Page currentPage = Page.None;
    [SerializeField] private Displayable[] pages;

    private void Start() {
        foreach (var page in pages)
            page.Hide();
        DisplayPage(currentPage);
    }

    public void DisplayPage(Page page) {
        HidePage(page);
        currentPage = page;
        GetPage(page).FadeIn();
    }

    private void HidePage(Page page) {
        if (page == Page.None)
            return;
        
        GetPage(currentPage).Hide();
    }


    public Displayable GetPage(Page page) {
        return pages[(int)page];
    }
    public Page CurrentPage { get => currentPage; set => currentPage = value; }
}
