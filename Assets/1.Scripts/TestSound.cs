using UnityEngine;

public class TestSound : MonoBehaviour
{
    int i = 0;

    private void OnTriggerEnter(Collider other)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        i++;
        if(i%2 == 0)
            Managers.Sound.Play("UnityChan/univ0001", Define.Sound.Bgm);
        else
            Managers.Sound.Play("UnityChan/univ0002", Define.Sound.Bgm);
    }
}
