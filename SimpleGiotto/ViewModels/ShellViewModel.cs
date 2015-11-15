using System.ComponentModel.Composition;
using System.Globalization;
using Core.Interfaces;
using SimpleGiotto.Interfaces;
using WPFLocalizeExtension.Engine;

namespace SimpleGiotto.ViewModels
{
  [Export(typeof(IShell))]
  public class ShellViewModel : IShell
  {
    [ImportingConstructor]
    public ShellViewModel(IGiottoApp app)
    {
      Control = app;
    }
    
    public IGiottoApp Control { get; set; }

    public void Italian()
    {
      LocalizeDictionary.Instance.Culture = new CultureInfo("it");
    }

    public void English()
    {
     LocalizeDictionary.Instance.Culture = new CultureInfo("");
    }
  }
}
