using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using Caliburn.Micro;
using Core.Interfaces;

namespace ExplorerApp.ViewModels
{
  [Export(typeof(IGiottoApp))]
  [Export(typeof(ExplorerViewModel))]
  public class ExplorerViewModel : Screen, IGiottoApp
  {
    public ExplorerViewModel()
    {
      Cultures = new List<CultureInfo>();

      // Get the list of all cultures.
      var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

      var A = Languages.Explorer.ResourceManager.GetResourceSet(new CultureInfo("it"), true, false);

      foreach (var c in cultures)
      {
        var rs = Languages.Explorer.ResourceManager.GetResourceSet(c, true, false);
        if (rs != null)
        {
          AddCulture(c);
        }
      }
    }

    public List<CultureInfo> Cultures { get; set; } 
    private void AddCulture(CultureInfo cultureInfo)
    {
      Cultures.Add(cultureInfo);
    }
  }
}
