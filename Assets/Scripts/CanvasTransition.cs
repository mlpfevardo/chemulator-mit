using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Unity.Editor;
using System.Text;



public class CanvasTransition : MonoBehaviour {
    public RectTransform loginPage, registerPage, registerSuccess, studentPage, instructorsPage,
        createClassPage, classesPageInstructor, exercisesPageInst, exercisesPageStud, classList, enrollClassPage, classesPageStud;
    //    enrollClassPage, chooseExpPage, createClassPage, studentClassPage, exercisesPage, addExercisePage,
    //    addInstructionsPage;
    RectTransform studorinstPanel;
    public GameObject typeofuser;
    public GameObject loadingScreenObject;
    public Slider slider;
    AsyncOperation async;
    FirebaseAuth auth;
    FirebaseAuth otherAuth;
    FirebaseUser user;
    DatabaseReference reference;
    public Button exitButton, generateKeyButton;
    string usertype, firstAndLastName;
    public InputField firstName, lastName, email, password, emaillogin, passwordlogin, classNameInput, classKeyInput;
    public Dropdown month, year, day;
    string userType, fn, ln, em, pass, uid, m, d , y;
    string cName, cKey;
    public Text userNotFound, className, displayName, emailAdd, passw, welcomInst, welcomeStud, classKey, classKeyHolder;
    public Text yrne, inst, instName, classNameEnroll;
    bool isStudent;
    private string[] alpb, num;

