// Copyright 2015 gRPC authors.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Helloworld;

namespace GreeterClient
{
  class Program
  {
    static string serverChannel
    {
      get {
        var r = Environment.GetEnvironmentVariable("GRPCSERVER");
        if (string.IsNullOrEmpty(r))
        {
          return "localhost:30051";
        }
        else return r;
      }
    }
    static async Task grpcRead(CancellationToken token)
    {
      Channel channel = new Channel(serverChannel, ChannelCredentials.Insecure);

      while (!token.IsCancellationRequested)
      {
        var client = new Greeter.GreeterClient(channel);

        String user = "you";

        var reply = await client.SayHelloAsync(new HelloRequest { Name = user });
        Console.WriteLine("Greeting: " + reply.Message + " " + DateTime.Now.ToString());

        await Task.Delay(200,token);
      }
      await channel.ShutdownAsync();

    }
    public static void Main(string[] args)
    {

      dotenv.net.DotEnv.Load();

      CancellationTokenSource cts = new CancellationTokenSource();
      var grpcTask= grpcRead(cts.Token);

      Console.WriteLine("Press ESC key to exit...");
      Console.CancelKeyPress += (o, a) =>
      {
        cts.Cancel();
      };
      grpcTask.Wait();

    }
  }
}
