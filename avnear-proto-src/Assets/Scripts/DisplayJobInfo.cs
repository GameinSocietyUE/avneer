using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayJobInfo : Displayable
{
    public static DisplayJobInfo Instance;

    private void Awake() {
        Instance = this;
    }

    public GameObject employement_Obj;
    public GameObject salary_Obj;
    public GameObject save_Obj;
    public Vector2Int duration;

    public void Display() {

    }

}
