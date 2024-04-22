using UnityEngine;

namespace Code.UI
{
    public class Tutorial : MonoBehaviour
    {
        private static bool s_shown;

        private void Awake()
        {
            if (s_shown)
            {
                Destroy(gameObject);
                return;
            }
            s_shown = true;
        }
    }
}