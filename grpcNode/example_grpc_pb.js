// GENERATED CODE -- DO NOT EDIT!

'use strict';
var grpc = require('grpc');
var example_pb = require('./example_pb.js');

function serialize_example_Message(arg) {
  if (!(arg instanceof example_pb.Message)) {
    throw new Error('Expected argument of type example.Message');
  }
  return Buffer.from(arg.serializeBinary());
}

function deserialize_example_Message(buffer_arg) {
  return example_pb.Message.deserializeBinary(new Uint8Array(buffer_arg));
}


var ExampleService = exports.ExampleService = {
  modifyMessage: {
    path: '/example.Example/ModifyMessage',
    requestStream: false,
    responseStream: false,
    requestType: example_pb.Message,
    responseType: example_pb.Message,
    requestSerialize: serialize_example_Message,
    requestDeserialize: deserialize_example_Message,
    responseSerialize: serialize_example_Message,
    responseDeserialize: deserialize_example_Message,
  },
};

exports.ExampleClient = grpc.makeGenericClientConstructor(ExampleService);
