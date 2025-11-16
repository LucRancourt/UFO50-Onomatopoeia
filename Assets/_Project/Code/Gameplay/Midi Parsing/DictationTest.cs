using UnityEngine;
using UnityEngine.Windows.Speech;

public class DictationScript : MonoBehaviour
{
    private string hypothesis = "";
    private string newWord;
    private string[] newWords;
    private DictationRecognizer m_DictationRecognizer;

    void Start()
    {
        m_DictationRecognizer = new DictationRecognizer();

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {
            if(hypothesis.Length >= text.Length)
            {
                hypothesis = "";
            }
            newWords = text.Substring(hypothesis.Length).Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            foreach (string word in newWords)
            {
                Debug.LogFormat(word);
            }
            //Debug.LogFormat("Dictation hypothesis: {0}", newWords.ToString());
            hypothesis = text;
        };
        m_DictationRecognizer.Start();
    }
}
