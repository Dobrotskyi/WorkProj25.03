using UnityEngine;

namespace Code.Utils
{
    public class OnAnimationEnd : MonoBehaviour
    {
        [SerializeField] private bool _destroyInsteadOfDisable = false;

        public void OnAnimationFinished()
        {
            if (_destroyInsteadOfDisable)
                Destroy(gameObject);
            else
                gameObject.SetActive(false);
        }
    }
}