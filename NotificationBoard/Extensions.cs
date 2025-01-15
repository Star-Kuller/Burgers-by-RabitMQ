namespace NotificationBoard;

public static class Extensions
{
    public static string GetDishName(this int dishId)
    {
        return dishId switch
        {
            1 => "Говяжий бургер \"Микросервисный\"",
            2 => "Куриный бургер \".NET Core\"",
            3 => "\"JavaScript Promise\" бургер",
            4 => "\"Вегетарианский\" Python Django\"",
            5 => "\"Docker Container\" бургер",
            6 => "NuGet'тсы",
            7 => "Redis'ка",
            8 => "Kafka-чипсы",
            9 => "Картошка API",
            10 => "Webpack-салат",
            11 => "Java кофе",
            12 => "C++ энергетик",
            13 => "Kubernetes смузи",
            14 => "Комбо \"Full Stack\"",
            15 => "\"Agile\" ланч",
            16 => "\"Debug\" набор",
            _ => throw new ArgumentOutOfRangeException(nameof(dishId), dishId, null)
        };
    }
}