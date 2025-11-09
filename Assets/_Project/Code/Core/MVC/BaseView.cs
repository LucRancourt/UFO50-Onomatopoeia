using UnityEngine;

namespace _Project.Code.Core.MVC
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        [SerializeField] protected bool _startVisible = true;

        public virtual void Initialize()
        {
            if (_startVisible)
                Show();
            else
                Hide();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void UpdateDisplay()
        {
            // Override in derived classes
        }

        public virtual void Dispose()
        {
            // Cleanup logic if needed
        }
    }
    public abstract class BaseView<T> : MonoBehaviour, IView<T>
    {
        [SerializeField] protected bool _startVisible = true;
        
        public virtual void Initialize()
        {
            if (_startVisible)
                Show();
            else
                Hide();
        }
        
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        public virtual void UpdateDisplay()
        {
            // Override in derived classes
        }

        public virtual void UpdateDisplay(T data)
        {
            // Override in derived classes
        }

        public virtual void Dispose()
        {
            // Cleanup logic if needed
        }
    }
}