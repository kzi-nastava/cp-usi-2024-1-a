using System.Windows;
using LangLang.MVVM;

namespace LangLang.View.Factories;

public interface IWindowFactory
{
    public Window CreateWindow(ViewModelBase viewModel);
}