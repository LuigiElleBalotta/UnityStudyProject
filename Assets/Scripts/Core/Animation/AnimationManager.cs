using UnityEngine;
using System.Collections;

public class AnimationManager : MonoBehaviour
{
    AnimatorOverrideController overrideController;
    static string prevLoadAnim;
    Animator animator;
    ResourceRequest request;
    AnimatorStateInfo[] layerInfo;

    // Use this for initialization
    void Start()
    {
        overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;
    }

    IEnumerator LoadAnimClip(string clipName, string boolName)
    {
        request = Resources.LoadAsync("Characters/LadySylvanasWindrunner/Sylvanas_Animations/Sylvanas_" + clipName);

        yield return request;

        AnimationClip animClip = request.asset as AnimationClip;

        overrideController[clipName] = animClip;

        // Push back state
        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.Play(layerInfo[i].nameHash, i, layerInfo[i].normalizedTime);
        }

        // Force an update
        animator.Update(0.0f);

        //		animator.SetBool(boolName, true);
    }

    /// <summary>
    /// Load Animation Clip
    /// </summary>

    public void LoadAnimation(Animator _animator, string animationName, string animationBoolName)
    {
        animator = _animator;
        //Save current state
        layerInfo = new AnimatorStateInfo[animator.layerCount];

        for (int i = 0; i < animator.layerCount; i++)
        {
            layerInfo[i] = animator.GetCurrentAnimatorStateInfo(i);
        }

        // Start Load Animation Clip
        StartCoroutine(LoadAnimClip(animationName, animationBoolName));

        prevLoadAnim = animationName;
    }

    public void UnloadPreviousLoadAnimation()
    {
        for (int i = 0; i < animator.layerCount; i++)
        {
            layerInfo[i] = animator.GetCurrentAnimatorStateInfo(i);
        }

        overrideController[prevLoadAnim + " Empty"] = null;
        Resources.UnloadUnusedAssets();

        for (int i = 0; i < animator.layerCount; i++)
        {
            animator.Play(layerInfo[i].nameHash, i, layerInfo[i].normalizedTime);
        }

        // Force an update
        animator.Update(0.0f);
    }
}