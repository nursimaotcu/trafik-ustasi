using UnityEngine;

// Bu scripti her 4 secenek kutusuna ekle
public class OptionTrigger : MonoBehaviour
{
    public int optionNumber; // Inspector'dan 1, 2, 3 veya 4 yaz
    private QuestionZone questionZone;

    void Start()
    {
        questionZone = GetComponentInParent<QuestionZone>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CarController>() != null)
        {
            if (questionZone != null)
                questionZone.OnOptionEntered(optionNumber);
        }
    }
}