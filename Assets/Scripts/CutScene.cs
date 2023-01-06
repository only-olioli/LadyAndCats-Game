using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutScene : MonoBehaviour
{
    [SerializeField] private Camera playerCam;
    [SerializeField] private AudioListener playerAudio;
    private Camera cutSceneCam;
    private AudioListener cutSceneAudio;

    public bool cutSceneNow = false;
    private bool cutSceneLast = false;

    private void Start()
    {
        cutSceneCam = GetComponent<Camera>();
        cutSceneAudio = GetComponent<AudioListener>();

        if (!cutSceneNow)
        {
            playerCam.enabled = true;
            cutSceneCam.enabled = false;
            playerAudio.enabled = true;
            cutSceneAudio.enabled = false;
        }
        else
        {
            playerCam.enabled = false;
            cutSceneCam.enabled = true;
            playerAudio.enabled = false;
            cutSceneAudio.enabled = true;
        }
    }

    private float _duration;
    private float _elapsedTime = 0;
    private bool _activeAction = false;
    private bool _activePos = false;
    private bool _activeRot = false;
    private Vector3 _targetPosition;
    private Vector3 _targetRotation;

    private void Update()
    {
        if (cutSceneNow != cutSceneLast) 
        {
            if (!cutSceneNow)
            {
                playerCam.enabled = true;
                cutSceneCam.enabled = false;
                playerAudio.enabled = true;
                cutSceneAudio.enabled = false;
            }
            else
            {
                playerCam.enabled = false;
                cutSceneCam.enabled = true;
                playerAudio.enabled = false;
                cutSceneAudio.enabled = true;
            }

            cutSceneLast = cutSceneNow;
        }

        if(_activeAction)
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime < _duration)
            {
                if (_activePos) transform.position = Vector3.Lerp(transform.position,
                    _targetPosition, _elapsedTime / _duration);
                if (_activeRot) transform.rotation = Quaternion.Lerp(transform.rotation,
                    Quaternion.Euler(_targetRotation), _elapsedTime / _duration);
            }
            else
            {
                if (_activePos) transform.position = _targetPosition;
                if (_activeRot) transform.rotation = Quaternion.Euler(_targetRotation);
                _activeAction = false;
                _elapsedTime = 0;
            }
        }
    }

    public void Move(Vector3 position, Vector3 rotation, float duration)
    {
        if (transform.position != position || transform.rotation != Quaternion.Euler(rotation) && duration <= 0)
        {
            _elapsedTime = 0;
            _duration = duration;
            _activeAction = true;

            if (transform.position != position)
            {
                _activePos = true;
                _targetPosition = position;
            }

            if (transform.rotation != Quaternion.Euler(rotation))
            {
                _activeRot = true;
                _targetRotation = rotation;
            }
        }
        else if (duration <= 0) 
        {
            transform.position = position;
            transform.rotation = Quaternion.Euler(rotation);
        }
        else
        {
            return;
        }
    }
}
