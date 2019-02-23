using UnityEngine;
using System.Collections;
using System.Text;

public class keyCode : MonoBehaviour
{
    private string[] alpb, num;
    void Start()
    {
        num = new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
        alpb = new string[26] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
        Debug.Log(generateKey(3, 5)); // Just for the sake of a simple test
    }
    /*
     * nodeCount of 3 and nodeCharCount of 5 will produce something like this
     * yu67h-7uyh8-i8uy6
     */
    public string generateKey(int nodeCount, int nodeCharCount)
    {
        //Shuffle our arrays first so that every time we get a random key
        shuffleArray<string>(num);
        shuffleArray<string>(alpb);
        nodeCount = Mathf.Clamp(nodeCount, 5, 5);
        nodeCharCount = Mathf.Clamp(nodeCharCount, 5, 5);
        int numIndex = 0, alpIndex = 0, insertInt = 0;
        StringBuilder sB = new StringBuilder();
        for (int i = 1; i <= nodeCount; i++)
        {
            for (int j = 0; j < nodeCharCount; j++)
            {
                insertInt = Random.Range(0, 2); // 0 means we will insert an alphabet in our key code and 1 means we will go with a number
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
            if (i < nodeCount)
            {
                sB.Append("-");
            }
        }
        return sB.ToString();
    }

    static void shuffleArray<T>(T[] arr)
    { // This will not create a new array but return the original but shuffled array
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }
}