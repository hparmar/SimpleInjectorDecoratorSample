using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;
using SimpleInjector.Extensions;

namespace SimpleInjectorDecoratorSample
{
    class Program
    {
        static void Main (string[] args)
        {
            var container = new Container();
            container.Register<ISomethingHandler, SimpleSomethingHandler>();
            container.RegisterDecorator(
                typeof(ISomethingHandler),
                typeof(DoSomethingHandler),
                ctx => ShouldDecorate(ctx.ServiceType));
            container.Verify();

            container.GetInstance<ISomethingHandler>().Handle();
        }

        private static bool ShouldDecorate (Type serviceType)
        {
#error koşulu değiştirerek dene. change condition for different result.
            if (serviceType == typeof(SimpleSomethingHandler))
                return true;
            return false;
        }
    }

    public interface ISomethingHandler
    {
        void Handle ();
    }

    public class SimpleSomethingHandler : ISomethingHandler
    {
        public void Handle ()
        {
            Console.WriteLine("Simple handle executing...");
        }
    }

    public class DoSomethingHandler : ISomethingHandler
    {
        private readonly ISomethingHandler _decorated;

        public DoSomethingHandler (ISomethingHandler decorated)
        {
            _decorated = decorated;
        }

        public void Handle ()
        {
            Console.WriteLine("Do handle executing...");
            _decorated.Handle();
        }
    }
}
