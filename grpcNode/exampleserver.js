

const grpc = require("@grpc/grpc-js");
const protoLoader = require("@grpc/proto-loader");
// const PROTO_PATH = path.resolve(__dirname, '../../proto/example.proto');


// const grpc = require("grpc");
const example = require("./example_pb");
const exampleService = require("./example_grpc_pb");

const server = new grpc.Server();

function modifyMessage(call, callback) {
  const response = new example.Message();
  response.setContent(call.request.getContent().toUpperCase());
  callback(null, response);
}

server.addService(exampleService.ExampleService, {
  modifyMessage: modifyMessage,
});

server.bind("localhost:50051", grpc.ServerCredentials.createInsecure());
console.log("Server running at http://localhost:50051");
server.start();