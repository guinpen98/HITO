using UnityEngine;

namespace Chat
{
    [System.Serializable]
    class YukkuriController : IAvatarController
    {
        [SerializeField] private Animator _animator;

        public void StartLipSync()
        {
            _animator.Play("Lipsync");
        }

        public void StopLipSync()
        {
            _animator.Play("Idle");
        }
    }
}
