using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Unity.Editor;
using System;


public class FirebaseAppz {

    // Use this for initialization
    //Firebase.Auth.FirebaseAuth auth;
    //Firebase.Auth.FirebaseAuth otherAuth;
    //Firebase.Auth.FirebaseUser user;
    //DatabaseReference reference;
    //Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
    //  new Dictionary<string, Firebase.Auth.FirebaseUser>();
    //bool fetchingToken = false;

    public Firebase.Auth.FirebaseUser Userz { get; set; }
    public Firebase.Auth.FirebaseAuth OtherAuth { get; set; }
    public Firebase.Auth.FirebaseAuth Auth { get; set; }
    public DatabaseReference reference { get; set; }

    public string RegEmail { get; set; }
    public string RegPassword { get; set; }
    public string LogPassword { get; set; }
    public string LogEmail { get; set; }

    public class User
    {
        string fname, lname, email, password, userid, address, usertype, month;
        int year, day;
        public User(string fname, string lname, string email, string password, string userid, 
            string month, int year, int day, string usertype)
        {
            this.fname = fname;
            this.lname = lname;
            this.email = email;
            this.password = password;
            this.userid = userid;
            this.month = month;
            this.year = year;
            this.day = day;
            this.usertype = usertype;
        }

        public User()
        { }

        public Dictionary<string, object> ToDictionary()
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result["userId"] = userid;
            result["fname"] = fname;
            result["lname"] = lname;
            result["email"] = email;
            result["password"] = password;
            result["month"] = month;
            result["year"] = year;
            result["day"] = day;
            result["usertype"] = usertype;
            return result;
        }
    }


    public FirebaseAppz()
    {
        
    }


    public void CreateUserWithEmailAndPassword(string fname, string lname, string email, string password, 
        string month, int year, int day, string usertype)
    {
        Auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
//FirebaseUser newUser = task.Result;
            //FirebaseAppz app = new FirebaseAppz();
            //Userz = task.Result;
            WriteNewUser(fname, lname, email, password, Userz.UserId, month, year, day, usertype);
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                Userz.DisplayName, Userz.UserId);
        });
    }

    public void SignInWithEmailAndPassword()
    {
        Auth.SignInWithEmailAndPasswordAsync(LogEmail, LogPassword).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void SignInWithCredential()
    {
        Firebase.Auth.Credential credential =
        Firebase.Auth.EmailAuthProvider.GetCredential(LogEmail, LogPassword);
        Auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void GetUserInformation()
    {
        Userz = Auth.CurrentUser;
        if (Userz != null)
        {
            string name = Userz.DisplayName;
            string email = Userz.Email;
            System.Uri photo_url = Userz.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = Userz.UserId;
        }
    }

    //public void InitializeFirebase()
    //{
        
    //    FirebaseApp app = FirebaseApp.DefaultInstance;
    //    app.SetEditorDatabaseUrl("https://chemulator-d8bce.firebaseio.com/");
    //    app.SetEditorP12FileName("chemulator-d8bce.p12");
    //    app.SetEditorServiceAccountEmail("turbocsharpplus@gmail.com");
    //    app.SetEditorP12Password("s0l4rm00nbyul");
    //    if (app.Options.DatabaseUrl != null)
    //    app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
    //    Debug.Log("Setting up Firebase Auth");

    //    Auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    //    Auth.StateChanged += AuthStateChanged;
    //    AuthStateChanged(this, null);

    //    Auth.SignInWithEmailAndPasswordAsync("turbocsharpplus@gmail.com", "s0l4rm00nbyul").ContinueWith(task =>
    //    {
    //        if (task.IsCanceled)
    //        {
    //            Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
    //            return;
    //        }
    //        if (task.IsFaulted)
    //        {
    //            Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
    //            return;
    //        }
    //        Userz = task.Result;
    //        Debug.LogFormat("User signed in successfully: {0} ({1})",
    //            Userz.DisplayName, Userz.UserId);
    //        reference = FirebaseDatabase.DefaultInstance.RootReference;
    //    }); 
    //}

    void OnDestroy()
    {
        Auth.StateChanged -= AuthStateChanged;
        Auth = null;
    }


    public void WriteNewUser(string fname, string lname, string email, string password, string userid,
            string month, int year, int day, string usertype)
    {
        //User user = new User(fname, lname, email, password, userid, month, year, day, usertype);
        //string json = JsonUtility.ToJson(user);
        reference.Child("Users").Child(userid).Child("fname").SetValueAsync(fname);
        reference.Child("Users").Child(userid).Child("lname").SetValueAsync(lname);
        reference.Child("Users").Child(userid).Child("email").SetValueAsync(email);
        reference.Child("Users").Child(userid).Child("password").SetValueAsync(password);
        reference.Child("Users").Child(userid).Child("usertype").SetValueAsync(usertype);
        reference.Child("Users").Child(userid).Child("birthday").Child("month").SetValueAsync(month);
        reference.Child("Users").Child(userid).Child("birthday").Child("day").SetValueAsync(day);
        reference.Child("Users").Child(userid).Child("birthday").Child("year").SetValueAsync(year);
    }

    public void GetUserDetails()
    {

    }

    

    
    public void SignOut()
    {
        Auth.SignOut();
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        Firebase.Auth.FirebaseUser user = null;
        if (Auth.CurrentUser != user)
        {
            bool signedIn = user != Auth.CurrentUser && Auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = Auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

  
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
