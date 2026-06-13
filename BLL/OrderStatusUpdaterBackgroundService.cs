using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BLL.DTOs;

namespace BLL
{
    public class OrderStatusUpdaterBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OrderStatusUpdaterBackgroundService> _logger;

        public OrderStatusUpdaterBackgroundService(
            IServiceProvider serviceProvider,
            ILogger<OrderStatusUpdaterBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Фоновий сервіс автооновлення статусів запущено.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                        var allOrders = await orderService.GetAllOrdersAsync();

                        var now = DateTime.Now;

                        foreach (var order in allOrders)
                        {
                            string? newStatus = null;
                            var timePassed = now - order.OrderDate;

                            if (order.Status == "Created" && timePassed.TotalMinutes >= 10)
                            {
                                newStatus = "InProcessing";
                            }
                            else if (order.Status == "InProcessing" && timePassed.TotalMinutes >= 15)
                            {
                                newStatus = "Shipped";
                            }
                            else if (order.Status == "Shipped" && timePassed.TotalDays >= 1)
                            {
                                newStatus = "Delivered";
                            }

                            if (newStatus != null)
                            {
                                _logger.LogInformation($"Авто-зміна статусу замовлення #{order.Id}: '{order.Status}' -> '{newStatus}'");

                                await orderService.UpdateOrderStatusAsync(new OrderUpdateStatusDto
                                {
                                    Id = order.Id,
                                    Status = newStatus
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Помилка під час автоматичного оновлення статусів.");
                }

                await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
            }
        }
    }
}