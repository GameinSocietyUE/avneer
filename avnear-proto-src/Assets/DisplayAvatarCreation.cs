using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayAvatarCreation : Displayable
{
    public enum Look {
        Hair,
        Face,
        Skin
    }

    public enum Genre {
        Female,
        Male
    }

    public static DisplayAvatarCreation Instance;

    private void Awake() {
        Instance = this;
    }

    public void ChangeLook(Look lookType, int lookId) {
        DisplayMessage.Instance.Display($"Changing look\n{lookType} / {lookId}");
    }

    public void ChangeGenre(int i) {
        DisplayMessage.Instance.Display($"Change genre to : {(Genre)i}");
    }
}
