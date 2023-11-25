/*
 *
 * Copyright 2015 gRPC authors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
import * as parseArgs from 'minimist';
import * as grpc from '@grpc/grpc-js';
import * as protoLoader from '@grpc/proto-loader';
import path from 'path'
import { fileURLToPath } from 'url'




const __filenameNew = fileURLToPath(import.meta.url)
const __dirnameNew = path.dirname(__filenameNew)
var PROTO_PATH = __dirnameNew + '/../protos/cbgw.proto';

var packageDefinition = protoLoader.loadSync(
    PROTO_PATH,
    {keepCase: true,
     longs: String,
     enums: String,
     defaults: true,
     oneofs: true
    });
var cbgw_proto = grpc.loadPackageDefinition(packageDefinition).cbgw;

/**
 * Implements the SayHello RPC method.
 */
function GetStatus(call, callback) {

  var num0 = Math.floor(Math.random()*256);
  var num1 = Math.floor(Math.random()*256);
  var num2 = Math.floor(Math.random()*256);
  const color = `rgb(${num0}, ${num1}, ${num2})`;
  callback(null, {message: color});
}

/**
 * Starts an RPC server that receives requests for the Greeter service at the
 * sample server port
 */
function main() {
  var server = new grpc.Server();
  server.addService(cbgw_proto.Status.service, {GetStatus: GetStatus});
  server.bindAsync('0.0.0.0:50051', grpc.ServerCredentials.createInsecure(), () => {
    server.start();
  });
}

main();
