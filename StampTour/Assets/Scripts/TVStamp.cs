using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JicsawPuzzle;
public class TVStamp : MonoBehaviour
{

    public Animator StampAnimator;
    protected int stampOutInAnimHash = Animator.StringToHash("Stamp_OutIn");

    [Header("SFX")]
    [SerializeField] AudioSource audioSourceStamp;
    [SerializeField] AudioClip stampSFX;
    // Start is called before the first frame update
    void Start()
    {
        StampAnimator.transform.localScale = new Vector3(0,0,1);
        StartCoroutine(Play());
    }

    public IEnumerator Play()
    {
        yield return new WaitForSeconds(5.0f);
        yield return OnAnimating(StampAnimator, stampOutInAnimHash);
    }

    IEnumerator OnAnimating(Animator animator, int animationHash)
    {
        animator.Play(animationHash);
        audioSourceStamp.PlayOneShot(stampSFX);
        yield return new WaitForEndOfFrame();

        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;
        GameManager.Instance.SetIsSceneFinished("TV",true);
    }


}
