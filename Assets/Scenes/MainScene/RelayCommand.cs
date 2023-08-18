using System;
using System.Windows.Input;

namespace Feudal.Scenes.Main
{
    /// <summary>
    /// A command whose purpose is to relay its functionality to other objects by invoking delegates.
    /// </summary>
    public class RelayCommand : ICommand
    {
        public Action execute;
        public Func<bool> canExecute;

        public RelayCommand()
        {

        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The delegate to be invoked for the <see cref="Execute(object)"/> method.</param>
        /// <param name="canExecute">The delegate to be invoked for the <see cref="CanExecute(object)"/> method.  If not provided <see cref="CanExecute(object)"/> will always return <c>true</c>.</param>
        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <inheritdoc />
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute();
        }

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            execute?.Invoke();
        }

        /// <summary>
        /// Raises <see cref="CanExecuteChanged"/> so every command invoker can requery to check if the command can execute.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;
    }

    /// <summary>
    /// A command whose purpose is to relay its functionality to other objects by invoking delegates.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool> _canExecute;

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The delegate to be invoked for the <see cref="Execute(object)"/> method.</param>
        /// <param name="canExecute">The delegate to be invoked for the <see cref="CanExecute(object)"/> method.  If not provided <see cref="CanExecute(object)"/> will always return <c>true</c>.</param>
        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <inheritdoc />
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null) return true;

            if (parameter is T)
                return _canExecute((T)parameter);
            return false;
        }

        /// <inheritdoc />
        public void Execute(object parameter)
        {
            if (parameter is T)
                _execute((T)parameter);
        }

        /// <summary>
        /// Raises <see cref="CanExecuteChanged"/> so every command invoker can requery to check if the command can execute.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;
    }
}