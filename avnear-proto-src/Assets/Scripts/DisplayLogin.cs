using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayLogin : Displayable {
    public static DisplayLogin Instance;

    public TMP_InputField userNameField;
    public TMP_InputField passwordField;
    public TMP_InputField passwordConfirmationField;

    private void Awake() {
        Instance = this;
    }

    public string GetUserName() {
        return userNameField.text;
    }

    public string GetPassword() {
        return passwordField.text;
    }

    public string GetPasswordConfirmation() {
        return passwordConfirmationField.text;
    }

}
