using UnityEngine;
using TMPro;

public class QuestionZone : MonoBehaviour
{
    [Header("Soru Indexi (0-8)")]
    public int questionIndex = 0;

    [Header("Soru Yazısı (3D Text)")]
    public TextMeshPro questionText;

    [Header("Sahneye Koyulan Sign Objesi")]
    public GameObject signObject;

    [Header("4 Seçenek Kutusu (Trigger Collider)")]
    public GameObject option1;
    public GameObject option2;
    public GameObject option3;
    public GameObject option4;

    [Header("Seçenek Yazıları (3D Text)")]
    public TextMeshPro optionText1;
    public TextMeshPro optionText2;
    public TextMeshPro optionText3;
    public TextMeshPro optionText4;

    [Header("Son Soru mu?")]
    public bool isLastQuestion = false;

    private bool answered = false;

    private static QuestionData[] questions = new QuestionData[]
    {
        new QuestionData("Dur Isareti",      "Hiz Limiti",        "Yol Ver",           "Park Yasak",        1),
        new QuestionData("Yaya Gecidi",      "Okul Gecidi",       "Bisiklet Yolu",     "Hayvan Gecisi",     3),
        new QuestionData("Saga Donus",       "Sola Donus",        "Tehlikeli Viraj",   "Yol Kapali",        3),
        new QuestionData("Tek Yon",          "Sollama Yasak",     "Yol Kapali",        "Cift Yonlu Trafik", 4),
        new QuestionData("Hayvan Gecisi",    "Yaya Gecidi",       "Bisiklet Yolu",     "Okul Gecidi",       1),
        new QuestionData("Sola Donus",       "Saga Donus",        "Duz Yol",           "Sollama Yasak",     2),
        new QuestionData("Sel Tehlikesi",    "Bisiklet Gecidi",   "Yol Bozuk",         "Hayvan Gecisi",     2),
        new QuestionData("Eczane",           "Polis Merkezi",     "Otel",              "Hastane",           4),
        new QuestionData("Park Yasak",       "Park Yapilabilir",  "Dur",               "Giris Yasak",       1),
    };

    void Start()
    {
        LoadQuestion();
    }

    void LoadQuestion()
    {
        if (questionIndex < 0 || questionIndex >= questions.Length) questionIndex = 0;
        QuestionData q = questions[questionIndex];
        answered = false;

        if (questionText)  questionText.text  = "Bu isaret ne anlama gelir?";
        if (optionText1)   optionText1.text   = q.option1;
        if (optionText2)   optionText2.text   = q.option2;
        if (optionText3)   optionText3.text   = q.option3;
        if (optionText4)   optionText4.text   = q.option4;

        if (signObject != null) signObject.SetActive(true);

        ResetColors();
    }

    public void OnOptionEntered(int optionNumber)
    {
        if (answered) return;
        answered = true;

        QuestionData q = questions[questionIndex];
        CarController car = Object.FindFirstObjectByType<CarController>();

        if (car == null)
        {
            Debug.LogError("CarController bulunamadi!");
            return;
        }

        if (optionNumber == q.correctAnswer)
        {
            car.score += 10;
            TrafficUIManager.Instance?.ShowWarning("DOGRU! +10 Puan");
            HighlightCorrect(optionNumber);
            car.DogruSesCal();

            if (isLastQuestion)
            {
                Invoke(nameof(EndGameSuccess), 3f);
            }
        }
        else
        {
            TrafficUIManager.Instance?.ShowWarning("YANLIS! -10 Puan");
            HighlightWrong(optionNumber, q.correctAnswer);
            car.ApplyPenalty(10);
            car.YanlisSesCal();
        }
    }

    void EndGameSuccess()
    {
        CarController car = Object.FindFirstObjectByType<CarController>();
        string msg = car != null
            ? "Tebrikler! Parkuru tamamladin!"
            : "Tebrikler! Parkuru tamamladin!";
        Debug.Log("EndGameSuccess: " + msg);
        TrafficUIManager.Instance?.EndGame(msg);
    }

    void HighlightCorrect(int correct)
    {
        SetOptionColor(GetOption(correct), Color.green);
    }

    void HighlightWrong(int wrong, int correct)
    {
        SetOptionColor(GetOption(wrong), Color.red);
        SetOptionColor(GetOption(correct), Color.green);
    }

    void ResetColors()
    {
        SetOptionColor(option1, Color.white);
        SetOptionColor(option2, Color.white);
        SetOptionColor(option3, Color.white);
        SetOptionColor(option4, Color.white);
    }

    GameObject GetOption(int num)
    {
        switch (num)
        {
            case 1: return option1;
            case 2: return option2;
            case 3: return option3;
            case 4: return option4;
            default: return null;
        }
    }

    void SetOptionColor(GameObject obj, Color color)
    {
        if (obj == null) return;
        Renderer r = obj.GetComponent<Renderer>();
        if (r != null) r.material.color = color;
    }
}

[System.Serializable]
public class QuestionData
{
    public string option1, option2, option3, option4;
    public int correctAnswer;

    public QuestionData(string o1, string o2, string o3, string o4, int correct)
    {
        option1 = o1; option2 = o2; option3 = o3; option4 = o4;
        correctAnswer = correct;
    }
}