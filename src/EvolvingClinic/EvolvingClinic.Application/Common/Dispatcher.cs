using EvolvingClinic.Application.Appointments;
using EvolvingClinic.Application.Doctors;
using EvolvingClinic.Application.HealthcareServices;
using EvolvingClinic.Application.Patients;

namespace EvolvingClinic.Application.Common;

public class Dispatcher
{
    private readonly Dictionary<Type, object> _handlers = new();
    private readonly Dictionary<Type, object> _services = new();

    public Dispatcher()
    {
        RegisterServices();
        AutoRegisterHandlers();
    }

    private void RegisterServices()
    {
        _services[typeof(IPatientRepository)] = new InMemoryPatientRepository();
        _services[typeof(IDailyAppointmentScheduleRepository)] = new InMemoryDailyAppointmentScheduleRepository();
        _services[typeof(IHealthcareServiceTypeRepository)] = new InMemoryHealthcareServiceTypeRepository();
        _services[typeof(IDoctorRepository)] = new InMemoryDoctorRepository();
    }

    private void AutoRegisterHandlers()
    {
        var applicationAssembly = GetType().Assembly;

        var handlerTypes = applicationAssembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && 
                   t.GetInterfaces().Any(i => i.IsGenericType && 
                   (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                    i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                    i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>))))
            .ToList();

        foreach (var handlerType in handlerTypes)
        {
            var handlerInterfaces = handlerType.GetInterfaces()
                .Where(i => i.IsGenericType && 
                       (i.GetGenericTypeDefinition() == typeof(ICommandHandler<>) ||
                        i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>) ||
                        i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)))
                .ToList();

            foreach (var handlerInterface in handlerInterfaces)
            {
                var requestType = handlerInterface.GetGenericArguments()[0];
                var handler = CreateHandler(handlerType);
                if (handler != null)
                {
                    _handlers[requestType] = handler;
                }
            }
        }
    }

    private object? CreateHandler(Type handlerType)
    {
        var constructors = handlerType.GetConstructors();
        var constructor = constructors.FirstOrDefault();
        
        if (constructor == null) return null;

        var parameters = constructor.GetParameters();
        var args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            var paramType = parameters[i].ParameterType;
            if (_services.TryGetValue(paramType, out var service))
            {
                args[i] = service;
            }
            else
            {
                throw new InvalidOperationException($"Service of type {paramType.Name} not registered");
            }
        }

        return Activator.CreateInstance(handlerType, args);
    }

    public async Task<TResult> Execute<TResult>(ICommand<TResult> command)
    {
        var commandType = command.GetType();
        if (!_handlers.TryGetValue(commandType, out var handler))
        {
            throw new InvalidOperationException($"No handler registered for command {commandType.Name}");
        }

        var handlerType = handler.GetType();
        var handleMethod = handlerType.GetMethod("Handle", [commandType]);
        
        if (handleMethod == null)
        {
            throw new InvalidOperationException($"Handler {handlerType.Name} does not have Handle method for {commandType.Name}");
        }

        var result = handleMethod.Invoke(handler, [command]);
        
        if (result is Task<TResult> taskResult)
        {
            return await taskResult;
        }

        throw new InvalidOperationException($"Handler method should return Task<{typeof(TResult).Name}>");
    }

    public async Task Execute(ICommand command)
    {
        var commandType = command.GetType();
        if (!_handlers.TryGetValue(commandType, out var handler))
        {
            throw new InvalidOperationException($"No handler registered for command {commandType.Name}");
        }

        var handlerType = handler.GetType();
        var handleMethod = handlerType.GetMethod("Handle", [commandType]);
        
        if (handleMethod == null)
        {
            throw new InvalidOperationException($"Handler {handlerType.Name} does not have Handle method for {commandType.Name}");
        }

        var result = handleMethod.Invoke(handler, [command]);
        
        if (result is Task task)
        {
            await task;
            return;
        }

        throw new InvalidOperationException("Handler method should return Task");
    }

    public async Task<TResult> ExecuteQuery<TResult>(IQuery<TResult> query)
    {
        var queryType = query.GetType();
        if (!_handlers.TryGetValue(queryType, out var handler))
        {
            throw new InvalidOperationException($"No handler registered for query {queryType.Name}");
        }

        var handlerType = handler.GetType();
        var handleMethod = handlerType.GetMethod("Handle", [queryType]);
        
        if (handleMethod == null)
        {
            throw new InvalidOperationException($"Handler {handlerType.Name} does not have Handle method for {queryType.Name}");
        }

        var result = handleMethod.Invoke(handler, [query]);
        
        if (result is Task<TResult> taskResult)
        {
            return await taskResult;
        }

        throw new InvalidOperationException($"Handler method should return Task<{typeof(TResult).Name}>");
    }
}