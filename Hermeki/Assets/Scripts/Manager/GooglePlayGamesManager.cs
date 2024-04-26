using GooglePlayGames;
using TMPro;
using UnityEngine;

public class GooglePlayGamesManager : MonoBehaviour
{
    public TMP_Text Google_Text;

    // Start is called before the first frame update
    void Start()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        LogIn();
    }

    //void OnLogin()
    //{
    //    /* Get login try result status. If user refused the sign-in already, not doing sign-in process */
    //    if (GetGoogleLoginResult() == false)
    //    {
    //        return;
    //    }

    //    // Initialize login
    //    InitializeSocialLogin();

    //    // If you are not signed in, sign in
    //    if (!IsSociallocalUserauthenticated())
    //    {
    //        Social.localUser.Authenticate((bool bSuccess, string message) =>
    //        {
    //            // If the trying signin is success,
    //            if (bSuccess)
    //            {
    //                /* Save login success status */
    //            }
    //            // If the trying signin is failed,
    //            else
    //            {
    //                // If user canceled google play sign-in
    //                if (message.Equals("User Canceled"))
    //                {
    //                    /* Save login refused status */
    //                    StatisticsData.SetGoogleLoginFailed();
    //                }
    //            }
    //        });
    //    }
    //}
    public void LogIn()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated == false)
        {
            Social.localUser.Authenticate((bool successed) =>
            {
                if (successed)
                    Google_Text.text = string.Format($"{Social.localUser.id} \n {Social.localUser.userName}");
                else
                    Google_Text.text = "Login Failed";
            });
        }
    }

    public void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Google_Text.text = "Logout";
    }
}
