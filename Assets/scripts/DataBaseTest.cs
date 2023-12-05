using Firebase;
using Firebase.Database;
using System;
using System.Collections;
using UnityEngine;

public class DatabaseTest : MonoBehaviour
{
    public User user;
    private DatabaseReference dataReference;
    private string userID;

    private void Awake()
    {
        userID = SystemInfo.deviceUniqueIdentifier;
    }

    void Start()
    {
        dataReference = FirebaseDatabase.DefaultInstance.RootReference;
        Invoke("CreateUser", 1f);
    }
    private void CreateUser()
    {
        //User newUser = new User("Pedro", "Piedrito", 9781235);
        string json = JsonUtility.ToJson(user);
        dataReference.Child("users").Child(userID).SetRawJsonValueAsync(json).ContinueWith(
            task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log("Done Update");
                }
                else
                {
                    Debug.Log("Failed Attempt");
                }
            });
    }

    private IEnumerator GetFirstName(Action<string> onCallBack)
    {
        var userNameData = dataReference.Child("users").Child(userID).Child("firstName").GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;
            onCallBack?.Invoke(snapshot.Value.ToString());
        }
    }

    private IEnumerator GetLastName(Action<string> onCallBack)
    {
        var userNameData = dataReference.Child("users").Child(userID).Child("lastName").GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;
            onCallBack?.Invoke(snapshot.Value.ToString());
        }
    }

    private IEnumerator GetCodeID(Action<int> onCallBack)
    {
        var userNameData = dataReference.Child("users").Child(userID).Child(nameof(User.codeID)).GetValueAsync();

        yield return new WaitUntil(predicate: () => userNameData.IsCompleted);

        if (userNameData != null)
        {
            DataSnapshot snapshot = userNameData.Result;
            //(int) -> Casting
            //int.Parse -> Parsing
            //https://teamtreehouse.com/community/when-should-i-use-int-and-intparse-whats-the-difference
            onCallBack?.Invoke(int.Parse(snapshot.Value.ToString()));
        }
    }

    public void GetUserInfo()
    {

        StartCoroutine(GetFirstName(PrintData));
        StartCoroutine(GetLastName(PrintData));
        StartCoroutine(GetCodeID(PrintData));
    }

    private void PrintData(string name)
    {
        Debug.Log(name);
    }

    private void PrintData(int code)
    {
        Debug.Log(code);
    }
}

[Serializable]
public struct User
{
    public string firstName;
    public string lastName;
    public int codeID;
    public User(string firstName, string lastName, int codeID)
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.codeID = codeID;
    }
}
