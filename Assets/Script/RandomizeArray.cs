using UnityEngine;
using System.Collections;
using System.Linq;

public class RandomizeArray : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}

    public static void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int r = Random.Range(0, i + 1);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }

    // Generate, Concatenate and Shuffle Arrays
    public static int[] GenerateArray(int totalTrials, int volatility_param)
    {
        // Divide trial in volatility_param percentage as Normal and rest as Conflict 
        // beware, number of trials in xml definition should be devidable by 2
        int normalTrials = (volatility_param * totalTrials)/100; // 75% of totalTrials are "normal"
        int conflictTrials = totalTrials-normalTrials; // the rest is considered "error" trial
        int[] normalTrialsArr = new int[normalTrials];
        int[] conflitTrialsArr = new int[conflictTrials];
        int[] allTrialsArr = new int[normalTrials + conflictTrials];
        for (int i = 0; i < normalTrials;i++) normalTrialsArr[i] = 0; // code normal trials with 0
        for (int i = 0; i < conflictTrials; i++) conflitTrialsArr[i] = 1; // and "error" trials with 1
        allTrialsArr = normalTrialsArr.Concat(conflitTrialsArr).ToArray(); // Concatenate two array
        ShuffleArray(allTrialsArr); // Shuffling whole array
        
        return allTrialsArr; 
    }

    // Generate, Concatenate and Shuffle Arrays with Filler trials
    // TODO define proportion of error / non-error trials and potentially add filler trials?
    public static int[] GenerateArrayWithFillerTrials(int totalTrials)
    {
        int normalTrials = (75 * totalTrials) / 100;  // Divide trial in 75% as Normal and 25% as Conflict 
        int conflictTrials = totalTrials - normalTrials;
        int fillerTrials = (10*normalTrials)/100;
        normalTrials = normalTrials - fillerTrials;  // Updating normal trials number
        int[] normalTrialsArr = new int[normalTrials];
        int[] conflitTrialsArr = new int[conflictTrials];
        int[] fillerTrialsArr = new int[fillerTrials];
        int[] allTrialsArr = new int[normalTrials + conflictTrials + fillerTrials];
        for (int i = 0; i < normalTrials; i++) normalTrialsArr[i] = 0; // Normal Trials
        for (int i = 0; i < conflictTrials; i++) conflitTrialsArr[i] = 1; // Conflict Trials
        for (int i = 0; i < fillerTrials; i++) fillerTrialsArr[i] = 2; // Filler Trials
        allTrialsArr = normalTrialsArr.Concat(conflitTrialsArr).ToArray(); // Concatenate two array
        allTrialsArr = allTrialsArr.Concat(fillerTrialsArr).ToArray(); // Contenate with Filler trials
        ShuffleArray(allTrialsArr); // Shuffling whole array

        return allTrialsArr;
    }

    // Generate, Concatenate and Shuffle Arrays
    public static int[] GenerateArraySequences(int totalTrials, double volatility_param, int number_of_sequences)
    {

        int[] tmp_sequences = new int[number_of_sequences];
        for (int i = 0; i < number_of_sequences; i++) tmp_sequences[i] = i+1;

        int[][] sequences = Enumerable.Repeat(tmp_sequences, number_of_sequences).ToArray();

        for (int i = 0; i < sequences.Length; i++)
        {
            ShuffleArray(sequences[i]);
            //for (int j = 0; j < sequences[i].Length; j++) Debug.Log(sequences[i][j]);
        
        }

        int[] allTrialsArr = new int[totalTrials];
        int count = 0;

        for (int i = 0; i < sequences.Length; i++){
            for (int j = 0; j < sequences[i].Length; j++){
                for (int k = 0; k <= sequences[i][j]; k++){

                    if (sequences[i][j] == k)
                    {
                        allTrialsArr[count] = 1; // code conflict trials with 1
                    }
                    else
                    {
                        allTrialsArr[count] = 0; // code normal trials with 0
                    }
                    count += 1;
                }
            }
        }

        //for (int i = 0; i < allTrialsArr.Length; i++) Debug.Log(allTrialsArr[i]);
        return allTrialsArr; 
    }

    // Generate, Array for Equal Number of 1s and 0s, to randomize the showing of big or small cube
    public static int[] GenerateBinaryArray(int totalTrials)
    {
        int[] BinaryArr = new int[totalTrials];
        for (int i = 0; i < totalTrials/2; i++) BinaryArr[i] = 0; // count to half of totalTrials and set to 0
        for (int i = totalTrials / 2; i < totalTrials; i++) BinaryArr[i] = 1; // count from half to end of totalTrials and set to 1
        ShuffleArray(BinaryArr); // Shuffling whole array

        return BinaryArr;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
