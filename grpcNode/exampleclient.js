const grpc = require("grpc");
const example = require("./example_pb");
const exampleService = require("./example_grpc_pb");

const client = new exampleService.ExampleClient(
  "localhost:50051",
  grpc.credentials.createInsecure()
);

const request = new example.Message();

request.setContent("Hello World!");

client.modifyMessage(request, function (err, response) {
  console.log("Modified message: ", response.getContent());
});