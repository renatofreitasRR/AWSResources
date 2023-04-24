using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaAnnotations;

public class Function
{
    private readonly DynamoDBContext _dynamoDbContext;

    public Function()
    {
        _dynamoDbContext = new DynamoDBContext(new AmazonDynamoDBClient());
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Get, "users/{userId}")]
    public async Task<User> FunctionHandler(string userId, ILambdaContext context)
    {
        Guid.TryParse(userId, out var id);

        var user = await _dynamoDbContext.LoadAsync<User>(id);

        return user;
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Post, "users")]
    public async Task PostFunctionHandler([FromBody]User user, ILambdaContext context)
    {
        await _dynamoDbContext.SaveAsync(user);

    }
}
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}