using ReactiveUI;
using System;

namespace Session2v2.ViewModels;

public class AuthorizeWindowViewModel : ViewModelBase
{
	private string _code;
	public string Code
	{
		get { return _code; }
		set { _code = this.RaiseAndSetIfChanged(ref _code, value); }
	}

	private bool _isAuthEnable;
	public bool IsAuthEnable
	{
		get { return _isAuthEnable; }
		set { _isAuthEnable = this.RaiseAndSetIfChanged(ref _isAuthEnable, value); }
	}

	public AuthorizeWindowViewModel()
    {
		this.WhenAnyValue(x=> x.Code).Subscribe(_=>Enable());
    }
	private void Enable()
	{
        IsAuthEnable =! string.IsNullOrEmpty(Code);
    }
}
