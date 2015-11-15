using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Caliburn.Micro;
using SimpleGiotto.Interfaces;

namespace SimpleGiotto
{
  public class Bootstrapper : BootstrapperBase
  {
    private CompositionContainer _container;

    public Bootstrapper()
    {
      try
      {
        Initialize();
      }
      catch (Exception e)
      {
        var error = e.Message;
      }
    }

    protected override void OnStartup(object sender, StartupEventArgs e)
    {
      DisplayRootViewFor<IShell>();
    }

    protected override void Configure()
    {
      var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      var catalog = new AggregateCatalog();
      if (path != null)
        catalog.Catalogs.Add(new DirectoryCatalog(path));

      var composablePartCatalog = AssemblySource.Instance.Select(x => new AssemblyCatalog(x)).OfType<ComposablePartCatalog>();

      foreach (var part in composablePartCatalog)
      {
        catalog.Catalogs.Add(part);
      }

      _container = new CompositionContainer(catalog);

      var batch = new CompositionBatch();

      batch.AddExportedValue<IWindowManager>(new WindowManager());
      batch.AddExportedValue<IEventAggregator>(new EventAggregator());
      batch.AddExportedValue(_container);
      _container.Compose(batch);
    }

    protected override object GetInstance(Type serviceType, string key)
    {
      var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
      var exports = _container.GetExportedValues<object>(contract);

      if (exports.Count() > 0)
        return exports.First();

      throw new Exception(string.Format("Could not locate any instances of contract {0}.", contract));
    }

    protected override IEnumerable<object> GetAllInstances(Type serviceType)
    {
      return _container.GetExportedValues<object>(AttributedModelServices.GetContractName(serviceType));
    }

    protected override IEnumerable<Assembly> SelectAssemblies()
    {
      var allAssemblies = base.SelectAssemblies().ToList();
      var applicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
      if (applicationPath != null)
      {
        var modulePath = Path.Combine(applicationPath, "Apps");
        allAssemblies.AddRange(Directory.GetFiles(modulePath, "*App.dll").Select(moduleFile => Assembly.LoadFile(moduleFile)));
      }

      return allAssemblies;
    }
  }
}
