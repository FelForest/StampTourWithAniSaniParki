using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class StampImageChange : MonoBehaviour
{
    // 스탬프 찍고 사용할 이미지
    [SerializeField]
    Sprite changedImage;

    // 확인용 스탬프 플래그 이름들
    [SerializeField]
    string[] flagNames;

    // 원본 이미지
    Sprite originalImage;

    // Image 컴포넌트
    Image stampImage;

    private void Awake()
    {
        // 원본 이미지 저장
        stampImage = GetComponent<Image>();
        originalImage = stampImage.sprite;  
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach(string flagName in flagNames)
        {
            try
            {
                // 플래그가 존재하고, 컨텐츠가 완료 되었는지 확인
                if (GameManager.Instance.IsContainKey(flagName) && GameManager.Instance.GetIsSceneFinished(flagName))
                {
                    // 이미지 변경
                    stampImage.sprite = changedImage;

                    //조건 만족시 더 이상 반복 필요 없어서 반환
                    return;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception occurred while checking flag {flagName}: {e.Message}");
            }
            
        }

        // 모든 이름을 확인해도 없으면 원본 이미지로 설정
        stampImage.sprite = originalImage;
    }
}
