import express from "express";
import * as parseArgs from 'minimist';
import * as grpc from '@grpc/grpc-js';
import * as protoLoader from '@grpc/proto-loader';
import path from 'path'
import { fileURLToPath } from 'url'
// var parseArgs = require('minimist');
// var grpc = require('@grpc/grpc-js');
// var protoLoader = require('@grpc/proto-loader');

const app = express()
const port = 9000
app.use(function(req, res, next) {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
    });


const __filenameNew = fileURLToPath(import.meta.url)
const __dirnameNew = path.dirname(__filenameNew)
var PROTO_PATH = __dirnameNew + '/../protos/helloworld.proto';
var packageDefinition = protoLoader.loadSync(
    PROTO_PATH,
    {
        keepCase: true,
        longs: String,
        enums: String,
        defaults: true,
        oneofs: true
    });
var hello_proto = grpc.loadPackageDefinition(packageDefinition).helloworld;

app.get('/', async (req, res) => {


    // new Promise(回调函数)，这里的(resolve, reject)就是Promise的回调函数
    const value =  (await new Promise((resolve, reject) => {
        // 在这里执行异步操作
        var client = new hello_proto.Greeter('localhost:50051',
            grpc.credentials.createInsecure());
    
        client.sayHello({ name: 'world' }, function (err, response) {
            console.log('Greeting:', response.message);
    
            if (err) {
                // 如果出现错误，调用 reject 函数，并传递错误原因作为参数
                reject(err);
            } else {
                // 如果操作成功，调用 resolve 函数，并传递操作结果作为参数
                // var aa = response.message + Math.floor((Math.random()*10)+1)*1000;
                resolve(response.message);
            }
        });
    }));

    //   // 对Promise对象进行后续处理
    //   promise.then(result => {
    //     // 处理操作成功的情况，result为操作成功的结果
    //   }).catch(error => {
    //     // 处理操作失败的情况，error为发生的错误
    //   });

    // res.writeHead(200, {
    //     'Access-Control-Allow-Origin': 'http://127.0.0.1:3000',
    //     'Access-Control-Allow-Headers': 'Test-CORS, Content-Type',
    //     'Access-Control-Allow-Methods': 'PUT,DELETE',
    //     'Access-Control-Max-Age': 86400
    //   });
    // res.writeHead(200, {
    //     'Access-Control-Allow-Origin': 'http://127.0.0.1:3000',
    //     'Access-Control-Allow-Credentials': false
    //   });
    res.send(value)

})


app.listen(port, () => {
    console.log(`Example app listening at http://localhost:${port}`)
})






