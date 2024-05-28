using System.Windows;
using LangLang.WPF.MVVM;

namespace LangLang.WPF.Views.Factories;

public interface IWindowFactory
{
    public Window CreateWindow(ViewModelBase viewModel);
}