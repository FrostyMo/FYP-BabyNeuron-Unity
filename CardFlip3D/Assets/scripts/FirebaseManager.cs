using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using TMPro;
using System.Linq;
using UnityEngine.SceneManagement;
public class FirebaseManager : MonoBehaviour
{


    //Firebase variables
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public static FirebaseAuth auth;
    public static FirebaseUser User;
    public DatabaseReference DBreference;

    //Login variables
    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    //Register variables
    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordRegisterVerifyField;
    public TMP_Text warningRegisterText;

    //Register variables
    [Header("UserData")]
    public TMP_InputField usernameField;
    public TMP_InputField xpField;
    // public TMP_InputField killsField;
    // public TMP_InputField deathsField;
    // public GameObject scoreElement;
    // public Transform scoreboardContent;


    //Register variables
    [Header("CorsiScore")]
    public TMP_InputField myCorsiScoreField;
    public TMP_InputField myCorsiScoreField2;
    private const string gameType = "3D";

    private double time;

    void Start()
    {
        time = Time.realtimeSinceStartup;
    }
    void Awake()
    {


        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    private void InitializeFirebase()
    {
        Debug.Log("Setting up Firebase Auth");
        //Set the authentication instance object
        auth = FirebaseAuth.DefaultInstance;
        DBreference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void ClearLoginFields()
    {
        emailLoginField.text = "";
        passwordLoginField.text = "";
        myCorsiScoreField.text = "";
    }

    public void ClearRegisterFields()
    {
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordRegisterVerifyField.text = "";
    }

    //Function for the login button
    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
    }

    public void SubmitButton()
    {
        //Call the submit corsi coroutine passing the score
        StartCoroutine(SubmitCorsi(int.Parse(myCorsiScoreField.text)));
    }
    public void SubmitPostButton()
    {
        ClearLoginFields();
        ClearRegisterFields();
        //Call the submit corsi coroutine passing the score
        StartCoroutine(SubmitPostCorsi(int.Parse(myCorsiScoreField2.text)));
    }

    public void SignOutButton()
    {
        auth.SignOut();
        UIManager.instance.LoginScreen();
        ClearRegisterFields();
        ClearLoginFields();
    }




    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";
            StartCoroutine(LoadUserData());
            yield return new WaitForSeconds(2);
            usernameField.text = User.DisplayName;
            if (myCorsiScoreField.text == "")
            {
                UIManager.instance.RegisterCorsiScoreScreen();

            }
            else
            {
                SceneManager.LoadScene("main_menu");
            }
            confirmLoginText.text = "";
            ClearLoginFields();
            ClearRegisterFields();
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        UIManager.instance.LoginScreen();
                        warningRegisterText.text = "";
                        ClearLoginFields();
                        ClearRegisterFields();
                    }
                }
            }
        }
    }


    private IEnumerator UpdateUsernameDB(string _username)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        // else usrname will be updated
    }

    private IEnumerator UpdateScore(int _score)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("score").SetValueAsync(_score);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        // else score  will be updated
    }

    private IEnumerator LoadUserData()
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        // no values
        else if (DBTask.Result.Value == null)
        {
            Debug.Log("No dataa");
            // No data yet
            xpField.text = "";
            myCorsiScoreField.text = "";
        }
        // values found
        else
        {
            DataSnapshot snapShot = DBTask.Result;
            if (snapShot.Child("score").Value != null)
                xpField.text = snapShot.Child("score").Value.ToString();
            if (xpField.text == "")
            {
                xpField.text = "0";
            }
            // add more if more fields needed
            myCorsiScoreField.text = snapShot.Child("corsiScore").Value.ToString();
        }
    }


    private IEnumerator SubmitCorsi(int _myCorsiScoreField)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("corsiScore").Child("preScore").SetValueAsync(_myCorsiScoreField);
        // string key = DBRef.Push().Key;
        // var DBTask = DBRef.Child(key).SetValueAsync(_myCorsiScoreField);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        // else corsi will be updated

        DBTask = DBreference.Child("users").Child(User.UserId).Child("score").SetValueAsync(0);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        // else score will be initialized to 0

        DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(usernameField.text);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        // else username will be set

        DBTask = DBreference.Child("users").Child(User.UserId).Child("gameType").SetValueAsync(gameType);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

        DBTask = DBreference.Child("users").Child(User.UserId).Child("totalTime").SetValueAsync(0.0);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

        myCorsiScoreField.text = _myCorsiScoreField.ToString();
        SceneManager.LoadScene("main_menu");

    }

    // After exiting, ask them to submit this
    private IEnumerator SubmitPostCorsi(int _myCorsiScoreField)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("corsiScore").Child("postScore").SetValueAsync(_myCorsiScoreField);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        // else corsi will be updated

        DBTask = DBreference.Child("users").Child(User.UserId).Child("gameType").SetValueAsync(gameType);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

        DBTask = DBreference.Child("users").Child(User.UserId).Child("totalTime").SetValueAsync(time);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        // else time will be set

        // create a new instance of scene controller for score
        // SceneController sc = new SceneController();
        DBTask = DBreference.Child("users").Child(User.UserId).Child("score").SetValueAsync(block_rotation.score);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }

        // else corsi will be updated
        UIManager.instance.ThankYouScreen();
    }

}
