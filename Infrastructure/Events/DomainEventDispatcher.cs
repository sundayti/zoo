using System.Reflection;
using Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Events;

/// <summary>
/// Подписывается на все DomainEvents и перенаправляет их в зарегистрированные обработчики.
/// Вызывается при старте приложения.
/// </summary>
public static class DomainEventDispatcher
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        // Регистрируем единый колбэк для всех событий
        DomainEvents.RegisterHandler(async domainEvent =>
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(domainEvent.GetType());
            var handlers = serviceProvider.GetServices(handlerType);
            foreach (var handler in handlers)
            {
                // вызываем метод Handle у каждого IDomainEventHandler<T>
                var handleMethod = handlerType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public);
                if (handleMethod != null)
                {
                    var task = (Task)handleMethod.Invoke(handler, new[] { domainEvent });
                    await task.ConfigureAwait(false);
                }
            }
        });
    }
}
