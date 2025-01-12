using System.Text;
using Newtonsoft.Json;
using OrderTerminal;
using RabbitMQ.Client;


var factory = new ConnectionFactory { HostName = "localhost" };
await using var connection = await factory.CreateConnectionAsync();
await using var channel = await connection.CreateChannelAsync();
while (true)
{
    ShowMenu();
    var selectedPositions = new List<int>();
    while (selectedPositions.Count == 0)
    {
        var userInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(userInput))
        {
            Console.WriteLine("Вы ничего не ввели!");
            continue;
        }
        try
        {
            selectedPositions = Parse(userInput);
        }
        catch
        {
            Console.WriteLine("Вы что-то неправильно ввели");
            selectedPositions = [];
        }
    }

    var order = new Order(selectedPositions);
    
    await channel.ExchangeDeclareAsync(exchange: "logs", type: ExchangeType.Fanout);
    var serializedOrder = JsonConvert.SerializeObject(order);
    var body = Encoding.UTF8.GetBytes(serializedOrder);

    await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "hello", body: body);

    Console.WriteLine($"Ваш заказ под номером {0}. Следите за его приготовлением на NotificationBoard");
    await Task.Delay(5000);
    Console.WriteLine("Следующий!");
    await Task.Delay(3000);
}
return;

// Меню хардкодить - не лучшее решение, но городить какое-то нормальное решение в таком проекте бессмысленно
void ShowMenu() => Console.WriteLine("""
                                     Добро пожаловать в кафе "Бургеры от RabbitMQ"!
                                     Вот наше меню

                                     БУРГЕРЫ:
                                     1. Говяжий бургер "Микросервисный" с распределённым соусом
                                     2. Куриный бургер ".NET Core" с горячей компиляцией
                                     3. "JavaScript Promise" бургер (спойлер: может выполниться позже)
                                     4. Вегетарианский "Python Django" с зелёным REST-соусом
                                     5. "Docker Container" бургер (упакован со всеми зависимостями)

                                     ЗАКУСКИ:
                                     6. NuGet'тсы в пакетном менеджере
                                     7. Redis'ка маринованная
                                     8. Kafka-чипсы (распределяются по очереди)
                                     
                                     ГАРНИРЫ:
                                     9. Картошка API (доступна через REST)
                                     10. Webpack-салат (собранный вручную)

                                     НАПИТКИ:
                                     11. Java кофе (горячий, объектно-ориентированный)
                                     12. C++ энергетик (с ручным управлением памятью)
                                     13. Kubernetes смузи (автомасштабируемый)
                                     
                                     КОМБО:
                                     14. Комбо "Full Stack" (бургер + напиток + десерт)
                                     15. "Agile" ланч (спринт-меню, меняется каждые 2 недели)
                                     16. "Debug" набор (когда что-то пошло не так)

                                     Примечания:
                                     - Все блюда подаются с open-source приправами
                                     - Доступна доставка через API
                                     - В случае сбоя работает механизм отката заказа
                                     
                                     Для заказа перечислите номера позиций которые хотите заказать через запятую.
                                     """);

List<int> Parse(string userInput)
{
    var splitedUserInput =
        userInput
            .Split(",")
            .Select(x => x.Trim());
    return splitedUserInput.Select(int.Parse).ToList();
} 