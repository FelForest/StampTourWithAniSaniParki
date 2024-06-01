using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QRButton : MonoBehaviour
{
    public GameObject QRArea;
    public Button qrButton;
    public QRcodeScanner QRScanner;

    private void Awake()
    {
        if (qrButton == null)
        {
            qrButton = GetComponent<Button>();
        }

        if (QRScanner == null)
        {
            QRScanner = FindObjectOfType<QRcodeScanner>();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        qrButton.onClick.AddListener(QRSetActive);
    }

    private void QRSetActive()
    {
        QRArea.SetActive(true);
        QRScanner.SwitchCamera();
    }
}
