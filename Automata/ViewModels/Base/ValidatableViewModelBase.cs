using System.Collections;
using System.ComponentModel;
using Automata.ViewModels.Validation;

namespace Automata.ViewModels.Base;

public abstract class ValidatableViewModelBase : ViewModelBase, INotifyDataErrorInfo
{
    protected readonly ValidationAdapter _validation;

    protected ValidatableViewModelBase()
    {
        _validation = new ValidationAdapter(OnErrorsChanged);
    }

    protected ValidatableViewModelBase(ValidationAdapter validation)
    {
        _validation = validation;
    }

    #region INotifyDataErrorInfo

    public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

    public bool HasErrors => _validation.HasErrors;

    public IEnumerable GetErrors(string? propertyName) => 
        _validation.GetPropertyErrors(propertyName ?? string.Empty);

    private void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        OnPropertyChanged(propertyName);
    }

    #endregion
}
