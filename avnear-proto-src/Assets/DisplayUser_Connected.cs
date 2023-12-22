using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayUser_Connected : Displayable
{
    public static DisplayUser_Connected instance;

    public DisplaySaves displaySaves;

    private void Awake() {
        instance = this;
    }

    public void SetSavedJobs(List<string> jobs) {
        displaySaves.Display(jobs);
    }

    public void Display() {
        FadeIn();
        SetSavedJobs(new List<string>() { "test 1", "test 2", "test 3" });
    }
}
