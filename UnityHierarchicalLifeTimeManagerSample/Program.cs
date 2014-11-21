using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;

namespace UnityHierarchicalLifeTimeManagerSample
{
    public static class Program
    {
        public static void Main()
        {
            var originalContainer = new UnityContainer();

            Console.WriteLine("Orig container: {0}", originalContainer.GetHashCode());

            // Register all types with the original container
            // and associate them with HierarchicalLifetimeManager
            originalContainer.RegisterType<ISnapshot>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(
                    c =>
                    {
                        Console.WriteLine("Resolving ISnapshot using container {0}", c.GetHashCode());

                        var snapshot = new Snapshot()
                        {
                            Message = string.Format(
                                CultureInfo.InvariantCulture,
                                "Container: {0}",
                                c.GetHashCode().ToString(CultureInfo.InvariantCulture))
                        };

                        return snapshot;
                    }));

            originalContainer.RegisterType<IConfig>(
                new HierarchicalLifetimeManager(),
                new InjectionFactory(
                    c =>
                    {
                        Console.WriteLine("Resolving IConfig using container {0}", c.GetHashCode());
                        var config = new Config(c.Resolve<ISnapshot>());
                        return config;
                    }));


            IUnityContainer childContainer1 = originalContainer.CreateChildContainer();
            Console.WriteLine("Child container 1: {0}", childContainer1.GetHashCode());

            // First time resolving IConfig using child container. Expectation:
            // + no instance of IConfig has been registered for the child container, 
            // so the delagate specified in InjectionFactory will be executed to create a new instance of IConfig
            // + no instance of ISnapshot has been registered, so new instance also created here
            // + We'll see 2 console messages. Both mentioning the smae child container 1
            Resolve<IConfig>(childContainer1);

            var childContainer2 = originalContainer.CreateChildContainer();
            Console.WriteLine("Child container 2: {0}", childContainer2.GetHashCode());

            // First time resolving IConfig using child container 2. 
            // So we have similar expectation with the experiment above: 2 console messages mentioning child container 2
            Resolve<IConfig>(childContainer2);

            // Second time resolving IConfig using child container 2
            // We should receive the same IConfig and ISnapshot object 
            // Also, the delegates specified in the InjectionFactory for both IConfig and ISnapshot should not be executed.
            Resolve<IConfig>(childContainer2);

            Console.WriteLine("Press anykey to continue");
            Console.ReadKey();
        }

        private static void Resolve<T>(IUnityContainer container)
        {
            var obj = container.Resolve<T>();
            Console.WriteLine("obj.id = {0}, type = {1}", obj.GetHashCode(), obj.GetType().Name);
        }
    }
}
