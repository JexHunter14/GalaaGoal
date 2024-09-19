using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabSignUp : MonoBehaviour
{
   [SerializeField] GameObject signUpTab, logInTab;
   [SerializeField] TMP_InputField username, userEmail, UserPassword, userEmailLogin, UserPasswordLogin;
   string encrytedPassword;

   public void Register(){
     var registerRequest = new RegisterPlayFabUserRequest{Username = username.text, Email = userEmail.text, Password = Encrypt(UserPassword.text)};
     PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterRequestSuccess, OnRegisterFailure);
   }

   void OnRegisterRequestSuccess(RegisterPlayFabUserResult result){
     Debug.Log("User Registered");
   }

   void OnRegisterFailure(PlayFabError error){
     Debug.Log($"User Registeration Failed: {error.ErrorMessage }");
   }

   public void Login(){
     var request = new LoginWithEmailAddressRequest {Email = userEmailLogin.text, Password = Encrypt(UserPasswordLogin.text)};
     PlayFabClientAPI.LoginWithEmailAddress(request, LoginSuccess, LoginFailure);
   }

   public void LoginSuccess(LoginResult result){
      Debug.Log("Signed In");
   }

   public void LoginFailure(PlayFabError error){
     Debug.LogError(error.GenerateErrorReport());
   }

   public void SignUpTab(){
     signUpTab.SetActive(true);
     logInTab.SetActive(false);
   }

   public void LogInTab(){
     logInTab.SetActive(true);
     signUpTab.SetActive(false);
   }

   string Encrypt(string pw){
     System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
     byte[] epw = System.Text.Encoding.UTF8.GetBytes(pw);
     epw = x.ComputeHash(epw);
     System.Text.StringBuilder s = new System.Text.StringBuilder();
     foreach(byte b in epw){
       s.Append(b.ToString("x2").ToLower());
     }
     return s.ToString();
   }
};
