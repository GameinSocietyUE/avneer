using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplaySaves : Displayable
{
    public DisplaySavedJob prefab;
    public Transform parent;
    public List<DisplaySavedJob> DisplaySavedJobs = new List<DisplaySavedJob>();
    public void Display(List<string> jobs) {
        FadeIn();

        foreach (var d in DisplaySavedJobs)
            d.Hide();

        for (int i = 0; i < jobs.Count; i++) {
            if ( i <= DisplaySavedJobs.Count )
                DisplaySavedJobs.Add(Instantiate(prefab, parent));
            DisplaySavedJobs[i].Display(jobs[i]);
        }
    }
}
