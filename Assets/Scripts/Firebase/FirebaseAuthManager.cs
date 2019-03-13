using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Assets.Scripts.Firebase.Database;
using Firebase.Auth;
using UnityEngine;

public class SignInData
{
    public bool IsSuccessful {get;set;}
    public string Message { get; set; }
}

public class FirebaseAuthManager : MonoBehaviour
{
    public static FirebaseAuthManager instance = null;

    protected Firebase.Auth.FirebaseAuth auth;
    //protected Firebase.Auth.FirebaseAuth otherAuth;
    protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
        new Dictionary<string, Firebase.Auth.FirebaseUser>();

    public string email = "";
    public string password = "";
    public string displayName = "";

    public UserInfo ActiveUserInfo { get; private set; } = null;

    // Flag set when a token is being fetched.  This is used to avoid printing the token
    // in IdTokenChanged() when the user presses the get token button.
    private bool fetchingToken = false;

    private Vector2 controlsScrollViewVector = Vector2.zero;
    private Vector2 scrollViewVector = Vector2.zero;
    bool UIEnabled = true;

    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

    // When the app starts, check to make sure that we have
    // the required dependencies to use Firebase, and if not,
    // add them if possible.
    public virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);

        dependencyStatus = Firebase.FirebaseApp.CheckDependencies();
        if (dependencyStatus != Firebase.DependencyStatus.Available)
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;

                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    instance = this;
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError(
                        "Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }
        else
        {
            instance = this;
            InitializeFirebase();
        }
        Debug.Log("Init auth");
    }

    protected void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        auth.IdTokenChanged += IdTokenChanged;

        AuthStateChanged(this, null);
    }

    private void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth.IdTokenChanged -= IdTokenChanged;
        auth = null;
    }

    void DisableUI()
    {
        UIEnabled = false;
    }

    void EnableUI()
    {
        UIEnabled = true;
    }

    public bool IsUIEnabled()
    {
        return UIEnabled;
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        Firebase.Auth.FirebaseUser user = null;

        if (senderAuth != null)
        {
            userByAuth.TryGetValue(senderAuth.App.Name, out user);
        }

        if (senderAuth == auth && senderAuth.CurrentUser != user)
        {
            bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }

            user = senderAuth.CurrentUser;
            userByAuth[senderAuth.App.Name] = user;

            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                displayName = user.DisplayName ?? "<user>";
                DisplayDetailedUserInfo(user);
            }
        }
    }

    // Track ID token changes.
    void IdTokenChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
        if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken)
        {
            senderAuth.CurrentUser.TokenAsync(false).ContinueWith(
                task => Debug.Log("Token[0:8] = " + task.Result.Substring(0, 8)));
        }
    }

    protected bool LogTaskCompletion(Task task, string operation)
    {
        bool complete = false;

        if (task.IsCanceled)
        {
            Debug.Log(operation + " canceled.");
        }
        else if (task.IsFaulted)
        {
            Debug.Log(operation + " encountered an error.");
            Debug.Log(task.Exception.ToString());
        }
        else if (task.IsCompleted)
        {
            Debug.Log(operation + " completed");
            complete = true;
        }

        return complete;
    }

    public Task<Firebase.Auth.FirebaseUser> CreateUser()
    {
        Debug.Log(String.Format("Attempting to create user {0}...", email));
        DisableUI();

        string newDisplayName = displayName;
        return auth.CreateUserWithEmailAndPasswordAsync(email, password)
            .ContinueWith((task) => ProcessUserRegistration(task, newDisplayName: newDisplayName)); ;
    }

    private Firebase.Auth.FirebaseUser ProcessUserRegistration(Task<Firebase.Auth.FirebaseUser> task, string newDisplayName = null)
    {
        EnableUI();

        // some callback functions here

        if (LogTaskCompletion(task, "User Creation"))
        {
            if (auth.CurrentUser != null)
            {
                Debug.Log(String.Format("User Info: {0} {1}", auth.CurrentUser.Email, auth.CurrentUser.UserId));
                UpdateUserProfile(newDisplayName: newDisplayName);
            }
        }

        return task.Result;
    }

    private void UpdateUserProfile(string newDisplayName = null)
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("Not signed in, unable to update user profile");
            return;
        }

        displayName = newDisplayName ?? displayName;
        Debug.Log("Updating user profile");
        DisableUI();
        auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile
        {
            DisplayName = displayName
        }).ContinueWith(ProcessUpdateUserProfile);
    }

    private void ProcessUpdateUserProfile(Task task)
    {
        EnableUI();
        if (LogTaskCompletion(task, "User profile"))
        {
            DisplayDetailedUserInfo(auth.CurrentUser);
        }
    }

    public Task<SignInData> SigninWithEmailAsync()
    {
        Debug.Log(String.Format("Attempting to sign in as {0}...", email));
        DisableUI();

        return auth.SignInWithEmailAndPasswordAsync(email, password)
            .ContinueWith(HandleSignIn)
            .Unwrap();
    }

    //public void SigninWithCredential()
    //{
    //    Debug.Log(String.Format("Attempting to sign in as {0}...", email));
    //    DisableUI();
    //    Firebase.Auth.Credential credential = Firebase.Auth.EmailAuthProvider.GetCredential(email, password);
    //    auth.SignInWithCredentialAsync(credential).ContinueWith(HandleSignIn);
    //}

    private async Task<SignInData> HandleSignIn(Task<Firebase.Auth.FirebaseUser> task)
    {
        password = "";
        Debug.Log("Handling");
        SignInData data = new SignInData();

        EnableUI();

        // some call backs when sign in is complete
        try
        {
            data.Message = task.Result.DisplayName == "" ? task.Result.Email : task.Result.DisplayName;
            data.IsSuccessful = true;

            //ActiveUserInfo = await FirebaseDatabaseManager.Instance.GetUserInfo(task.Result.UserId);
            ActiveUserInfo = await UserDatabase.GetUserInfo(task.Result.UserId);
        }
        catch (System.AggregateException e)
        {
            data.IsSuccessful = false;

            foreach (var exception in e.Flatten().InnerExceptions)
            {
                if (exception is Firebase.FirebaseException)
                {
                    Debug.LogWarning("Firebase: " + exception.Message);
                    data.Message = exception.Message;
                }
                else
                {
                    Debug.LogError(exception.Message);
                }
            }
        }

        LogTaskCompletion(task, "Sign-in");

        return data;
    }

    // is re-authenticate needed?

    public void ReloadUser()
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("Not signed in, unable to reload user.");
            return;
        }

        Debug.Log("Reload user data");
        auth.CurrentUser.ReloadAsync().ContinueWith(HandleReloadUser);
    }

    private void HandleReloadUser(Task task)
    {
        if (LogTaskCompletion(task, "Reload"))
        {
            DisplayDetailedUserInfo(auth.CurrentUser);
        }
    }

    public void GetUserToken()
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("Not signed in, unable to get token");
            return;
        }
        Debug.Log("Fetching user token");
        fetchingToken = true;

        auth.CurrentUser.TokenAsync(false).ContinueWith(HandleGetUserToken);
    }

    private void HandleGetUserToken(Task<string> task)
    {
        fetchingToken = false;
        if (LogTaskCompletion(task, "User fetch token"))
        {
            Debug.Log("Token = " + task.Result);
        }
    }

    public Firebase.Auth.FirebaseUser GetUserInfo()
    {
        if (auth.CurrentUser == null)
        {
            displayName = "Guest";
            Debug.Log("Not signed in, unable to get info");
            return null;
        }
        else
        {
            Debug.Log("Current user info");
            DisplayDetailedUserInfo(auth.CurrentUser);
            return auth.CurrentUser;
        }
    }

    public bool IsAuthenticated()
    {
        return auth.CurrentUser != null;
    }

    protected void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user)
    {
        DisplayUserInfo(user);
        Debug.Log("Email Verified: " + user.IsEmailVerified);
    }

    protected void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo)
    {
        var userProperties = new Dictionary<string, string>
        {
            {"Display Name", userInfo.DisplayName },
            {"Email", userInfo.Email },
            {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null },
            {"Provider ID", userInfo.ProviderId },
            {"User ID", userInfo.UserId }
        };
        foreach (var property in userProperties)
        {
            if (!String.IsNullOrEmpty(property.Value))
            {
                Debug.Log(String.Format("{0}: {1}", property.Key, property.Value));
            }
        }
    }

    public void SignOut()
    {
        Debug.Log("Signing out");

        // callback for sign out

        auth.SignOut();
        ActiveUserInfo = null;
    }
}
