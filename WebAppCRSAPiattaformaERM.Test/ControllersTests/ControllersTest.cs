using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;
using WebAppCRSAPiattaformaERM.Handlers.QueryHandlers;

namespace WebAppCRSAPiattaformaERM.Test.ControllersTests;

public class ControllersTest
{
    [Theory]
    [InlineData("WebAppCRSAPiattaformaERM", "WebAppCRSAPiattaformaERM.Controllers")]
    public void TestControllers(string assemblyName, params string[] namespaces)
    {
        // Carica l'assembly specificato 
        var assembly = Assembly.Load(assemblyName);

        // Ottieni tutti i tipi dall'assembly che appartengono ai namespace specificati
        var types = assembly.GetTypes().Where(t => namespaces.Contains(t.Namespace)).ToList();

        // Definisce le azioni dei controller
        var actions = new List<string>() { "Get", "Post", "Put", "Delete", "GetCurrent", "GetSelect" };

        // Itera sui tipi (controller) non astratti
        foreach (var tipo in types.Where(x => !x.IsAbstract))
        {
            // Ottieni il tipo del controller corrente
            var controllerType = tipo;

            // Creare il controller utilizzando il tipo corrente
            var loggerMockType = typeof(ILogger<>).MakeGenericType(controllerType);
            var loggerMock = Activator.CreateInstance(typeof(Mock<>).MakeGenericType(loggerMockType)) as Mock;
            var mediatorMock = new Mock<IMediator>();

            // Configura i mock, se necessario
            // Esempio: mediatorMock.Setup(m => m.Send(It.IsAny<IRequest<IResult>>(), It.IsAny<CancellationToken>())).ReturnsAsync(...);

            var controller = Activator.CreateInstance(controllerType, loggerMock.Object, mediatorMock.Object);

            // Itera sulle azioni dei controller
            foreach (var action in actions)
            {
                // Ottieni il metodo corrente del controller
                var methodInfo = controllerType.GetMethod(action);
                if (methodInfo != null)
                {
                    // Creare i parametri richiesti dal metodo
                    var parameters = methodInfo.GetParameters().Select(p => p.ParameterType.IsValueType ? Activator.CreateInstance(p.ParameterType) : null).ToArray();

                    try
                    {
                        // Esegui il metodo del controller
                        var result = methodInfo.Invoke(controller, parameters);
                    }
                    catch (TargetInvocationException ex)
                    {
                        // Gestisci le eccezioni se necessario
                        Console.WriteLine($"Errore nell'invocazione del metodo {action} del controller {controllerType.Name}: {ex.InnerException.Message}");
                    }
                }
            }
        }
    }
}
