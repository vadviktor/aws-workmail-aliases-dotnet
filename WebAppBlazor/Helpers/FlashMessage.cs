namespace WebAppBlazor.Helpers;

public class FlashMessage
{
    private string? _error;
    private string? _info;
    private string? _success;
    private string? _successClickable;

    public string? Error
    {
        get => _error;
        set
        {
            ClearAll();
            _error = value;
        }
    }

    public string? Info
    {
        get => _info;
        set
        {
            ClearAll();
            _info = value;
        }
    }

    public string? Success
    {
        get => _success;
        set
        {
            ClearAll();
            _success = value;
        }
    }

    public string? SuccessClickable
    {
        get => _successClickable;
        set
        {
            ClearAll();
            _successClickable = value;
        }
    }

    private void ClearAll()
    {
        _error = null;
        _info = null;
        _success = null;
    }
}