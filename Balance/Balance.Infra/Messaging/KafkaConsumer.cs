using Balance.Domain.Interfaces;
using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Balance.Infra.Messaging
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaConsumer(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = "balance-group",
                BootstrapServers = "kafka:29092",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe("balances");

            while (!stoppingToken.IsCancellationRequested)
            {
                var result = consumer.Consume(stoppingToken);
                var balanceEvent = JsonSerializer.Deserialize<BalanceUpdatedEvent>(result.Message.Value);

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var balanceRepository = scope.ServiceProvider.GetRequiredService<IBalanceRepository>();

                    var balance = balanceRepository.GetByAccountId(balanceEvent.AccountId);
                    if (balance == null)
                    {
                        balance = new Domain.Entities.Balance
                        {
                            Id = Guid.NewGuid().ToString(),
                            AccountId = balanceEvent.AccountId,
                            Amount = balanceEvent.Amount,
                            LastUpdated = DateTime.UtcNow
                        };
                    }
                    else
                    {
                        balance.Amount = balanceEvent.Amount;
                        balance.LastUpdated = DateTime.UtcNow;
                    }

                    balanceRepository.UpdateBalance(balance);
                }
            }
        }
    }
}
