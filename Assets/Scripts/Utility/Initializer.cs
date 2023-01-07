using System;

namespace Utility
{
    public class Initializer : IInitializer
    {
        private bool _isInitialized;
        private readonly Action _initialize;

        public Initializer(Action initialize)
        {
            _initialize = initialize;
        }

        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }
            _isInitialized = true;
            _initialize?.Invoke();
        }
    }
}