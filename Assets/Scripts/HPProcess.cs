using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPProcess : MonoBehaviour
{
    // Start is called before the first frame update
    public Image _hpProcess;

    public void updateHPProcess(float hpNow, float hpMax){
        // Để đảm bảo giá trị fillAmount nằm trong khoảng 0-1
        float fillAmount = Mathf.Clamp01(hpNow / hpMax);
        _hpProcess.fillAmount = fillAmount;
    }
}
