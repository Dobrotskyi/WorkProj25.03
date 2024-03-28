using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class PopUp : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _titleField;
        [SerializeField] private TextMeshProUGUI _descriptionField;
        [SerializeField] private Animator _animator;

        public void Close()
        {
            _animator.SetTrigger("Close");
        }

        public void WinningText()
        {
            Initialize("Congratulations", "You won");
        }

        public void LosingText()
        {
            Initialize("Ooops", "You lost");
        }

        public void Initialize(string title, string description)
        {
            _titleField.text = title;
            _descriptionField.text = description;
        }
    }
}