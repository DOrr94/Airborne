using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }


public class Player : NetworkBehaviour {

    [SerializeField]
    ToggleEvent onToggleShared;

    [SerializeField]
    ToggleEvent onToggleLocal;

    [SerializeField]
    ToggleEvent onToggleRemote;

    public GameObject mainCamera;
    

    void Start()
    {
        enablePlayer();
    }

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
      Vector3 moveCamTo = transform.position - transform.forward * 10.0f + Vector3.up * 5.0f;
      float bias = 0.96f;
      Camera.main.transform.position = Camera.main.transform.position * bias + moveCamTo * (1.0f - bias);
      Camera.main.transform.LookAt(transform.position + transform.forward * 30.0f);
    }

    void disablePlayer()
    {
        onToggleShared.Invoke(false);
        

        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(false);
        }
        else
        {
            onToggleRemote.Invoke(false);
        }



    }

    void enablePlayer()
    {
        
        onToggleShared.Invoke(true);

        if (isLocalPlayer)
        {
            onToggleLocal.Invoke(true);
        }
        else
        {
            onToggleRemote.Invoke(true);
        }

    }

}