    private void Awake()
    {
        //Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        //    var dependencyStatus = task.Result;
        //    if (dependencyStatus == Firebase.DependencyStatus.Available)
        //    {
        //        FirebaseApp app = FirebaseApp.DefaultInstance;
        //        app.SetEditorDatabaseUrl("https://chemulator-d8bce.firebaseio.com/");
                
        //        if (app.Options.DatabaseUrl != null)
        //            app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
        //        InitializeFirebase();
        //        reference = FirebaseDatabase.DefaultInstance.RootReference;
        //    }
        //    else
        //    {
        //        Debug.LogError(String.Format(
        //          "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        //        // Firebase Unity SDK is not safe to use here.
        //    }
        //});
        
    }
    public virtual void Start()
    {
        
        num = new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        alpb = new string[52] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V",
            "W", "X", "Y", "Z","a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v",
            "w", "x", "y", "z" };
        //Debug.Log(generateKey(0, 10));

    }

    public string generateKey(int nodeCount, int nodeCharCount)
    {
        //Shuffle our arrays first so that every time we get a random key
        shuffleArray<string>(num);
        shuffleArray<string>(alpb);
        nodeCount = Mathf.Clamp(nodeCount, 1, 1);
        nodeCharCount = Mathf.Clamp(nodeCharCount, 10, 10);
        int numIndex = 0, alpIndex = 0, insertInt = 0;
        StringBuilder sB = new StringBuilder();
        for (int i = 1; i <= nodeCount; i++)
        {
            for (int j = 0; j < nodeCharCount; j++)
            {
                insertInt = UnityEngine.Random.Range(0, 2); // 0 means we will insert an alphabet in our key code and 1 means we will go with a number
                if (insertInt == 0)
                {
                    sB.Append(alpb[alpIndex]);
                    alpIndex++;
                    if (alpIndex == alpb.Length)
                    {
                        alpIndex = 0;
                    }
                }
                else
                {
                    sB.Append(num[numIndex]);
                    numIndex++;
                    if (numIndex == num.Length)
                    {
                        numIndex = 0;
                    }
                }
            }
            //if (i < nodeCount)
            //{
            //    sB.Append("-");
            //}
        }
        return sB.ToString();
    }
    static void shuffleArray<T>(T[] arr)
    { // This will not create a new array but return the original but shuffled array
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = UnityEngine.Random.Range(0, i + 1);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

    //public void 

    public void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void CreateUserWithEmailAndPassword(string fname, string lname, string email, string password,
        string month, int year, int day, string usertype)
    {
        Debug.Log("ok na");
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
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
            
            if (task.IsCompleted)
            {
                FirebaseUser newUser = task.Result;
                uid = newUser.UserId;
                UnityMainThreadDispatcher.Instance().Enqueue(()=>WriteNewUser(fname, lname, email, password, newUser.UserId, month, year, day, usertype));
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
                UnityMainThreadDispatcher.Instance().Enqueue(()=>registerSuccess.DOAnchorPos(new Vector2(0, 0), 0.25f));
                UnityMainThreadDispatcher.Instance().Enqueue(()=>registerSuccess.DOAnchorPos(new Vector2(0, 720), 0.25f).SetDelay(2.0f));
                RegGetCredentials(uid);
            }
            
        });

        Debug.Log(uid);
    }

    public void Exit()
    {
        //if(exitButton.) 
        Application.Quit();
        Debug.Log("Quitting");
        
    }
    public void WriteNewUser(string fname, string lname, string email, string password, string userid,
            string month, int year, int day, string usertype)
    {
        //User user = new User(fname, lname, email, password, userid, month, year, day, usertype);
        //string json = JsonUtility.ToJson(user);
        Debug.Log("huhu");
        reference.Child("Users").Child(userid).Child("fname").SetValueAsync(fname);
        reference.Child("Users").Child(userid).Child("lname").SetValueAsync(lname);
        reference.Child("Users").Child(userid).Child("email").SetValueAsync(email);
        reference.Child("Users").Child(userid).Child("password").SetValueAsync(password);
        reference.Child("Users").Child(userid).Child("usertype").SetValueAsync(usertype);
        reference.Child("Users").Child(userid).Child("birthday").Child("month").SetValueAsync(month);
        reference.Child("Users").Child(userid).Child("birthday").Child("day").SetValueAsync(day);
        reference.Child("Users").Child(userid).Child("birthday").Child("year").SetValueAsync(year);
        Debug.Log("yea database lmao");
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in " + user.UserId);
                //displayName = user.DisplayName ?? "";
                //emailAddress = user.Email ?? "";
                //photoUrl = user.PhotoUrl ?? "";
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}


    public void OnClickRegister()
    {
        Debug.Log(generateKey(0, 10));
        loginPage.DOAnchorPos(new Vector2(-1280, 0), 0.25f);
        registerPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
        ClearFields();
    }

    public void OnClickBackRegister()
    {
        loginPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
        registerPage.DOAnchorPos(new Vector2(1280, 0), 0.25f);
        ClearFieldsLogIn();
    }

    public void Register()
    {
        try
        {
            Debug.Log(uid);
            if (typeofuser.GetComponent<ToggleGroup>() != null)
            { 
                for (int i = 0; i < typeofuser.transform.childCount; i++)
                {
                    if (typeofuser.transform.GetChild(i).GetComponent<Toggle>().isOn)
                    {
                        userType = typeofuser.transform.GetChild(i).Find("Label").GetComponent<Text>().text;
                        break;
                    }
                }
            }
                //if registered successfully
            Debug.LogFormat("Month: {0}", month.options[month.value].text);
            Debug.LogFormat("user type: {0}", userType);
            CreateUserWithEmailAndPassword(firstName.text, lastName.text, email.text, password.text,
                month.options[month.value].text, Convert.ToInt32(year.options[year.value].text), Convert.ToInt32(day.options[day.value].text), userType);
        }
        catch (Exception)
        {

        }
    }
    void GetUserCredentials(string uid)
    {
        //var nani = reference.GetValueAsync().ContinueWith(task =>
        //{
        //    if (task.IsFaulted)
        //    {

        //    }
        //    else if (task.IsCompleted)
        //    {
        //        DataSnapshot snap = task.Result;
        //        fn = snap.Child(uid).Child("fname").Value.ToString();
        //        ln = snap.Child(uid).Child("lname").Value.ToString();
        //        usertype = snap.Child(uid).Child("usertype").Value.ToString();
        //        em = snap.Child(uid).Child("email").Value.ToString();
        //    }
        //});
        //yield return new WaitUntil(() => nani.IsCompleted);
        this.uid = uid;
        fn = reference.Child("Users").Child(uid).Child("fname").GetValueAsync().Result.Value.ToString();
        ln = reference.Child("Users").Child(uid).Child("lname").GetValueAsync().Result.Value.ToString();
        usertype = reference.Child("Users").Child(uid).Child("usertype").GetValueAsync().Result.Value.ToString();
        em = reference.Child("Users").Child(uid).Child("email").GetValueAsync().Result.Value.ToString();
    }

    void RegGetCredentials(string uid)
    {
        //UnityMainThreadDispatcher.Instance().Enqueue(()=>GetUserCredentials(uid));
        //UnityMainThreadDispatcher.Instance().Enqueue(() => GetUserCredentials(uid));
        //fn = reference.Child(uid).Child("fname").GetValueAsync().Result.Value.ToString();
        //ln = reference.Child(uid).Child("lname").GetValueAsync().Result.Value.ToString();
        //usertype = reference.Child(uid).Child("usertype").GetValueAsync().Result.Value.ToString();
        //em = reference.Child(uid).Child("email").GetValueAsync().Result.Value.ToString();
        this.uid = uid;
        GetUserCredentials(uid);
        Debug.Log(usertype);
        Debug.Log(fn);
        Debug.Log(ln);
        Debug.Log(em);

        if (usertype == "Student")
            isStudent = true;
        if (usertype == "Instructor")
            isStudent = false;

        if (isStudent)
        {
            Debug.Log(fn);
            UnityMainThreadDispatcher.Instance().Enqueue(SetWelcomeStud("Welcome, " + fn + "!"));
            UnityMainThreadDispatcher.Instance().Enqueue(()=>registerPage.DOAnchorPos(new Vector2(1280, 0), 0.25f));
            UnityMainThreadDispatcher.Instance().Enqueue(()=>studentPage.DOAnchorPos(new Vector2(0, 0), 0.25f).SetDelay(2.0f));
        }
        else if (!isStudent)
        {
            Debug.Log(fn);
            UnityMainThreadDispatcher.Instance().Enqueue(SetWelcomeInst("Welcome, " + fn + "!"));
            UnityMainThreadDispatcher.Instance().Enqueue(()=>registerPage.DOAnchorPos(new Vector2(1280, 0), 0.25f));
            //loginPage.DOAnchorPos(new Vector2(-1280, 0), 0.25f);
            UnityMainThreadDispatcher.Instance().Enqueue(()=>instructorsPage.DOAnchorPos(new Vector2(0, 0), 0.25f).SetDelay(2.0f));
        }
    }

    void SignInGetUserCredentials(string uid)
    {
        //fn = reference.Child(uid).Child("fname").GetValueAsync().Result.Value.ToString();
        //ln = reference.Child(uid).Child("lname").GetValueAsync().Result.Value.ToString();
        //usertype = reference.Child(uid).Child("usertype").GetValueAsync().Result.Value.ToString();
        //em = reference.Child(uid).Child("email").GetValueAsync().Result.Value.ToString();
        //UnityMainThreadDispatcher.Instance().Enqueue(() => GetUserCredentials(uid));
        //UnityMainThreadDispatcher.Instance().Enqueue(()=>
        this.uid = uid;
        GetUserCredentials(uid);
        Debug.Log(usertype);
        Debug.Log(fn);
        Debug.Log(ln);
        Debug.Log(em);
        if (usertype == "Student")
            isStudent = true;
        if (usertype == "Instructor")
            isStudent = false;
        if (isStudent)
        {
            Debug.Log(fn);
            UnityMainThreadDispatcher.Instance().Enqueue(SetWelcomeStud("Welcome, " + fn + "!"));
            UnityMainThreadDispatcher.Instance().Enqueue(()=>loginPage.DOAnchorPos(new Vector2(-1280, 0), 0.25f));
            UnityMainThreadDispatcher.Instance().Enqueue(()=>studentPage.DOAnchorPos(new Vector2(0, 0), 0.25f).SetDelay(2.0f));
        }
        else if (!isStudent)
        {
            Debug.Log(fn);
            UnityMainThreadDispatcher.Instance().Enqueue(SetWelcomeInst("Welcome, " + fn + "!"));
            UnityMainThreadDispatcher.Instance().Enqueue(()=>loginPage.DOAnchorPos(new Vector2(-1280, 0), 0.25f));
            UnityMainThreadDispatcher.Instance().Enqueue(() => instructorsPage.DOAnchorPos(new Vector2(0, 0), 0.25f).SetDelay(2.0f));
        }
    }

    //IEnumerator S

    IEnumerator SetWelcomeStud(string fnln)
    {
        welcomeStud.text = fnln;
        yield return null;
    }

    IEnumerator SetWelcomeInst(string fnln)
    {
        welcomInst.text = fnln;
        yield return null;
    }
    void GetExercises(string uid)
    {

    }

    void GetGrades(string uid)
    {

    }
    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        //DataSnapshot snapshot = task.Result;
        fn = args.Snapshot.Value.ToString();
        Debug.Log(fn);
        firstAndLastName = "Welcome, " + fn + "!";
        
        // Do something with the data in args.Snapshot
    }
    void OnDestroy()
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void SignOut()
    {
        auth.SignOut();
    }

    public void OnClickLogin()
    {
        Login(emaillogin.text, passwordlogin.text);
    }

    void Login(string e, string p)
    {
        try
        {
            auth.SignInWithEmailAndPasswordAsync(e, p).ContinueWith(task =>
            {
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

                if (task.IsCompleted)
                {
                    FirebaseUser newUser = task.Result;
                    uid = newUser.UserId;
                    //UnityMainThreadDispatcher.Instance().Enqueue(() => SignInGetUserCredentials(uid));
                    SignInGetUserCredentials(uid);
                    Debug.Log(uid);
                    Debug.LogFormat("User signed in successfully: {0} ({1})",
                        newUser.DisplayName, newUser.UserId);
                }
            });
        }
        catch(Exception)
        { }
    }

    public void CreateAClass()
    {
        //this.uid = uid;
        string ck = generateKey(0, 10);
        reference.Child("Classes").Child(ck).Child("Name").SetValueAsync(classNameInput.text);
        reference.Child("Classes").Child(ck).Child("Instructor").SetValueAsync(uid);
        UnityMainThreadDispatcher.Instance().Enqueue(EnableClassKeyObjects(ck));
        //generateKeyButton.enabled = false;
        //classKeyHolder.enabled = true;
        //classKey.text = ck;
        //classKey.enabled = true;
        Debug.Log("Class Created Successfully");
    }

    void ChooseClasses()
    {

    }

    IEnumerator EnableClassKeyObjects(string ckey)
    {
        //generateKeyButton.enabled = false;
        generateKeyButton.gameObject.SetActive(false);
        //generateKeyButton.set
        //classKeyHolder.enabled = true;
        classKey.text = ckey;
        //classKey.enabled = true;
        classKeyHolder.gameObject.SetActive(true);
        classKey.gameObject.SetActive(true);
        yield return null;
    }

    public void EnrollToClass()
    {
        cKey = classKeyInput.text;
        
        UnityMainThreadDispatcher.Instance().Enqueue(()=>GetClassDetails(cKey));
        UnityMainThreadDispatcher.Instance().Enqueue(EnableEnrollToClassObjects());
    }

    void GetClassDetails(string key)
    {
        string cn, instructor, instfname, instlname;
        reference.Child("Classes").Child(cKey).Child("Students").Child(uid).Child("EX1").Child("Grade").SetValueAsync(0);
        reference.Child("Classes").Child(cKey).Child("Students").Child(uid).Child("EX2").Child("Grade").SetValueAsync(0);
        reference.Child("Classes").Child(cKey).Child("Students").Child(uid).Child("EX3").Child("Grade").SetValueAsync(0);
        reference.Child("Classes").Child(cKey).Child("Students").Child(uid).Child("EX4").Child("Grade").SetValueAsync(0);
        reference.Child("Classes").Child(cKey).Child("Students").Child(uid).Child("EX5").Child("Grade").SetValueAsync(0);
       
        cn = reference.Child("Classes").Child(key).Child("Name").GetValueAsync().Result.Value.ToString();
        instructor = reference.Child("Classes").Child(key).Child("Instructor").GetValueAsync().Result.Value.ToString();
        instfname = reference.Child("Users").Child(instructor).Child("fname").GetValueAsync().Result.Value.ToString();
        instlname = reference.Child("Users").Child(instructor).Child("lname").GetValueAsync().Result.Value.ToString();
        classNameEnroll.text = cn;
        instName.text = instfname + " " + instlname;
    }

    void ResetEnrollToClassObjects()
    {
        yrne.gameObject.SetActive(false);
        inst.gameObject.SetActive(false);
        instName.gameObject.SetActive(false);
        classNameEnroll.gameObject.SetActive(false);
    }

    void ResetClassKeyObjects()
    {
        //generateKeyButton.enabled = false;
        generateKeyButton.gameObject.SetActive(true);
        //generateKeyButton.set
        //classKeyHolder.enabled = true;
        classKey.text = "";
        //classKey.enabled = true;
        classKeyHolder.gameObject.SetActive(false);
        classKey.gameObject.SetActive(false);
        
    }

    IEnumerator EnableEnrollToClassObjects()
    {
        //yrne.enabled = true;
        //inst.enabled = true;
        //instName.enabled = true;
        //classNameEnroll.enabled = true;
        yrne.gameObject.SetActive(true);
        inst.gameObject.SetActive(true);
        instName.gameObject.SetActive(true);
        classNameEnroll.gameObject.SetActive(true);
        yield return null;
    }

    public void EnrolClass()
    {
        studentPage.DOAnchorPos(new Vector2(0, -720), 0.25f);
        enrollClassPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }

    public void EnrolClassBack()
    {
        studentPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
        enrollClassPage.DOAnchorPos(new Vector2(-1280, 0), 0.25f);
        ResetEnrollToClassObjects();
    }
    public void CreateClass()
    {
        instructorsPage.DOAnchorPos(new Vector2(0, -720), 0.25f);
        createClassPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }

    public void CreateClassBack()
    {
        createClassPage.DOAnchorPos(new Vector2(1280, 0), 0.25f);
        instructorsPage.DOAnchorPos(new Vector2(0, 0), 0.25f);

    }

    public void ViewClassesInstructor()
    {
        instructorsPage.DOAnchorPos(new Vector2(0, -720), 0.25f);
        classesPageInstructor.DOAnchorPos(new Vector2(0, 0), 0.25f);

    }

    public void ViewClassesStudent()
    {
        studentPage.DOAnchorPos(new Vector2(0, -720), 0.25f);
        classesPageStud.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }

    public void BackClassListInstructor()
    {
        classesPageInstructor.DOAnchorPos(new Vector2(0,-720), 0.25f);
        instructorsPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }

    public void BackClassListStudent()
    {
        classesPageStud.DOAnchorPos(new Vector2(0, -720), 0.25f);
        studentPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }

    public void ViewExperimentsInstructor()
    {
        exercisesPageInst.DOAnchorPos(new Vector2(0,0), 0.25f);
        instructorsPage.DOAnchorPos(new Vector2(0, -720), 0.25f);
    }

    public void BackExperimentsInstructor()
    {
        exercisesPageInst.DOAnchorPos(new Vector2(0, -720), 0.25f);
        instructorsPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }

    public void ViewExperimentsStudent()
    {
        exercisesPageStud.DOAnchorPos(new Vector2(0, 0), 0.25f);
        studentPage.DOAnchorPos(new Vector2(0, -720), 0.25f);

    }

    public void StudBackExperiments()
    {
        exercisesPageStud.DOAnchorPos(new Vector2(0, -720), 0.25f);
        studentPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
    }

    public void LoadExperiment(int sceneNumber)
    {
        SceneManager.LoadScene(sceneNumber);
        //exercisesPage.DOAnchorPos(new Vector2(0, -720), 0.25f);
        //loadingScreen.DOAnchorPos(new Vector2(0, 0), 0.25f);
        //StartCoroutine(LoadingScreen(sceneNumber));
    }

    public void LogOut()
    {
        //firebase shit
        SignOut();
        user.DeleteAsync();
        if (isStudent)
        {
            studentPage.DOAnchorPos(new Vector2(0, -720), 0.25f);
            loginPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
            
        }
        else if (!isStudent)
        {
            instructorsPage.DOAnchorPos(new Vector2(0, -720), 0.25f);
            loginPage.DOAnchorPos(new Vector2(0, 0), 0.25f);
            classKeyHolder.enabled = false;
            classKey.text = "";
            classKey.enabled = false;
        }
    }

    void ClearFields()
    {
        firstName.text = "";
        lastName.text = "";
        email.text = "";
        password.text = "";
    }
    
    void ClearFieldsLogIn()
    {
        emaillogin.text = "";
        passwordlogin.text = "";
    }

    void ClearFieldsClass()
    {
        className.text = "";
    }

    IEnumerator LoadingScreen(int sceneNumber)
    {
        loadingScreenObject.SetActive(true);
        async = SceneManager.LoadSceneAsync(sceneNumber);
        async.allowSceneActivation = false;
        while (async.isDone == false)
        {
            slider.value = async.progress;
            if (async.progress == 0.9f)
            {
                slider.value = 1f;
                async.allowSceneActivation = true;
            }
        }
        yield return null;
    }

}
