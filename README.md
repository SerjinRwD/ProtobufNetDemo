Реализация [Get Started](https://protobuf-net.github.io/protobuf-net.Grpc/gettingstarted) туторила по [protobuf-net.Grpc](https://github.com/protobuf-net/protobuf-net.Grpc).

Этот пакет позволяет создавать сервисы и их контракты для коммуникации по gRPC используя подход code first ("в стиле" WCF). Субъективно, это гораздо удобнее для разработки внутренних систем, чем использование ```.proto```.

## Заметки по проектам

### Service
Нужно настроить сервис на поддержку HTTP/2. Для этого нужно немного переписать Program.cs. Я переписал по [официальному примеру](https://github.com/protobuf-net/protobuf-net.Grpc/blob/master/examples/pb-net-grpc/Server_CS/Program.cs). Про метод расширения ConfigureKestrel читать [здесь](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.hosting.webhostbuilderkestrelextensions.configurekestrel?view=aspnetcore-3.1).

Метод Division проверяет аргументы. Если Y == 0, выбрасывает исключение RpcException с кодом 13 (InvalidArgument).

### Client
Нужно дополнительно установить пакет Grpc.Net.Client (конкретно в примере без него класс GrpcChannel не доступен).

Код вызова методов обёрнут в try/catch. Блок catch перехватывает ошибки RpcException для корректной обработки gRPC кодов ошибок.

Код по обработке ошибок добавлен на основе материалов:
* Официальный гайд [Error Handling](https://grpc.io/docs/guides/error/)
* Официальный пример [для C#](https://github.com/avinassh/grpc-errors/tree/master/csharp/Hello)

Пример я немного переписал. Чтобы не затирать заданные в конфигурации прослушиваемые адреса, использую опцию ConfigureEndpointDefaults:
```csharp
kestrelOptions.ConfigureEndpointDefaults(x => x.Protocols = HttpProtocols.Http2);
```